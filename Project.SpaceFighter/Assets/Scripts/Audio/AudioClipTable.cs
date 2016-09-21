
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

//------------------------------------------------------------
// TIPS:
// プロジェクトに含まれる全ての音声ファイルを管理します。
// 利用時に自動的に音声ファイルを読み込みます。
//
//------------------------------------------------------------
// NOTE:
// リソースの総数、および合計サイズが少ないため、
// まとめて読み込んで管理しています。
//
//------------------------------------------------------------

public static class AudioClipTable
{
  static readonly AudioClip[] _clips = null;

  static AudioClipTable()
  {
    _clips = Resources.LoadAll<AudioClip>("Audio");
  }

  /// <summary> 同じ名前の <see cref="AudioClip"/> を取得 </summary>
  public static AudioClip GetClip(this ClipIndex index)
  {
    return _clips.First(clip => Regex.IsMatch(clip.name, index.clipName));
  }
}
