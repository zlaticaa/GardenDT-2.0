using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GardenButton : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Button btn;

    private string gardenName;
    private SetLoadingData loader;

    public void Initialize(string name, SetLoadingData loadingData)
    {
        gardenName = name;
        loader = loadingData;

        if (label != null)
            label.text = gardenName;

        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                loader.SendGardenName(gardenName);
            });
        }
    }
}