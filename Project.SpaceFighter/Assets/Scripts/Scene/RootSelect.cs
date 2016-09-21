
using UnityEngine;
using System.Collections;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// マシンの選択シーンを管理します。
//
//------------------------------------------------------------

public class RootSelect : SceneRootBehaviour
{
  [Header("プレイヤー情報")]
  [SerializeField]
  SelectController[] _controllers = null;

  [SerializeField]
  ModelGenerator _generator = null;

  [SerializeField]
  SceneIndex _title = null;


  protected override IEnumerator MainLoop()
  {
    // モデルを生成する
    GenerateModels();

    // シーン遷移の演出が終わるまで待機
    if (existSequencer) { yield return StartCoroutine(WaitSequence()); }

    // 全てのプレイヤーが決定ボタンを押したらループを終了する
    while (!_controllers.All(ctrl => ctrl.isApply))
    {
      if (_controllers.Any(ctrl => ctrl.isCancel))
      {
        SetOtherScene(_title);
        _controllers.Execute(ctrl => ctrl.isStop = true);
        break;
      }
      yield return null;
    }
  }

  void GenerateModels()
  {
    var indices = new[]
    {
      JoystickIndex.P1,
      JoystickIndex.P2,
      JoystickIndex.P3,
      JoystickIndex.P4,
    };

    for (int i = 0; i < indices.Length; i++)
    {
      _generator.GenerateModel(indices[i], i);
    }
  }
}
