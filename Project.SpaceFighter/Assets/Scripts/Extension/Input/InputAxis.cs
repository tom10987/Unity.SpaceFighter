
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//------------------------------------------------------------
// TIPS:
//
//------------------------------------------------------------

[System.Serializable]
public class InputAxis
{
  public class InputAxisIndexAttribute : IndexAttribute
  {
    public InputAxisIndexAttribute() : base() { }
  }

  [SerializeField, InputAxisIndex]
  string _name = string.Empty;

  public string axisName { get { return _name; } }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(InputAxis.InputAxisIndexAttribute))]
public class InputAxisDrawer : IndexDrawer
{
  protected override string[] GetNames()
  {
    return InputManager.axisNames;
  }
}

[CustomPropertyDrawer(typeof(InputAxis))]
public class InputAxisPropertyDrawer : IndexPropertyDrawer { }

#endif
