using System.Collections.Generic;
using UnityEngine;
using Unity.UIToolkit;
using UnityEngine.UIElements;
using Newtonsoft.Json;


public class CopyButton : MonoBehaviour
{
    [SerializeField] private BuildingSystem _buildings; 
    [SerializeField] private FloorBuilding _floor;
    private Button _copyButton;
    [SerializeField] private UIDocument _document;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _copyButton = _document.rootVisualElement.Q<Button>("CopyButton");
        _copyButton.clicked += copyClick;
        
    }

    private void copyClick()
    {
        Wrapper.data.objects.Clear();
        
        foreach (var building in _buildings.GetAllBuildings())
        {
            print("object");
            ObjectDTO buildDTO = new ObjectDTO(building.transform.position, building.data.name);
            Wrapper.data.objects.Add(buildDTO);
        }

        foreach (var tile in _floor.Tiles )
        {
            print("tile");
            ObjectDTO tileDTO = new ObjectDTO(tile.transform.position, tile.gameObject.name);
            Wrapper.data.tiles.Add(tileDTO);
        }
        print("Copied");
        TextEditor te = new TextEditor();
        te.text = JsonConvert.SerializeObject(Wrapper.data, Formatting.Indented);
        te.SelectAll();
        te.Copy();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
