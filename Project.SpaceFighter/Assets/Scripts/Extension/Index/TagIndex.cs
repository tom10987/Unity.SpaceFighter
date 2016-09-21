
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ユーザー定義のタグ名も含めて、
// 全てのタグ名をリストにして表示する、エディター拡張付きクラスです。
//
//------------------------------------------------------------

[System.Serializable]
public class TagIndex
{
  public sealed class TagNameAttribute : IndexAttribute
  {
    public TagNameAttribute() : base() { }
  }

  [SerializeField, TagName]
  string _name = string.Empty;

  /// <summary> インスペクターで指定されたタグ名を取得 </summary>
  public string name { get { return _name; } }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(TagIndex.TagNameAttribute))]
public class TagNameDrawer : IndexDrawer
{
  protected override string[] GetNames() { return InternalEditorUtility.tags; }
}

[CustomPropertyDrawer(typeof(TagIndex))]
public class TagIndexDrawer : IndexPropertyDrawer { }

#endif
