using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadButton : MonoBehaviour
{
    [SerializeField] private BuildingSystem _builder;
    [SerializeField] private List<BuildingData> _buildDatas;
    [SerializeField] private List<FloorData> _floorDatas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void LoadButtonClick()
    {
        if (PlayerPrefs.GetString("save") != "" && PlayerPrefs.GetString("save") != null)
        {
            string json = PlayerPrefs.GetString("save");
            print(json);
            WrapperDTO wrap = JsonConvert.DeserializeObject<WrapperDTO>(json);
            foreach (var obj in wrap.objects)
            {
                BuildingData data = new();
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
                FloorData data = new();
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
            PillarSettings.Spiders=wrap.Spiders;
            PillarSettings.OtherAnimals = wrap.Others;
            PillarSettings.nrOfPlants = wrap.AmountOfPlants;
            PillarSettings.cleanup = wrap.Cleanup;
            PillarSettings.fertilizer = wrap.Fertilizer;
            Support.GridHeight = wrap.GridHeight;
            Support.GridWidth = wrap.GridWidth;
            Wrapper.data = wrap;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
