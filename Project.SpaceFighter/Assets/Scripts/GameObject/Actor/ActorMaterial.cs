
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// プレイヤーが操作する自機オブジェクトの色を変更します。
//
// モデルデータによって、
// 割り当てられているマテリアルのインデックスが変わるため、
// インデックスの違いを吸収するために作成しました。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer))]
public class ActorMaterial : MonoBehaviour
{
  [SerializeField]
  MeshRenderer _renderer = null;

  [Header("変更したいマテリアルのインデックス")]
  [SerializeField, Range(0, 4)]
  int _index = 0;


  /// <summary> プレイヤー番号に対応する色で機体の色を変える </summary>
  public void SetColor(JoystickIndex playerIndex)
  {
    var material = _renderer.materials[_index];
    material.color = playerIndex.GetPlayerColor();
  }
}
