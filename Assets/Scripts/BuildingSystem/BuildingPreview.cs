using UnityEngine;
using System.Collections.Generic;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Material PositiveMaterial;
    [SerializeField] private Material NegativeMaterial;

    public Support.PreviewState previewState { get; private set; } = Support.PreviewState.Negative;
    public BuildingData buildingData { get; private set; }
    public FloorData floorData { get; private set; }
    public BuildingModel buildingModel { get; private set; }

    private List<Renderer> renderers = new();
    private List<Collider> colliders = new();

    public void Setup(BuildingData data)
    {
        buildingData = data;
        buildingModel = Instantiate(data.buildingModel,transform.position,Quaternion.identity,transform);
        renderers.AddRange(buildingModel.GetComponentsInChildren<Renderer>());
        colliders.AddRange(buildingModel.GetComponentsInChildren<Collider>());

        foreach(var col in colliders)
        {
            col.enabled= false;
        }
        SetPreviewMaterial(previewState);
    }

    public void Setup(FloorData data)
    {
        floorData = data;
        buildingModel = Instantiate(data.floorModel, transform.position, Quaternion.identity, transform);
        buildingModel.transform.position -= new Vector3(0, 0.49f, 0);
        renderers.AddRange(buildingModel.GetComponentsInChildren<Renderer>());
        colliders.AddRange(buildingModel.GetComponentsInChildren<Collider>());

        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        SetPreviewMaterial(previewState);
    }

    public void ChangeState(Support.PreviewState state)
    {
        if (state == previewState) return;
        previewState = state;
        SetPreviewMaterial(previewState);
    }

    private void SetPreviewMaterial(Support.PreviewState state)
    {
        Material previewMat = state == Support.PreviewState.Positive ? PositiveMaterial : NegativeMaterial;
        foreach(var rend in renderers)
        {
            Material[] mats = new Material[rend.sharedMaterials.Length];
            for(int i = 0; i < mats.Length; i++)
            {
                mats[i] = previewMat;
            }
            rend.materials = mats;
        }
    }
}