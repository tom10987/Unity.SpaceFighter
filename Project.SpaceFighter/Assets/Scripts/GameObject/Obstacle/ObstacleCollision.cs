
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// 障害物オブジェクト用の衝突判定と反発させる処理です。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class ObstacleCollision : EffectGenerator
{
  [Header("衝突したオブジェクトに対する反発力と制限値")]
  [SerializeField, Range(1, 10)]
  int _force = 2;

  [SerializeField, Range(1, 100)]
  int _limit = 80;


  [Header("ショット攻撃判別用")]
  [SerializeField]
  TagIndex _tag = null;


  void OnCollisionEnter(Collision other)
  {
    if (!other.rigidbody) { return; }

    // 衝突してきたオブジェクトを押し返す
    other.rigidbody.AddForce(other.impulse * _force, ForceMode.Impulse);

    // 速度制限をかける
    var magnitude = other.rigidbody.velocity.magnitude;
    if (magnitude > _limit)
    {
      other.rigidbody.velocity.Normalize();
      other.rigidbody.velocity *= _limit;
    }

    // エフェクトを衝突した座標に発生させる
    // ただし、ショット攻撃のオブジェクトなら何もしない
    if (other.gameObject.tag == _tag.name)
    {
      var effect = CreateEffect();
      effect.transform.position = other.contacts[0].point;
    }

    PlayClip();
  }
}
