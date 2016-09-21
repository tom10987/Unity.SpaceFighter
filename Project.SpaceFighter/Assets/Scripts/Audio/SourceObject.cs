
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// このクラスをコンポーネントとして持つオブジェクトに割り当てられた
// AudioSource コンポーネントを一括で管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class SourceObject : MonoBehaviour
{
  public static Transform parent { get; private set; }
  static SourceObject() { parent = new GameObject("SourceObjects").transform; }

  /// <summary> 指定したオブジェクトの直下にインスタンスを生成する </summary>
  public static SourceObject Create(Transform target)
  {
    var source = new GameObject("SourceObject").AddComponent<SourceObject>();
    source.AddSource();
    source.transform.SetParent(target != null ? target : parent);
    return source;
  }

  // 生成されると自身で管理クラスへの登録、削除を行う
  void Awake() { SourceObjectManager.Add(this); }
  void OnDestroy() { SourceObjectManager.Remove(this); }


  /// <summary> 新しい <see cref="AudioSource"/> を追加する </summary>
  public AudioSource AddSource()
  {
    var source = gameObject.AddComponent<AudioSource>();
    source.playOnAwake = false;
    return source;
  }

  /// <summary> 全ての <see cref="AudioSource"/> を無条件に取得 </summary>
  public IEnumerable<AudioSource> GetSources()
  {
    var sources = GetComponents<AudioSource>();
    foreach (var source in sources) { yield return source; }
  }

  /// <summary> ループ設定の <see cref="AudioSource"/> を全て取得 </summary>
  public IEnumerable<AudioSource> GetLoopSources()
  {
    return GetSources().Where(s => s.loop);
  }

  /// <summary> 再生していない <see cref="AudioSource"/> を取得 </summary>
  public AudioSource GetSource()
  {
    return GetSources().FirstOrDefault(s => !s.isPlaying);
  }

  /// <summary> <see cref="AudioSource"/> の取得に成功したら true を返す </summary>
  public bool GetSource(out AudioSource source)
  {
    source = GetSource();
    return source != null;
  }


  /// <summary> ループ再生中ではない、１つでも再生中の
  /// <see cref="AudioSource"/> があれば true を返す </summary>
  public bool IsPlaying()
  {
    return GetSources().Any(s => !s.loop && s.isPlaying);
  }

  /// <summary> ループ中も含めて、１つでも再生中の
  /// <see cref="AudioSource"/> があれば true を返す </summary>
  public bool IsPlayingWithLoop()
  {
    return GetSources().Any(s => s.isPlaying);
  }

  /// <summary> <see cref="AudioSource"/> が存在すれば true を返す </summary>
  public bool ExistSource() { return GetSources().Any(); }

  /// <summary> 再生中でない <see cref="AudioSource"/> があれば true </summary>
  public bool ExistStopSource() { return GetSources().Any(s => !s.isPlaying); }

  /// <summary> ループ設定の <see cref="AudioSource"/> があれば true </summary>
  public bool ExistLoopSource() { return GetSources().Any(s => s.loop); }


  /// <summary> 再生中でない <see cref="AudioSource"/> を全て削除 </summary>
  public void Refresh()
  {
    var sources = GetSources().Where(s => !s.isPlaying);
    foreach (var source in sources) { Destroy(source); }
  }

  /// <summary> <see cref="AudioSource"/> を全て削除 </summary>
  public void Release()
  {
    AllStop();
    foreach (var source in GetSources()) { Destroy(source); }
  }


  /// <summary> <see cref="AudioClip"/> が登録された
  /// <see cref="AudioSource"/> を全て同時に停止する </summary>
  public void AllStop()
  {
    var sources = GetSources().Where(s => s.clip != null);
    foreach (var source in sources) { source.Stop(); }
  }
}
