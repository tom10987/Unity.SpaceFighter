
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// UI の要素を回転させる演出用のスクリプトです。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIObjectRotation : MonoBehaviour
{
  [Header("アニメーション角度")]
  [SerializeField, Range(0f, 90f)]
  float _angle = 15f;

  [Header("アニメーション速度（単位：x 倍速）")]
  [SerializeField, Range(1f, 5f)]
  float _velocity = 3f;

  static readonly float _twoPI = Mathf.PI * 2f;


  void OnEnable() { StartCoroutine(RotationLoop()); }

  IEnumerator RotationLoop()
  {
    var rectTransform = GetComponent<RectTransform>();

    float time = 0f;

    while (isActiveAndEnabled)
    {
      time = Mathf.Repeat(time + Time.deltaTime * _velocity, _twoPI);

      var angle = Mathf.Sin(time) * _angle;
      rectTransform.localRotation = Quaternion.Euler(Vector3.forward * angle);

      yield return null;
    }
  }
}
