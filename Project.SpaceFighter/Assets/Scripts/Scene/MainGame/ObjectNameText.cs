
using UnityEngine;
using System.Collections;

//------------------------------------------------------------
// TIPS:
// 画面にオブジェクト名を表示するためのコンポーネントを管理します。
//
// オブジェクトには１つのレイヤーしか登録できず、
// 各プレイヤーのカメラ向けに、個別にオブジェクトを用意するため、
// スクリプトからはレイヤーの変更を行いません。
//
//------------------------------------------------------------

[DisallowMultipleComponent]
public class ObjectNameText : MonoBehaviour
{
  [SerializeField]
  TextMesh[] _meshes = null;

  /// <summary> オブジェクト名を表示する <see cref="TextMesh"/> を取得 </summary>
  public TextMesh nameMesh { get { return _meshes[0]; } }


  /// <summary> プレイヤー番号に対応する名前と色を設定する </summary>
  public void SetPlayer(JoystickIndex index)
  {
    nameMesh.text = index.GetPlayerNumber();
    SetColor(index);
  }

  /// <summary> プレイヤー番号に対応する色に設定する </summary>
  public void SetColor(JoystickIndex index)
  {
    foreach (var mesh in _meshes) { mesh.color = index.GetPlayerColor(); }
  }


  Transform _camera = null;

  void OnWillRenderObject()
  {
    if (_camera) { return; }
    if (Camera.current.gameObject.layer != gameObject.layer) { return; }

    _camera = Camera.current.gameObject.transform;
  }

  IEnumerator Start()
  {
    while (!_camera) { yield return null; }

    while (isActiveAndEnabled)
    {
      transform.rotation = _camera.rotation;
      yield return null;
    }
  }
}
