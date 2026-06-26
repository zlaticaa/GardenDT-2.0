using UnityEngine;

/// <summary>
/// A scriptable object class to create data for the buildings
/// </summary>
[CreateAssetMenu(menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    // The model to use when instantiating.
    [field: SerializeField] public BuildingModel buildingModel { get; private set; }
    // The floortype. This will be used to filter all types of floors for the pillarmath.
    [field: SerializeField] public Support.FloorType floorType { get; private set; }
}