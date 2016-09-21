
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// UI のボタンに波紋を発生させます。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class ButtonRippleEffect : MonoBehaviour
{
  [Header("エフェクト対象の画像")]
  [SerializeField]
  Image _sprite = null;

  [Header("ボタンのエフェクトの大きさ")]
  [SerializeField, Range(1f, 2f)]
  float _size = 1.5f;

  [Header("エフェクトの速さ")]
  [SerializeField, Range(0.1f, 2.0f)]
  float _velocity = 1f;


  Vector3 _scale = Vector3.zero;

  void Awake() { _scale = _sprite.rectTransform.localScale; }

  void OnEnable() { StartCoroutine(EffectLoop()); }

  IEnumerator EffectLoop()
  {
    var color = _sprite.color;

    float deltaTime = 0f;

    while (isActiveAndEnabled)
    {
      yield return null;

      // 時間経過でサイズを大きくする
      deltaTime = Mathf.Repeat(deltaTime + Time.deltaTime * _velocity, 1f);
      _sprite.rectTransform.localScale = _scale * (1f + deltaTime * (_size - 1f));

      // サイズが大きくなるほど透明度を高くする
      color.a = 1f - deltaTime;
      _sprite.color = color;
    }
  }
}
