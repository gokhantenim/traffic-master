using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    public MeshRenderer Renderer;
    void Start()
    {
        int randomMaterialIndex = Random.Range(0, LevelManager.Instance.BuildingMaterials.Length);
        Renderer.material = LevelManager.Instance.BuildingMaterials[randomMaterialIndex];
    }
}
