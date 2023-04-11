using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] GameObject _roadPartPrefab;
    Transform _lastEndPoint;
    List<GameObject> _roadParts = new ();
    Vector3 _myPosition => PlayerController.Instance.MyVehicle.transform.position;

    void Awake()
    {
        ResetEndPoint();
        //FirstSpawn();
    }

    void ResetEndPoint()
    {
        _lastEndPoint = transform;
    }

    public void Clear()
    {
        foreach (GameObject road in _roadParts)
        {
            Destroy(road);
        }
        _roadParts.Clear();
        ResetEndPoint();
    }

    public void SpawnForDistance(float spawnDistance=200)
    {
        while (_lastEndPoint.position.z - _myPosition.z < spawnDistance)
        {
            SpawnRoad();
        }
    }

    public void ReSpawn()
    {
        SpawnForDistance();
        RemoveBackRoads();
        //GameObject firstRoadPart = _roadParts[0];
        //Destroy(firstRoadPart);
        //_roadParts.RemoveAt(0);
    }

    void RemoveBackRoads()
    {
        GameObject[] backRoads = _roadParts.Where((v) => _myPosition.z - v.transform.position.z > 20).ToArray();
        foreach (GameObject road in backRoads)
        {
            _roadParts.Remove(road);
            Destroy(road);
        }
    }

    public void SpawnRoad()
    {
        GameObject roadPartGameObject = Instantiate(_roadPartPrefab, _lastEndPoint.position, Quaternion.identity, transform);
        RoadPart roadPart = roadPartGameObject.GetComponent<RoadPart>();
        _lastEndPoint = roadPart.EndPoint;
        _roadParts.Add(roadPartGameObject);
    }
}
