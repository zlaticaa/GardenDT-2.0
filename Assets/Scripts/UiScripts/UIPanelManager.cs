using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject panelFloor;
    public GameObject panelPlants;
    public GameObject panePillarsInput;


    void Start()
    {
        ShowPanelFloor();
        panePillarsInput.SetActive(false);
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
}
