using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Floor")]
public class FloorData : ScriptableObject
{
    [field: SerializeField] public BuildingModel floorModel { get; private set; }
    [field: SerializeField] public Support.FloorType floorType { get; private set; }
}