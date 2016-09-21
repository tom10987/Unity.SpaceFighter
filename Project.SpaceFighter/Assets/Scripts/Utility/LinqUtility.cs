
using System;
using System.Collections.Generic;

//------------------------------------------------------------
// TIPS:
// Linq に関わる、より使いやすくする処理をまとめています。
//
//------------------------------------------------------------

public static class LinqUtility
{
  /// <summary> コレクションの全要素に指定したデリゲートを実行する </summary>
  public static void Execute<TSource>(this IEnumerable<TSource> collection,
                                      Action<TSource> action)
  {
    foreach (var element in collection) { action(element); }
  }
}
