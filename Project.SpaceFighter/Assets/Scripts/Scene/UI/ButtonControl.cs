
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// コントローラー操作による UI への入力を管理します。
//
// ボタンを押した場合のイベント処理は、
// あらかじめボタン側に指定しておく必要があります。
//
//------------------------------------------------------------
// NOTE:
// EventSystem (Standalone Input Module) による操作が不便だったため、
// 自作したコントローラー入力で操作可能な仕組みとして実装しました。
//
// Standalone Input Module による入力の場合、
// 設定できる入力軸の種類に制限がかかることが不満点でした。
//
// そのため、複数のコントローラー、複数の入力軸、
// コントローラー操作の対象にするボタンを設定できるようにしました。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ButtonControl : MonoBehaviour
{
  [Header("入力を受け付けるコントローラー")]
  [SerializeField]
  JoystickIndex[] _joysticks = { JoystickIndex.P1, };

  [Header("決定ボタンとして機能する入力軸")]
  [SerializeField]
  Joystick.AxisType[] _axes = { Joystick.AxisType.Apply, };


  enum Direction
  {
    Horizontal,
    Vertical,
    VerticalInvert,
  }

  [Header("操作を受け付ける入力の方向")]
  [SerializeField]
  Direction _direction = Direction.Horizontal;

  bool isHorizontal { get { return _direction == Direction.Horizontal; } }

  [Header("コントローラー操作の対象にするボタン")]
  [SerializeField]
  Button[] _buttons = null;

  [Header("選択中のボタンサイズ")]
  [SerializeField, Range(1f, 3f)]
  float _buttonScale = 1f;

  [Header("選択中のボタンカラー")]
  [SerializeField]
  Color _selected = Color.yellow;

  [Header("非選択時のボタンカラー")]
  [SerializeField]
  Color _normal = Color.white;


  /// <summary> コントローラー操作の状態 </summary>
  public bool isStop { get; set; }

  /// <summary> いずれかのコントローラーが決定ボタンを押したら true を返す </summary>
  public bool isPushApplyButton { get; private set; }

  /// <summary> 現在選択中のボタン番号 </summary>
  public int buttonIndex { get; private set; }


  IEnumerator Start()
  {
    // 入力を受け付けるコントローラー情報をまとめて取得
    var joysticks = _joysticks.Select(i => JoystickManager.GetJoystick(i));

    // 最初のボタンのサイズを大きくしておく
    buttonIndex = 0;
    ChangeButtonState(buttonIndex, _buttonScale, _selected);

    // 直前フレームの入力を記憶
    int previousInput = 0;

    while (isActiveAndEnabled)
    {
      yield return null;

      // 決定ボタンの状態を取得
      isPushApplyButton = IsPushApplyButton(joysticks);

      // 待機状態ならコントローラー入力を受け付けない
      if (isStop) { continue; }

      // 決定ボタンが押されたとき、イベント登録されたボタンなら実行
      if (isPushApplyButton && ExistCallBackEvents())
      {
        _buttons[buttonIndex].onClick.Invoke();
      }

      // いずれかのコントローラーからの入力を受け付けたら実行
      var currentInput = GetAxisToInt(joysticks);
      if (currentInput != 0 && previousInput == 0)
      {
        // 移動前のインデックスにあるボタンの状態を戻す
        ChangeButtonState(buttonIndex, 1f, _normal);

        // インデックスを移動、ボタンの状態を更新する
        buttonIndex = MoveIndex(currentInput);
        ChangeButtonState(buttonIndex, _buttonScale, _selected);
      }

      previousInput = currentInput;
    }
  }

  // 指定した番号のボタンの状態を変更する
  void ChangeButtonState(int index, float scale, Color color)
  {
    var button = _buttons[index];
    button.transform.localScale = Vector3.one * scale;
    button.image.color = color;
  }

  // インデックスをボタンの配列内に収まるように補正する
  int MoveIndex(int nextIndex)
  {
    return Mathf.Clamp(buttonIndex + nextIndex, 0, _buttons.Length - 1);
  }


  static readonly Joystick.AxisType _horizontal = Joystick.AxisType.LHorizontal;
  static readonly Joystick.AxisType _vertical = Joystick.AxisType.LVertical;

  // インスペクターで指定された方向の入力を取得
  int GetAxisToInt(IEnumerable<Joystick> joysticks)
  {
    var axis = isHorizontal ? _horizontal : _vertical;
    return joysticks.Sum(joystick => joystick.GetAxisToInt(axis) * GetSign());
  }

  // 縦方向入力を反転する設定なら -1 を返す
  int GetSign()
  {
    return _direction == Direction.VerticalInvert ? -1 : 1;
  }


  // 選択中のボタンにイベント処理が登録されていれば true を返す
  bool ExistCallBackEvents()
  {
    return _buttons[buttonIndex].onClick != null;
  }

  // いずれかのコントローラーが決定ボタンを押したら true を返す
  bool IsPushApplyButton(IEnumerable<Joystick> joysticks)
  {
    return joysticks.Any(joystick => _axes.Any(axis => joystick.IsPush(axis)));
  }
}
