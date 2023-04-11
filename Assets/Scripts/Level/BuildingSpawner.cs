using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public enum SpawnerSides
    {
        Left,
        Right
    }
    public SpawnerSides SpawnerSide;
    Vector3 _lastEndPoint;
    List<GameObject> _buildings = new();
    Vector3 _myPosition => PlayerController.Instance.MyVehicle.transform.position;

    void Awake()
    {
        ResetEndPoint();
    }

    void ResetEndPoint()
    {
        _lastEndPoint = transform.position;
    }

    public void Clear()
    {
        foreach (GameObject building in _buildings)
        {
            Destroy(building);
        }
        _buildings.Clear();
        ResetEndPoint();
    }

    public void SpawnForDistance(float spawnDistance=200)
    {
        while (_lastEndPoint.z - _myPosition.z < spawnDistance)
        {
            SpawnBuilding();
        }
    }

    public void ReSpawn()
    {
        SpawnForDistance();
        RemoveBackBuildings();
    }

    void RemoveBackBuildings()
    {
        GameObject[] backBuildings = _buildings.Where((v) => _myPosition.z - v.transform.position.z > 20).ToArray();
        foreach (GameObject building in backBuildings)
        {
            _buildings.Remove(building);
            Destroy(building);
        }
    }

    public void SpawnBuilding()
    {
        int randomIndex = Random.Range(0, LevelManager.Instance.Buildings.Length);
        GameObject randomBuilding = LevelManager.Instance.Buildings[randomIndex];
        GameObject buildingGameObject = Instantiate(randomBuilding, 
            _lastEndPoint,
            Quaternion.identity, 
            transform);
        Building building = buildingGameObject.GetComponent<Building>();
        if(SpawnerSide == SpawnerSides.Right)
        {
            building.transform.Rotate(0, 180, 0);
            building.transform.Translate(new Vector3(0, 0, -building.Renderer.bounds.extents.z));
        } else
        {
            building.transform.Translate(new Vector3(0, 0, building.Renderer.bounds.extents.z));
        }
        //Debug.Log(building.Renderer.bounds.center.z + " - " + building.Renderer.bounds.extents.z + " - " + building.Renderer.bounds.max.z);
        //_lastEndPoint += new Vector3(0, 0, building.Renderer.bounds.size.z + 0.2f);
        _lastEndPoint += new Vector3(0, 0, building.Renderer.bounds.size.z);
        _buildings.Add(buildingGameObject);
    }
}
