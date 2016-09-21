
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// プレイヤーのモデルを表示します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ModelGenerator : MonoBehaviour
{
  [Header("表示位置の基準になる子オブジェクト")]
  [SerializeField]
  Transform[] _positions = null;

  [Header("表示するモデルのプレハブ")]
  [SerializeField]
  EventActor[] _models = null;

  [Header("モデルの角度")]
  [SerializeField, Range(30f, 60f)]
  float _angle = 45f;


  /// <summary> 指定した位置に指定したモデルを表示する </summary>
  public void GenerateModel(JoystickIndex playerNumber, int index)
  {
    var model = Instantiate(_models[(int)playerNumber - 1]);

    model.transform.SetParent(_positions[index]);
    model.transform.localPosition = Vector3.zero;
    model.transform.localRotation = Quaternion.Euler(Vector3.up * _angle);

    model.SetColor(playerNumber);
  }
}
