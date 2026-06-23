

public static class StaticFormulas
{
    public static float P1Water(int totalArea, int tiledArea, int seepthroughArea, int dirtArea, int flowerArea, int grassArea, int bushArea, int treeArea)
    {
        return (
            tiledArea*FormulaParam.TileCoefficient + 
            seepthroughArea*FormulaParam.SeepTileCoefficient + 
            dirtArea*FormulaParam.DirtCoefficient + 
            flowerArea*FormulaParam.FlowerCoefficient +
            grassArea*FormulaParam.GrassCoefficient +
            bushArea*FormulaParam.BushCoefficient +
            treeArea*FormulaParam.TreeCoefficient
            )/totalArea;
    }

    public static float P2Soil(fertilizerCleanupType fertilizer, fertilizerCleanupType cleanupStyle)
    {
        int input = (int)cleanupStyle + (int)fertilizer;
        switch (input)
        {
            case ((int)fertilizerCleanupType.CleanAll + (int)fertilizerCleanupType.NoFertilizer):
                return FormulaParam.AllCleanNoFert;
            case ((int)fertilizerCleanupType.CleanAll + (int) fertilizerCleanupType.ArtificialFertilizer):
                return FormulaParam.AllCleanArtificialFert;
            case ((int)fertilizerCleanupType.CleanAll + (int) fertilizerCleanupType.BioFertilizer):
                return FormulaParam.AllCleanBioFert;
            case ((int)fertilizerCleanupType.CleanHalf + (int)fertilizerCleanupType.NoFertilizer):
                return FormulaParam.HalfCleanNoFert;
            case ((int)fertilizerCleanupType.CleanHalf + (int) fertilizerCleanupType.ArtificialFertilizer):
                return FormulaParam.HalfCleanArtificialFert;
            case ((int)fertilizerCleanupType.CleanHalf + (int) fertilizerCleanupType.BioFertilizer):
                return FormulaParam.HalfCleanBioFert;
            case ((int)fertilizerCleanupType.CleanNone + (int)fertilizerCleanupType.NoFertilizer):
                return FormulaParam.NoneCleanNoFert;
            case ((int)fertilizerCleanupType.CleanNone + (int) fertilizerCleanupType.ArtificialFertilizer):
                return FormulaParam.NoneCleanArtificialFert;
            case ((int)fertilizerCleanupType.CleanNone + (int) fertilizerCleanupType.BioFertilizer):
                return FormulaParam.NoneCleanBioFert;
            default:
                return 0;
        }
    }

    public static float P3Environment(int amountInsects, int amountBirds, int amountSpiders, int amountOthers,
        int growthArea, int totalArea)
    {
        float growthRatio = (float)growthArea/totalArea;
        float insectVar = amountInsects*FormulaParam.InsectCoefficient*growthRatio;
        float birdVar = amountBirds * FormulaParam.BirdCoefficient * growthArea;
        float spiderVar = amountSpiders * FormulaParam.SpiderCoefficient *growthArea;
        float otherVar = amountOthers * FormulaParam.OtherCoefficient * growthArea;
        
        return (insectVar + birdVar + spiderVar + otherVar);
    }

    public static float P4PlantDiversity(int flowerArea, int grassArea, int bushArea, int treeArea, int totalArea, int amountOfPlantTypes)
    {
        float plantDiverseConst = 0;
        if (amountOfPlantTypes < 1) plantDiverseConst = FormulaParam.NoPlantConst;
        else if (amountOfPlantTypes < 4) plantDiverseConst = FormulaParam.LessThan3Const;
        else if (amountOfPlantTypes < 11) plantDiverseConst = FormulaParam.LessThan10Const;
        else if (amountOfPlantTypes < 26) plantDiverseConst = FormulaParam.LessThan25Const;
        else plantDiverseConst = FormulaParam.MoreThan25Const;

        float flowerVar = (flowerArea / (float)totalArea) * plantDiverseConst * FormulaParam.FlowerDiversityCoefficient;
        float grassVar = (grassArea / (float)totalArea) * plantDiverseConst * FormulaParam.GrassDiversityCoefficient;
        float bushVar = (bushArea / (float)totalArea) * plantDiverseConst * FormulaParam.BushDiversityCoefficient;
        float treeVar = (treeArea / (float)totalArea) * plantDiverseConst * FormulaParam.TreeDiversityCoefficient;
        
        return (flowerVar + grassVar + bushVar + treeVar);
    }
    
}


