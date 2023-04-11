using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    Transform _lastEndPoint;
    float _myPlayerLength => PlayerController.Instance.MyVehicle.EndPoint.localPosition.z;
    Vector3 _myPosition => PlayerController.Instance.MyVehicle.transform.position;
    List<GameObject> _vehicles = new();
    int _lastBlankLane;

    void Awake()
    {
        ResetEndPoint();
    }

    public void Clear()
    {
        foreach (GameObject vehicle in _vehicles)
        {
            Destroy(vehicle);
        }
        _vehicles.Clear();
        ResetEndPoint();
    }

    void ResetEndPoint()
    {
        _lastEndPoint = transform;
    }

    public void SpawnForDistance(float spawnDistance=100)
    {
        while (_lastEndPoint.position.z - _myPosition.z < spawnDistance)
        {
            SpawnVehicleRow();
        }
    }

    public void ReSpawn()
    {
        RemoveBackVehicles();
        SpawnForDistance();
    }

    void RemoveBackVehicles()
    {
        GameObject[] backVehicles = _vehicles.Where((v) => _myPosition.z - v.transform.position.z > 30).ToArray();
        foreach (GameObject vehicle in backVehicles)
        {
            _vehicles.Remove(vehicle);
            Destroy(vehicle);
        }
    }

    public void SpawnVehicleRow()
    {
        Transform maxEndPoint = null;
        int blankLane = Random.Range(0,3);
        if(blankLane == _lastBlankLane)
        {
            blankLane = Random.Range(0, 3);
        }
        _lastBlankLane = blankLane;
        int lastVehicleIndex = -1;
        for (int i = 0; i < 3; i++)
        {
            if (i == blankLane) continue;

            int randomVehicleIndex = Random.Range(0, LevelManager.Instance.Vehicles.Length);
            if(lastVehicleIndex == randomVehicleIndex)
            {
                randomVehicleIndex = Random.Range(0, LevelManager.Instance.Vehicles.Length);
            }
            lastVehicleIndex = randomVehicleIndex;
            GameObject randomVehicle = LevelManager.Instance.Vehicles[randomVehicleIndex];
            GameObject vehicleGameObject = Instantiate(randomVehicle, 
                new Vector3(
                    LevelManager.LaneSize.x * i - LevelManager.LaneSize.x, 
                    0,
                    _lastEndPoint.position.z 
                    + _myPlayerLength * 2 + _myPlayerLength * GameManager.Instance.Level * 0.01f), 
                Quaternion.identity, transform);
            _vehicles.Add(vehicleGameObject);
            Vehicle vehicle = vehicleGameObject.GetComponent<Vehicle>();
            vehicle.Speed = 4;
            vehicle.CurrentLane = i;
            if (maxEndPoint == null || vehicle.EndPoint.localPosition.z > maxEndPoint.localPosition.z)
            {
                maxEndPoint = vehicle.EndPoint;
            }
        }

        _lastEndPoint = maxEndPoint;
    }
}
