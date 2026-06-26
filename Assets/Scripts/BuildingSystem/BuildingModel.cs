using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// A class to create building models.
/// This class is used for both buildings and floors.
/// </summary>
public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform wrapper;

    public float rotation => wrapper.transform.eulerAngles.y;
    private BuildingShapeUnit[] shapeUnits;

    private void Awake()
    {
        shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
        print(shapeUnits.Last().name);
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}