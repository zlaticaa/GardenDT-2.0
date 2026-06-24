using UnityEngine;
 
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
