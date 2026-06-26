using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for all the formulas per pillar
/// </summary>
public class PillarMath : MonoBehaviour
{
    [SerializeField] private BuildingSystem bSystem; private List<Building> allBuildings = new();
    [SerializeField] private List<FloorBuilding> allFloors = new();

    // Integers for total number of each floorType
    private int nrOfPaved;
    private int nrOfLeakThrough;
    private int nrOfUnpaved;
    private int nrOfGrass;
    private int nrOfFlower;
    private int nrOfBush;
    private int nrOfTree;
    private int totalArea;
    private int totalPlants;

    // Variables for fertilizerType
    public fertilizerCleanupType fertilizer;
    public fertilizerCleanupType cleanupStyle;

    // Booleans for animals
    public bool Insects;
    public bool Birds;
    public bool Spiders;
    public bool OtherAnimals;

    // Integet for different plant types
    public int nrOfPlantTypes;

    //Variables to be displayed on the UI
    private float pillar1Score;
    private float pillar2Score;
    private float pillar3Score;
    private float pillar4Score;

    public void Start()
    {
        Setup();
    }

    /// <summary>
    /// Reloads and calculates all variables and pillars
    /// </summary>
    private void ReloadAndCalculate()
    {
        Insects = PillarSettings.Insects;
        Birds = PillarSettings.Birds;
        Spiders = PillarSettings.Spiders;
        OtherAnimals = PillarSettings.OtherAnimals;
        fertilizer = PillarSettings.fertilizer; 
        cleanupStyle = PillarSettings.cleanup;
        nrOfPlantTypes = PillarSettings.nrOfPlants;

        allBuildings = bSystem.GetAllBuildings();
        allFloors = bSystem.GetAllFloors();

        allFloors = FilterGrassWithoutPlants();

        GetAllNumbers(allBuildings, allFloors);
        CalculateAllPillars();
        
        Debug.Log((allBuildings.Count+ allFloors.Count) + " = " + totalArea + "; " + (totalPlants/totalArea));
        
        FindFirstObjectByType<PillarUI>()?.UpdateUI();
    }

    /// <summary>
    /// Setup method to be called at the beginning of the scene
    /// </summary>
    private void Setup()
    {

        ReloadAndCalculate();
    }

    /// <summary>
    /// Calculates pillar 1
    /// </summary>
    public void CalculatePillar1()
    {
         pillar1Score = StaticFormulas.P1Water(GetTotalArea(), nrOfPaved, nrOfLeakThrough, nrOfUnpaved, nrOfFlower, nrOfGrass, nrOfBush, nrOfTree);
    }

    /// <summary>
    /// Calculates pillar 2
    /// </summary>
    public void CalculatePillar2()
    {

         pillar2Score = StaticFormulas.P2Soil(fertilizer, cleanupStyle,totalPlants,totalArea);
    }

    /// <summary>
    /// Calculates pillar 3
    /// </summary>
    public void CalculatePillar3()
    {
        pillar3Score = StaticFormulas.P3Environment(Insects, Birds, Spiders, OtherAnimals, totalPlants, totalArea);
    }

    /// <summary>
    /// Calculates pillar 4
    /// </summary>
    public void CalculatePillar4()
    {
        pillar4Score = StaticFormulas.P4PlantDiversity(nrOfFlower, nrOfGrass, nrOfBush, nrOfTree, totalArea, nrOfPlantTypes);
    }

    /// <summary>
    /// Gets total area
    /// </summary>
    /// <returns></returns>
    private int GetTotalArea()
    {
        totalArea = Support.GridHeight * Support.GridWidth;

        return totalArea;
    }

    // Set all pillar scores
    public float Pillar1Score => pillar1Score;
    public float Pillar2Score => pillar2Score;
    public float Pillar3Score => pillar3Score;
    public float Pillar4Score => pillar4Score;

    /// <summary>
    /// Runs all calculate pillar methods
    /// </summary>
    private void CalculateAllPillars()
    {
        CalculatePillar1();
        CalculatePillar2();
        CalculatePillar3();
        CalculatePillar4();
    }

    /// <summary>
    /// Invokes calculation everytime a user adds an object to the scene
    /// </summary>
    public void Recalculate()
    {
        ReloadAndCalculate();
    }

    /// <summary>
    /// Filters floor blocks that don't have plants on top
    /// </summary>
    /// <returns></returns>
    private List<FloorBuilding> FilterGrassWithoutPlants()
    {
        List<FloorBuilding> allFloorsCopy = GetCopyAllFloors();

        foreach (Building building in allBuildings)
        {
            foreach (FloorBuilding floor in allFloors)
            {
                if (floor == null)
                    continue;

                if ((building.transform.position - new Vector3(0, 0.5f, 0)) == floor.transform.position)
                {
                    allFloorsCopy.Remove(floor);
                }

            }
        }

        return allFloorsCopy;
    }

    /// <summary>
    /// Gets a copy of the allFloors list
    /// </summary>
    /// <returns></returns>
    private List<FloorBuilding> GetCopyAllFloors()
    {
        List<FloorBuilding> builings = new();
        foreach (FloorBuilding b in allFloors)
        {
            builings.Add(b);
        }

        return builings;
    }

    /// <summary>
    /// Gets all total numbers
    /// </summary>
    /// <param name="allBuildings"></param>
    /// <param name="allFloorsCopy"></param>
    private void GetAllNumbers(List<Building> allBuildings, List<FloorBuilding> allFloorsCopy)
    {
        nrOfPaved = 0;
        nrOfUnpaved = 0;
        nrOfLeakThrough = 0;
        nrOfFlower = 0;
        nrOfGrass = 0;
        nrOfBush = 0;
        nrOfTree = 0;
        totalPlants = 0;
        foreach (Building building in allBuildings)
        {
            if (building.GetData().floorType == Support.FloorType.Paved)
            {
                nrOfPaved++;
            }
            else if (building.GetData().floorType == Support.FloorType.LeakThrough)
            {
                nrOfLeakThrough++;
            }
            else if (building.GetData().floorType == Support.FloorType.Unpaved)
            {
                nrOfUnpaved++;
            }
            else if (building.GetData().floorType == Support.FloorType.Grass)
            {
                nrOfGrass++;
                totalPlants++;
            }
            else if (building.GetData().floorType == Support.FloorType.Flower)
            {
                nrOfFlower++;
                totalPlants++;
            }
            else if (building.GetData().floorType == Support.FloorType.Bush)
            {
                nrOfBush++;
                totalPlants++;
            }
            else if (building.GetData().floorType == Support.FloorType.Tree)
            {
                nrOfTree++;
                totalPlants++;
            }
        }

        foreach (FloorBuilding floorBuilding in allFloorsCopy)
        {
            if (floorBuilding.GetData().floorType == Support.FloorType.Paved)
            {
                nrOfPaved++;
            }
            else if (floorBuilding.GetData().floorType == Support.FloorType.LeakThrough)
            {
                nrOfLeakThrough++;
            }
            else if (floorBuilding.GetData().floorType == Support.FloorType.Unpaved)
            {
                nrOfUnpaved++;
            }
            else if (floorBuilding.GetData().floorType == Support.FloorType.Grass)
            {
                nrOfGrass++;
                totalPlants++;
            }
            else if (floorBuilding.GetData().floorType == Support.FloorType.Flower)
            {
                nrOfFlower++;
            }
            else if (floorBuilding.GetData().floorType == Support.FloorType.Bush)
            {
                nrOfBush++;
            }
            else if (floorBuilding.GetData().floorType == Support.FloorType.Tree)
            {
                nrOfTree++;
            }
        }
        Debug.Log("All numbers (paved, leak through, unpaved, grass, flower, bush, tree " + nrOfPaved + ", " + nrOfLeakThrough + ", " + nrOfUnpaved + ", " + nrOfGrass + ", " + nrOfFlower + ", " + nrOfBush + ", " + nrOfTree);
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
    /// public getter for allFloors
    /// </summary>
    /// <returns></returns>
    public List<FloorBuilding> GetAllFloors()
    {
        return allFloors;
    }
}