using UnityEngine;
using TMPro;

public class GardenSizeManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;

    public void ApplyValues()
    {
        Support.GridWidth = Parse(widthInput);
        Support.GridHeight = Parse(heightInput);
    }

    private int Parse(TMP_InputField input)
    {
        if (input == null) return 0;

        if (int.TryParse(input.text, out int value))
            return Mathf.Max(0, value);

        return 0;
    }
}
