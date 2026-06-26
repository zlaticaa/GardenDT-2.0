using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void TogglePanel()
    {
        if (panel == null) return;

        panel.SetActive(!panel.activeSelf);
    }

    public void OpenPanel()
    {
        if (panel == null) return;

        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        if (panel == null) return;

        panel.SetActive(false);
    }
}
