
using UnityEngine;

public class ItemNameColor : MonoBehaviour
{
  [SerializeField]
  ObjectNameController _controller = null;

  static readonly JoystickIndex[] _indices = new JoystickIndex[]
  {
    JoystickIndex.P1,
    JoystickIndex.P2,
    JoystickIndex.P3,
    JoystickIndex.P4,
  };


  void OnValidate()
  {
    if (!_controller) { return; }

    foreach (var index in _indices)
    {
      var objectIndex = (int)index - 1;
      var name = _controller.GetObject(objectIndex);
      name.SetColor(index);
    }
  }
}
