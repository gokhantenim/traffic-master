using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGateSpawner : MonoBehaviour
{
    [SerializeField] GameObject _levelGatePrefab;
    Transform _lastEndPoint;
    List<GameObject> _levelGates = new ();
    Vector3 _myPosition => PlayerController.Instance.MyVehicle.transform.position;

    void Awake()
    {
        ResetEndPoint();
    }

    void ResetEndPoint()
    {
        _lastEndPoint = transform;
    }

    public void Clear()
    {
        foreach (GameObject gate in _levelGates)
        {
            Destroy(gate);
        }
        _levelGates.Clear();
        ResetEndPoint();
    }

    public void SpawnForDistance(float spawnDistance=200)
    {
        while (_lastEndPoint.position.z - _myPosition.z < spawnDistance)
        {
            SpawnGate();
        }
    }

    public void ReSpawn()
    {
        SpawnForDistance();
        RemoveBackGates();
    }

    void RemoveBackGates()
    {
        GameObject[] backGates = _levelGates.Where((v) => _myPosition.z - v.transform.position.z > 100).ToArray();
        foreach (GameObject gate in backGates)
        {
            _levelGates.Remove(gate);
            Destroy(gate);
        }
    }

    public void SpawnGate()
    {
        GameObject gateGameObject = Instantiate(_levelGatePrefab, _lastEndPoint.position + new Vector3(0, 0, GameManager.LevelLimit), Quaternion.identity, transform);
        LevelGate gate = gateGameObject.GetComponent<LevelGate>();
        gate.LevelTitle.text = "Level " + ((gate.transform.position.z/GameManager.LevelLimit)+1);
        _lastEndPoint = gate.transform;
        _levelGates.Add(gateGameObject);
    }
}
