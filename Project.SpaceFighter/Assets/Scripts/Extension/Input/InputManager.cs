
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//------------------------------------------------------------
// TIPS:
// InputManager.asset の読み込み、初期化、入力軸の登録を行います。
//
// 入力軸の登録は、専用のウィンドウを表示、
// 必要なパラメータを入力してボタンをクリックすれば完了します。
//
//------------------------------------------------------------

public class InputManager : ScriptableWizard
{
  static readonly string _FilePath = "ProjectSettings/InputManager.asset";
  static readonly string _Property = "m_Axes";
  static readonly string _AxisName = "m_Name";
  static readonly string _Joystick = "joyNum";

  static SerializedObject _inputManager = null;
  static SerializedProperty _axes = null;

  // 入力軸の名前を文字列として取り出せるようにする
  [InitializeOnLoadMethod]
  [MenuItem("Input Manager/Load Input Manager", priority = 1)]
  static void LoadInputManager()
  {
    var assets = AssetDatabase.LoadAllAssetsAtPath(_FilePath);
    _inputManager = new SerializedObject(assets[0]);
    _axes = _inputManager.FindProperty(_Property);
  }

  // 入力軸のデータを全て破棄する
  [MenuItem("Input Manager/Destroy Axis Data", priority = 2)]
  static void DestroyAxisData()
  {
    _axes.ClearArray();
    _inputManager.ApplyModifiedProperties();
  }

  // InputManager.asset から入力軸名のみを抽出
  static IEnumerable<string> GetAxisNames()
  {
    int length = _axes.arraySize;
    for (int i = 0; i < length; i++)
    {
      var property = _axes.GetArrayElementAtIndex(i);
      yield return property.FindPropertyRelative(_AxisName).stringValue;
    }
  }

  /// <summary> 入力軸名の一覧を取得 </summary>
  public static string[] axisNames { get { return GetAxisNames().ToArray(); } }

  /// <summary> コントローラー番号に対応する入力軸をまとめて取得 </summary>
  public static IEnumerable<string> GetAxes(int index)
  {
    int length = _axes.arraySize;
    for (int i = 0; i < length; i++)
    {
      var property = _axes.GetArrayElementAtIndex(i);
      var number = property.FindPropertyRelative(_Joystick).intValue;
      if (number != index) { continue; }
      yield return property.FindPropertyRelative(_AxisName).stringValue;
    }
  }


  // 新規の入力軸を格納できるように拡張、末尾のインデックスを返す
  static int IncreaseAxisIndex()
  {
    _axes.arraySize++;
    _inputManager.ApplyModifiedProperties();
    return _axes.arraySize - 1;
  }

  // 新規の入力軸を格納できるように拡張、空の要素を返す
  static SerializedProperty GetEmptyAxis()
  {
    var index = IncreaseAxisIndex();
    return _axes.GetArrayElementAtIndex(index);
  }


  // 新しい入力軸を設定、登録するためのウィンドウを表示する
  [MenuItem("Input Manager/Create New Axis", priority = 11)]
  static void OpenWizard()
  {
    DisplayWizard<InputManager>("New Input Axis Setup", "OK");
  }

  // OK (Create) ボタンが押されたら入力軸として登録する
  void OnWizardCreate()
  {
    if (IsEmpty()) { return; }

    var axis = GetEmptyAxis();
    _virtual.Register(ref axis);

    _inputManager.ApplyModifiedProperties();
  }

  // 軸名が入力されてなければ、警告を表示する
  bool IsEmpty()
  {
    var isEmpty = !_virtual.axisName.Any();
    if (isEmpty) { Debug.LogWarning("axis name is empty! (can not create)"); }
    return isEmpty;
  }


  [SerializeField]
  VirtualAxis _virtual = null;
}

#endif
