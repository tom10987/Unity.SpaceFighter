
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// シーンの背景を回転させる演出用のスクリプトです。
//
//------------------------------------------------------------

public class BackGroundRotation : MonoBehaviour
{
  [Header("回転速度")]
  [SerializeField, Range(-10f, 10f)]
  float _torque = 0f;

  void Update()
  {
    var angle = _torque * Time.deltaTime;
    transform.Rotate(Vector3.up, angle);
  }
}
