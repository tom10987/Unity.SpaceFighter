
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ショット命中時のダメージ計算を行います。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ShotCollider : EffectGenerator
{
  [Header("ショットの攻撃力")]
  [SerializeField, Range(1, 10)]
  int _attack = 2;

  [Header("ショット攻撃によって吹き飛ばされる強さと制限値")]
  [SerializeField, Range(1, 10)]
  int _force = 1;

  [SerializeField, Range(1, 100)]
  int _limit = 80;


  void Start() { PlayClip(); }

  void OnCollisionEnter(Collision other)
  {
    var shotObject = GetComponent<ShotObject>();
    var index = shotObject.shooter.actorObject.joystickIndex;

    // プレイヤーに命中していたら耐久値を減らす
    var actor = other.gameObject.GetComponent<ActorController>();
    if (actor)
    {
      // 発射したプレイヤー自身なら処理を中断
      if (actor.actorObject.joystickIndex == index) { return; }

      // 耐久値を減少させる
      // 無敵状態の判定は内部で実行している
      actor.UpdateDamage(_attack);

      // 撃破したらプレイヤーのスコアを大きく加算する
      // 撃破された側のプレイヤースコアは、復活処理で半減している
      if (actor.actorObject.isDead)
      {
        index.GetState().score += shotObject.score;
        shotObject.shooter.UpdateScore();
      }
    }

    // 対象が動くオブジェクトなら吹き飛ばしてエフェクト発生
    if (other.rigidbody)
    {
      other.rigidbody.AddForce(-other.impulse * _force, ForceMode.Impulse);

      // 速度制限をかける
      var magnitude = other.rigidbody.velocity.magnitude;
      if (magnitude > _limit)
      {
        other.rigidbody.velocity.Normalize();
        other.rigidbody.velocity *= _limit;
      }

      var effect = CreateEffect();
      effect.startColor = index.GetPlayerColor();
    }

    Destroy(gameObject);
  }
}
