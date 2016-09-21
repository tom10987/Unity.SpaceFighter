
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// カメラの機能を集約、一括で管理します。
//
// 注視対象となるオブジェクトの参照を持つ以外は、
// 一部コンポーネントのラッパーとして機能します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class CameraObject : MonoBehaviour
{
  [Header("オブジェクトを表示するカメラ")]
  [SerializeField]
  ActorCamera _camera = null;

  [Header("UI を表示するカメラ")]
  [SerializeField]
  UICamera _uiCamera = null;

  [Header("カメラに表示される UI のコンポーネント")]
  [SerializeField]
  ActorUI _actorUI = null;


  /// <summary> 機体の耐久値が危険域なら true を返す </summary>
  public bool isDangerous { get { return _actorUI.isDangerous; } }

  /// <summary> カメラの注視対象 </summary>
  public Transform target { get; private set; }

  /// <summary> ターゲットが登録済みなら true を返す </summary>
  public bool existTarget { get { return target != null; } }


  /// <summary> カメラの初期化を行う </summary>
  public void Setup(Transform target, JoystickIndex playerNumber)
  {
    this.target = target;
    _camera.Setup(playerNumber);
    _uiCamera.Setup(playerNumber);
    _actorUI.Setup(playerNumber);
  }


  /// <summary> オブジェクトの耐久値を UI に反映する </summary>
  public void UpdateGauge(float value)
  {
    _actorUI.UpdateGauge(value);
  }

  /// <summary> オブジェクトのスコアを UI に反映する </summary>
  public void UpdateScore(int value)
  {
    _actorUI.UpdateScore(value);
  }


  /// <summary> ターゲットの向きに合わせて、カメラの向きを補正する </summary>
  public void UpdateRotation(float deltaTime)
  {
    if (!existTarget) { return; }
    _camera.UpdateRotation(target, deltaTime);
  }


  void LateUpdate()
  {
    // ターゲットが登録されてなければ何もしない
    if (!existTarget) { return; }

    var deltaTime = Time.deltaTime;

    // カメラの位置のみ更新、
    // 本体はターゲットを画面中央にとらえるように補正
    _camera.Translate(target, deltaTime);
    _camera.LookAt(target);
  }
}
