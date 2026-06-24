using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadButton : MonoBehaviour
{
    private Button _loadButton;
    [SerializeField] private UIDocument _document;
    [SerializeField] private BuildingSystem _builder;
    [SerializeField] private List<BuildingData> _buildDatas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _loadButton = _document.rootVisualElement.Q<Button>("LoadButton");
        _loadButton.clicked += LoadButtonClick;
    }

    private void LoadButtonClick()
    {
        TextEditor te = new TextEditor();
        te.Paste();
        te.SelectAll();
        string json = te.SelectedText;
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
            _builder.preview = _builder.CreatePreview(data, new Vector3(obj.position.x, obj.position.y, obj.position.z));
            _builder.HandlePreview(new Vector3(obj.position.x, obj.position.y, obj.position.z), true);
        }

        Wrapper.data = wrap;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
