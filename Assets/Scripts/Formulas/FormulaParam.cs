using UnityEngine.Experimental.GlobalIllumination;
//enums for lookup table pillar 2
public enum fertilizerCleanupType
{
    CleanAll = 1,
    CleanHalf = 2,
    CleanNone = 4,
    NoFertilizer = 8,
    ArtificialFertilizer = 16,
    BioFertilizer = 32
}

public static class FormulaParam
{
  //pillar 1 tile coefficient  
  public static float TileCoefficient = 0f;
  public static float SeepTileCoefficient = 2f;
  public static float DirtCoefficient = 3f;
  public static float FlowerCoefficient = 3.5f;  
  public static float GrassCoefficient = 4f;
  public static float BushCoefficient = 10f;
  public static float TreeCoefficient = 15f;

  //pillar 2 outcomes
  public static float AllCleanNoFert = 7f;
  public static float HalfCleanNoFert = 2f;
  public static float NoneCleanNoFert = 0f;
  public static float AllCleanArtificialFert = 5f;
  public static float HalfCleanArtificialFert = 4f;
  public static float NoneCleanArtificialFert = 3f;
  public static float AllCleanBioFert = 10f;
  public static float HalfCleanBioFert = 9f;
  public static float NoneCleanBioFert = 8f;
  
  //pillar 3 animal coefficients
  public static float InsectCoefficient = 2.5f;
  public static float BirdCoefficient = 2.5f;
  public static float SpiderCoefficient = 2.5f;
  public static float OtherCoefficient = 2.5f;

  //pillar 4 plant type amount constants
  public static float NoPlantConst = 0;
  public static float LessThan3Const = 2f;
  public static float LessThan10Const = 5f;
  public static float LessThan25Const = 8f;
  public static float MoreThan25Const = 12f;
  
  //pillar 4 plant diversity coefficient
  public static float FlowerDiversityCoefficient = 1f;
  public static float GrassDiversityCoefficient = 0.25f;
  public static float BushDiversityCoefficient = 2f;
  public static float TreeDiversityCoefficient = 3f;
}