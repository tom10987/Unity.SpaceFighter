
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// ゲーム本編シーンの遷移状態を管理します。
//
//------------------------------------------------------------

public class RootMainGame : SceneRootBehaviour
{
  static readonly float _minuteToSecond = 60f;

  [Header("残り時間（単位：分）")]
  [SerializeField, Range(0.5f, 3f)]
  float _gameTimeLimit = 1f;

  [Header("残り時間表示 UI のコンポーネント")]
  [SerializeField]
  MainGameUI _gameUI = null;

  [Header("残り時間が少ないときの UI カラーとその判定時間（単位：秒）")]
  [SerializeField]
  Color _caution = Color.yellow;

  [SerializeField]
  Color _danger = Color.red;

  [SerializeField, Range(10f, 60f)]
  float _cautionTime = 15f;

  [SerializeField, Range(1f, 10f)]
  float _dangerTime = 5f;


  [Header("残り時間が少ないときの BGM エフェクト")]
  [SerializeField, Range(1f, 2f)]
  float _pitch = 1.5f;

  [SerializeField, Range(0.1f, 0.5f)]
  float _volume = 0.15f;


  [Header("残り時間が少ないときの UI サイズ")]
  [SerializeField, Range(1f, 2f)]
  float _cautionScale = 1.25f;

  [SerializeField, Range(1f, 2f)]
  float _dangerScale = 1.5f;


  [Header("カウントダウン SE")]
  [SerializeField]
  ClipIndex _seCountDown = null;

  [SerializeField]
  ClipIndex _seDanger = null;


  [Header("プレイヤーのプレハブ")]
  [SerializeField]
  ActorController[] _players = null;

  [Header("プレイヤーのスタート地点")]
  [SerializeField]
  Transform[] _positions = null;


  protected override IEnumerator MainLoop()
  {
    // スコア初期化
    ActorStateManager.Clear();

    // プレイヤーのインスタンス化
    var players = GeneratePlayers();

    // ゲームの残り時間を初期化、UI に反映する
    float gameTime = _gameTimeLimit * _minuteToSecond;
    _gameUI.UpdateTime(gameTime);
    _gameUI.SetColor(Color.white);

    // カウントダウン SE が終了するまでゲーム進行を待機
    yield return StartCoroutine(WaitForEndOfSE(_seCountDown));

    System.Action changeUI = () =>
    {
      _gameUI.SetColor(_caution);
      _gameUI.SetScale(Vector3.one * _cautionScale);
      bgmSource.pitch *= _pitch;
    };

    System.Action timeup = () =>
    {
      _gameUI.SetColor(_danger);
      _gameUI.SetScale(Vector3.one * _dangerScale);
      bgmSource.volume = _volume;
      audio.Play(_seDanger);
    };

    while (gameTime > 0f)
    {
      var deltaTime = Time.deltaTime;

      // ゲームの残り時間を減らして UI に反映
      gameTime -= deltaTime;
      _gameUI.UpdateTime(gameTime);

      // 残り時間が少なくなったら一度だけ色を変更
      if (gameTime < _cautionTime && changeUI != null)
      {
        changeUI();
        changeUI = null;
      }

      // 終了直前にカウントダウン SE を再生
      if (gameTime < _dangerTime && timeup != null)
      {
        timeup();
        timeup = null;
      }

      // プレイヤー全体の更新
      foreach (var player in players)
      {
        // 戦闘不能の演出中でなければ実行
        if (!player.actorObject.dead.isPlaying)
        {
          player.Rotate(deltaTime);
          player.Translate(deltaTime);
          player.Attack();
          player.SuperAttack();
        }

        // 耐久値が０のプレイヤーがいれば復活
        if (player.actorObject.isDead) { player.Restart(); }
      }

      yield return null;
    }

    foreach (var player in players) { player.TimeUp(); }
  }


  // プレイヤー生成
  ActorController[] GeneratePlayers()
  {
    var indices = new[]
    {
      JoystickIndex.P1, JoystickIndex.P2, JoystickIndex.P3, JoystickIndex.P4,
    };

    var result = new List<ActorController>();

    foreach (var index in indices)
    {
      // プレイヤーが選択したマシンのプレハブを取得
      //var state = index.GetState();
      var prefab = _players[(int)index - 1];

      // プレハブをインスタンス化、初期化する
      var player = Instantiate(prefab);
      player.Setup(index);

      // プレイヤーの位置を所定の位置に設定
      var position = _positions[((int)index) - 1].position;
      player.transform.position = position;
      player.transform.LookAt(Vector3.zero);

      // カメラをプレイヤーと同じ向きにする
      player.camera.transform.rotation = player.transform.rotation;

      result.Add(player);
    }

    return result.ToArray();
  }
}
