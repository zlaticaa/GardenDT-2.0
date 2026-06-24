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
    private int observationsCounter = 0;
    private PillarMath mathComponent;
    private List<Vector3> allBuildings = new();
    private List<Vector3> allFloors = new();
    [SerializeField] private float scoreBase = 10;
    [SerializeField] private float pillar1Mult;
    [SerializeField] private float pillar2Mult;
    [SerializeField] private float pillar3Mult;
    [SerializeField] private float pillar4Mult;

    public override void CollectObservations(VectorSensor sensor)
    {
        mathComponent.Recalculate();
        sensor.AddObservation(mathComponent.Pillar1Score);
        sensor.AddObservation(mathComponent.Pillar2Score);
        sensor.AddObservation(mathComponent.Pillar3Score);
        sensor.AddObservation(mathComponent.Pillar4Score);
        
        
        
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
            }
        }
        else
        {
            throw new NullReferenceException("Error: list allBuildings is void.");
        }

        if (mathComponent.GetAllFloors() != null)
        {
            List<FloorBuilding> tempList = mathComponent.GetAllFloors();
            foreach (FloorBuilding floorBuilding in tempList)
            {
                allFloors.Add(floorBuilding.gameObject.transform.position);
            }
        }
        else
        {
            throw new NullReferenceException("Error: list allBuildings is void.");
        }
    }
}