
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class InputAxisAsset
{
  static InputAxisAsset()
  {
    _axes = new string[]
    {
      "P1_LHorizontal",
      "P1_LVertical",
      "P1_RHorizontal",
      "P1_RVertical",
      "P1_Trigger",
      "P1_TriggerL",
      "P1_TriggerR",
      "P1_Apply",
      "P1_Cancel",
      "P1_Attack",
      "P1_Super",
      "P1_Pause",

      "P2_LHorizontal",
      "P2_LVertical",
      "P2_RHorizontal",
      "P2_RVertical",
      "P2_Trigger",
      "P2_TriggerL",
      "P2_TriggerR",
      "P2_Apply",
      "P2_Cancel",
      "P2_Attack",
      "P2_Super",
      "P2_Pause",

      "P3_LHorizontal",
      "P3_LVertical",
      "P3_RHorizontal",
      "P3_RVertical",
      "P3_Trigger",
      "P3_TriggerL",
      "P3_TriggerR",
      "P3_Apply",
      "P3_Cancel",
      "P3_Attack",
      "P3_Super",
      "P3_Pause",

      "P4_LHorizontal",
      "P4_LVertical",
      "P4_RHorizontal",
      "P4_RVertical",
      "P4_Trigger",
      "P4_TriggerL",
      "P4_TriggerR",
      "P4_Apply",
      "P4_Cancel",
      "P4_Attack",
      "P4_Super",
      "P4_Pause",
    };
  }

  static readonly string[] _axes = null;

  /// <summary> 指定したコントローラー番号に対応する文字列を取得 </summary>
  public static IEnumerable<string> GetAxes(JoystickIndex index)
  {
    return _axes.Where(axis => Regex.IsMatch(axis, index.ToString()));
  }
}
