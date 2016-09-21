
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// テキストの文字列を点滅させます。
//
// コンポーネントとして登録するだけで動作します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(Text))]
public class TextBlink : MonoBehaviour
{
  [Header("点滅速度")]
  [SerializeField, Range(0.1f, 5f)]
  float _velocity = 1f;

  [Header("文字列の色")]
  [SerializeField]
  Color _color = Color.white;

  [Header("半透明を使って滑らかに変化させるか")]
  [SerializeField]
  bool _alphaEnabled = false;


  IEnumerator Start()
  {
    var text = GetComponent<Text>();

    float time = 0f;

    while (isActiveAndEnabled)
    {
      time = Mathf.Repeat(time + Time.deltaTime * _velocity, Mathf.PI);

      // 半透明を許可しない設定なら値を整数に丸める
      var sin = Mathf.Sin(time);
      if (!_alphaEnabled) { sin = Mathf.Round(sin); }

      var color = text.color;
      color.a = sin;
      text.color = color;

      yield return null;
    }
  }

  void OnValidate()
  {
    var text = GetComponent<Text>();
    text.color = _color;
  }
}
