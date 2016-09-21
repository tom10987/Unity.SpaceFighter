
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// トリガーに触れたプレイヤーを追尾します。
//
// トリガーは子オブジェクトとして配置する前提になっています。
// 親オブジェクトの位置を移動させます。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class ItemChaser : MonoBehaviour
{
  Transform _target = null;

  float _deltaTime = 0f;


  void OnTriggerEnter(Collider other)
  {
    // 一度ターゲットを認識したら以降の処理をスキップ
    if (_target) { return; }

    var actor = other.GetComponent<ActorController>();
    if (!actor) { return; }

    _target = actor.transform;
  }

  void FixedUpdate()
  {
    if (!_target) { return; }

    _deltaTime = Mathf.Clamp01(_deltaTime + Time.deltaTime);

    var parent = transform.parent;
    var distance = Vector3.Lerp(parent.position, _target.position, _deltaTime);
    parent.position = distance;
  }
}
