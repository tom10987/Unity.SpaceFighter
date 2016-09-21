
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// マシン選択画面の選択アイコンの挙動を管理します。
//
// 振る舞いのみで、このコンポーネント自体は単独で動作しません。
// UI 要素の１つとして機能します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class SelectIcon : MonoBehaviour
{
  [Header("プレイヤー番号を表示するコンポーネント")]
  [SerializeField]
  TextMesh _mesh = null;

  [SerializeField]
  SpriteRenderer _circle = null;


  [Header("アイコンの移動速度")]
  [SerializeField, Range(1f, 10f)]
  float _velocity = 5f;


  /// <summary> コントローラー番号に対応する色で初期化する </summary>
  public void Setup(JoystickIndex index)
  {
    SetColor(index.GetPlayerColor());
    UpdateName(index, true);
  }

  /// <summary> UI を指定した色に設定する </summary>
  public void SetColor(Color color)
  {
    _mesh.color = color;
    _circle.color = color;
  }

  /// <summary> プレイヤーの状態に合わせて名前を更新する </summary>
  public void UpdateName(JoystickIndex index, bool isPlayable)
  {
    _mesh.text = isPlayable ? index.GetPlayerNumber() : "AI";
  }


  /// <summary> アイコンが動作中なら true を返す </summary>
  public bool isIconActive { get; private set; }


  /// <summary> 指定したオブジェクトの位置までアイコンを移動する <para>
  /// 動作中は <see cref="isIconActive"/> が true になる </para></summary>
  public void MoveToNext(Transform target)
  {
    StartCoroutine(MoveToNextCoroutine(target));
  }

  IEnumerator MoveToNextCoroutine(Transform target)
  {
    isIconActive = true;

    while (transform.GetDistance(target).magnitude > 0.05f)
    {
      yield return null;

      var velocity = _velocity * Time.deltaTime;
      var distance = Vector3.Lerp(transform.position, target.position, velocity);
      transform.position = distance;
    }

    isIconActive = false;
  }
}
