
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// キャラクターの物理挙動の計算に関する処理をまとめています。
//
// 振る舞い自体はプレイヤーも AI も同じ挙動を取りますが、
// 進行方向の入力手段が異なるため、計算式のみ外部に実装しました。
//
//------------------------------------------------------------

public static class ActorPhysics
{
  /// <summary> 現在の向きと進行方向から移動量の大きさを求める </summary>
  /// <param name="direction"> 進行方向 </param>
  public static float GetMagnitude(this Transform transform, Vector3 direction)
  {
    // 現在の向きと進行方向の内積を取り、その結果を移動量の大きさとする
    // 現在の向きに対して背面方向だった場合、後退してしまうため、０未満の値を無視
    var dot = Vector3.Dot(transform.forward, direction);
    return Mathf.Clamp01(dot);
  }

  /// <summary> 現在の向きと進行方向から移動量を求める </summary>
  /// <param name="direction"></param>
  public static Vector3 GetForce(this Transform transform, Vector3 direction)
  {
    var force = transform.GetMagnitude(direction);
    return direction * force;
  }

  /// <summary> 現在の向きと進行方向から回転量の大きさを求める </summary>
  /// <param name="direction"> 進行方向 </param>
  public static float GetAngle(this Transform transform, Vector3 direction)
  {
    // 現在の向きに対する右方向と進行方向の内積を取り、結果をそのまま回転量にする
    // 内積の値が０＝進行方向と同じまたは逆の方向
    var dotR = Vector3.Dot(transform.right, direction);

    // 右方向との内積が０かつ現在の向きに対して真逆の方向を向いていた場合、
    // 自機が動かなくなるため、わずかに回転量を加える
    var dotF = transform.GetMagnitude(direction);
    if (dotR == 0f && dotF == 0f) { dotR = 0.01f; }

    return dotR;
  }
}
