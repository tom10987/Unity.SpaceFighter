
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]
public class LayerSettings : MonoBehaviour
{
  void Awake()
  {
    var renderer = GetComponent<Renderer>();
    renderer.sortingLayerID = 1;
  }
}
