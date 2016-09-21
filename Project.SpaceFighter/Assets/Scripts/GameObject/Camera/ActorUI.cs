
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// キャラクターのスコア、耐久ゲージを管理します。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ActorUI : MonoBehaviour
{
  [Header("耐久ゲージ表示用 Image コンポーネント")]
  [SerializeField]
  Image _endurance = null;

  [SerializeField]
  Image _damage = null;

  [SerializeField, Range(1, 10)]
  int _damageSpeed = 3;

  /// <summary> 耐久値の割合が危険域なら true を返す </summary>
  public bool isDangerous { get { return _endurance.fillAmount < _dangerous; } }


  [Header("耐久ゲージの背景 Image の色と条件")]
  [SerializeField]
  Image _enduranceBack = null;

  [SerializeField]
  Color _fine = Color.gray;

  [SerializeField]
  Color _danger = Color.red;

  [SerializeField, Range(0.1f, 1f)]
  float _dangerous = 0.5f;

  [SerializeField, Range(1, 5)]
  int _velocity = 2;


  [Header("UI 全体の親オブジェクト")]
  [SerializeField]
  RectTransform _boardParent = null;

  [Header("プレイヤー名表示用 Text コンポーネント")]
  [SerializeField]
  Text _actorNumber = null;

  [Header("スコア表示用 Text コンポーネント")]
  [SerializeField]
  Text _scoreBoard = null;


  /// <summary> UI の表示を指定したパラメータで初期化 <para>
  /// CAUTION: Instantiate() の直後に呼び出すこと </para></summary>
  public void Setup(JoystickIndex playerNumber)
  {
    var color = playerNumber.GetPlayerColor();
    _endurance.color = color;
    _actorNumber.color = color;

    _actorNumber.text = playerNumber.GetPlayerNumber() + "<color=FFFFFFFF>:</color>";

    // 画面右のプレイヤーのみ、UI の位置を右側に移動する
    if (((int)playerNumber % 2) > 0) { return; }

    var x = _boardParent.localPosition.x;
    var position = Vector3.right * Mathf.Abs(x) * 2f;
    _boardParent.localPosition += position;
  }


  /// <summary> 値をゲージに反映する <para>
  /// [0, 1] の範囲で、ゲージの長さを調整する </para></summary>
  public void UpdateGauge(float value)
  {
    // 現在値を反映、ダメージ演出を開始
    _endurance.fillAmount = Mathf.Clamp01(value);
    StartCoroutine(GaugeCoroutine());

    if (!isDangerous) { _enduranceBack.color = _fine; }
  }

  IEnumerator GaugeCoroutine()
  {
    // 復活したときなど、耐久値の割合が高くなったら演出バーを更新
    if (_damage.fillAmount < _endurance.fillAmount)
    {
      _damage.fillAmount = _endurance.fillAmount;

      // 危険な状態ではなくなったら処理を中断
      if (!isDangerous) { yield break; }
    }

    // 危険な状態なら指定した色でゲージを点滅させる
    while (_damage.fillAmount > _endurance.fillAmount)
    {
      var amount = _endurance.fillAmount - _damage.fillAmount;
      amount = amount * Time.deltaTime * _damageSpeed;
      _damage.fillAmount = Mathf.Clamp01(_damage.fillAmount + amount);

      yield return null;
    }
  }


  /// <summary> 値をボードに反映する </summary>
  public void UpdateScore(int value)
  {
    _scoreBoard.text = value.ToString();
  }


  IEnumerator Start()
  {
    float time = 0f;

    while (isActiveAndEnabled)
    {
      yield return null;

      // 耐久ゲージが危険域でなければ何もしない
      if (!isDangerous) { continue; }

      time = Mathf.Repeat(time + Time.deltaTime * _velocity, Mathf.PI);

      var sin = Mathf.Sin(time);
      var color = _fine * sin + _danger * (1f - sin);

      _enduranceBack.color = color;
    }
  }


  void OnValidate()
  {
    _enduranceBack.color = _fine;
  }
}
