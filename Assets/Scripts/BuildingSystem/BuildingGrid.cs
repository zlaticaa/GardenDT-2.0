using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    private int width = Support.GridWidth;
    private int height = Support.GridHeight;
    private BuildingGridCell[,] grid;
    private LineRenderer _lineRenderer;
    [SerializeField] private float lineWidth = 0.1f;
    
    private void Start()
    {
        grid = new BuildingGridCell[width, height];
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new(); 
            }
        }
    }

    public void SetBuilding(Building building, List<Vector3> allBuildingPositions)
    {
        foreach (var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x, y].SetBuilding(building);
        }
    }

    public void SetFloor(FloorBuilding floorBuilding, List<Vector3> allBuildingPositions)
    {
        foreach (var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x, y].SetFloor(floorBuilding);
        }
    }

    public bool CanBuildBuilding(List<Vector3> allBuildingPositions)
    {
        foreach (var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            if (!grid[x, y].IsEmpty()) return false;
        }
        return true;
    }

    public bool CanBuildFloor(List<Vector3> allBuildingPositions)
    {
        foreach (var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
        }
        return true;
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / BuildingSystem.cellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).z / BuildingSystem.cellSize);
        return (x, y);
    }

    private void DrawLine(Vector3[] pos, float width)
    {
        GameObject newLine = new GameObject("Line");
        newLine.transform.SetParent(transform);
        _lineRenderer = newLine.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
        _lineRenderer.startColor = Color.lightGoldenRodYellow;
        _lineRenderer.endColor = Color.lightGoldenRodYellow;
        _lineRenderer.positionCount = pos.Length;
        _lineRenderer.SetPositions(pos);
        
    }
    public void DrawLineGrid()
    {
        
        Gizmos.color = Color.lightGoldenRodYellow;
        if (BuildingSystem.cellSize <= 0 || width <= 0 || height <= 0) return;
        Vector3 origin = transform.position;
        for(int y = 0; y <= height; y++)
        {
            Vector3[] poses = new Vector3[2];
            poses[0] = origin + new Vector3(0, 0.01f, y * BuildingSystem.cellSize);
            poses[1] = origin + new Vector3(width * BuildingSystem.cellSize, 0.01f, y * BuildingSystem.cellSize);
            DrawLine(poses, lineWidth);
        
        }

        for (int x = 0; x <= width; x++)
        {
            Vector3[] poses = new Vector3[2];
            poses[0] = origin + new Vector3(x * BuildingSystem.cellSize, 0.01f, 0);
            poses[1] = origin + new Vector3(x * BuildingSystem.cellSize, 0.01f, height * BuildingSystem.cellSize);
            DrawLine(poses, lineWidth);
        }
        
    }
}

public class BuildingGridCell
{
    private Building building;
    private FloorBuilding floorBuilding;

    public void SetBuilding(Building building)
    {
        this.building = building;
    }
    
    public void SetFloor(FloorBuilding floorBuilding)
    {
        this.floorBuilding = floorBuilding;
    }

    public bool IsEmpty()
    {
        return building == null;
    }
}