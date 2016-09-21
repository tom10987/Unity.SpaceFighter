
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// 入力軸の設定を行う、エディター拡張用のクラスです。
//
// インスペクターなど、エディター上でパラメータを表示させる前提のため、
// ローカル変数としても宣言は可能ですが、機能しません。
//
//------------------------------------------------------------

[System.Serializable]
public class VirtualAxis
{
#if UNITY_EDITOR
  // SerializedProperty のインスタンスが外部にあるため、
  // インスタンスを持つ側から呼び出してパラメータを入力しています。
  public void Register(ref SerializedProperty axis)
  {
    axis.FindPropertyRelative("m_Name").stringValue = _axisName;
    axis.FindPropertyRelative("descriptiveName").stringValue = string.Empty;
    axis.FindPropertyRelative("descriptiveNegativeName").stringValue = string.Empty;
    axis.FindPropertyRelative("negativeButton").stringValue = _negativeButton.GetCode();
    axis.FindPropertyRelative("positiveButton").stringValue = _positiveButton.GetCode();
    axis.FindPropertyRelative("altNegativeButton").stringValue = _altNegativeButton.GetCode();
    axis.FindPropertyRelative("altPositiveButton").stringValue = _altPositiveButton.GetCode();
    axis.FindPropertyRelative("gravity").floatValue = _gravity;
    axis.FindPropertyRelative("dead").floatValue = _dead;
    axis.FindPropertyRelative("sensitivity").floatValue = _sensitivity;
    axis.FindPropertyRelative("snap").boolValue = false;
    axis.FindPropertyRelative("invert").boolValue = _invert;
    axis.FindPropertyRelative("type").intValue = (int)_type;
    axis.FindPropertyRelative("axis").intValue = (int)_axis;
    axis.FindPropertyRelative("joyNum").intValue = (int)_joystick;
  }
#endif


  [Header("仮想軸の名前")]
  [SerializeField]
  string _axisName = string.Empty;

  /// <summary> 仮想の入力軸名を取得 </summary>
  public string axisName { get { return _axisName; } }


  // UnityEngine.KeyCode とは無関係です。
  // InputManager.asset の入力軸に登録できる、対応キーのインデックスです。
  // デバッグ向けに、キーボード用のインデックスを含みます。
  // 詳細は VirtualAxisUtility クラスを確認してください。
  public enum KeyCode
  {
    None,

    A, B, C, D, E, F, G,
    H, I, J, K, L, M, N,
    O, P, Q, R, S, T, U,
    V, W, X, Y, Z,

    Num0,
    Num1,
    Num2,
    Num3,
    Num4,
    Num5,
    Num6,
    Num7,
    Num8,
    Num9,

    Up,
    Down,
    Left,
    Right,

    Space,
    Enter,
    Escape,

    JoystickButton0,
    JoystickButton1,
    JoystickButton2,
    JoystickButton3,
    JoystickButton4,
    JoystickButton5,
    JoystickButton6,
    JoystickButton7,
    JoystickButton8,
    JoystickButton9,
  }

  [Header("0 <= 1 の値を返す、入力として受け付けるキー")]
  [SerializeField]
  KeyCode _positiveButton = KeyCode.None;
  [SerializeField]
  KeyCode _altPositiveButton = KeyCode.None;

  [Header("-1 <= 0 の値を返す、入力として受け付けるキー")]
  [SerializeField]
  KeyCode _negativeButton = KeyCode.None;
  [SerializeField]
  KeyCode _altNegativeButton = KeyCode.None;


  [Header("キーが離された状態で、0 に戻るまでの遷移時間の長さ")]
  [SerializeField, Range(0f, 10f)]
  float _gravity = 3f;

  [Header("入力として反応する、しきい値の下限")]
  [SerializeField, Range(0f, 1f)]
  float _dead = 0.15f;

  [Header("キーが入力された状態で、最大値になるまでの遷移時間の長さ")]
  [SerializeField, Range(0f, 10f)]
  float _sensitivity = 3f;

  [Header("キーの値を反転させるかどうかを指定")]
  [SerializeField]
  bool _invert = false;


  // キーボード または コントローラーにのみ対応しています。
  // Type.JoystickAxis は、コントローラーのアナログ入力を取得できます。
  enum Type
  {
    KeyOrMouseButton = 0,
    JoystickAxis = 2,
  }

  [Header("ボタン入力、アナログ入力のいずれかを指定")]
  [SerializeField]
  Type _type = Type.JoystickAxis;


  // Xbox360 コントローラーを使用する前提で作成しています。
  enum Axis
  {
    L_Horizontal = 0,
    L_Vertical = 1,
    R_Horizontal = 3,
    R_Vertical = 4,

    XboxDPadX = 5,
    XboxDPadY = 6,

    XboxTrigger = 2,
    XboxTriggerL = 8,
    XboxTriggerR = 9,
  }

  [Header("入力として受け付ける軸の種類")]
  [SerializeField]
  Axis _axis = Axis.L_Horizontal;


  // 接続されたコントローラーの番号に対応します。
  enum JoystickNumber
  {
    All,
    Player1,
    Player2,
    Player3,
    Player4,
  }

  [Header("どの番号のコントローラーに反応するかを指定")]
  [SerializeField]
  JoystickNumber _joystick = JoystickNumber.All;
}
