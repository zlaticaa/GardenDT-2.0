using System.Collections.Generic;
using System;
using UnityEngine;



public class GardenListManager : MonoBehaviour
{
    [SerializeField] private Transform gridParent;
    [SerializeField] private GardenButton buttonPrefab;
    [SerializeField] private SetLoadingData loadingData;

    private void Start()
    {
        RefreshGardenList();
    }

    public void RefreshGardenList()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        string saveList = PlayerPrefs.GetString("GardenList", "");

        if (string.IsNullOrEmpty(saveList))
            return;

        List<string> gardens = new List<string>(
            saveList.Split(';', StringSplitOptions.RemoveEmptyEntries)
        );

        foreach (string gardenName in gardens)
        {
            GardenButton button =
                Instantiate(buttonPrefab, gridParent);

            button.Initialize(gardenName, loadingData);
        }
    }


    public void ClearGardenList()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("ALL PlayerPrefs cleared!");

        // Refresh UI after reset
        RefreshGardenList();
    }
}
