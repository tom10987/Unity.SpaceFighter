
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// マシンの型式を表すパラメータです。
//
// 特殊攻撃ボタンを押した時の効果をコメントとして記入しています。
//
//------------------------------------------------------------

public enum MachineType
{
  /// <summary> X: 広範囲にショット攻撃 </summary>
  Balance,
  /// <summary> X: 瞬間的に超加速 </summary>
  Speed,
  /// <summary> X: 高威力・極大ショット攻撃 </summary>
  Power,
  /// <summary> X: 高威力・広範囲のボム攻撃 </summary>
  Bommer,
  /// <summary> マシン選択シーン用、ランダム </summary>
  Random,
}

public static class MachineTypeTable
{
  public class TypeText
  {
    public string name { get; private set; }
    public string text { get; private set; }

    public TypeText(string name, string text)
    {
      this.name = name;
      this.text = text;
    }
  }

  static MachineTypeTable()
  {
    _table = new Dictionary<MachineType, TypeText>
    {
      { MachineType.Balance, new TypeText("バランス型", "広範囲にショット攻撃") },
      { MachineType.Speed,   new TypeText("スピード型", "瞬間的に超加速") },
      { MachineType.Power,   new TypeText("パワー型", "高威力・極大ショット攻撃") },
      { MachineType.Bommer,  new TypeText("ボマー型", "高威力・広範囲ボム攻撃") },
      { MachineType.Random,  new TypeText("ランダム", "???") },
    };
  }

  static readonly Dictionary<MachineType, TypeText> _table = null;

  /// <summary> マシンタイプの情報を取得 </summary>
  public static TypeText GetText(this MachineType type)
  {
    return _table[type];
  }
}
