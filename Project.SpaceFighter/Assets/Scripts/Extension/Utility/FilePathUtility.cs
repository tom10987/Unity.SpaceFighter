
using UnityEngine;
using System.IO;

//------------------------------------------------------------
// TIPS:
// ファイルパスの操作を扱います。
//
// 一部エディター拡張が特定フォルダの監視を行うため、
// 主に、フォルダが見つからない場合に生成を行う目的で実装しました。
//
//------------------------------------------------------------

public static class DirectoryPath
{
  public const string resources = "/Resources";
}

public static class PathUtility
{
  /// <summary> プロジェクトのアセットフォルダのパスを先頭に結合する
  /// <para> 渡す文字列の先頭に必ず / を含めること </para></summary>
  public static string ReplaceAssetPath(this string path)
  {
    return Application.dataPath + path;
  }

  /// <summary> ファイルが存在すれば true を返す </summary>
  public static bool ExistsFilePath(this string path)
  {
    return File.Exists(path);
  }

  /// <summary> フォルダが存在すれば true を返す </summary>
  public static bool ExistsDirectoryPath(this string path)
  {
    return Directory.Exists(path);
  }


  /// <summary> アセットフォルダの指定したパスにフォルダを作成する </summary>
  public static void CreateDirectory(string path)
  {
    // フォルダがなかったときだけ作成する
    var directory = path.ReplaceAssetPath();
    if (!directory.ExistsDirectoryPath()) { Directory.CreateDirectory(directory); }
  }
}
