using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GardenAgent : Agent
{
    private PillarMath mathComponent;
    private List<Vector3> allBuildings = new();
    private List<int> allBuildingIds = new();
    private List<Vector3> allFloors = new();
    private List<int> allFloorIds = new();

    [SerializeField] private BuildingSystem buildSystem;
    [SerializeField] private List<BuildingData> buildings = new();
    [SerializeField] private List<FloorData> floors = new();
    [SerializeField] private float scoreBase = 10;
    [SerializeField] private float pillar1Mult;
    [SerializeField] private float pillar2Mult;
    [SerializeField] private float pillar3Mult;
    [SerializeField] private float pillar4Mult;
    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;

    private int steps;
    private const int MAX_STEPS = 300;
    private int totalTiles;

    public void Start()
    {
        totalTiles = height* width;
        steps = 0;
    }

    public override void OnEpisodeBegin()
    {
        steps = 0;
        allBuildings.Clear();
        allFloors.Clear();
        buildSystem.ResetBuilds();
        mathComponent.Recalculate();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        mathComponent.Recalculate();
        sensor.AddObservation(mathComponent.Pillar1Score);
        sensor.AddObservation(mathComponent.Pillar2Score);
        sensor.AddObservation(mathComponent.Pillar3Score);
        sensor.AddObservation(mathComponent.Pillar4Score);

        for (int i = 0; i < totalTiles; i++)
        {
            if (allBuildings != null)
            {
                if (allBuildingIds.Count <= i && allBuildings.Count <= i)
                {
                    sensor.AddObservation(allBuildingIds[i]);
                    sensor.AddObservation(allBuildings[i]);
                }
            }

            if (allFloors != null)
            {
                if (allFloors.Count <= i && allFloors.Count <= i)
                {
                    sensor.AddObservation(allFloors[i]);
                    sensor.AddObservation(allFloorIds[i]);
                }
            }
        }
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        steps++;
        float StartP1 = mathComponent.Pillar1Score;
        float StartP2 = mathComponent.Pillar2Score;
        float StartP3 = mathComponent.Pillar3Score;
        float StartP4 = mathComponent.Pillar4Score;
        
        Vector3 pos = new Vector3(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2]);
        int buildingId = actions.DiscreteActions[3];
        int floorId = actions.DiscreteActions[4];

        if(buildingId >= 0 && buildingId < buildings.Count) buildSystem.SetBuilding(pos, buildings[buildingId]);
        if(floorId >= 0 && floorId < floors.Count) buildSystem.SetFloor(pos - new Vector3(0f,-0.5f,0f), floors[floorId]);
        mathComponent.Recalculate();
        float p1Reward = scoreBase * (mathComponent.Pillar1Score - StartP1) * pillar1Mult;
        float p2Reward = scoreBase * (mathComponent.Pillar2Score - StartP2) * pillar2Mult;
        float p3Reward = scoreBase * (mathComponent.Pillar3Score - StartP3) * pillar3Mult;
        float p4Reward = scoreBase * (mathComponent.Pillar4Score - StartP4) * pillar4Mult;
        SetReward(p1Reward+p2Reward+p3Reward+p4Reward);
        if(steps > MAX_STEPS) EndEpisode();
    }

    private void CheckLists()
    {
        if(mathComponent.GetAllBuildings() != null)
        {
            List<Building> tempList = mathComponent.GetAllBuildings();
            foreach(Building building in tempList)
            {
                allBuildings.Add(building.gameObject.transform.position);
                for (int i = 0; i < buildings.Count; i++)
                {
                   if(building.GetData() == buildings[i]) allBuildingIds.Add(i);
                }
            }
        }
        else
        {
            throw new NullReferenceException("Error: list allBuildings is void.");
        }

        if (mathComponent.GetAllFloors() != null)
        {
            List<FloorBuilding> tempList = mathComponent.GetAllFloors();
            foreach (FloorBuilding floor in tempList)
            {
                allFloors.Add(floor.gameObject.transform.position);
                for (int i = 0; i < floors.Count; i++)
                {
                    if(floor.GetData() == floors[i]) allFloorIds.Add(i);
                }
            }
        }
        else
        {
            throw new NullReferenceException("Error: list allBuildings is void.");
        }
    }
}