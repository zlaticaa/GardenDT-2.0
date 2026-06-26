using UnityEngine;


[CreateAssetMenu(menuName = "Data/Building")]

// A ScriptableObject class to create data for each Building
public class BuildingData : ScriptableObject
{
    // The model to use when instantiating.
    [field: SerializeField] public BuildingModel buildingModel { get; private set; }
    // The floortype. This will be used to filter all types of floors for the pillarmath.
    [field: SerializeField] public Support.FloorType floorType { get; private set; }
}