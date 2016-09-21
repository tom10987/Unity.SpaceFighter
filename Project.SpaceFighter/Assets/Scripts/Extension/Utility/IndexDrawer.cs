
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// 特定の要素の集まりを取得、
// ドロップダウンリストにして表示できるようにするエディター拡張です。
//
// 派生クラスごとにインデックスとして使用する配列の取得処理を実装します。
//
// IndexPropertyDrawer クラスは、基本的に継承させるだけで動作します。
// ただし、データメンバを必ず string 型の _name にする必要があります。
//
//------------------------------------------------------------

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public abstract class IndexAttribute : PropertyAttribute
{
  public IndexAttribute() { selected = 0; }
  public int selected { get; set; }
}

#if UNITY_EDITOR

public abstract class IndexDrawer : PropertyDrawer
{
  protected IndexAttribute customAttribute
  {
    get { return attribute as IndexAttribute; }
  }

  public sealed override void OnGUI(Rect position,
                                    SerializedProperty property,
                                    GUIContent label)
  {
    var names = GetNames();
    if (!names.Any()) { property.EmptyField(ref position); return; }

    var name = property.stringValue;
    var isEmpty = string.IsNullOrEmpty(name);
    customAttribute.selected = isEmpty ? 0 : names.GetIndex(name);

    var result = customAttribute.PopupField(ref position, label.text, names);
    result = Mathf.Clamp(result, 0, names.Length - 1);
    customAttribute.selected = result;
    property.stringValue = names[result];
  }

  protected abstract string[] GetNames();
}

public abstract class IndexPropertyDrawer : PropertyDrawer
{
  public sealed override void OnGUI(Rect position,
                                    SerializedProperty property,
                                    GUIContent label)
  {
    property.DrawField(ref position, ref label, "_name");
  }
}

#endif
