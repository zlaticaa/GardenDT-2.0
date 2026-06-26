using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A script to handle the change between scenes
/// </summary>
public class SceneFlowManager : MonoBehaviour
{
    /// <summary>
    /// Loads garden menu scene
    /// </summary>
    public void LoadGardenMenu()
    {
        SceneManager.LoadScene("GardenMenuScene");
    }

    /// <summary>
    /// Loads template select scene
    /// </summary>
    public void LoadTemplateSelect()
    {
        SceneManager.LoadScene("GardenSelectTemplateScene");
    }

    /// <summary>
    /// Loads garden editor scene
    /// </summary>
    public void LoadGardenEditor()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
