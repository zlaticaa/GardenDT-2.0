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
    [SerializeField] private List<BuildingData> buildings = new();
    [SerializeField] private List<FloorData> floors = new();
    [SerializeField] private float scoreBase = 10;
    [SerializeField] private float pillar1Mult;
    [SerializeField] private float pillar2Mult;
    [SerializeField] private float pillar3Mult;
    [SerializeField] private float pillar4Mult;

    private int steps;
    private const int MAX_STEPS = 300;
    private const int totalTiles = 3 * 3;

    public void Start()
    {
        steps = 0;
    }

    public override void OnEpisodeBegin()
    {
        steps = 0;
        allBuildings.Clear();
        allFloors.Clear();
        //add reload floor/grid
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
            if (allBuildingIds.Count <= i && allBuildings.Count <= i)
            {
                sensor.AddObservation(allBuildingIds[i]);
                sensor.AddObservation(allBuildings[i]);
            }

            if (allFloors.Count <= i && allFloors.Count <= i)
            {
                sensor.AddObservation(allFloors[i]);
                sensor.AddObservation(allFloorIds[i]);
            }
        }
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Benodigde acties:
        //  plaats boom, bosje, bloem, moestuin, trampoline, tegels, grind, doorlektegels, zand, grond, gras, water -> discrete actie

        // Pseudo
        // float actionTree = actionBuffers.DiscreetActions[0];
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