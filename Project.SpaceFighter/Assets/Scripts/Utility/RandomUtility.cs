
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// 乱数関係の雑多な処理をまとめています。
//
//------------------------------------------------------------

public static class RandomUtility
{
  /// <summary> 指定した値を最大値とする 0 から始まる乱数を生成する </summary>
  public static int GetIndex(int max) { return Random.Range(0, max); }

  /// <summary> XZ 平面に平行なランダムな方向ベクトルを取得 </summary>
  public static Vector3 GetUnitVectorXZ()
  {
    var vector = Random.insideUnitSphere;
    vector.y = 0f;
    return vector;
  }

  /// <summary> <see cref="Prefab"/> をランダムに１つ選択、インスタンス化する </summary>
  public static GameObject GetElement(this Prefab[] prefabs)
  {
    int index = GetIndex(prefabs.Length - 1);
    return prefabs[index].Instantiate();
  }
}
