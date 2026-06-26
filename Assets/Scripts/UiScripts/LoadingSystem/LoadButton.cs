using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class LoadButton : MonoBehaviour
{
    [SerializeField] private BuildingSystem _builder;
    [SerializeField] private List<BuildingData> _buildDatas;
    [SerializeField] private List<FloorData> _floorDatas;

    public void Start()
    {
        if (LoadingSceneSettings.isLoading)
        {
            string gardenName = LoadingSceneSettings.gardenName;

            if (string.IsNullOrEmpty(gardenName))
            {
                Debug.LogWarning("Please enter a garden name.");
                return;
            }

            if (PlayerPrefs.GetString(gardenName) != "" && PlayerPrefs.GetString(gardenName) != null)
            {
                string json = PlayerPrefs.GetString(gardenName);
                print(json);
                WrapperDTO wrap = JsonConvert.DeserializeObject<WrapperDTO>(json);
                foreach (var obj in wrap.objects)
                {
                    BuildingData data = null;
                    foreach (var build in _buildDatas)
                    {
                        if (build.name == obj.type)
                        {
                            data = build;
                        }
                    }

                    _builder.buildingPreview =
                        _builder.CreateBuildingPreview(data, new Vector3(obj.position.x, obj.position.y, obj.position.z));
                    _builder.HandleBuildingPreview(new Vector3(obj.position.x, obj.position.y, obj.position.z), true);
                }
                foreach (var obj in wrap.tiles)
                {
                    FloorData data = null;
                    foreach (var floor in _floorDatas)
                    {
                        if (floor.name == obj.type)
                        {
                            data = floor;
                        }
                    }

                    _builder.floorPreview =
                        _builder.CreateFloorPreview(data, new Vector3(obj.position.x, obj.position.y, obj.position.z));
                    _builder.HandleFloorPreview(new Vector3(obj.position.x, obj.position.y, obj.position.z), true);
                }

                PillarSettings.Birds = wrap.Birds;
                PillarSettings.Insects = wrap.Insects;
                PillarSettings.Spiders = wrap.Spiders;
                PillarSettings.OtherAnimals = wrap.Others;
                PillarSettings.nrOfPlants = wrap.AmountOfPlants;
                PillarSettings.cleanup = wrap.Cleanup;
                PillarSettings.fertilizer = wrap.Fertilizer;
                LoadingSceneSettings.GardenLength = wrap.GridHeight;
                LoadingSceneSettings.GardenWidth = wrap.GridWidth;

                Wrapper.data = wrap;


                print("=== LOADED SAVE ===");
                print($"Objects: {wrap.objects.Count}");
                print($"Tiles: {wrap.tiles.Count}");
                print($"Birds: {wrap.Birds}, Insects: {wrap.Insects}");
                print($"Grid: {wrap.GridWidth} x {wrap.GridHeight}");

                foreach (var obj in wrap.objects)
                {
                    print($"Object: {obj.type} at {obj.position}");
                }

                foreach (var tile in wrap.tiles)
                {
                    print($"Tile: {tile.type} at {tile.position}");
                }


                //Reset the loading settings
                LoadingSceneSettings.gardenName = "";
                LoadingSceneSettings.isLoading = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
