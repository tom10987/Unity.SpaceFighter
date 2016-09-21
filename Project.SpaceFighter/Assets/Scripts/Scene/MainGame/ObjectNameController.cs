
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ObjectNameText をまとめて管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ObjectNameController : MonoBehaviour
{
  [SerializeField]
  ObjectNameText[] _names = null;


  /// <summary> 管理しているオブジェクト全てを変更する </summary>
  public void SetPlayer(JoystickIndex index)
  {
    foreach (var name in _names) { name.SetPlayer(index); }
  }

  /// <summary> 指定したインデックスのオブジェクトを取得 </summary>
  public ObjectNameText GetObject(int index) { return _names[index]; }
}
