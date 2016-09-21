
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// エディター拡張用の雑多な処理をまとめています。
//
//------------------------------------------------------------

#if UNITY_EDITOR

public static class GUIOption
{
  /// <summary> 空であることを示すフィールドを表示 </summary>
  public static void EmptyField(this SerializedProperty property,
                                ref Rect position)
  {
    var label = ObjectNames.NicifyVariableName(property.name);
    EditorGUI.LabelField(position, label, "this property is empty");
  }

  /// <summary> 指定したパスに存在するリソース名を一覧で取得 </summary>
  public static IEnumerable<string> GetAssetNames<T>(string path)
    where T : Object
  {
    var assets = Resources.LoadAll<T>(path);
    return assets.Select(asset => asset.name);
  }

  /// <summary> 指定した名前の一覧から、特定の名前が入っているインデックスを返す </summary>
  public static int GetIndex(this IEnumerable<string> names, string element)
  {
    int i = 0;
    foreach (var name in names)
    {
      if (name == element) { break; }
      ++i;
    }
    return i %= names.Count();
  }

  /// <summary> 名前の一覧をドロップダウンリストとして表示する </summary>
  public static int PopupField(this IndexAttribute attribute,
                               ref Rect position,
                               string label,
                               string[] names)
  {
    var selected = attribute.selected;
    var indices = names.Select((name, index) => index).ToArray();
    return EditorGUI.IntPopup(position, label, selected, names, indices);
  }

  /// <summary> 指定した要素を指定した名前に置き換えて表示する </summary>
  public static void DrawField(this SerializedProperty property,
                               ref Rect position,
                               ref GUIContent label,
                               string propertyName)
  {
    var field = property.FindPropertyRelative(propertyName);
    EditorGUI.PropertyField(position, field, label);
  }
}

#endif
