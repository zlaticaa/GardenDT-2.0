using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;



//Garden 2
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

    public BuildingPreview buildingPreview;
    public BuildingPreview floorPreview;

    private List<Building> allBuildings = new();
    private List<FloorBuilding> allFloors = new();


    //Fields for the touch system
    [SerializeField] private InputManager inputManager;

    private Vector2 currentTouchPosition;
    private bool touchReleasedThisFrame;

    private void Start()
    {
        InstantiateFloor();
    }

    public void ResetBuilds()
    {
        foreach (var building in allBuildings)
        {
            Destroy(building.gameObject);
        }

        foreach (var floor in allFloors)
        {
            Destroy(floor.gameObject);
        }
        allBuildings.Clear();
        allFloors.Clear();
        grid.CreateEmptyGrid();
        InstantiateFloor();
    }

    private void Update()
    {
        Vector3 pointerPos = GetCurrentPointerWorldPosition();

        if (buildingPreview != null)
        {
            HandleBuildingPreview(pointerPos);
        }
        else if (floorPreview != null)
        {
            HandleFloorPreview(pointerPos);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buildingPreview = CreateBuildingPreview(treeData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buildingPreview = CreateBuildingPreview(bushData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buildingPreview = CreateBuildingPreview(flowerData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buildingPreview = CreateBuildingPreview(vegGarData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buildingPreview = CreateBuildingPreview(trampData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                floorPreview = CreateFloorPreview(waterData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                floorPreview = CreateFloorPreview(gravelData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                floorPreview = CreateFloorPreview(noBuildData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                floorPreview = CreateFloorPreview(dirtData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                floorPreview = CreateFloorPreview(grassData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                floorPreview = CreateFloorPreview(sandData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                floorPreview = CreateFloorPreview(tileData, pointerPos);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                floorPreview = CreateFloorPreview(leakThroughTileData, pointerPos);
            }
        }

        touchReleasedThisFrame = false;
    }

    public void HandleBuildingPreview(Vector3 mouseWorldPos, bool force = false)
    {
        buildingPreview.transform.position = mouseWorldPos;
        List<Vector3> buildPositions = buildingPreview.buildingModel.GetAllBuildingPositions();

        bool canBuild = grid.CanBuildBuilding(buildPositions);
        if (canBuild)
        {
            buildingPreview.transform.position = GetSnappedCentrePosition(buildPositions);
            buildingPreview.ChangeState(Support.PreviewState.Positive);

            if (Input.GetMouseButtonDown(0) || touchReleasedThisFrame || force)
            {
                foreach (var vec in buildPositions)
                {
                    print("building position " + vec);
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

    public void SetBuilding(Vector3 pos, BuildingData data)
    {
        BuildingPreview preview = CreateBuildingPreview(data, pos);
        List<Vector3> buildPositions = preview.buildingModel.GetAllBuildingPositions();
        bool canBuild = grid.CanBuildBuilding(buildPositions);
        if (canBuild)
        {
            Building build = Instantiate(buildingPrefab, GetSnappedCentrePosition(buildPositions), Quaternion.identity);
            build.Setup(data);
            grid.SetBuilding(build, buildPositions);
            allBuildings.Add(build);
        }
    }

    public void SetFloor(Vector3 pos, FloorData data)
    {
       BuildingPreview preview = CreateFloorPreview(data, pos);
       List<Vector3> buildPositions = preview.buildingModel.GetAllBuildingPositions();
       FloorBuilding floor = Instantiate(floorBuildingPrefab, GetSnappedCentrePosition(buildPositions), Quaternion.identity);
       floor.Setup(data);
       floor.transform.position -= new Vector3(0f, 0.5f, 0f);
       grid.SetFloor(floor,buildPositions);
       allFloors.Add(floor);
    }
    public void HandleFloorPreview(Vector3 mouseWorldPos, bool force = false)
    {
        floorPreview.transform.position = mouseWorldPos;
        List<Vector3> buildPositions = floorPreview.buildingModel.GetAllBuildingPositions();
        bool canBuild = grid.CanBuildFloor(buildPositions);
        if (canBuild)
        {
            floorPreview.transform.position = GetSnappedCentrePosition(buildPositions);
            floorPreview.ChangeState(Support.PreviewState.Positive);
            if (Input.GetMouseButtonDown(0) || touchReleasedThisFrame || force)
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

    public BuildingPreview CreateBuildingPreview(BuildingData data, Vector3 position)
    {
        BuildingPreview buildingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        buildingPreview.Setup(data);
        return buildingPreview;
    }
    public BuildingPreview CreateFloorPreview(FloorData data, Vector3 position)
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
        Vector3 pointerPos = GetCurrentPointerWorldPosition();

        ClearCurrentPreview();

        buildingPreview = CreateBuildingPreview(data, pointerPos);
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
        Vector3 pointerPos = GetCurrentPointerWorldPosition();

        ClearCurrentPreview();

        floorPreview = CreateFloorPreview(dirtData, pointerPos);
        pillarMath.Recalculate();
    }


    //Methods for the touch system

    private bool HasTouchInput()
    {
       return currentTouchPosition != Vector2.zero;
        
    }

    private Vector3 GetCurrentPointerWorldPosition()
    {
        if (HasTouchInput())
        {
            return GetWorldTouchPosition();
        }

        return GetWorldMousePosition();
    }

    private Vector3 GetWorldTouchPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(currentTouchPosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    private void OnEnable()
    {
        if (inputManager == null) return;

        inputManager.onStartTouch += OnTouchStart;
        inputManager.onDrag += OnTouchDrag;
        inputManager.onEndTouch += OnTouchEnd;
    }

    private void OnDisable()
    {
        if (inputManager == null) return;

        inputManager.onStartTouch -= OnTouchStart;
        inputManager.onDrag -= OnTouchDrag;
        inputManager.onEndTouch -= OnTouchEnd;
    }

    private void OnTouchStart(Vector2 position, float time)
    {

        Debug.Log($"[BUILDING] Touch Start: {position}");
        currentTouchPosition = position;
    }

    private void OnTouchDrag(Vector2 position)
    {
        Debug.Log($"[BUILDING] Touch Drag: {position}");

        currentTouchPosition = position;
    }

    private void OnTouchEnd(Vector2 position, float time)
    {

        Debug.Log($"[BUILDING] Touch End: {position}");

        currentTouchPosition = position;
        touchReleasedThisFrame = true;
    }
}