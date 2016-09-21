
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// インスペクターから登録できるオブジェクトをプレハブに限定する、
// エディター拡張付きクラスです。
//
// Prefab 型のデータメンバを宣言、
// public か、SerializeField 属性をつければ機能します。
//
// フィールドには GameObject 型として登録されるため、
// コンポーネントを利用する場合のために、専用のメソッドを用意しています。
//
//------------------------------------------------------------

[System.Serializable]
public class Prefab
{
  [SerializeField]
  GameObject _prefab = null;

  /// <summary> プレハブをインスタンス化して取得 </summary>
  public GameObject Instantiate()
  {
    return Object.Instantiate(_prefab);
  }
  /// <summary> プレハブをインスタンス化、コンポーネントを取得 </summary>
  public T InstantiateTo<T>() where T : Component
  {
    return Instantiate().GetComponent<T>();
  }

  public static implicit operator bool(Prefab exist) { return exist._prefab; }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(Prefab))]
public class PrefabDrawer : PropertyDrawer
{
  public override void OnGUI(Rect position,
                             SerializedProperty property,
                             GUIContent label)
  {
    // コンポーネントのメンバを取得
    var component = property.FindPropertyRelative("_prefab");

    // エディターの変更を監視、変更されてなければ何もしない
    EditorGUI.BeginChangeCheck();
    var field = GUIField(ref position, ref label, component) as GameObject;
    if (!EditorGUI.EndChangeCheck() || !field) { return; }

    // プレハブがフィールドに格納された場合のみ、要素として許可する
    if (field.IsPrefab()) { component.objectReferenceValue = field; }
  }

  // 参照データを使って入力フィールドを作成、そのまま GameObject 型に変換する
  Object GUIField(ref Rect position,
                  ref GUIContent label,
                  SerializedProperty component)
  {
    var value = component.objectReferenceValue;
    return EditorGUI.ObjectField(position, label, value, typeof(GameObject), false);
  }
}

static class GameObjectUtility
{
  public static bool IsPrefab(this GameObject gameObject)
  {
    return PrefabUtility.GetPrefabType(gameObject) == PrefabType.Prefab;
  }
}

#endif
