
using UnityEngine;
using System.Collections;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// リザルトシーンの遷移状態を管理します。
//
//------------------------------------------------------------

public class RootResult : SceneRootBehaviour
{
  [Header("UI 要素と SE")]
  [SerializeField]
  ButtonControl _control = null;

  [SerializeField]
  ClipIndex _seSelect = null, _seApply = null;


  [Header("リザルト画面に遷移直後、操作を受け付けない時間（単位：秒）")]
  [SerializeField]
  CanvasGroup _buttonObject = null;

  [SerializeField, Range(1f, 10f)]
  float _stopTime = 3f;


  [Header("順位を表示する UI 要素")]
  [SerializeField]
  ResultActorUI[] _actors = null;

  [SerializeField]
  Color[] _colors = null;

  static readonly string[] _rankNames = { "1st", "2nd", "3rd", "4th", };

  static readonly JoystickIndex[] _playerNumbers = new[]
  {
    JoystickIndex.P1,
    JoystickIndex.P2,
    JoystickIndex.P3,
    JoystickIndex.P4,
  };


  [Header("順位を表す文字列の横幅とスケーリング幅")]
  [SerializeField, Range(0f, 1f)]
  float _width = 0.25f;

  [SerializeField, Range(0f, 0.5f)]
  float _scale = 0.1f;


  [Header("モデル表示のコンポーネント")]
  [SerializeField]
  ModelGenerator _generator = null;


  // いずれかのボタンが選択されたかを判定
  bool _isSelect = false;


  void Awake()
  {
    _buttonObject.interactable = false;
    _control.isStop = true;
  }

  protected override IEnumerator MainLoop()
  {
    // 他のコンポーネントの初期化完了を待機
    yield return null;

    // スコアの高い順に、上から表示する
    foreach (var player in _playerNumbers)
    {
      UpdateRankBoard(player);
    }

    // モデルを表示する
    for (int i = 0; i < _actors.Count(); i++)
    {
      _generator.GenerateModel(_actors[i].playerNumber, i);
    }

    // SE 再生判断のため、インデックス位置を記憶する
    int buttonIndex = _control.buttonIndex;

    // シーン遷移の処理および待機時間終了まで操作を受け付けない
    if (existSequencer) { yield return StartCoroutine(WaitSequence()); }
    yield return new WaitForSeconds(_stopTime);
    _buttonObject.interactable = true;
    _control.isStop = false;

    while (!_isSelect)
    {
      // インデックスが移動していたら SE を再生する
      if (buttonIndex != _control.buttonIndex)
      {
        audio.Play(_seSelect);
        buttonIndex = _control.buttonIndex;
      }

      yield return null;
    }
  }

  void UpdateRankBoard(JoystickIndex playerNumber)
  {
    // 取得したプレイヤーよりもスコアの高いプレイヤーの人数を取得
    var count = GetCount(playerNumber);

    // UI を更新する
    for (var i = count; i < _actors.Count(); i++)
    {
      var actor = _actors[i];

      // 登録済みの UI なら次の UI へ進む
      if (actor.isActive) { continue; }

      actor.SetPlayerName(playerNumber);

      // 順位の文字列と色を順位に合わせて更新
      actor.rank.text = _rankNames[count];
      actor.rank.color = _colors[count];

      // 順位の文字列サイズを順位に合わせて設定
      var baseScale = Vector3.one + Vector3.right * _width;
      actor.rank.rectTransform.localScale = baseScale * (1 - count * _scale);

      break;
    }
  }

  // 指定したプレイヤー番号のスコアよりも高いスコアを持つプレイヤーの数を返す
  int GetCount(JoystickIndex index)
  {
    var scores = _playerNumbers.Select(player => player.GetState().score);
    return scores.Count(score => score > index.GetState().score);
  }


  // タイトルへ戻る
  public void OnGoToTitle()
  {
    OnPlayApply();
    _control.isStop = true;
    _isSelect = true;
  }

  // ゲーム終了ボタン
  public void OnQuit()
  {
    OnGoToTitle();
    callback = CallBackQuit;
  }


  // 決定 SE を再生
  void OnPlayApply()
  {
    if (_buttonObject.interactable) { audio.Play(_seApply); }
  }

  // ゲーム終了処理のコールバック
  void CallBackQuit()
  {
    Application.Quit();
  }
}
