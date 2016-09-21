
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// マシン選択画面における、プレイヤーの操作です。
//
// プレイヤー自身の UI コンポーネントの参照を持ちますが、
// プレイヤーの操作の結果は、外部で反映します。
//
//------------------------------------------------------------
// NOTE:
// マシンのパラメータは全てのプレイヤーで共通のため、
// マシンへの参照を持つ必要がないと判断しました。
//
// データが外部で管理されるため、
// 管理先でプレイヤーの操作の結果を受け取り、UI の更新を行います。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioClipPlayer))]
public class SelectController : MonoBehaviour
{
  [Header("UI を操作できるコントローラー")]
  [SerializeField]
  JoystickIndex _joystickIndex = JoystickIndex.P1;

  [SerializeField]
  SelectActorUI _ui = null;


  [Header("再生する SE")]
  [SerializeField]
  ClipIndex _seSelect = null;
  [SerializeField]
  ClipIndex _seApply = null;
  [SerializeField]
  ClipIndex _seCancel = null;


  /// <summary> 決定ボタンが押されていれば true を返す </summary>
  public bool isApply { get; private set; }

  /// <summary> 決定していない状態でキャンセルボタンが押されたら true を返す </summary>
  public bool isCancel { get; private set; }

  /// <summary> コントローラー操作の状態 </summary>
  public bool isStop { get; set; }


  IEnumerator Start()
  {
    var joystick = JoystickManager.GetJoystick(_joystickIndex);
    var audio = GetComponent<AudioClipPlayer>();

    isApply = false;
    isCancel = false;

    // UI を初期化する
    var state = _joystickIndex.GetState();
    _ui.ChangePlayerState(state.isPlayer);
    _ui.SwitchState(isApply);

    while (isActiveAndEnabled)
    {
      yield return null;

      if (isStop) { continue; }

      // キャンセルボタンの判定を取得
      isCancel = joystick.IsPush(Joystick.AxisType.Cancel);

      // 決定ボタンが押されていれば、状態切り替えをスキップ
      if (isApply)
      {
        // キャンセルボタンが押されたら、切り替え判定を再開
        if (isCancel)
        {
          isApply = false;
          isCancel = false;
          audio.Play(_seCancel);
          _ui.SwitchState(isApply);
        }
        continue;
      }

      // 決定ボタンが押されていない状態でキャンセルボタンが押されたらスキップ
      if (isCancel) { audio.Play(_seCancel); continue; }

      // 決定ボタン
      if (joystick.IsPush(Joystick.AxisType.Attack))
      {
        isApply = true;
        audio.Play(_seApply);
        _ui.SwitchState(isApply);
        continue;
      }

      // X ボタンが押されたらプレイヤーの状態を切り替える
      if (joystick.IsPush(Joystick.AxisType.Super))
      {
        state.isPlayer = !state.isPlayer;
        _ui.ChangePlayerState(state.isPlayer);

        audio.Play(_seSelect);
      }
    }
  }


  void OnValidate()
  {
    _ui.UpdateName(_joystickIndex);
  }
}
