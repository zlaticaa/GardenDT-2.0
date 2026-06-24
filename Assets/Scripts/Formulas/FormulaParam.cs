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
  //normalizes p1 so a plot of all trees isn't 15 but 10    
  private const float normalizeP1 = 1f; //10 / 15f; (would be the normalization necessary to make the outcome between 1-10)
  //pillar 1 tile coefficient  
  public static float TileCoefficient = 0f*normalizeP1;
  public static float SeepTileCoefficient = 2f*normalizeP1;
  public static float DirtCoefficient = 3f*normalizeP1;
  public static float FlowerCoefficient = 3.5f*normalizeP1;  
  public static float GrassCoefficient = 4f*normalizeP1;
  public static float BushCoefficient = 10f*normalizeP1;
  public static float TreeCoefficient = 15f*normalizeP1;

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

  //normalizes the paramaters so a garden of exclusively trees will not have an outcome of 36;
  private const float normalizeP4const = 1f; //10f / 12f; (would be to make the base between 0 and 10 while keeping it relative to eachother the same)
  private const float normalizeP4coef = 1f; //1f / 3f; (would be to make the coefficent between 0-1 so that the outcome can't go higher than the base)
  //pillar 4 plant type amount constants
  public static float NoPlantConst = 0f*normalizeP4const;
  public static float LessThan3Const = 2f*normalizeP4const;
  public static float LessThan10Const = 5f*normalizeP4const;
  public static float LessThan25Const = 8f*normalizeP4const;
  public static float MoreThan25Const = 12f*normalizeP4const;
  
  //pillar 4 plant diversity coefficient
  public static float FlowerDiversityCoefficient = 1f*normalizeP4coef;
  public static float GrassDiversityCoefficient = 0.25f*normalizeP4coef;
  public static float BushDiversityCoefficient = 2f*normalizeP4coef;
  public static float TreeDiversityCoefficient = 3f*normalizeP4coef;
}