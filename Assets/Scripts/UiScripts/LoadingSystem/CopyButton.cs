using System.Collections.Generic;
using UnityEngine;
using Unity.UIToolkit;
using UnityEngine.UIElements;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.SceneManagement;


public class CopyButton : MonoBehaviour
{
    [SerializeField] private BuildingSystem _buildings; 
    [SerializeField] private FloorBuilding _floor;
    [SerializeField] private TMP_InputField gardenNameInput;
    [SerializeField] private TMP_Text warning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeGardenList();
    }

    public void copyClick()
    {
        if (_buildings == null)
        {
            _buildings = FindFirstObjectByType<BuildingSystem>();

            if (_buildings == null)
            {
                Debug.LogError("BuildingSystem not found in scene!");
                return;
            }
        }

        Wrapper.data.objects.Clear();
        Wrapper.data.tiles.Clear();

        foreach (var building in _buildings.GetAllBuildings())
        {
            ObjectDTO buildDTO =
                new ObjectDTO(building.transform.position, building.GetData().name);

            Wrapper.data.objects.Add(buildDTO);
        }

        foreach (var tile in _buildings.GetAllFloors())
        {
            ObjectDTO tileDTO =
                new ObjectDTO(tile.transform.position, tile.GetData().name);

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

        string gardenName = gardenNameInput.text.Trim();

        if (string.IsNullOrEmpty(gardenName))
        {
            warning.text = "Please enter a garden name.";
            return;
        }

        string saveList = PlayerPrefs.GetString("GardenList", "");

        List<string> gardens = new List<string>();

        if (!string.IsNullOrEmpty(saveList))
        {
            gardens.AddRange(saveList.Split(';', System.StringSplitOptions.RemoveEmptyEntries));
        }

        bool isNewGarden = !PlayerPrefs.HasKey(gardenName);

        if (isNewGarden && gardens.Count >= 5)
        {
            warning.text = "Maximum of 5 gardens reached.";
            return;
        }

        if (gardens.Contains(gardenName))
        {
            gardens.Remove(gardenName);
        }

        string json = JsonConvert.SerializeObject(Wrapper.data, Formatting.Indented);

        PlayerPrefs.SetString(gardenName, json);

        gardens.Add(gardenName);

        PlayerPrefs.SetString("GardenList", string.Join(";", gardens));
        PlayerPrefs.Save();

        warning.text = "Garden saved successfully!";

        Debug.Log($"Garden '{gardenName}' saved (overwrite enabled).");


        SceneManager.LoadScene("GardenMenuScene");

    }

    //This method will be used for saving gardens. The system should not save more than 5 gardens and this function will store garden names so we can count the number of gardens in the system
    private void InitializeGardenList()
    {
        if (!PlayerPrefs.HasKey("GardenList"))
        {
            PlayerPrefs.SetString("GardenList", "");
            PlayerPrefs.Save();

            Debug.Log("GardenList created.");
        }
        else
        {
            Debug.Log("GardenList already exists.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
