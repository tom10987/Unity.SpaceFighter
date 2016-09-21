
using UnityEngine;
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// プレイヤー、AI のコンポーネントを中心とした、
// オブジェクトのパラメータを管理します。
//
// 攻撃力など、ゲーム用パラメータについては、
// MachineParameter クラスを確認してください。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ActorObject : MonoBehaviour
{
  [SerializeField]
  Collider _collider = null;

  /// <summary> 当たり判定を無効にする </summary>
  public void Deactivate() { _collider.enabled = false; }


  [SerializeField]
  Rigidbody _rigidbody = null;
  public new Rigidbody rigidbody { get { return _rigidbody; } }

  // FixedUpdate() で使用する
  Vector3 _force = Vector3.zero;
  float _torque = 0f;

  /// <summary> <see cref="Rigidbody.AddForce(Vector3)"/> のラッパー 
  /// <para> <see cref="ForceMode.VelocityChange"/> で動作する </para></summary>
  public void AddForce(Vector3 direction)
  {
    // 進行方向のベクトルに自身のパラメータを反映させて移動量とする
    _force = direction * _machine.velocity;
  }

  /// <summary> 指定した角度で回転する </summary>
  public void AddTorque(float angle)
  {
    _torque = angle * _machine.torque;
  }

  /// <summary> 移動と回転を強制的に停止する </summary>
  public void Stop()
  {
    _force = Vector3.zero;
    _torque = 0f;
    _rigidbody.velocity = Vector3.zero;
  }


  [Header("レンダラーを持つオブジェクト")]
  [SerializeField]
  ActorMaterial _renderer = null;

  /// <summary> プレイヤーカラーを指定した色に変更する </summary>
  public void SetPlayerColor(JoystickIndex index)
  {
    joystickIndex = index;
    _renderer.SetColor(index);
  }


  [Header("マシン固有のパラメータ")]
  [SerializeField]
  MachineParameter _machine = null;

  /// <summary> マシン固有のパラメータを取得 </summary>
  public MachineParameter machine { get { return _machine; } }

  /// <summary> 現在の耐久値 </summary>
  public int endurance { get; set; }

  /// <summary> 現在の耐久値の割合を取得 <para>
  /// 0 ~ 1 までの範囲を返す </para></summary>
  public float GetEnduranceRatio()
  {
    return endurance / (float)_machine.endurance;
  }

  /// <summary> 耐久値が最大まで回復していれば true を返す </summary>
  public bool isFine { get { return endurance >= _machine.endurance; } }

  /// <summary> 耐久値が０なら true を返す </summary>
  public bool isDead { get { return endurance <= 0; } }


  /// <summary> プレイヤー番号も兼ねる、コントローラー番号 </summary>
  public JoystickIndex joystickIndex { get; private set; }

  /// <summary> プレイヤー番号を使ってスコア情報を取得 </summary>
  public int score
  {
    get { return joystickIndex.GetState().score; }
    set { joystickIndex.GetState().score = value; }
  }


  [Header("ショット攻撃で使用するプレハブ")]
  [SerializeField]
  ShotObject _shot = null;

  [SerializeField, Range(1f, 3f)]
  float _offset = 2f;


  [Header("強力なショット攻撃で使用するプレハブ")]
  [SerializeField]
  ShotObject _super = null;

  [Header("発射コスト")]
  [SerializeField, Range(1, 10)]
  int _cost = 5;

  [Header("耐久値が値以下になったら使えないようにする制限値")]
  [SerializeField, Range(0.1f, 1f)]
  float _limit = 0.5f;

  [Header("追加で発射されるショットの角度")]
  [SerializeField, Range(0, 90)]
  int _superAngle = 30;


  [Header("ショット攻撃の初速")]
  [SerializeField, Range(1, 20)]
  int _start = 15;


  readonly List<ShotObject> _shots = new List<ShotObject>();

  /// <summary> ショットを発射 </summary>
  public void CreateShot(ActorController controller)
  {
    // ショットを決められた数以上、発射できないようにする
    if (_shots.Count >= _machine.shot) { return; }

    var shot = Instantiate(_shot);

    // 機体の正面をスタート地点にする
    var offset = transform.forward * _offset;
    shot.transform.position = transform.position + offset;
    shot.transform.rotation = transform.rotation;

    // 機体の色をショットに反映する
    shot.SetColor(controller);
    shot.StartForce(_start);

    shot.callback = CallBackDestroy;
    _shots.Add(shot);

    // AI 用の発射カウンタをリセット
    _distance = 0f;
  }

  /// <summary> 攻撃力の高いショットを発射 </summary>
  public void CreateSuperShot(ActorController controller)
  {
    // 耐久値の割合が制限値未満なら発射できないようにする
    if (GetEnduranceRatio() < _limit) { return; }

    var shots = new List<ShotObject>();
    for (int i = 0; i < 3; i++) { shots.Add(Instantiate(_super)); }

    // 機体の正面をスタート地点にする
    var offset = transform.forward * _offset;
    foreach (var shot in shots)
    {
      shot.transform.position = transform.position + offset;
      shot.transform.rotation = transform.rotation;

      shot.SetColor(controller);
      shot.StartForce(_start);

      shot.callback = CallBackDestroy;
    }

    // 左右に進むショットの角度を設定
    shots[1].transform.Rotate(Vector3.up, _superAngle);
    shots[2].transform.Rotate(Vector3.up, -_superAngle);

    // 耐久値減少
    endurance -= _cost;
  }

  // 削除用イベント
  void CallBackDestroy(ShotObject shot) { _shots.Remove(shot); }


  [Header("戦闘不能時の演出")]
  [SerializeField]
  ActorDeadEffect _dead = null;

  /// <summary> 戦闘不能演出の状態を取得 </summary>
  public ActorDeadEffect dead { get { return _dead; } }

  /// <summary> 無敵時間のカウント中（＝無敵）なら true を返す </summary>
  public bool isInvincible { get { return _dead.isInvincible; } }


  [Header("機体に発生させるエフェクト")]
  [SerializeField]
  ParticleSystem _acceleration = null;

  /// <summary> 機体が大きく加速しているときに発生させるエフェクト </summary>
  public ParticleSystem acceleration { get { return _acceleration; } }

  [SerializeField]
  ParticleSystem _spark = null;

  [SerializeField, Range(1, 10)]
  int _sparkScale = 5;

  /// <summary> 機体の耐久値が危険な状態のときに発生させるエフェクト </summary>
  public ParticleSystem spark { get { return _spark; } }


  [Header("AI 用、ショットの発射間隔")]
  [SerializeField, Range(10f, 100f)]
  float _shotInterval = 80f;

  // AI 用、ショットの発射カウンタ
  // 移動距離を判定基準にする
  float _distance = 0f;
  public bool enableFire { get { return _distance > _shotInterval; } }

  void Awake()
  {
    _force = Vector3.zero;
    endurance = _machine.endurance;

    _spark.transform.localScale = Vector3.one * _sparkScale;
  }

  void FixedUpdate()
  {
    _rigidbody.AddForce(_force, ForceMode.VelocityChange);
    transform.Rotate(Vector3.up, _torque);

    // AI 用、ショットの発射カウンタを更新
    if (_distance < _shotInterval) { _distance += _rigidbody.velocity.magnitude; }

    // 最高速度に満たなければ何もしない
    if (_rigidbody.velocity.magnitude < _machine.maxSpeed) { return; }

    // 最高速度以上なら速度を抑える
    var normal = _rigidbody.velocity.normalized;
    _rigidbody.velocity = normal * _machine.maxSpeed;
  }
}
