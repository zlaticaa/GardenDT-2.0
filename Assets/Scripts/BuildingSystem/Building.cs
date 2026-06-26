using UnityEngine;

// A class for the objecten to be built on top of the floor
public class Building : MonoBehaviour
{
    // Each instance of this class gets a model to be placed and data to refer to.
    private BuildingModel model;
    private BuildingData data;
    
    public void Setup(BuildingData data)
    {
        this.data = data;
        model = Instantiate(data.buildingModel, transform.position, Quaternion.identity, transform);
    }

    public BuildingData GetData()
    {
        return data;
    }
}