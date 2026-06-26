using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SetLoadingData : MonoBehaviour
{
    public void SendGardenName(string gardenName)
    {
        if (string.IsNullOrWhiteSpace(gardenName))
        {
            Debug.LogWarning("Garden name is empty.");
            return;
        }

        string json = PlayerPrefs.GetString(gardenName, "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning($"Garden '{gardenName}' not found.");
            return;
        }

        WrapperDTO wrap = JsonConvert.DeserializeObject<WrapperDTO>(json);

        // Grid size must be set before loading the scene
        Support.GridHeight = wrap.GridHeight;
        Support.GridWidth = wrap.GridWidth;

        LoadingSceneSettings.isLoading = true;
        LoadingSceneSettings.gardenName = gardenName;

        SceneManager.LoadScene("SampleScene");
    }
}