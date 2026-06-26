using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for the floor block objects
/// </summary>
public class FloorBuilding : MonoBehaviour
{
    private BuildingModel model;
    private FloorData data;
    public void Setup(FloorData data)
    {
        this.data = data;
        model = Instantiate(data.floorModel, transform.position, Quaternion.identity, transform);
    }

    public FloorData GetData()
    {
        return data;
    }
} 