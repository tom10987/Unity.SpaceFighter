
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// エフェクトの再生が終了したら自発的にオブジェクトを削除します。
//
// エフェクトの再生状態で削除の判断をしているため、
// ループ設定になっているオブジェクトは削除されません。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioClipPlayer))]
public class EffectAutoDestroy : MonoBehaviour
{
  /// <summary> エフェクトのコンポーネントを取得 </summary>
  public ParticleSystem particle { get; private set; }

  /// <summary> SE 再生用 </summary>
  public new AudioClipPlayer audio { get; private set; }

  [Header("再生したい SE")]
  [SerializeField]
  ClipIndex _clip = null;

  [SerializeField]
  bool _onPlaySE = false;


  IEnumerator Start()
  {
    particle = GetComponent<ParticleSystem>();

    if (_onPlaySE)
    {
      audio = GetComponent<AudioClipPlayer>();
      audio.Play(_clip);
    }

    // エフェクト再生が完了するまで待機
    while (particle.isPlaying) { yield return null; }

    Destroy(gameObject);
  }
}
