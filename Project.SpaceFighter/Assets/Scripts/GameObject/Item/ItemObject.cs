
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// アイテムの状態を管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ItemObject : MonoBehaviour
{
  /// <summary> アイテム削除用コールバック </summary>
  public System.Action<ItemObject> callback { get; set; }

  void OnDestroy() { callback(this); }
}
