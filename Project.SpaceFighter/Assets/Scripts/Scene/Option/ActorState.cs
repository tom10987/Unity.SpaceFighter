
//------------------------------------------------------------
// TIPS:
// マシン選択シーンでのプレイヤーの状態、
// および、選択されたマシンの状態を保持します。
//
//------------------------------------------------------------

public class ActorState
{
  public ActorState()
  {
    isPlayer = true;
    machineType = MachineType.Balance;
    score = 0;
  }

  /// <summary> プレイヤーの状態 (playable or AI) </summary>
  public bool isPlayer { get; set; }

  /// <summary> 選択されたマシンの種類 </summary>
  public MachineType machineType { get; set; }

  /// <summary> プレイヤーのスコア </summary>
  public int score { get; set; }
}
