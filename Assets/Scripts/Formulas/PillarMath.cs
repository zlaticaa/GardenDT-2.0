using System.Collections.Generic;
using UnityEngine;

public class PillarMath : MonoBehaviour
{
    [SerializeField] private BuildingSystem bSystem; private List<Building> allBuildings = new();
    [SerializeField] private List<FloorBuilding> allFloors = new();

    private int nrOfPaved;
    private int nrOfLeakThrough;
    private int nrOfUnpaved;
    private int nrOfGrass;
    private int nrOfFlower;
    private int nrOfBush;
    private int nrOfTree;
    private int totalArea;

    public fertilizerCleanupType fertilizer;
    public fertilizerCleanupType cleanupStyle;

    public bool Insects;
    public bool Birds;
    public bool Spiders;
    public bool OtherAnimals;
    private int totalPlants;

    public int nrOfPlantTypes;

    //I will display these variables on the UI
    private float pillar1Score;
    private float pillar2Score;
    private float pillar3Score;
    private float pillar4Score;

    public void Start()
    {
        Setup();
    }

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


    private void Setup()
    {

        ReloadAndCalculate();
    }

    public void CalculatePillar1()
    {
         pillar1Score = StaticFormulas.P1Water(GetTotalArea(), nrOfPaved, nrOfLeakThrough, nrOfUnpaved, nrOfFlower, nrOfGrass, nrOfBush, nrOfTree);
    }

    public void CalculatePillar2()
    {

         pillar2Score = StaticFormulas.P2Soil(fertilizer, cleanupStyle,totalPlants,totalArea);
    }

    public void CalculatePillar3()
    {
        pillar3Score = StaticFormulas.P3Environment(Insects, Birds, Spiders, OtherAnimals, totalPlants, totalArea);
    }

    public void CalculatePillar4()
    {
        pillar4Score = StaticFormulas.P4PlantDiversity(nrOfFlower, nrOfGrass, nrOfBush, nrOfTree, totalArea, nrOfPlantTypes);
    }

    private int GetTotalArea()
    {
        totalArea = Support.GridHeight * Support.GridWidth;

        return totalArea;
    }

    public float Pillar1Score => pillar1Score;
    public float Pillar2Score => pillar2Score;
    public float Pillar3Score => pillar3Score;
    public float Pillar4Score => pillar4Score;

    private void CalculateAllPillars()
    {
        CalculatePillar1();
        CalculatePillar2();
        CalculatePillar3();
        CalculatePillar4();
    }

    //We need this method to invok calculation every time a user add an object to the scene
    public void Recalculate()
    {
        ReloadAndCalculate();
    }

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

    private List<FloorBuilding> GetCopyAllFloors()
    {
        List<FloorBuilding> builings = new();
        foreach (FloorBuilding b in allFloors)
        {
            builings.Add(b);
        }

        return builings;
    }

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
    public List<Building> GetAllBuildings()
    {
        return allBuildings;
    }

    public List<FloorBuilding> GetAllFloors()
    {
        return allFloors;
    }
}