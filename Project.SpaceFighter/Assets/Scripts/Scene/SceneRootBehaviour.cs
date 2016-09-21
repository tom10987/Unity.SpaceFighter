
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// シーンの状態を管理するための基底クラスです。
// 最低限の共通要素のみまとめています。
//
// 各シーンごとにこのクラスを継承、MainLoop() にて処理を実装します。
//
// コルーチン開始前に初期化が必要になった場合に備えて、
// コルーチン開始のタイミングに Start() を利用しています。
//
// また、Start() 自体も virtual にしているため、
// 必要に応じて override もできるようにしています。
//
// メインループが終了したら自動的にシーンの切り替えを行います。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioClipPlayer))]
public abstract class SceneRootBehaviour : MonoBehaviour
{
  [Header("遷移先のシーン名")]
  [SerializeField]
  SceneIndex _scene = null;

  /// <summary> 切り替えるシーンを変更する </summary>
  protected void SetOtherScene(SceneIndex other) { _scene = other; }


  [Header("遷移演出の時間（単位：秒）")]
  [SerializeField, Range(0.1f, 3f)]
  float _sequenceTime = 1f;

  [Header("このシーンで使用する BGM 名")]
  [SerializeField]
  ClipIndex _bgm = null;

  [SerializeField, Range(0f, 1f)]
  float _bgmVolume = 0.75f;

  [SerializeField]
  bool _bgmLoop = true;


  /// <summary> <see cref="AudioClipPlayer"/> コンポーネントへアクセスする </summary>
  protected new AudioClipPlayer audio { get; private set; }

  /// <summary> BGM 再生に使用している <see cref="AudioSource"/> </summary>
  protected AudioSource bgmSource { get; private set; }

  /// <summary> シーン遷移直前に実行するイベント処理 </summary>
  protected System.Action callback { get; set; }

  /// <summary> シーン遷移の管理オブジェクトが存在すれば true を返す </summary>
  protected bool existSequencer { get { return SceneSequencer.existObject; } }


  protected virtual IEnumerator Start()
  {
    audio = GetComponent<AudioClipPlayer>();
    bgmSource = audio.BgmPlay(_bgm, _bgmVolume, _bgmLoop);

    yield return StartCoroutine(MainLoop());

    // callback?.Invoke() の記法は .NET 3.5 相当の C# では使えない
    if (callback != null) { callback(); }

    yield return StartCoroutine(ChangeScene());
  }

  protected abstract IEnumerator MainLoop();


  protected IEnumerator ChangeScene()
  {
    // シーンに演出オブジェクトがあれば演出を実行
    if (existSequencer)
    {
      SceneSequencer.instance.FadeInStart(_sequenceTime);
      yield return StartCoroutine(WaitSequence());
    }

    _scene.LoadScene();
  }

  /// <summary> シーン遷移の演出が終わるまで待機 </summary>
  protected IEnumerator WaitSequence()
  {
    while (SceneSequencer.instance.isPlaying) { yield return null; }
  }

  /// <summary> 指定したクリップを再生、その再生が終了するまでコルーチンを停止 </summary>
  protected IEnumerator WaitForEndOfSE(ClipIndex clip)
  {
    audio.Play(clip);
    while (audio.source.IsPlaying()) { yield return null; }
  }
}
