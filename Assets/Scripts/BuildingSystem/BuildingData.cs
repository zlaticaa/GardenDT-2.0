using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public BuildingModel buildingModel { get; private set; }
    [field: SerializeField] public Support.FloorType floorType { get; private set; }
}