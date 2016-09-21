
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//------------------------------------------------------------
// TIPS:
// ビルド対象に登録されたシーンの一覧を使って、
// インスペクターにリスト表示するエディター拡張付きクラスです。
//
// シーン切り替え機能も実装しているため、
// 利用したいタイミングで呼び出すだけでシーン切り替えができます。
//
//------------------------------------------------------------

[System.Serializable]
public class SceneIndex
{
  public sealed class SceneNameAttribute : IndexAttribute
  {
    public SceneNameAttribute(bool enable = true) : base()
    {
      enableOnly = enable;
    }

    public bool enableOnly { get; private set; }
  }

  [SerializeField, SceneName]
  string _name = string.Empty;

  /// <summary> シーン名を取得 </summary>
  public string sceneName { get { return _name; } }

  /// <summary> シーンを上書きして切り替える </summary>
  public void LoadScene()
  {
    SceneManager.LoadScene(_name);
  }

  /// <summary> シーンを追加する </summary>
  public void AddScene()
  {
    SceneManager.LoadScene(_name, LoadSceneMode.Additive);
  }

  /// <summary> シーンを削除、成功したら true を返す </summary>
  public bool UnloadScene()
  {
    return SceneManager.UnloadScene(_name);
  }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(SceneIndex.SceneNameAttribute))]
public class SceneNameDrawer : IndexDrawer
{
  new SceneIndex.SceneNameAttribute customAttribute
  {
    get { return base.customAttribute as SceneIndex.SceneNameAttribute; }
  }

  // ビルド対象シーンの名前を一覧として取得
  protected override string[] GetNames()
  {
    var scenes = customAttribute.enableOnly ?
      EditorBuildSettings.scenes.Where(scene => scene.enabled) :
      EditorBuildSettings.scenes;

    // ファイルの絶対パスとして取得するので、シーン名のみ抽出する
    return RegexSceneNames(scenes.Select(scene => scene.path));
  }

  string[] RegexSceneNames(IEnumerable<string> paths)
  {
    var filePaths = paths.Select(p => Regex.Split(p, "/").Last());
    return filePaths.Select(path => Regex.Replace(path, ".unity", "")).ToArray();
  }
}

[CustomPropertyDrawer(typeof(SceneIndex))]
public class SceneIndexDrawer : IndexPropertyDrawer { }

#endif
