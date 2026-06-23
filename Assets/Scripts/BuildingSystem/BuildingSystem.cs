using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuildingSystem : MonoBehaviour
{
    public const float cellSize = 1f;

    [SerializeField] private PillarMath pillarMath;

    [SerializeField] private BuildingData treeData;
    [SerializeField] private BuildingData bushData;
    [SerializeField] private BuildingData flowerData;
    [SerializeField] private BuildingData vegGarData;
    [SerializeField] private BuildingData trampData;

    [SerializeField] private FloorData dirtData;
    [SerializeField] private FloorData grassData;
    [SerializeField] private FloorData gravelData;
    [SerializeField] private FloorData leakThroughTileData;
    [SerializeField] private FloorData sandData;
    [SerializeField] private FloorData tileData;
    [SerializeField] private FloorData waterData;
    [SerializeField] private FloorData noBuildData;

    [SerializeField] private BuildingPreview previewPrefab;
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private FloorBuilding floorBuildingPrefab;
    [SerializeField] private BuildingGrid grid;
    [SerializeField] private FloorBuilding standardFloor;

    private List<BuildingShapeUnit> shapeUnits = new();

    private BuildingPreview buildingPreview;
    private BuildingPreview floorPreview;

    private List<Building> allBuildings = new();
    private List<FloorBuilding> allFloors = new();

    private void Start()
    {
        InstantiateFloor();
    }

    private void Update()
    {
        Vector3 mousePos = GetWorldMousePosition();

        if(buildingPreview != null)
        {
            HandleBuildingPreview(mousePos);
        }
        else if(floorPreview != null)
        {
            HandleFloorPreview(mousePos);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buildingPreview = CreateBuildingPreview(treeData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buildingPreview = CreateBuildingPreview(bushData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buildingPreview = CreateBuildingPreview(flowerData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buildingPreview = CreateBuildingPreview(vegGarData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buildingPreview = CreateBuildingPreview(trampData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                floorPreview = CreateFloorPreview(waterData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                floorPreview = CreateFloorPreview(gravelData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                floorPreview = CreateFloorPreview(noBuildData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                floorPreview = CreateFloorPreview(dirtData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                floorPreview = CreateFloorPreview(grassData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                floorPreview = CreateFloorPreview(sandData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                floorPreview = CreateFloorPreview(tileData, mousePos);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                floorPreview = CreateFloorPreview(leakThroughTileData, mousePos);
            }
        }
    }

    private void HandleBuildingPreview(Vector3 mouseWorldPos)
    {
        buildingPreview.transform.position = mouseWorldPos;
        List<Vector3> buildPositions = buildingPreview.buildingModel.GetAllBuildingPositions();

        bool canBuild = grid.CanBuildBuilding(buildPositions);
        if (canBuild)
        {
            buildingPreview.transform.position = GetSnappedCentrePosition(buildPositions);
            buildingPreview.ChangeState(Support.PreviewState.Positive);
            if (Input.GetMouseButtonDown(0))
            {
                foreach(var vec in buildPositions)
                {
                    print("building position" + vec);
                }
                PlaceBuilding(buildPositions);
            }
        }
        else
        {
            buildingPreview.ChangeState(Support.PreviewState.Negative);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(buildingPreview.gameObject);
            buildingPreview = null;
        }
    }

    private void HandleFloorPreview(Vector3 mouseWorldPos)
    {
        floorPreview.transform.position = mouseWorldPos;
        List<Vector3> buildPositions = floorPreview.buildingModel.GetAllBuildingPositions();
        bool canBuild = grid.CanBuildFloor(buildPositions);
        if (canBuild)
        {
            floorPreview.transform.position = GetSnappedCentrePosition(buildPositions);
            floorPreview.ChangeState(Support.PreviewState.Positive);
            if (Input.GetMouseButtonDown(0))
            {
                PlaceFloor(buildPositions);
            }
        }
        else
        {
            floorPreview.ChangeState(Support.PreviewState.Negative);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(floorPreview.gameObject);
            floorPreview = null;
        }
    }

    private void PlaceBuilding(List<Vector3> buildingPositions)
    {
        Building building = Instantiate(buildingPrefab, buildingPreview.transform.position, Quaternion.identity);
        building.Setup(buildingPreview.buildingData);
        shapeUnits.Add(building.GetComponentInChildren<BuildingShapeUnit>());
        grid.SetBuilding(building, buildingPositions);
        Destroy(buildingPreview.gameObject);
        buildingPreview = null;

        allBuildings.Add(building);
    }

    private void PlaceFloor(List<Vector3> buildingPositions)
    {
        DestroyObject();

        FloorBuilding floorBuilding = Instantiate(floorBuildingPrefab, floorPreview.transform.position, Quaternion.identity);
        floorBuilding.Setup(floorPreview.floorData);
        floorBuilding.transform.position -= new Vector3(0, 0.5f, 0);
        shapeUnits.Add(floorBuilding.GetComponentInChildren<BuildingShapeUnit>());
        grid.SetFloor(floorBuilding, buildingPositions);
        if(floorPreview.floorData.name == "NoBuildData")
        {
            GameObject fakeBuild = new();
            fakeBuild.AddComponent<Building>();
            fakeBuild.transform.SetParent(floorBuilding.transform);
            grid.SetBuilding(fakeBuild.GetComponent<Building>(), buildingPositions);
        }
        Destroy(floorPreview.gameObject);
        floorPreview = null;

        allFloors.Add(floorBuilding);
    }

    private GameObject GetHitObject()
    {
        GameObject hitObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            var hitTransform = hit.transform;
            int count = 0;
            while (hitTransform != null && hitTransform.gameObject.GetComponent<FloorBuilding>() == null && count < 10)
            {
                Debug.Log(hitTransform);
                hitTransform = hitTransform.parent;
                count++;
            }
            hitObject = hitTransform?.gameObject;
        }
        Debug.Log("Hit Object: " +  hitObject.name);
        return hitObject;
    }

    private void DestroyObject()
    {
        GameObject objectToDestroy = GetHitObject();
        Debug.Log(objectToDestroy);
        if(objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
        else
        {
            Debug.Log("No object to destroy");
        }
    }

    private void InstantiateFloor()
    {
        for (int x = 0; x < Support.GridWidth; x++)
        {
            for (int y = 0; y < Support.GridHeight; y++)
            {
                FloorBuilding newObject = Instantiate(standardFloor, new Vector3(x * BuildingSystem.cellSize + 0.5f, -0.5f, y * BuildingSystem.cellSize + 0.5f), Quaternion.identity);
                newObject.transform.SetParent(transform);
                newObject.Setup(grassData);


                allFloors.Add(newObject);
            }
        }

        Debug.Log(allFloors);
        grid.DrawLineGrid();
    }

    private Vector3 GetSnappedCentrePosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> zs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();
        float centreX = (xs.Min() + xs.Max()) / 2f + cellSize / 2f;
        float centreZ = (zs.Min() + zs.Max()) / 2f + cellSize / 2f;
        return new(centreX, 0, centreZ);
    }

    private Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new(Vector3.up, Vector3.zero);
        if(groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private BuildingPreview CreateBuildingPreview(BuildingData data, Vector3 position)
    {
        BuildingPreview buildingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        buildingPreview.Setup(data);
        return buildingPreview;
    }
    private BuildingPreview CreateFloorPreview(FloorData data, Vector3 position)
    {
        BuildingPreview floorPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        floorPreview.Setup(data);
        return floorPreview;
    }

    public List<Building> GetAllBuildings()
    {
        return allBuildings;
    }

    public List<FloorBuilding> GetAllFloors()
    {
        return allFloors;
    }

    private List<Vector3> GetBuildingPositions(List<BuildingShapeUnit> shapeUnits)
    {
        return shapeUnits.Select(unit => unit.transform.position).ToList();
    }

    private void ClearCurrentPreview()
    {
        if (buildingPreview != null)
        {
            HandleBuildingPreview(GetWorldMousePosition());
        }
        else if (floorPreview != null)
        {
            HandleFloorPreview(GetWorldMousePosition());
        }
    }

    private void SpawnBuilding(BuildingData data)
    {
        Vector3 mousePos = GetWorldMousePosition();

        ClearCurrentPreview();

        buildingPreview = CreateBuildingPreview(data, mousePos);
        pillarMath.Recalculate();
    }

    public void SpawnTree()
    {
        SpawnBuilding(treeData);
    }

    public void SpawnBushe()
    {
        SpawnBuilding(bushData);
    }

    public void SpawnFlower()
    {
        SpawnBuilding(flowerData);
    }

    public void SpawnVegatable()
    {
        SpawnBuilding(vegGarData);
    }

    public void SpawnTramp()
    {
        SpawnBuilding(trampData);
    }

    public void SpawnFloor()
    {
        Vector3 mousePos = GetWorldMousePosition();

        ClearCurrentPreview();

        floorPreview = CreateFloorPreview(dirtData, mousePos);
        pillarMath.Recalculate();
    }
}