
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// タイトルシーンの遷移状態を管理します。
//
//------------------------------------------------------------

public class RootTitle : SceneRootBehaviour
{
  [Header("UI 要素と SE")]
  [SerializeField]
  ButtonControl _control = null;

  [SerializeField]
  ClipIndex _seSelect = null, _seApply = null;


  [Header("ルール説明を表示する UI オブジェクト")]
  [SerializeField]
  RuleBoardController _ruleBoard = null;


  // ゲーム開始ボタンが選択されたかどうかを受け取る
  bool _isStart = false;


  protected override IEnumerator MainLoop()
  {
    while (!_isStart)
    {
      yield return StartCoroutine(Select());
      if (!_isStart) { yield return StartCoroutine(GameRule()); }
    }
  }

  // ゲーム開始 or ルール説明 の選択
  IEnumerator Select()
  {
    // SE 再生判断のため、インデックス位置を記憶する
    int buttonIndex = _control.buttonIndex;

    while (!_control.isPushApplyButton)
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

  // ルール説明の表示中
  IEnumerator GameRule()
  {
    // UI オブジェクト表示、アニメーション完了まで待機
    _ruleBoard.StartAnimation();
    while (_ruleBoard.isPlaying) { yield return null; }

    yield return null;

    // コントローラー入力があるまで待機
    while (!_control.isPushApplyButton) { yield return null; }

    // UI オブジェクトを隠す処理開始、完了するまで待機
    _ruleBoard.ReturnAnimation();
    while (_ruleBoard.isPlaying) { yield return null; }

    _control.isStop = false;
  }


  // ゲーム開始ボタンの処理
  public void OnStart()
  {
    OnPlayApply();

    // UI オブジェクト表示中にボタンが押されてもシーンを切り替えない
    _isStart = !_ruleBoard.isActive;
  }

  // 操作説明ボタンの処理
  public void OnRuleVisible()
  {
    OnPlayApply();
  }


  // 決定 SE を再生する
  void OnPlayApply()
  {
    if (!_ruleBoard.isPlaying) { audio.Play(_seApply); }
    _control.isStop = true;
  }
}
