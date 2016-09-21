
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ショットの追尾判定を行うトリガーの処理です。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class ShotTrigger : MonoBehaviour
{
  [Header("親オブジェクトのコンポーネント")]
  [SerializeField]
  ShotObject _parent = null;

  // 親オブジェクトの座標を取り出しやすくする
  Transform parent { get { return _parent.transform; } }


  [Header("追尾性能")]
  [SerializeField, Range(0, 100)]
  int _angle = 15;
  int angle { get { return _angle * 10; } }

  [Header("追尾を無視するしきい値")]
  [SerializeField, Range(0f, 0.5f)]
  float _ignoreAngle = 0.1f;

  [Header("追尾範囲")]
  [SerializeField, Range(1, 50)]
  int _range = 10;


  void OnValidate()
  {
    var scale = Vector3.one - Vector3.up;
    transform.localScale = scale * _range + Vector3.up;
  }


  Transform _target = null;

  void OnTriggerStay(Collider other)
  {
    // プレイヤー以外には反応しない
    var actor = other.GetComponent<ActorObject>();
    if (!actor) { return; }

    // 発射したプレイヤーなら無視する
    var index = _parent.shooter.actorObject.joystickIndex;
    if (actor.joystickIndex == index) { return; }

    // ターゲットが未登録ならそのまま登録、
    // すでにターゲットを発見しているなら、より近いほうをターゲットに登録する
    _target = _target == null ? actor.transform :
      IsNearThanTarget(actor.transform) ? actor.transform : _target;

    // トリガー内にいる別プレイヤーへの方向を取得
    var direction = _target.position - transform.position;
    direction.Normalize();

    // 内積の結果を回転量とする
    var dot = Vector3.Dot(transform.right, direction);
    if (Mathf.Abs(dot) < _ignoreAngle) { return; }

    parent.Rotate(Vector3.up, dot * angle * Time.deltaTime);
  }

  // より近いオブジェクトに向かう
  bool IsNearThanTarget(Transform actor)
  {
    var older = _target.position - transform.position;
    var newer = actor.position - transform.position;

    return older.magnitude > newer.magnitude;
  }
}
