
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// ゲーム内の状態、データを各シーン間で共有、管理します。
//
// シーンをまたいで必要になるデータを全て扱います。
//
//------------------------------------------------------------

public static class ActorStateManager
{
  static ActorStateManager()
  {
    _actors = new Dictionary<JoystickIndex, ActorState>
    {
      { JoystickIndex.P1, new ActorState() },
      { JoystickIndex.P2, new ActorState() },
      { JoystickIndex.P3, new ActorState() },
      { JoystickIndex.P4, new ActorState() },
    };
  }

  static readonly Dictionary<JoystickIndex, ActorState> _actors = null;


  /// <summary> 指定したプレイヤーの状態を取得 </summary>
  public static ActorState GetState(this JoystickIndex index)
  {
    return _actors[index];
  }

  /// <summary> 状態を初期化する </summary>
  public static void Clear()
  {
    foreach (var actor in _actors) { actor.Value.score = 0; }
  }
}
