using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : AbstractSingleton<LevelManager>
{
    public static Vector3 LaneSize = new Vector3(4, 0);
    public GameObject[] Vehicles;
    public GameObject[] Buildings;
    public Material[] BuildingMaterials;
    public BuildingSpawner LeftBuildings;
    public BuildingSpawner RightBuildings;
    public RoadSpawner Road;
    public LevelGateSpawner LevelGates;
    public TrafficSpawner Traffic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetLevel()
    {
        Road.Clear();
        LevelGates.Clear();
        LeftBuildings.Clear();
        RightBuildings.Clear();
        Traffic.Clear();

        Road.SpawnForDistance(GameManager.LevelLimit);
        LevelGates.SpawnForDistance(GameManager.LevelLimit);
        LeftBuildings.SpawnForDistance(200);
        RightBuildings.SpawnForDistance(200);
    }

    public void ReSpawnAll()
    {
        Road.ReSpawn();
        LevelGates.ReSpawn();
        Traffic.ReSpawn();
        LeftBuildings.ReSpawn();
        RightBuildings.ReSpawn();
    }

}
