
#if UNITY_EDITOR

using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// 仮想軸で使用する対応キーから、文字列を出力します。
//
//------------------------------------------------------------
// NOTE:
// InputManager.asset は ProjectSettings -> Input からも編集できますが、
// 文字列で入力する必要があり、不便だったため、
// リストから選択できるようにしたいと考えました。
//
// enum で一覧を作成するのが一番簡単ですが、
// そのままでは空白文字を使えないため、
// enum をキーとした、文字列表を作成して対応することにしました。
//
//------------------------------------------------------------

public static class VirtualAxisUtility
{
  static readonly Dictionary<VirtualAxis.KeyCode, string> _table = null;

  static VirtualAxisUtility()
  {
    _table = new Dictionary<VirtualAxis.KeyCode, string>()
    {
      { VirtualAxis.KeyCode.None, string.Empty },

      { VirtualAxis.KeyCode.A, "a" },
      { VirtualAxis.KeyCode.B, "b" },
      { VirtualAxis.KeyCode.C, "c" },
      { VirtualAxis.KeyCode.D, "d" },
      { VirtualAxis.KeyCode.E, "e" },
      { VirtualAxis.KeyCode.F, "f" },
      { VirtualAxis.KeyCode.G, "g" },
      { VirtualAxis.KeyCode.H, "h" },
      { VirtualAxis.KeyCode.I, "i" },
      { VirtualAxis.KeyCode.J, "j" },
      { VirtualAxis.KeyCode.K, "k" },
      { VirtualAxis.KeyCode.L, "l" },
      { VirtualAxis.KeyCode.M, "m" },
      { VirtualAxis.KeyCode.N, "n" },
      { VirtualAxis.KeyCode.O, "o" },
      { VirtualAxis.KeyCode.P, "p" },
      { VirtualAxis.KeyCode.Q, "q" },
      { VirtualAxis.KeyCode.R, "r" },
      { VirtualAxis.KeyCode.S, "s" },
      { VirtualAxis.KeyCode.T, "t" },
      { VirtualAxis.KeyCode.U, "u" },
      { VirtualAxis.KeyCode.V, "v" },
      { VirtualAxis.KeyCode.W, "w" },
      { VirtualAxis.KeyCode.X, "x" },
      { VirtualAxis.KeyCode.Y, "y" },
      { VirtualAxis.KeyCode.Z, "z" },

      { VirtualAxis.KeyCode.Num0, "0" },
      { VirtualAxis.KeyCode.Num1, "1" },
      { VirtualAxis.KeyCode.Num2, "2" },
      { VirtualAxis.KeyCode.Num3, "3" },
      { VirtualAxis.KeyCode.Num4, "4" },
      { VirtualAxis.KeyCode.Num5, "5" },
      { VirtualAxis.KeyCode.Num6, "6" },
      { VirtualAxis.KeyCode.Num7, "7" },
      { VirtualAxis.KeyCode.Num8, "8" },
      { VirtualAxis.KeyCode.Num9, "9" },

      { VirtualAxis.KeyCode.Up, "up" },
      { VirtualAxis.KeyCode.Down, "down" },
      { VirtualAxis.KeyCode.Left, "left" },
      { VirtualAxis.KeyCode.Right, "right" },

      { VirtualAxis.KeyCode.Space, "space" },
      { VirtualAxis.KeyCode.Enter, "return" },
      { VirtualAxis.KeyCode.Escape, "escape" },

      { VirtualAxis.KeyCode.JoystickButton0, "joystick button 0" },
      { VirtualAxis.KeyCode.JoystickButton1, "joystick button 1" },
      { VirtualAxis.KeyCode.JoystickButton2, "joystick button 2" },
      { VirtualAxis.KeyCode.JoystickButton3, "joystick button 3" },
      { VirtualAxis.KeyCode.JoystickButton4, "joystick button 4" },
      { VirtualAxis.KeyCode.JoystickButton5, "joystick button 5" },
      { VirtualAxis.KeyCode.JoystickButton6, "joystick button 6" },
      { VirtualAxis.KeyCode.JoystickButton7, "joystick button 7" },
      { VirtualAxis.KeyCode.JoystickButton8, "joystick button 8" },
      { VirtualAxis.KeyCode.JoystickButton9, "joystick button 9" },
    };
  }

  /// <summary> InputManager.asset に登録できる文字列に置換する </summary>
  public static string GetCode(this VirtualAxis.KeyCode button)
  {
    return _table[button];
  }
}

#endif
