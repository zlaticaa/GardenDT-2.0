using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneFlowManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadGardenMenu()
    {
        SceneManager.LoadScene("GardenMenuScene");
    }

    public void LoadTemplateSelect()
    {
        SceneManager.LoadScene("GardenSelectTemplateScene");
    }

    public void LoadGardenEditor()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
