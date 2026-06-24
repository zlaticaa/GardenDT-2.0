using System.Collections.Generic;
using UnityEngine;
using Unity.UIToolkit;
using UnityEngine.UIElements;
using Newtonsoft.Json;


public class CopyButton : MonoBehaviour
{
    [SerializeField] private BuildingSystem _buildings; 
    [SerializeField] private FloorBuilding _floor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void copyClick()
    {
        Wrapper.data.objects.Clear();
        
        foreach (var building in _buildings.GetAllBuildings())
        {
            print("object");
            ObjectDTO buildDTO = new ObjectDTO(building.transform.position, building.GetData().name);
            Wrapper.data.objects.Add(buildDTO);
        }

        foreach (var tile in _buildings.GetAllFloors())
        {
            print("tile");
            ObjectDTO tileDTO = new ObjectDTO(tile.transform.position, tile.GetData().name);
            Wrapper.data.tiles.Add(tileDTO);
        }

        Wrapper.data.AmountOfPlants = PillarSettings.nrOfPlants;
        Wrapper.data.Birds = PillarSettings.Birds;
        Wrapper.data.Insects = PillarSettings.Insects;
        Wrapper.data.Spiders = PillarSettings.Spiders;
        Wrapper.data.Others = PillarSettings.OtherAnimals;
        Wrapper.data.Cleanup = PillarSettings.cleanup;
        Wrapper.data.Fertilizer = PillarSettings.fertilizer;
        Wrapper.data.GridHeight = Support.GridHeight;
        Wrapper.data.GridWidth = Support.GridWidth;
        PlayerPrefs.SetString("save",JsonConvert.SerializeObject(Wrapper.data, Formatting.Indented));
        PlayerPrefs.Save();
       
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
