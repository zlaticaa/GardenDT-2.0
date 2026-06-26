using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panelFloor;
    public GameObject panelPlants;
    public GameObject panelPillarsInput;

    public GameObject panelSaveGarden;
    public GameObject panelLoadGarden;




    void Start()
    {
        ShowPanelFloor();
        panelPillarsInput.SetActive(false);
        panelSaveGarden.SetActive(false);
        panelLoadGarden.SetActive(false);
    }

    public void ShowPanelFloor()
    {
        panelFloor.SetActive(true);
        panelPlants.SetActive(false);
    }

    public void ShowPanelPlants()
    {
        panelFloor.SetActive(false);
        panelPlants.SetActive(true);
    }


    public void ClosePanelSaveGarden()
    {
        panelSaveGarden.SetActive(false);
    }

    public void OpenPanelSaveGarden()
    {
        panelSaveGarden.SetActive(true);
   }

    public void ClosePanelLoadGarden()
    {
        panelLoadGarden.SetActive(false);
    }

    public void OpenPanelLoadGarden()
    {
        panelLoadGarden.SetActive(true);
    }
}
