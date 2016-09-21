
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// アイテム獲得のトリガー判定です。
//
//------------------------------------------------------------

public class ItemTrigger : EffectGenerator
{
  [Header("親オブジェクト")]
  [SerializeField]
  Transform _parent = null;

  [Header("獲得スコア")]
  [SerializeField, Range(1, 100)]
  int _score = 10;

  [Header("耐久値の回復量")]
  [SerializeField, Range(1, 10)]
  int _recover = 2;


  void OnTriggerEnter(Collider other)
  {
    var actor = other.GetComponent<ActorController>();
    if (!actor) { return; }

    // スコアを加算して UI を更新する
    actor.actorObject.score += _score;
    actor.UpdateScore();

    // 耐久値を回復
    if (!actor.actorObject.isFine)
    {
      actor.actorObject.endurance += _recover;
      actor.UpdateGauge();
    }

    PlayClip();

    Destroy(_parent.gameObject);
  }
}
