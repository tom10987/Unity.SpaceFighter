
using System;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// ゲームで必要とする分のコントローラー情報を持つ、
// コントローラーのインスタンスを管理します。
//
// 取得できる入力の種類など、
// コントローラー自体の仕様については Joystick クラスを確認してください。
//
// ゲーム起動中にコントローラーの順番を入れ替えたり、
// 新しいものに交換することを想定していません。
//
//------------------------------------------------------------

public static class JoystickManager
{
  static JoystickManager()
  {
    _joysticks = new Dictionary<JoystickIndex, Joystick>();

    // 列挙型の一覧を取得
    string[] names = Enum.GetNames(typeof(JoystickIndex));

    // 列挙型の名前から、それぞれの名前に一致する入力軸の文字列を登録する
    foreach (var name in names)
    {
      var index = (JoystickIndex)Enum.Parse(typeof(JoystickIndex), name);
      string[] axes = index.GetAxes();

      _joysticks.Add(index, new Joystick(axes));
    }
  }

  static readonly Dictionary<JoystickIndex, Joystick> _joysticks = null;

  /// <summary> 指定した番号のコントローラー情報を取得 </summary>
  public static Joystick GetJoystick(this JoystickIndex index)
  {
    return _joysticks[index];
  }

  // 指定したコントローラー番号から、その番号に一致する入力軸の一覧を取得
  static string[] GetAxes(this JoystickIndex index)
  {
    //var axes = InputManager.GetAxes((int)index);
    var axes = InputAxisAsset.GetAxes(index);
    return axes.ToArray();
  }
}
