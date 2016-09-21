
using UnityEngine;
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// キャラクターごとに調整の必要がないパラメータ、データのみ、
// 一括で管理しています。
//
//------------------------------------------------------------
// NOTE:
// 本来であれば JSON 形式など、
// 外部に別データとして保存するのが望ましいと考えています。
//
// しかし、個人での制作ということもあり、時間を短縮するため、
// 頻繁に変更されないパラメータに限り、
// 今回はスクリプトに直接埋め込むことにしました。
//
// 細かい調整が必要なパラメータのみ、
// 各プレハブのインスペクターを通して編集できるようにしています。
//
//------------------------------------------------------------

public static class ActorDataTable
{
  /// <summary> キャラクターごとの値を管理 </summary>
  public class ActorData
  {
    public string name { get; set; }
    public Color color { get; set; }
    public Vector2 viewportPosition { get; set; }

    public ActorData(string name, Color color, Vector2 position)
    {
      this.name = name;
      this.color = color;
      viewportPosition = position;
    }
  }


  // 主に UI 表示で使用します。
  // そもそもの仕様として、数字から始まる名前が使用できないためです。

  /// <summary> コントローラー番号に一致するプレイヤー名を取得 </summary>
  public static string GetPlayerNumber(this JoystickIndex index)
  {
    return _dataTable[index].name;
  }


  // 主に UI、機体のマテリアル色で使用します。

  /// <summary> プレイヤーごとに割り当てられた色を取得 </summary>
  public static Color GetPlayerColor(this JoystickIndex index)
  {
    return _dataTable[index].color;
  }


  // ビューポートの大きさは、全てのカメラで共通のため、
  // 始点の情報だけを取り出せるようにしています。

  /// <summary> 各プレイヤーのカメラ表示位置を取得 </summary>
  public static Vector2 GetViewportPosition(this JoystickIndex index)
  {
    return _dataTable[index].viewportPosition;
  }

  /// <summary> ビューポートの大きさを取得 </summary>
  public static Vector2 viewportScale { get { return _viewportScale; } }

  /// <summary> プレイヤー番号に対応するオブジェクトレイヤーを取得 </summary>
  public static int GetObjectLayer(this JoystickIndex index)
  {
    var offset = (int)index - 1;
    return _baseLayer + offset;
  }


  //------------------------------------------------------------

  static ActorDataTable()
  {
    _dataTable = new Dictionary<JoystickIndex, ActorData>
    {
      { JoystickIndex.P1, new ActorData("1P", Color.cyan, Vector2.up * 0.51f) },
      { JoystickIndex.P2, new ActorData("2P", Color.magenta, Vector2.one * 0.51f) },
      { JoystickIndex.P3, new ActorData("3P", Color.green, Vector2.zero) },
      { JoystickIndex.P4, new ActorData("4P", Color.yellow, Vector2.right * 0.51f) },
    };
  }

  static readonly Dictionary<JoystickIndex, ActorData> _dataTable = null;

  static readonly Vector2 _viewportScale = Vector2.one * 0.49f;
  static readonly int _baseLayer = 16;
}
