
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// シーン遷移の演出を行います。
//
// インスタンスはシーン切り替えの際に破棄されます。
// シーン開始時は暗転した状態で開始するようにしてください。
//
// あくまで演出の管理のみを行います。
// コールバックなどのイベントを行うことはできません。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class SceneSequencer : MonoBehaviour
{
  static SceneSequencer _instance = null;
  public static SceneSequencer instance { get { return _instance; } }
  public static bool existObject { get { return _instance != null; } }


  [Header("シーン遷移演出に使用する UI 要素")]
  [SerializeField]
  Image _image = null;

  [SerializeField]
  Color _color = Color.black;

  [SerializeField, Range(0f, 1f)]
  float _alpha = 0f;

  [Header("シーン開始時のフェードアウト速度（単位：秒）")]
  [SerializeField, Range(0.1f, 3f)]
  float _startSpeed = 1f;


  /// <summary> 遷移実行中なら true を返す </summary>
  public bool isPlaying { get; private set; }


  /// <summary> シーン遷移のフェードイン開始（単位：秒）<para>
  /// 最小値は 0.1f </para></summary>
  public void FadeInStart(float time)
  {
    // 極端に小さい値を丸める
    var sequenceTime = time > 0.1f ? time : 0.1f;
    StartCoroutine(FadeIn(sequenceTime));
  }

  /// <summary> シーン遷移のフェードアウト開始（単位：秒）<para>
  /// 最小値は 0.1f </para></summary>
  public void FadeOutStart(float time)
  {
    // 極端に小さい値を丸める
    var sequenceTime = time > 0.1f ? time : 0.1f;
    StartCoroutine(FadeOut(sequenceTime));
  }


  // UI のフェードイン
  // alpha: 0 => 1
  IEnumerator FadeIn(float time)
  {
    isPlaying = true;

    while (_alpha < 1f)
    {
      _alpha += Time.deltaTime / time;
      OnValidate();
      yield return null;
    }

    isPlaying = false;
  }

  // UI のフェードアウト
  // alpha: 1 => 0
  IEnumerator FadeOut(float time)
  {
    isPlaying = true;

    while (_alpha > 0f)
    {
      _alpha -= Time.deltaTime / time;
      OnValidate();
      yield return null;
    }

    isPlaying = false;
  }


  void Awake() { _instance = this; }

  IEnumerator Start()
  {
    yield return StartCoroutine(FadeOut(_startSpeed));
  }

  void OnValidate()
  {
    var color = _color;
    color.a = _alpha;
    _image.color = color;
  }
}
