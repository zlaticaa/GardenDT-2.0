using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class PillarAnimalInput : MonoBehaviour
{
    [SerializeField] private Toggle insectsToggle;
    [SerializeField] private Toggle birdsToggle;
    [SerializeField] private Toggle spidersToggle;
    [SerializeField] private Toggle otherAnimalsToggle;

    [SerializeField] private TMP_Dropdown fertilizer;
    [SerializeField] private TMP_Dropdown cleanup;

    private fertilizerCleanupType[] enumValues;

    private void Start()
    {
        SetupDropdown(fertilizer);
        SetupDropdown(cleanup);

        fertilizer.onValueChanged.AddListener(OnFertilizerChanged);
        cleanup.onValueChanged.AddListener(OnCleanupChanged);
    }

    public void ApplyValues()
    {
        PillarSettings.Insects = insectsToggle != null && insectsToggle.isOn;
        PillarSettings.Birds = birdsToggle != null && birdsToggle.isOn;
        PillarSettings.Spiders = spidersToggle != null && spidersToggle.isOn;
        PillarSettings.OtherAnimals = otherAnimalsToggle != null && otherAnimalsToggle.isOn;
    }

    void SetupDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        enumValues = (fertilizerCleanupType[])Enum.GetValues(typeof(fertilizerCleanupType));

        var names = new List<string>();
        foreach (var v in enumValues)
            names.Add(v.ToString());

        dropdown.AddOptions(names);
    }

    void OnFertilizerChanged(int index)
    {
        PillarSettings.fertilizer = enumValues[index];
        Debug.Log("Fertilizer: " + PillarSettings.fertilizer);
    }

    void OnCleanupChanged(int index)
    {
        PillarSettings.cleanup = enumValues[index];
        Debug.Log("Cleanup: " + PillarSettings.cleanup);
    }
}