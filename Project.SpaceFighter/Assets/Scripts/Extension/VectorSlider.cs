
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// Vector2, 3, 4 の各ベクトル型をスライダーで調整できるようにする、
// エディター拡張です。
//
// 対応する各構造体に対して、VectorRange 属性を指定することで、
// インスペクター上から、スライダーによるパラメータ調整が可能になります。
//
// インスペクターによる表示が前提のため、必ず public なデータメンバか、
// SerializeField 属性をつけたデータメンバに対して指定します。
//
//------------------------------------------------------------

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class VectorRangeAttribute : PropertyAttribute
{
  public VectorRangeAttribute(float min, float max)
  {
    this.min = min;
    this.max = max;
  }

  public float min { get; private set; }
  public float max { get; private set; }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(VectorRangeAttribute))]
public class VectorSlider : PropertyDrawer
{
  VectorRangeAttribute customAttribute
  {
    get { return attribute as VectorRangeAttribute; }
  }

  public override float GetPropertyHeight(SerializedProperty property,
                                          GUIContent label)
  {
    float result = base.GetPropertyHeight(property, label);

    // ベクトル型の大きさに合わせてインスペクターのフィールドを広げる
    switch (property.propertyType)
    {
      default: break;
      case SerializedPropertyType.Vector2: result *= 2f; break;
      case SerializedPropertyType.Vector3: result *= 3f; break;
      case SerializedPropertyType.Vector4: result *= 4f; break;
    }

    return result;
  }


  string _propertyName = string.Empty;
  float _height = 0f;

  public override void OnGUI(Rect position,
                             SerializedProperty property,
                             GUIContent label)
  {
    // Vector 型以外なら何もしない
    if (IsOtherPropertyType(property))
    {
      EditorGUI.PropertyField(position, property, label);
      return;
    }

    // フィールドのパラメータを取得
    _propertyName = label.text;
    _height = base.GetPropertyHeight(property, label);

    // 各ベクトル型の要素数に合わせてスライダーを表示
    switch (property.propertyType)
    {
      case SerializedPropertyType.Vector2:
        var v2 = property.vector2Value;
        v2.x = Slider(ref position, _height * 0f, "X", property.vector2Value.x);
        v2.y = Slider(ref position, _height * 1f, "Y", property.vector2Value.y);
        property.vector2Value = v2;
        break;

      case SerializedPropertyType.Vector3:
        var v3 = property.vector3Value;
        v3.x = Slider(ref position, _height * 0f, "X", property.vector3Value.x);
        v3.y = Slider(ref position, _height * 1f, "Y", property.vector3Value.y);
        v3.z = Slider(ref position, _height * 2f, "Z", property.vector3Value.z);
        property.vector3Value = v3;
        break;

      case SerializedPropertyType.Vector4:
        var v4 = property.vector4Value;
        v4.x = Slider(ref position, _height * 0f, "X", property.vector4Value.x);
        v4.y = Slider(ref position, _height * 1f, "Y", property.vector4Value.y);
        v4.z = Slider(ref position, _height * 2f, "Z", property.vector4Value.z);
        v4.w = Slider(ref position, _height * 3f, "W", property.vector4Value.w);
        property.vector4Value = v4;
        break;

      default: break;
    }
  }

  // 型比較用
  static SerializedPropertyType[] vectorProperty = {
    SerializedPropertyType.Vector2,
    SerializedPropertyType.Vector3,
    SerializedPropertyType.Vector4,
  };

  // Vector 型以外なら true を返す
  bool IsOtherPropertyType(SerializedProperty property)
  {
    return vectorProperty.Any(type => type == property.propertyType);
  }

  // スライダーを表示、スライダーの値を返す
  float Slider(ref Rect position, float offsetY, string name, float value)
  {
    var rect = new Rect
    {
      x = position.x,
      y = position.y + offsetY,
      width = position.width,
      height = _height,
    };

    var min = customAttribute.min;
    var max = customAttribute.max;
    var label = string.Format("{0}: {1}", _propertyName, name);
    return EditorGUI.Slider(rect, label, value, min, max);
  }
}

#endif
