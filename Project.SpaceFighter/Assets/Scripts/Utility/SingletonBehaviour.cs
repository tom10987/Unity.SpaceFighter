
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// 継承したクラスをシングルトン化します。
//
// ヒエラルキー上に同じコンポーネントを持つオブジェクトが
// 複数存在しないように自動的に排除します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public abstract class SingletonBehaviour<T> : MonoBehaviour
  where T : SingletonBehaviour<T>
{
  static T _instance = null;
  public static T instance { get { return _instance; } }

  [Header("シーン切り替え時に削除されないようにする")]
  [SerializeField]
  bool _dontDestroyOnLoad = false;

  protected virtual void Awake()
  {
    if (_dontDestroyOnLoad) { DontDestroyOnLoad(transform.root.gameObject); }
    if (!IsSingleInstance()) { Destroy(transform.root.gameObject); }
  }

  protected virtual void OnDestroy() { _instance = null; }

  /// <summary> インスタンスが生成済みなら true を返す </summary>
  protected bool IsSingleInstance()
  {
    if (_instance == null) { _instance = this as T; }
    return (_instance == this);
  }
}
