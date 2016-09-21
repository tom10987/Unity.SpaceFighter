
using UnityEngine;
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// UnityEngine に含まれる機能に関する雑多な処理をまとめています。
//
//------------------------------------------------------------

public static class UnityUtility
{
  /// <summary> 子要素を全て取得 </summary>
  public static IEnumerable<Transform> GetChildren(this Transform transform)
  {
    int count = transform.childCount;
    for (int i = 0; i < count; ++i) { yield return transform.GetChild(i); }
  }

  /// <summary> 他のオブジェクトとの距離を取得 </summary>
  public static Vector3 GetDistance(this Transform transform, Transform other)
  {
    return other.position - transform.position;
  }
}
