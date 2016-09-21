
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//------------------------------------------------------------
// TIPS:
// コントローラーからの入力結果を返します。
//
// コンストラクタ呼び出しによる初期化は可能ですが、
// ファイル読み込みが前提のため、インスタンス化は行わないでください。
//
//------------------------------------------------------------
// WHY?:
// 各プレイヤーのコントローラー入力を一括で管理する目的で実装しました。
//
// また、各プレイヤーは、コントローラー入力の結果を扱えればよく、
// コントローラーのインスタンス自体を持たせる必要がないと判断したため、
// インスタンスの管理を JoystickManager クラスで行うようにしました。
//
//------------------------------------------------------------

/// <summary> CAUTION: <see cref="JoystickManager"/> にて管理されます <para>
/// コンストラクタ呼び出しは可能ですが、インスタンス化しないでください </para></summary>
public class Joystick
{
  /// <summary> 入力軸の種類 <para>
  /// 検索用インデックスとしても使用する </para></summary>
  public enum AxisType
  {
    /// <summary> 左スティックの横方向 </summary>
    LHorizontal,
    /// <summary> 左スティックの縦方向 </summary>
    LVertical,

    /// <summary> 右スティックの横方向 </summary>
    RHorizontal,
    /// <summary> 右スティックの縦方向 </summary>
    RVertical,

    /// <summary> トリガー LR </summary>
    Trigger,
    /// <summary> トリガー L </summary>
    TriggerL,
    /// <summary> トリガー R </summary>
    TriggerR,

    /// <summary> スタートボタン、A、X ボタン </summary>
    Apply,
    /// <summary> B、Y ボタン </summary>
    Cancel,

    /// <summary> A ボタン </summary>
    Attack,
    /// <summary> X ボタン </summary>
    Super,

    /// <summary> スタートボタン </summary>
    Pause,
  }

  public Joystick(string[] axes)
  {
    _axes = new Dictionary<AxisType, string>();

    // 列挙型の一覧を作る
    var names = Enum.GetNames(typeof(AxisType));

    // 受け取った文字列をインデックスと関連付けて登録する
    foreach (var name in names)
    {
      // インデックスと一致する文字列がなければ、次のインデックスに進む
      var find = axes.FirstOrDefault(axis => Regex.IsMatch(axis, name));
      if (find == null) { continue; }

      // インデックス名を列挙型に戻して、見つかった文字列と一緒に登録する
      var index = (AxisType)Enum.Parse(typeof(AxisType), name);
      _axes.Add(index, find);
    }
  }

  readonly Dictionary<AxisType, string> _axes = null;


  /// <summary> 左スティック横方向の入力の値を取得 </summary>
  public float leftAxisX
  {
    get { return GetAxis(AxisType.LHorizontal); }
  }
  /// <summary> 左スティック縦方向の入力の値を取得 </summary>
  public float leftAxisY
  {
    get { return GetAxis(AxisType.LVertical); }
  }
  /// <summary> 左スティック入力の値を XY 平面として取得 </summary>
  public Vector3 directionLeftXY
  {
    get { return new Vector3(leftAxisX, leftAxisY, 0f); }
  }
  /// <summary> 左スティック入力の値を XZ 平面として取得 </summary>
  public Vector3 directionLeftXZ
  {
    get { return new Vector3(leftAxisX, 0f, leftAxisY); }
  }


  /// <summary> 右スティック縦方向の入力の値を取得 </summary>
  public float rightAxisY
  {
    get { return GetAxis(AxisType.RVertical); }
  }
  /// <summary> 右スティック横方向の入力の値を取得 </summary>
  public float rightAxisX
  {
    get { return GetAxis(AxisType.RHorizontal); }
  }
  /// <summary> 右スティック入力の値を XY 平面として取得 </summary>
  public Vector3 directionRightXY
  {
    get { return new Vector3(rightAxisX, rightAxisY, 0f); }
  }
  /// <summary> 右スティック入力の値を XZ 平面として取得 </summary>
  public Vector3 directionRightXZ
  {
    get { return new Vector3(rightAxisX, 0f, rightAxisY); }
  }


  /// <summary> 左右のトリガー入力の値を取得 </summary>
  public float trigger
  {
    get { return GetAxis(AxisType.Trigger); }
  }
  /// <summary> 左トリガー入力の値を取得 </summary>
  public float triggerL
  {
    get { return GetAxis(AxisType.TriggerL); }
  }
  /// <summary> 右トリガー入力の値を取得 </summary>
  public float triggerR
  {
    get { return GetAxis(AxisType.TriggerR); }
  }


  /// <summary> 指定した入力軸から入力があれば true を返す </summary>
  public bool IsPress(AxisType type)
  {
    return Input.GetButton(_axes[type]);
  }
  /// <summary> 指定した入力軸が入力された瞬間 true を返す </summary>
  public bool IsPush(AxisType type)
  {
    return Input.GetButtonDown(_axes[type]);
  }
  /// <summary> 指定した入力軸からの入力が切れた瞬間 true を返す </summary>
  public bool IsPull(AxisType type)
  {
    return Input.GetButtonUp(_axes[type]);
  }


  /// <summary> 指定した入力軸の入力を取得 </summary>
  public float GetAxis(AxisType type)
  {
    return Input.GetAxis(_axes[type]);
  }
  /// <summary> 指定した入力軸の入力を整数値に丸めて取得 </summary>
  public float GetAxisRaw(AxisType type)
  {
    return Input.GetAxisRaw(_axes[type]);
  }
  /// <summary> 指定した入力軸の入力を整数値で取得する </summary>
  public int GetAxisToInt(AxisType type)
  {
    return (int)GetAxisRaw(type);
  }
}
