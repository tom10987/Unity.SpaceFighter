
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ショットの移動を管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ShotObject : MonoBehaviour
{
  [SerializeField]
  Rigidbody _rigidbody = null;

  [Header("ショットの速度")]
  [SerializeField, Range(1, 20)]
  int _velocity = 5;

  public int velocity { get { return _velocity * 10; } }

  /// <summary> 発射直後の初速を与える </summary>
  public void StartForce(int forceMagnitude)
  {
    var force = transform.forward * forceMagnitude;
    _rigidbody.AddForce(force, ForceMode.VelocityChange);
  }


  [Header("このショットを使って相手プレイヤーを倒したときの獲得スコア")]
  [SerializeField, Range(1, 100)]
  int _score = 50;

  public int score { get { return _score; } }


  [SerializeField]
  Renderer _renderer = null;

  /// <summary> ショットを発射したプレイヤーの番号 </summary>
  public ActorController shooter { get; private set; }

  /// <summary> 指定したプレイヤー番号に対応する色に変更 </summary>
  public void SetColor(ActorController actor)
  {
    shooter = actor;

    foreach (var material in _renderer.materials)
    {
      material.color = actor.actorObject.joystickIndex.GetPlayerColor();
    }
  }


  /// <summary> 削除イベントのコールバック登録 </summary>
  public System.Action<ShotObject> callback { get; set; }


  void FixedUpdate()
  {
    var force = transform.forward * velocity * Time.deltaTime;
    _rigidbody.AddForce(force, ForceMode.VelocityChange);
  }

  void OnDestroy() { callback(this); }
}
