
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// プレイヤーの UI を表示するカメラの設定を代行します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class UICamera : MonoBehaviour
{
  [SerializeField]
  Camera _camera = null;

  /// <summary> プレイヤー番号に対応するビューポートに設定する </summary>
  public void Setup(JoystickIndex index)
  {
    var viewportPosition = index.GetViewportPosition();
    _camera.rect = new Rect(viewportPosition, ActorDataTable.viewportScale);

    // 描画対象レイヤーの登録
    var layer = index.GetObjectLayer();
    _camera.cullingMask += 1 << layer;
    _camera.gameObject.layer = layer;
  }
}
