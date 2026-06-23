using System;
using System.Collections.Generic;
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

    public override void CollectObservations(VectorSensor sensor)
    {
        CheckLists();

        foreach (var building in allBuildings)
        {
            sensor.AddObservation(building);
            observationsCounter++;
        }

        foreach(var floor in allFloors)
        {
            sensor.AddObservation(floor);
            observationsCounter++;
        }
        Debug.Log("Nr of observations: " + observationsCounter);

        this.gameObject.GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize = observationsCounter;
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