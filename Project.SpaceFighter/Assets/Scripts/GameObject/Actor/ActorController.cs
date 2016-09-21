
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// プレイヤー、AI の操作を統括します。
//
// メインゲームシーンにて、ゲーム開始前のカウントダウン中など、
// 操作不可能なタイミングでの操作をできないようにするため、
// このコンポーネント単独で動作することはありません。
//
// このコンポーネントを持つオブジェクトは、
// メインゲームシーンの管理クラス内で更新処理を呼び出されることで
// 初めて動作可能になります。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
[RequireComponent(typeof(ActorObject))]
public class ActorController : MonoBehaviour
{
  [Header("アナログ入力軸のしきい値")]
  [SerializeField, Range(0f, 1f)]
  float _range = 0.2f;

  [Header("加速操作の強度")]
  [SerializeField, Range(1, 10)]
  int _accel = 3;

  [Header("加速、ブレーキ中の回転力の減衰度")]
  [SerializeField, Range(0f, 1f)]
  float _downTorque = 0.5f;

  [Header("エフェクトが有効になる加速値")]
  [SerializeField, Range(1f, 10f)]
  float _enableEffect = 2f;


  /// <summary> コントローラー </summary>
  public Joystick joystick { get; private set; }


  [Header("カメラのプレハブ")]
  [SerializeField]
  CameraObject _camera = null;

  /// <summary> カメラのインスタンス </summary>
  public new CameraObject camera { get; private set; }

  /// <summary> 耐久ゲージを更新 </summary>
  public void UpdateDamage(int damage)
  {
    // 無敵時間中なら耐久値を減らさない
    if (actorObject.isInvincible) { return; }

    actorObject.endurance -= damage;
    UpdateGauge();
  }

  /// <summary> 耐久ゲージを現在の耐久値で更新する </summary>
  public void UpdateGauge()
  {
    camera.UpdateGauge(actorObject.GetEnduranceRatio());

    // 耐久ゲージの割合でエフェクトの状態を切り替える
    actorObject.spark.gameObject.SetActive(camera.isDangerous);
  }

  /// <summary> スコア表示を現在のスコアを使って更新する </summary>
  public void UpdateScore()
  {
    camera.UpdateScore(actorObject.score);
  }


  [SerializeField]
  ObjectNameController _nameController = null;


  [Header("AI 用、判定対象のレイヤー、距離")]
  [SerializeField]
  LayerMask _actor;

  [SerializeField]
  LayerMask _obstacle;

  [SerializeField]
  LayerMask _wall;

  [SerializeField]
  LayerMask _item;

  [SerializeField, Range(1, 50)]
  int _moveDistance = 25;

  [SerializeField, Range(1, 100)]
  int _attackDistance = 50;

  [Header("ステージ端の壁から離れようとする行動の強さ")]
  [SerializeField, Range(1, 10)]
  int _wallNormal = 7;


  /// <summary> プレイヤーのパラメータ </summary>
  public ActorObject actorObject { get; private set; }

  // マシンパラメータの取得
  MachineParameter machine { get { return actorObject.machine; } }


  /// <summary> コントローラー番号に合わせた初期化を行う </summary>
  public void Setup(JoystickIndex index)
  {
    // 割り当てられたコントローラー情報を取得
    joystick = index.GetJoystick();

    // 各プレイヤーのカメラに表示されるオブジェクト名を設定
    _nameController.SetPlayer(index);

    // 自身のパラメータを表すコンポーネントを取得
    actorObject = GetComponent<ActorObject>();
    actorObject.SetPlayerColor(index);

    // 自身をターゲットとして追尾するカメラを生成、初期化
    camera = Instantiate(_camera);
    camera.Setup(transform, index);
    UpdateGauge();
    UpdateScore();

    // プレイヤーの状態で振る舞いを切り替える
    var isPlayer = index.GetState().isPlayer;
    _translate = isPlayer ? (System.Action<float>)TranslatePlayer : TranslateAI;
    _rotate = isPlayer ? (System.Action<float>)RotatePlayer : RotateAI;
    _attack = isPlayer ? (System.Action)AttackPlayer : AttackAI;
    _superAttack = isPlayer ? (System.Action)SuperAttackPlayer : null;
  }


  /// <summary> 耐久値が０になったプレイヤーを復活させる <para>
  /// 耐久値がなくなったかどうかは外部で判定 </para></summary>
  public void Restart()
  {
    actorObject.Stop();

    // 耐久値を回復させて戦闘不能演出を開始
    actorObject.endurance = machine.endurance;
    actorObject.dead.StartEffect(UpdateGauge);

    // スコア半減、UI を更新する
    actorObject.score /= 2;
    UpdateScore();
  }

  /// <summary> 時間切れ処理：当たり判定を消失させる </summary>
  public void TimeUp()
  {
    actorObject.Stop();
    actorObject.Deactivate();
  }


  // 初期化処理にて使用、プレイヤーの状態で振る舞いを切り替える
  System.Action<float> _translate = null;
  System.Action<float> _rotate = null;
  System.Action _attack = null;
  System.Action _superAttack = null;

  /// <summary> 移動処理 </summary>
  public void Translate(float deltaTime) { _translate(deltaTime); }

  /// <summary> 回転処理 </summary>
  public void Rotate(float deltaTime) { _rotate(deltaTime); }

  /// <summary> 攻撃 </summary>
  public void Attack() { _attack(); }//AttackPlayer(); }

  /// <summary> 強力なショット攻撃 </summary>
  public void SuperAttack() { if (_superAttack != null) { _superAttack(); } }


  // ユーザー入力による移動
  void TranslatePlayer(float deltaTime)
  {
    // コントローラー入力を取得、しきい値未満なら値を無視する
    var axis = joystick.leftAxisY;
    if (Mathf.Abs(axis) < _range) { axis = 0f; }

    // 加速量を強化
    if (axis > 0f) { axis *= _accel; }

    // 加速値が一定以上ならエフェクト有効
    var isActive = axis > _enableEffect;
    actorObject.acceleration.gameObject.SetActive(isActive);

    if (isActive)
    {
      // エフェクトのサイズを加速値の割合に合わせて変更する
      var scale = Vector3.one * (axis - _enableEffect) * 0.1f;
      actorObject.acceleration.transform.localScale = scale;
    }

    // 正面方向に移動
    // コントローラー入力の影響を与える
    var force = transform.forward * deltaTime;
    actorObject.AddForce(force * (1f + axis));
  }

  // ユーザー入力による回転（カメラ）
  void RotatePlayer(float deltaTime)
  {
    // コントローラー入力を取得、しきい値未満なら値を無視する
    var axisX = joystick.leftAxisX;
    if (Mathf.Abs(axisX) < _range) { axisX = 0f; }

    // 加速、ブレーキ中は回転量を抑える
    var axisY = Mathf.Abs(joystick.leftAxisY);
    axisX *= (1f - axisY * _downTorque);

    actorObject.AddTorque(axisX * deltaTime);
    camera.UpdateRotation(deltaTime);
  }

  // ユーザー入力による攻撃
  void AttackPlayer()
  {
    if (!joystick.IsPush(Joystick.AxisType.Attack)) { return; }
    actorObject.CreateShot(this);
  }

  void SuperAttackPlayer()
  {
    if (!joystick.IsPush(Joystick.AxisType.Super)) { return; }
    actorObject.CreateSuperShot(this);
    UpdateGauge();
  }


  // AI 用、制御パラメータ
  Transform _target = null;
  RaycastHit _hit;
  float _angle = 0f;

  // AI の移動
  void TranslateAI(float deltaTime)
  {
    var direction = GetDirection();
    _angle = 0f;

    // アイテムとのレイキャスト判定
    RaycastHit item;
    if (Raycast(out item, _moveDistance, _item)) { _target = item.transform; }

    // 障害物とのレイキャスト判定
    RaycastHit obstacle;
    if (Raycast(out obstacle, _moveDistance, _obstacle + _wall))
    {
      // 現在地と衝突点の距離、衝突点の法線を取得
      var distance = obstacle.point - transform.position;
      var normal = obstacle.normal * _moveDistance * 0.5f;

      direction = distance + normal;
      direction.Normalize();

      // 再度レイキャストを行い、さらに障害物があるようなら、大きく移動する
      if (Raycast(direction, _moveDistance, _obstacle))
      {
        direction += normal;
        direction.Normalize();
      }

      _angle = transform.GetAngle(direction);
      if (_angle == 0f) { _angle = 0.1f; }

      if (!obstacle.rigidbody) { _angle *= _wallNormal; }
    }

    // 正面方向と進行方向の差から移動量を求める
    var force = transform.GetForce(direction);
    actorObject.AddForce(force * deltaTime);
  }

  // AI の回転
  void RotateAI(float deltaTime)
  {
    actorObject.AddTorque(_angle * deltaTime);
    camera.UpdateRotation(deltaTime);
  }

  // AI の攻撃
  void AttackAI()
  {
    // プレイヤーがヒットしなければ何もしない
    RaycastHit actorHit;
    if (!Raycast(out actorHit, _attackDistance, _actor)) { return; }

    // 障害物がないか確認する
    RaycastHit obstacle;
    var obstacleHit = Raycast(out obstacle, _attackDistance, _obstacle);

    // 一番近い位置にあるオブジェクトがプレイヤーでなければ何もしない
    if (obstacleHit)
    {
      if (obstacle.distance < actorHit.distance) { return; }
    }

    // 自分がショット可能でなければ何もしない
    if (!actorObject.enableFire) { return; }

    actorObject.CreateShot(this);
  }


  // Physics.Raycast のラッパー A
  bool Raycast(Vector3 direction, float distance, LayerMask layer)
  {
    var position = transform.position;
    return Physics.Raycast(position, direction, distance, layer);
  }

  // Physics.Raycast のラッパー B
  bool Raycast(out RaycastHit hit, float distance, LayerMask layer)
  {
    var p = transform.position;
    var f = transform.forward;
    return Physics.Raycast(p, f, out hit, distance, layer);
  }

  // AI 用、アイテムを見つけていたらアイテムの方向を返す
  Vector3 GetDirection()
  {
    if (!_target) { return transform.forward; }

    var direction = _target.position - transform.position;
    return direction.normalized;
  }
}
