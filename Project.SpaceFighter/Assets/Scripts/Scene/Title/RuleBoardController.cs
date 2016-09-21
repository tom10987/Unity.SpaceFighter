
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// ゲームルールを表示する UI オブジェクトの操作を行います。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class RuleBoardController : MonoBehaviour
{
  [SerializeField]
  CanvasGroup _group = null;

  [Header("UI オブジェクトの始点と終点")]
  [SerializeField]
  Vector3 _start = Vector3.zero;

  [SerializeField]
  Vector3 _finish = Vector3.zero;

  [SerializeField]
  bool _debug = false;


  /// <summary> 演出中なら true を返す </summary>
  public bool isPlaying { get; private set; }

  /// <summary> UI オブジェクトが表示中なら true を返す </summary>
  public bool isActive { get; private set; }


  /// <summary> UI 要素の移動開始 </summary>
  public void StartAnimation()
  {
    StartCoroutine(StartAnimationCoroutine());
  }

  IEnumerator StartAnimationCoroutine()
  {
    isActive = true;
    isPlaying = true;

    while (_group.alpha < 1f)
    {
      var deltaTime = Time.deltaTime;

      _group.alpha += deltaTime;

      var position = _group.transform.localPosition;
      var distance = Vector3.Lerp(position, _finish, _group.alpha);
      _group.transform.localPosition = distance;

      yield return null;
    }

    isPlaying = false;
  }


  /// <summary> 始点に戻るアニメーションを開始 </summary>
  public void ReturnAnimation()
  {
    StartCoroutine(ReturnAnimationCoroutine());
  }

  IEnumerator ReturnAnimationCoroutine()
  {
    isPlaying = true;

    while (_group.alpha > 0f)
    {
      var deltaTime = Time.deltaTime;

      _group.alpha -= deltaTime;

      var position = _group.transform.localPosition;
      var distance = Vector3.Lerp(position, _start, 1f - _group.alpha);
      _group.transform.localPosition = distance;

      yield return null;
    }

    isActive = false;
    isPlaying = false;
  }


  void OnValidate()
  {
    _group.transform.localPosition = _debug ? _finish : _start;
    _group.alpha = 1f;
  }

  void Awake()
  {
    _debug = false;
    _group.transform.localPosition = _start;
    _group.alpha = 0f;
  }
}
