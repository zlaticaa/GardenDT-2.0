using System;
using UnityEngine;

/// <summary>
/// A scriptable object class to create data for the floors
/// </summary>
[CreateAssetMenu(menuName = "Data/Floor")]
public class FloorData : ScriptableObject
{
    [field: SerializeField] public BuildingModel floorModel { get; private set; }
    [field: SerializeField] public Support.FloorType floorType { get; private set; }
}