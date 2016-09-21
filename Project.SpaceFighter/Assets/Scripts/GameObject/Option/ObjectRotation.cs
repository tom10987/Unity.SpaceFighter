
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// コンポーネントのついたオブジェクトをランダムな方向に回転させます。
//
//------------------------------------------------------------

public class ObjectRotation : MonoBehaviour
{
  [Header("回転速度")]
  [SerializeField, Range(1, 10)]
  int _velocity = 5;


  IEnumerator Start()
  {
    var direction = Random.onUnitSphere;
    var random = (Random.value + 1f) * 10f;

    while (isActiveAndEnabled)
    {
      var velocity = _velocity * Time.deltaTime;
      transform.Rotate(direction, velocity * random);
      yield return null;
    }
  }
}
