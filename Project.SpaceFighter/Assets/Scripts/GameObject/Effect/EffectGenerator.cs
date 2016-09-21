
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// 指定した SE、エフェクトの再生が可能です。
//
//------------------------------------------------------------
// NOTE:
// 基本的に、音が鳴るのはエフェクト発生時のため、
// エフェクトのオブジェクトに音を管理させたほうが効率的だと判断しました。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioClipPlayer))]
public class EffectGenerator : MonoBehaviour
{
  [Header("再生したい SE")]
  [SerializeField]
  ClipIndex _se = null;

  protected new AudioClipPlayer audio { get; private set; }

  /// <summary> 登録済みの SE を再生する </summary>
  public void PlayClip() { audio.Play(_se); }


  [Header("発生させるエフェクトのプレハブ")]
  [SerializeField]
  EffectAutoDestroy _effect;

  /// <summary> 指定した座標にエフェクトを発生させる </summary>
  public ParticleSystem CreateEffect(Vector3 position)
  {
    var effect = Instantiate(_effect);
    effect.transform.position = position;
    return effect.GetComponent<ParticleSystem>();
  }

  /// <summary> 自身の座標にエフェクトを発生させる </summary>
  public ParticleSystem CreateEffect()
  {
    return CreateEffect(transform.position);
  }


  // 継承して利用する場合でも動作するようにしておく
  protected virtual void Awake()
  {
    audio = GetComponent<AudioClipPlayer>();
  }
}
