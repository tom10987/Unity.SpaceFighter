
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ターゲットに指定したオブジェクトを追尾します。
//
// Camera コンポーネントを持つオブジェクトは、
// このクラスをコンポーネントとして持つ、
// 空のオブジェクトの子オブジェクトとして配置してください。
//
//------------------------------------------------------------
// NOTE:
// Camera コンポーネントのついたオブジェクトは、
// 空オブジェクトの子として配置する前提になっています。
//
// 親オブジェクトの座標系をターゲットの座標系に合わせることで、
// 子オブジェクト（Camera 本体）のローカル座標系を
// そのままオフセットとして利用します。
//
// そのため、Camera 本体に対する座標系の操作は一切行いません。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ActorCamera : MonoBehaviour
{
  [Header("カメラ本体のコンポーネント")]
  [SerializeField]
  Camera _camera = null;

  /// <summary> カメラのオブジェクトレイヤーを取得 </summary>
  public int cameraLayer { get { return _camera.gameObject.layer; } }


  [Header("カメラの追尾速度")]
  [SerializeField, Range(1, 5)]
  int _velocity = 3;

  [Header("カメラの回転速度")]
  [SerializeField, Range(1, 10)]
  int _torque = 5;

  [Header("カメラ注視点の高さ")]
  [SerializeField, Range(0f, 5f)]
  float _height = 2f;


  /// <summary> カメラパラメータの初期化 </summary>
  public void Setup(JoystickIndex index)
  {
    // ビューポート設定
    var viewportPosition = index.GetViewportPosition();
    _camera.rect = new Rect(viewportPosition, ActorDataTable.viewportScale);
  }


  /// <summary> ターゲットの位置に合わせて、自動でカメラ位置を更新する </summary>
  public void UpdateRotation(Transform target, float deltaTime)
  {
    // オブジェクトの向きをターゲットの向きに補間で近づける
    var torque = deltaTime * _torque;
    var quaternion = Quaternion.Lerp(transform.rotation, target.rotation, torque);
    transform.rotation = quaternion;
  }


  /// <summary> カメラをターゲットに近づける </summary>
  public void Translate(Transform target, float deltaTime)
  {
    var velocity = deltaTime * _velocity;
    var distance = Vector3.Lerp(transform.position, target.position, velocity);
    transform.position = distance;
  }

  /// <summary> ターゲットを画面中央にとらえるように角度を補正する </summary>
  public void LookAt(Transform target)
  {
    _camera.transform.LookAt(target.position + Vector3.up * _height);
  }
}
