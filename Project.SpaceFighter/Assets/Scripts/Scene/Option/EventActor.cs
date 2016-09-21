
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ゲーム本編以外で使用するモデルのマテリアルを操作します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class EventActor : MonoBehaviour
{
  [SerializeField]
  ActorMaterial _renderer = null;

  /// <summary> プレイヤー番号に対応した色に変更する </summary>
  public void SetColor(JoystickIndex index)
  {
    _renderer.SetColor(index);
  }
}
