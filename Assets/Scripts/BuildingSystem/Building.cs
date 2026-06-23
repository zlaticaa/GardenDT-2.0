using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    private BuildingModel model;
    private BuildingData data;
    public void Setup(BuildingData data)
    {
        this.data = data;
        model = Instantiate(data.buildingModel, transform.position, Quaternion.identity, transform);
    }

    public BuildingData GetData()
    {
        return data;
    }
}