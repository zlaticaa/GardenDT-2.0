using UnityEngine;
 
/// <summary>
/// A static support class to store specific information for ease of access
/// </summary>
public static class Support
{
    public static int GridWidth = 3;
    public static int GridHeight = 3;

    public enum FloorType
    {
        Paved,
        LeakThrough,
        Unpaved,
        Grass,
        Flower,
        Bush,
        Tree,
        NoBuild
    }

    public enum PreviewState
    {
        Positive,
        Negative
    }
}
 