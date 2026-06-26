using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Handles the placing of buildings and floors
/// </summary>
public class BuildingSystem : MonoBehaviour
{
    public const float cellSize = 1f;

    // Math component
    [SerializeField] private PillarMath pillarMath;

    // Building Data
    [SerializeField] private BuildingData treeData;
    [SerializeField] private BuildingData bushData;
    [SerializeField] private BuildingData flowerData;
    [SerializeField] private BuildingData vegGarData;
    [SerializeField] private BuildingData trampData;

    // Floor Data
    [SerializeField] private FloorData dirtData;
    [SerializeField] private FloorData grassData;
    [SerializeField] private FloorData gravelData;
    [SerializeField] private FloorData leakThroughTileData;
    [SerializeField] private FloorData sandData;
    [SerializeField] private FloorData tileData;
    [SerializeField] private FloorData waterData;
    [SerializeField] private FloorData noBuildData;

    // Objects necessary for handling
    [SerializeField] private BuildingPreview previewPrefab;
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private FloorBuilding floorBuildingPrefab;
    [SerializeField] private BuildingGrid grid;
    [SerializeField] private FloorBuilding standardFloor;

    // All shapeunits
    private List<BuildingShapeUnit> shapeUnits = new();

    // Preview handlers
    public BuildingPreview buildingPreview;
    public BuildingPreview floorPreview;

    // Lists to save all placed objects
    private List<Building> allBuildings = new();
    private List<FloorBuilding> allFloors = new();


    //Fields for the touch system
    [SerializeField] private InputManager inputManager;
    private Vector2 currentTouchPosition;
    private bool touchReleasedThisFrame;

    /// <summary>
    /// At the start of this script, the floor is instantiated.
    /// </summary>
    private void Start()
    {
        InstantiateFloor();
        pillarMath.ResetMath();
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

    /// <summary>
    /// This method is called every tick
    /// </summary>
    private void Update()
    {
        // Get the current mouse position in the world
        Vector3 pointerPos = GetCurrentPointerWorldPosition();

        // If a buildingPreview exists, handle buildingPreview
        if (buildingPreview != null)
        {
            HandleBuildingPreview(pointerPos);
        }
        // If a floorPreview exists, handle floorPreview
        else if (floorPreview != null)
        {
            HandleFloorPreview(pointerPos);
        }
        // Check if a specific key is pressed. This only works on keyboard and is mainly used for testing and debugging
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

    /// <summary>
    /// Handles the building previews
    /// </summary>
    /// <param name="mouseWorldPos"></param>
    /// <param name="force"></param>
    public void HandleBuildingPreview(Vector3 mouseWorldPos, bool force = false)
    {
        // Get mousepostition and all building positions
        buildingPreview.transform.position = mouseWorldPos;
        List<Vector3> buildPositions = buildingPreview.buildingModel.GetAllBuildingPositions();

        // Check if a building can be placed
        bool canBuild = grid.CanBuildBuilding(buildPositions);
        if (canBuild)
        {
            // If can be placed, snap object to centre positions in the grid
            buildingPreview.transform.position = GetSnappedCentrePosition(buildPositions);
            buildingPreview.ChangeState(Support.PreviewState.Positive);

            // Place the building upon input
            if (Input.GetMouseButtonDown(0) || touchReleasedThisFrame || force)
            {
                foreach (var vec in buildPositions)
                {
                    print("building position " + vec);
                }

                PlaceBuilding(buildPositions);
            }
        }
        // Change preview state to negative if can't be placed
        else
        {
            buildingPreview.ChangeState(Support.PreviewState.Negative);
        }
        

        // Delete preview and gameobject on input
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(buildingPreview.gameObject);
            buildingPreview = null;
        }
    }
    
    public bool SetBuilding(Vector3 pos, BuildingData data)
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
            Destroy(preview.gameObject);
            return true;
        }
        Destroy(preview.gameObject);
        return false;

    }

    public void SetFloor(Vector3 pos, FloorData data)
    {
       BuildingPreview preview = CreateFloorPreview(data, pos);
       List<Vector3> buildPositions = preview.buildingModel.GetAllBuildingPositions();
       
       DestroyObject(buildPositions);
       FloorBuilding floor = Instantiate(floorBuildingPrefab, GetSnappedCentrePosition(buildPositions), Quaternion.identity);
       floor.Setup(data);
       floor.transform.position -= new Vector3(0f, 0.5f, 0f);
       grid.SetFloor(floor,buildPositions);
       allFloors.Add(floor);
       
       Destroy(preview.gameObject);
    }
    /// <summary>
    /// Handles the floor preview
    /// </summary>
    /// <param name="mouseWorldPos"></param>
    /// <param name="force"></param>
    public void HandleFloorPreview(Vector3 mouseWorldPos, bool force = false)
    {
        // Get mousepostition and all building positions
        floorPreview.transform.position = mouseWorldPos;
        List<Vector3> buildPositions = floorPreview.buildingModel.GetAllBuildingPositions();

        // Check if a building can be placed
        bool canBuild = grid.CanBuildFloor(buildPositions);
        if (canBuild)
        {
            // If can be placed, snap object to centre positions in the grid
            floorPreview.transform.position = GetSnappedCentrePosition(buildPositions);
            floorPreview.ChangeState(Support.PreviewState.Positive);

            // Place the building upon input
            if (Input.GetMouseButtonDown(0) || touchReleasedThisFrame || force)
            {
                PlaceFloor(buildPositions);
            }
        }
        // Change preview state to negative if can't be placed
        else
        {
            floorPreview.ChangeState(Support.PreviewState.Negative);
        }

        // Delete preview and gameobject on input
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(floorPreview.gameObject);
            floorPreview = null;
        }
    }

    /// <summary>
    /// Places buildings
    /// </summary>
    /// <param name="buildingPositions"></param>
    private void PlaceBuilding(List<Vector3> buildingPositions)
    {
        // Instantiate Building
        Building building = Instantiate(buildingPrefab, buildingPreview.transform.position, Quaternion.identity);
        
        // Setup instance
        building.Setup(buildingPreview.buildingData);
        
        // Add shapeunits to list
        shapeUnits.Add(building.GetComponentInChildren<BuildingShapeUnit>());

        // Set the Building on the grid
        grid.SetBuilding(building, buildingPositions);

        // Detroy the preview gameobject
        Destroy(buildingPreview.gameObject);

        // Set preview to be empty
        buildingPreview = null;

        //Add Building to all buildings list
        allBuildings.Add(building);
    }

    private void PlaceFloor(List<Vector3> buildingPositions)
    {
        DestroyObject(buildingPositions);

        // Instantiate Floor
        FloorBuilding floorBuilding = Instantiate(floorBuildingPrefab, floorPreview.transform.position, Quaternion.identity);

        // Setup instance
        floorBuilding.Setup(floorPreview.floorData);

        // Change transform to be lower to fit the position of the floor
        floorBuilding.transform.position -= new Vector3(0, 0.5f, 0);

        // Add shapeunits to list
        shapeUnits.Add(floorBuilding.GetComponentInChildren<BuildingShapeUnit>());

        // Set the Floor on the grid
        grid.SetFloor(floorBuilding, buildingPositions);

        // Check if the building is a no build building and add a fake empty building on top to seal the shapeunits
        if(floorPreview.floorData.name == "NoBuildData")
        {
            GameObject fakeBuild = new();
            fakeBuild.AddComponent<Building>();
            fakeBuild.transform.SetParent(floorBuilding.transform);
            grid.SetBuilding(fakeBuild.GetComponent<Building>(), buildingPositions);
        }

        // Destroy the preview gameobject
        Destroy(floorPreview.gameObject);

        // Set the preview to be empty
        floorPreview = null;

        // Add Floor to all floors list
        allFloors.Add(floorBuilding);
    }
     /// <summary>
     /// Gets the floorpiece that needs to be deleted
     /// </summary>
     /// <returns></returns>
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
    
    /// <summary>
    /// Destroys the gameobject that's underneath the mouse position
    /// </summary>
    private void DestroyObject(List<Vector3> buildPositions)
    {
        
        foreach (Vector3 checkPos in buildPositions)
        {
            
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(math.floor(checkPos.x)+cellSize*0.5f, checkPos.y, math.floor(checkPos.z) + cellSize*0.5f), 0.4f,LayerMask.GetMask("Default"));
            foreach (Collider hitCollider in hitColliders)
            {
                Debug.Log(hitCollider.gameObject.name);
                allFloors.Remove(hitCollider.gameObject.GetComponent<FloorBuilding>());
                if(hitCollider.gameObject != null)
                {
                    Destroy(hitCollider.gameObject);
                }
                else
                {
                    Debug.Log("No object to destroy");
                }
            }
        }
       
    }

    /// <summary>
    /// Instantiates floor grid
    /// </summary>
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

    /// <summary>
    /// Snaps previews to centre position
    /// </summary>
    /// <param name="allBuildingPositions"></param>
    /// <returns></returns>
    private Vector3 GetSnappedCentrePosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> zs = allBuildingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();
        float centreX = (xs.Min() + xs.Max()) / 2f + cellSize / 2f;
        float centreZ = (zs.Min() + zs.Max()) / 2f + cellSize / 2f;
        return new(centreX, 0, centreZ);
    }

    /// <summary>
    /// Gets the mouse position in relation to the world
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Creates buildingpreview with correct data
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public BuildingPreview CreateBuildingPreview(BuildingData data, Vector3 position)
    {
        BuildingPreview buildingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        buildingPreview.Setup(data);
        return buildingPreview;
    }

    /// <summary>
    /// Creates floorpreview with correct data
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public BuildingPreview CreateFloorPreview(FloorData data, Vector3 position)
    {
        BuildingPreview floorPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        floorPreview.Setup(data);
        return floorPreview;
    }

    /// <summary>
    /// Public getter for allBuildings
    /// </summary>
    /// <returns></returns>
    public List<Building> GetAllBuildings()
    {
        return allBuildings;
    }

    /// <summary>
    /// Public getter for allFloors
    /// </summary>
    /// <returns></returns>
    public List<FloorBuilding> GetAllFloors()
    {
        return allFloors;
    }

    /// <summary>
    /// Clears current preview object
    /// </summary>
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

    /// <summary>
    /// Spawns Building with correct data
    /// </summary>
    /// <param name="data"></param>
    private void SpawnBuilding(BuildingData data)
    {
        Vector3 pointerPos = GetCurrentPointerWorldPosition();

        ClearCurrentPreview();

        buildingPreview = CreateBuildingPreview(data, pointerPos);
        pillarMath.Recalculate();
    }

    /// <summary>
    /// Spawns tree in scene
    /// </summary>
    public void SpawnTree()
    {
        SpawnBuilding(treeData);
    }

    /// <summary>
    /// Spawns bushes in scene
    /// </summary>
    public void SpawnBushe()
    {
        SpawnBuilding(bushData);
    }

    /// <summary>
    /// Spawns flowers in scene
    /// </summary>
    public void SpawnFlower()
    {
        SpawnBuilding(flowerData);
    }

    /// <summary>
    /// Spawns vegetable garden in scene
    /// </summary>
    public void SpawnVegatable()
    {
        SpawnBuilding(vegGarData);
    }

    /// <summary>
    /// Spawns trampoline in scene
    /// </summary>
    public void SpawnTramp()
    {
        SpawnBuilding(trampData);
    }

    /// <summary>
    /// Spawns Floor with correct data
    /// </summary>
    /// <param name="data"></param>
    public void SpawnFloor(FloorData data)
    {
        Vector3 pointerPos = GetCurrentPointerWorldPosition();

        ClearCurrentPreview();

        floorPreview = CreateFloorPreview(data, pointerPos);
        pillarMath.Recalculate();
    }

    /// <summary>
    /// Spawns dirt in scene
    /// </summary>
    public void SpawnDirt()
    {
        SpawnFloor(dirtData);
    }

    /// <summary>
    /// Spawns water in scene
    /// </summary>
    public void SpawnWater()
    {
        SpawnFloor(waterData);
    }

    /// <summary>
    /// Spawns gravel in scene
    /// </summary>
    public void SpawnGravel()
    {
        SpawnFloor(gravelData);
    }

    /// <summary>
    /// Spawns no build in scene
    /// </summary>
    public void SpawnNoBuild()
    {
        SpawnFloor(noBuildData);
    }

    /// <summary>
    /// Spawns grass in scene
    /// </summary>
    public void SpawnGrass()
    {
        SpawnFloor(grassData);
    }

    /// <summary>
    /// Spawns sand in scene
    /// </summary>
    public void SpawnSand()
    {
        SpawnFloor(sandData);
    }

    /// <summary>
    /// Spawns tiles in scene
    /// </summary>
    public void SpawnTile()
    {
        SpawnFloor(tileData);
    }

    /// <summary>
    /// Spawns leak through tiles in scene
    /// </summary>
    public void SpawnLeak()
    {
        SpawnFloor(leakThroughTileData);
    }

    //Methods for the touch system
    /// <summary>
    /// Checks if there is touch input
    /// </summary>
    /// <returns></returns>
    private bool HasTouchInput()
    {
       return currentTouchPosition != Vector2.zero;
        
    }

    /// <summary>
    /// Gets mouse position in world based on whether there is touch input
    /// </summary>
    /// <returns></returns>
    private Vector3 GetCurrentPointerWorldPosition()
    {
        if (HasTouchInput())
        {
            return GetWorldTouchPosition();
        }

        return GetWorldMousePosition();
    }

    /// <summary>
    /// Gets touch input position in relation to the world
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Adds touchpositions on enable
    /// </summary>
    private void OnEnable()
    {
        if (inputManager == null) return;

        inputManager.onStartTouch += OnTouchStart;
        inputManager.onDrag += OnTouchDrag;
        inputManager.onEndTouch += OnTouchEnd;
    }

    /// <summary>
    /// SUbstracts touchpostion on disable
    /// </summary>
    private void OnDisable()
    {
        if (inputManager == null) return;

        inputManager.onStartTouch -= OnTouchStart;
        inputManager.onDrag -= OnTouchDrag;
        inputManager.onEndTouch -= OnTouchEnd;
    }

    /// <summary>
    /// Updates touch position on start
    /// </summary>
    /// <param name="position"></param>
    /// <param name="time"></param>
    private void OnTouchStart(Vector2 position, float time)
    {

        Debug.Log($"[BUILDING] Touch Start: {position}");
        currentTouchPosition = position;
    }

    /// <summary>
    /// Updates touch position on drag
    /// </summary>
    /// <param name="position"></param>
    private void OnTouchDrag(Vector2 position)
    {
        Debug.Log($"[BUILDING] Touch Drag: {position}");

        currentTouchPosition = position;
    }

    /// <summary>
    /// Updates touch position on end and sets touchReleasedThisFrame true
    /// </summary>
    /// <param name="position"></param>
    /// <param name="time"></param>
    private void OnTouchEnd(Vector2 position, float time)
    {

        Debug.Log($"[BUILDING] Touch End: {position}");

        currentTouchPosition = position;
        touchReleasedThisFrame = true;
    }
}