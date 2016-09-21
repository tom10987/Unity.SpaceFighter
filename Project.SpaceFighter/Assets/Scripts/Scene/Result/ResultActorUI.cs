
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------
// TIPS:
// リザルトシーンにて、
// プレイヤー情報を表示するための UI を管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ResultActorUI : MonoBehaviour
{
  [Header("プレイヤーのパラメータを表すコンポーネント類")]
  [SerializeField]
  Text _playerName = null;

  [SerializeField]
  Text _score = null, _rank = null;

  /// <summary> 順位を表示するためのコンポーネント </summary>
  public Text rank { get { return _rank; } }

  /// <summary> 登録されたプレイヤー </summary>
  public JoystickIndex playerNumber { get; private set; }


  /// <summary> プレイヤー情報が登録済みなら true を返す </summary>
  public bool isActive { get; private set; }

  /// <summary> プレイヤー番号に対応した色と名前を設定する </summary>
  public void SetPlayerName(JoystickIndex index)
  {
    _playerName.text = index.GetPlayerNumber();
    _playerName.color = index.GetPlayerColor();

    _score.text = index.GetState().score.ToString();

    playerNumber = index;
    isActive = true;
  }
}
