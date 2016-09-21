
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// コンポーネントのついたオブジェクトを
// 設定された値をもとに、ある程度ランダムな大きさにスケーリングします。
//
//------------------------------------------------------------

public class ObjectScale : MonoBehaviour
{
  [Header("オブジェクトサイズの最低値")]
  [SerializeField, Range(1, 20)]
  int _minScale = 5;

  [Header("最低値に上乗せされる乱数の変化量")]
  [SerializeField, Range(1, 20)]
  int _random = 10;

  void Awake()
  {
    var scale = Random.Range(_minScale, _minScale + _random);
    transform.localScale = Vector3.one * scale;
  }
}
