
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------
// TIPS:
// メインゲームの残り時間を表示します。
//
// パラメータを可視化するのみで、直接管理することはしていません。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class MainGameUI : MonoBehaviour
{
  [Header("残り時間を表示する Text コンポーネント")]
  [SerializeField]
  Text _timeText = null;

  [SerializeField]
  RectTransform _timeBoard = null;


  /// <summary> UI 要素を指定したサイズに変更する </summary>
  public void SetScale(Vector3 scale)
  {
    _timeBoard.localScale = scale;
  }

  /// <summary> 文字色を変更する </summary>
  public void SetColor(Color color)
  {
    _timeText.color = color;
  }

  /// <summary> 指定された値で表示を更新する </summary>
  public void UpdateTime(float value)
  {
    // 小数点以下の値を整数値に丸める
    int ceil = Mathf.CeilToInt(value);
    if (ceil < 0) { ceil = 0; }

    // 分単位と秒単位で切り分けて表示する
    int minute = ceil / 60;
    int second = ceil % 60;
    _timeText.text = ShiftToText(minute) + ":" + ShiftToText(second);
  }

  // 受け取った値を２桁に右寄せで詰める
  string ShiftToText(int value)
  {
    var result = value.ToString();
    return value < 10 ? "0" + result : result;
  }
}
