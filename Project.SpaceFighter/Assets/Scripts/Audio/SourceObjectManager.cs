
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// SourceObject の管理を行います。
//
//------------------------------------------------------------

public static class SourceObjectManager
{
  static readonly List<SourceObject> _sources = null;
  static SourceObjectManager() { _sources = new List<SourceObject>(); }

  /// <summary> <see cref="SourceObject"/> を登録 </summary>
  public static void Add(SourceObject src) { _sources.Add(src); }

  /// <summary> <see cref="SourceObject"/> を削除 </summary>
  public static bool Remove(SourceObject src) { return _sources.Remove(src); }


  /// <summary> <see cref="SourceObject"/> を全て削除 </summary>
  public static void Clear()
  {
    // SourceObject が削除されるときにリストから外れるようになっている
    foreach (var source in _sources) { Object.Destroy(source.gameObject); }
  }


  /// <summary> 未使用の <see cref="SourceObject"/> を取得 </summary>
  public static SourceObject GetFreeObject()
  {
    return _sources.FirstOrDefault(s => !s.IsPlaying());
  }
  /// <summary> 未使用の <see cref="SourceObject"/> があれば true を返す </summary>
  public static bool ExistsFreeObject()
  {
    return _sources.Any(s => !s.IsPlaying());
  }
  /// <summary> 未使用の <see cref="SourceObject"/> を全て削除する </summary>
  public static void DestroyFreeObjects()
  {
    var freeObjects = _sources.Where(s => !s.IsPlaying());
    foreach (var source in freeObjects) { Object.Destroy(source.gameObject); }
  }
}
