
using UnityEngine;

//------------------------------------------------------------
// TIPS:
// ゲーム中のキャラクターのパラメータを表します。
//
// パラメータはそれぞれ、下記の能力を表します。
//
// 識別用
// type     : マシンタイプ
//
// 戦闘パラメータ
// endurance : 耐久力（最大値）
// shot      : 一度の攻撃で発射されるショットの数
//
// 移動パラメータ
// velocity : 加速度
// torque   : 旋回性能
// maxSpeed : 最高速度
//
//------------------------------------------------------------

[System.Serializable]
public class MachineParameter
{
  [SerializeField]
  MachineType _type = MachineType.Balance;
  public MachineType type { get { return _type; } }


  [SerializeField, Range(1, 100)]
  int _endurance = 10;
  public int endurance { get { return _endurance; } }

  [SerializeField, Range(1, 5)]
  int _shot = 1;
  public int shot { get { return _shot; } }


  [SerializeField, Range(1, 10)]
  int _velocity = 5;
  public float velocity { get { return _velocity * 5; } }

  [SerializeField, Range(1, 10)]
  int _torque = 5;
  public int torque { get { return _torque * 10; } }

  [SerializeField, Range(10f, 50f)]
  float _maxSpeed = 30f;
  public float maxSpeed { get { return _maxSpeed; } }
}
