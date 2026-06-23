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
        PillarSettings.nrOfInsects = Parse(insectsInput);
        PillarSettings.nrOfBirds = Parse(birdsInput);
        PillarSettings.nrOfSpiders = Parse(spidersInput);
        PillarSettings.nrOfOtherAnimals = Parse(otherAnimalsInput);

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
