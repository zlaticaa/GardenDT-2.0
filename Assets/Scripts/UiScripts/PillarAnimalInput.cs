using UnityEngine;
using TMPro;

public class PillarAnimalInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField insectsInput;
    [SerializeField] private TMP_InputField birdsInput;
    [SerializeField] private TMP_InputField spidersInput;
    [SerializeField] private TMP_InputField otherAnimalsInput;

    public void ApplyValues()
    {
        PillarSettings.Insects = Parse(insectsInput) >0;
        PillarSettings.Birds = Parse(birdsInput) >0;
        PillarSettings.Spiders = Parse(spidersInput) >0;
        PillarSettings.OtherAnimals = Parse(otherAnimalsInput) >0;

        //pillarMath.Recalculate();  
    }

    private int Parse(TMP_InputField input)
    {
        if (input == null) return 0;

        if (int.TryParse(input.text, out int value))
            return Mathf.Max(0, value);

        return 0;
    }
}
