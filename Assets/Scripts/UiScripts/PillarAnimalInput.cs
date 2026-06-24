using UnityEngine;
using UnityEngine.UI;    
using TMPro;

public class PillarAnimalInput : MonoBehaviour
{
    [SerializeField] private Toggle insectsToggle;
    [SerializeField] private Toggle birdsToggle;
    [SerializeField] private Toggle spidersToggle;
    [SerializeField] private Toggle otherAnimalsToggle;

    public void ApplyValues()
    {
        PillarSettings.Insects = (Parse(insectsToggle) > 0);
        PillarSettings.Birds = (Parse(birdsToggle) > 0);
        PillarSettings.Spiders = (Parse(spidersToggle) > 0);
        PillarSettings.OtherAnimals = (Parse(otherAnimalsToggle) > 0);
    }

    private int Parse(Toggle toggle)
    {
        if (toggle == null)
            return 0;

        return toggle.isOn ? 1 : 0;
    }
}
