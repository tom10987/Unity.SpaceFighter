
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// 特定パスのアセットフォルダを監視、
// 音声ファイルをリストにして表示するエディター拡張付きクラスです。
//
//------------------------------------------------------------
// FIXME:
// リスト更新のタイミングがやや限定的なため、より安全な方法を検討中です。
//
// 深刻な問題として、ファイル数、及び全体のファイルサイズが増えるほど、
// 表示に時間がかかる状態になっています。
//
// 名前取得のためにリソース自体を読み込んでいるのが原因のため、
// 名前のみを取り出す方法を検討中です。
//
//------------------------------------------------------------

[System.Serializable]
public class ClipIndex
{
  public class ClipNameAttribute : IndexAttribute
  {
    public ClipNameAttribute() : base() { }
  }

  [SerializeField, ClipName]
  string _name = string.Empty;

  /// <summary> インスペクターから設定されたクリップ名を返す </summary>
  public string clipName { get { return _name; } }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ClipIndex.ClipNameAttribute))]
public class ClipNameDrawer : IndexDrawer
{
  static readonly string audio = "Audio";
  static string audioFolder { get { return DirectoryPath.resources + "/" + audio; } }

  [InitializeOnLoadMethod]
  static void CreateAssetDirectory()
  {
    // メソッド内部でフォルダの有無を確認している
    PathUtility.CreateDirectory(DirectoryPath.resources);
    PathUtility.CreateDirectory(audioFolder);
  }

  static HashSet<string> _clips = null;
  static ClipNameDrawer() { _clips = new HashSet<string>(); }

  [InitializeOnLoadMethod]
  static void LoadAssets()
  {
    var assets = GetAssetNames();
    _clips.UnionWith(assets);
    _clips.IntersectWith(assets);
  }

  static IEnumerable<string> GetAssetNames()
  {
    return Resources.LoadAll<AudioClip>(audio).Select(clip => clip.name);
  }

  protected override string[] GetNames() { return _clips.ToArray(); }
}

[CustomPropertyDrawer(typeof(ClipIndex))]
public class ClipIndexDrawer : IndexPropertyDrawer { }

#endif
