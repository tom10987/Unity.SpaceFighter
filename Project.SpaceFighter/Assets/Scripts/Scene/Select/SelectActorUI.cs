
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------
// TIPS:
// プレイヤーの UI を管理します。
//
//------------------------------------------------------------

public class SelectActorUI : MonoBehaviour
{
  [Header("プレイヤー番号表示")]
  [SerializeField]
  Text _playerNumber = null;

  [Header("プレイヤーの状態を表示するコンポーネント")]
  [SerializeField]
  Text _playerState = null;

  [SerializeField]
  Color _player = Color.yellow;

  [SerializeField]
  Color _ai = Color.white;


  [Header("演出対象のオブジェクト")]
  [SerializeField]
  GameObject _effect = null;

  [SerializeField]
  GameObject _button = null;


  /// <summary> プレイヤーの状態に合わせて名前を更新する </summary>
  public void UpdateName(JoystickIndex index)
  {
    _playerNumber.text = index.GetPlayerNumber();
    _playerNumber.color = index.GetPlayerColor();
  }

  /// <summary> プレイヤーの状態を UI に表示する </summary>
  public void ChangePlayerState(bool isPlayer)
  {
    _playerState.text = isPlayer ? "参加" : "AI";
    _playerState.color = isPlayer ? _player : _ai;
  }

  /// <summary> 状態を切り替える </summary>
  public void SwitchState(bool isApply)
  {
    _effect.SetActive(isApply);
    _button.SetActive(!isApply);
  }
}
