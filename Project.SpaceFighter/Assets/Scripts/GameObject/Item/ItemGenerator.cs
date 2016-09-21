
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// アイテムをランダムに生成します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ItemGenerator : MonoBehaviour
{
  [Header("アイテムのプレハブ")]
  [SerializeField]
  ItemObject _item = null;

  // 生成したアイテムを保持
  readonly List<ItemObject> _items = new List<ItemObject>();

  // アイテムをリストから削除する
  void CallBackDestroy(ItemObject item) { _items.Remove(item); }


  [Header("アイテムを出現させる座標")]
  [SerializeField]
  Transform[] _positions = null;


  [Header("アイテムを出現させる間隔（単位：秒）")]
  [SerializeField, Range(0.5f, 10f)]
  float _interval = 5f;

  [Header("アイテム出現範囲の広さ")]
  [SerializeField, Range(1f, 10f)]
  float _radius = 1f;

  [Header("シーンに生成されるアイテムの最大数")]
  [SerializeField, Range(1, 10)]
  int _itemCountMax = 5;


  IEnumerator Start()
  {
    while (isActiveAndEnabled)
    {
      yield return new WaitForSeconds(_interval);

      // アイテムが一定数以上、フィールドにある場合は処理をスキップ
      if (_items.Count >= _itemCountMax) { continue; }

      var item = Instantiate(_item);

      // 生成したアイテムの位置を登録済みの出現位置からランダムに選択
      var index = Random.Range(0, _positions.Length - 1);
      var position = _positions[index].position;
      item.transform.position = position + Random.insideUnitSphere * _radius;
      item.callback = CallBackDestroy;

      _items.Add(item);
    }
  }
}
