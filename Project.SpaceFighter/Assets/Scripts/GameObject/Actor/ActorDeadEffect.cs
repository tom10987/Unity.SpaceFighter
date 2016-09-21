
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// プレイヤーが戦闘不能になったときのエフェクトを管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ActorDeadEffect : MonoBehaviour
{
  [Header("機体のレンダラーとアルファ値")]
  [SerializeField]
  MeshRenderer _renderer = null;

  [SerializeField, Range(0f, 1f)]
  float _alpha = 0.5f;


  [Header("発生させるエフェクトのプレハブ")]
  [SerializeField]
  EffectAutoDestroy _effect = null;

  [Header("エフェクトの再生時間と発生間隔（単位：秒）")]
  [SerializeField, Range(0.1f, 3f)]
  float _playTime = 2f;

  [SerializeField, Range(0.1f, 1f)]
  float _interval = 0.5f;

  [Header("無敵時間の長さ（単位：秒）")]
  [SerializeField, Range(1f, 5f)]
  float _invincibleTime = 3f;


  [Header("エフェクト発生の半径")]
  [SerializeField, Range(0.5f, 3f)]
  float _radius = 1.5f;
  Vector3 random { get { return Random.onUnitSphere * _radius; } }

  [Header("エフェクトのサイズ")]
  [SerializeField, Range(0.1f, 2f)]
  float _scale = 0.5f;


  /// <summary> エフェクト実行中なら true を返す </summary>
  public bool isPlaying { get; private set; }

  /// <summary> 無敵時間中なら true を返す </summary>
  public bool isInvincible { get; private set; }


  /// <summary> エフェクト実行開始 </summary>
  public void StartEffect(System.Action callback)
  {
    StartCoroutine(StartEffectCoroutine(callback));
  }

  IEnumerator StartEffectCoroutine(System.Action callback)
  {
    // 機体の透明度を変更する
    _renderer.materials.Execute(material => SetAlpha(material, _alpha));

    // エフェクトの経過時間
    float time = 0f;

    // エフェクト開始時の時間を記録
    float start = Time.time;

    // 機体の操作を停止
    isPlaying = true;
    isInvincible = true;

    while ((Time.time - start) < _playTime)
    {
      GenerateEffect(time);

      time += Time.deltaTime;
      if (time > _interval) { time = 0f; }

      yield return null;
    }

    // 機体の操作を再開
    isPlaying = false;

    // 無敵時間が終了したら無敵状態を解除
    yield return new WaitForSeconds(_invincibleTime);
    isInvincible = false;

    // 耐久ゲージの状態を UI に反映
    callback();

    // 機体を不透明に戻す
    _renderer.materials.Execute(material => SetAlpha(material, 1f));
  }

  // マテリアルのアルファ値を変更
  void SetAlpha(Material material, float alpha)
  {
    var color = material.color;
    color.a = alpha;
    material.color = color;
  }

  // 条件を満たしていればエフェクトを再生する
  void GenerateEffect(float time)
  {
    if (time > 0f) { return; }

    var effect = Instantiate(_effect);
    effect.transform.position = transform.position + random;
    effect.transform.localScale = Vector3.one * _scale;
  }
}
