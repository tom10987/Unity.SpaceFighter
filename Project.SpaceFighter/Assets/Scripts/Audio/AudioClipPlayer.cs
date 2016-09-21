
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// 音声ファイルの再生、停止に対応しています。
//
// インスペクターで設定された状態に合わせて、
// オブジェクトのコンポーネントを操作します。
//
// このコンポーネントは、
// 管理下にあるオブジェクトのコンポーネントを操作します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class AudioClipPlayer : MonoBehaviour
{
  public enum PlayMode
  {
    /// <summary> <see cref="SourceObject"/> に対して何もしない </summary>
    Free,
    /// <summary> <see cref="SourceObject"/> を直下に置く <para></para>
    /// それ以外は特に何もしない </summary>
    Bind,
    /// <summary> 再生できる <see cref="AudioSource"/> がなければ
    /// 自動的に追加する <para></para>
    /// <see cref="SourceObject"/> は直下に置かない </summary>
    Additive,
    /// <summary> <see cref="SourceObject"/> を直下に置き、
    /// 再生できる <see cref="AudioSource"/> がなければ追加する </summary>
    Domination,
  }

  [Header("SourceObject の管理方法")]
  [SerializeField]
  PlayMode _managerMode = PlayMode.Free;

  // インスペクターから設定された管理方法の状態を判断する
  bool isBind { get { return ((int)_managerMode % 2) > 0; } }
  bool isAdditive { get { return _managerMode > PlayMode.Bind; } }


  [Header("再生の終了した AudioSource を破棄する")]
  [SerializeField]
  bool _autoRelease = false;

  // 自動解放コルーチンのインスタンス管理
  Coroutine _release = null;

  /// <summary> 再生の終了した <see cref="AudioSource"/> の管理方法を指定 </summary>
  public void AutoRelease(bool state)
  {
    _autoRelease = state;
    if (_release == null) { _release = StartCoroutine(Refresh()); }
  }

  // AudioSource の自動解放コルーチン
  IEnumerator Refresh()
  {
    while (_autoRelease)
    {
      if (source.ExistStopSource()) { source.Refresh(); }
      yield return null;
    }
    _release = null;
  }


  /// <summary> リンクされた <see cref="SourceObject"/> の情報を返す </summary>
  public SourceObject source { get; private set; }

  /// <summary> <see cref="SourceObject"/> を割り当てる <para></para>
  /// 割り当て済みなら、既存のインスタンスを破棄して新しく割り当てなおす </summary>
  public void Bind()
  {
    if (source) { Unbind(); }

    // 未使用の SourceObject があれば再利用する
    var existsFreeObject = SourceObjectManager.ExistsFreeObject();
    source = existsFreeObject ?
      SourceObjectManager.GetFreeObject() :
      SourceObject.Create(isBind ? transform : null);

    // 未使用のものを割り当てられたとき、管理下に置く設定なら移動させる
    if (existsFreeObject && isBind) { source.transform.SetParent(transform); }
  }

  /// <summary> 割り当て済みの <see cref="SourceObject"/> を解放する </summary>
  public void Unbind()
  {
    if (source) { Destroy(source.gameObject); }
    source = null;
  }


  public AudioSource BgmPlay(ClipIndex index, float volume, bool isLoop)
  {
    // まだ割り当てられていなければ割り当てる
    if (!source) { Bind(); }

    // AudioSource の取得を試みる
    AudioSource freeSource = null;
    var result = source.GetSource(out freeSource);
    if (!result && isAdditive) { freeSource = source.AddSource(); }

    // 取得できなければ何もしない
    if (!freeSource) { return null; }

    freeSource.clip = index.GetClip();
    freeSource.volume = volume;
    freeSource.loop = isLoop;
    freeSource.Play();

    // 自身の状態を利用して解放処理を開始する
    AutoRelease(_autoRelease);

    return freeSource;
  }


  /// <summary> 指定した <see cref="ClipIndex"/> に対応した
  /// <see cref="AudioClip"/> を使って再生する </summary>
  /// <param name="volume"> 音量（0.0 ~ 1.0） </param>
  /// <param name="isLoop"> true = ループ再生を許可 </param>
  public void Play(ClipIndex index, float volume, bool isLoop)
  {
    // まだ割り当てられていなければ割り当てる
    if (!source) { Bind(); }

    // AudioSource の取得を試みる
    AudioSource freeSource = null;
    var result = source.GetSource(out freeSource);
    if (!result && isAdditive) { freeSource = source.AddSource(); }

    // 取得できなければ何もしない
    if (!freeSource) { return; }

    freeSource.clip = index.GetClip();
    freeSource.volume = volume;
    freeSource.loop = isLoop;
    freeSource.Play();

    // 自身の状態を利用して解放処理を開始する
    AutoRelease(_autoRelease);
  }

  /// <summary> 指定した <see cref="ClipIndex"/> に対応した
  /// <see cref="AudioClip"/> を使って再生する
  /// <para> ループ再生はしない </para></summary>
  /// <param name="volume"> 音量（0.0 ~ 1.0） </param>
  public void Play(ClipIndex index, float volume) { Play(index, volume, false); }

  /// <summary> 指定した <see cref="ClipIndex"/> に対応した
  /// <see cref="AudioClip"/> を使って再生する
  /// <para> 音量は最大設定にする </para></summary>
  /// <param name="isLoop"> true = ループ再生を許可 </param>
  public void Play(ClipIndex index, bool isLoop) { Play(index, 1f, isLoop); }

  /// <summary> 指定した <see cref="ClipIndex"/> に対応した
  /// <see cref="AudioClip"/> を使って再生する
  /// <para> 音量は最大、ループ再生はしない </para></summary>
  public void Play(ClipIndex index) { Play(index, 1f, false); }


  /// <summary> 再生中の <see cref="AudioSource"/> を全て停止する </summary>
  public void Stop() { source.AllStop(); }
}
