
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// オブジェクトを 90° 単位でランダムに回転させます。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class StageRotation : MonoBehaviour
{
  void Awake()
  {
    var random = Random.Range(0, 200) % 4;
    transform.localRotation = Quaternion.Euler(Vector3.up * 90f * random);
  }
}
