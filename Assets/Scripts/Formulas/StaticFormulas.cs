

using System;
using System.ComponentModel;

public static class StaticFormulas
{
    public static float P1Water(int totalArea, int tiledArea, int seepthroughArea, int dirtArea, int flowerArea, int grassArea, int bushArea, int treeArea)
    {
        float res = (tiledArea*FormulaParam.TileCoefficient + 
                    seepthroughArea*FormulaParam.SeepTileCoefficient + 
                    dirtArea*FormulaParam.DirtCoefficient + 
                    flowerArea*FormulaParam.FlowerCoefficient +
                    grassArea*FormulaParam.GrassCoefficient +
                    bushArea*FormulaParam.BushCoefficient +
                    treeArea*FormulaParam.TreeCoefficient
            )/totalArea;
        if (res >= 10f) return 10f;
        return res;
    }

    public static float P2Soil(fertilizerCleanupType fertilizer, fertilizerCleanupType cleanupStyle, float growthArea, int totalArea)
    {
        int input = (int)cleanupStyle + (int)fertilizer;
        float res = 0;
        switch (input)
        {
            case ((int)fertilizerCleanupType.CleanAll + (int)fertilizerCleanupType.NoFertilizer):
                res = FormulaParam.AllCleanNoFert;
                break;
            case ((int)fertilizerCleanupType.CleanAll + (int) fertilizerCleanupType.ArtificialFertilizer):
                res = FormulaParam.AllCleanArtificialFert;
                break;
            case ((int)fertilizerCleanupType.CleanAll + (int) fertilizerCleanupType.BioFertilizer):
                res= FormulaParam.AllCleanBioFert;
                break;
            case ((int)fertilizerCleanupType.CleanHalf + (int)fertilizerCleanupType.NoFertilizer):
                res = FormulaParam.HalfCleanNoFert;
                break;
            case ((int)fertilizerCleanupType.CleanHalf + (int) fertilizerCleanupType.ArtificialFertilizer):
                res= FormulaParam.HalfCleanArtificialFert;
                break;
            case ((int)fertilizerCleanupType.CleanHalf + (int) fertilizerCleanupType.BioFertilizer):
                res = FormulaParam.HalfCleanBioFert;
                break;
            case ((int)fertilizerCleanupType.CleanNone + (int)fertilizerCleanupType.NoFertilizer):
                res = FormulaParam.NoneCleanNoFert;
                break;
            case ((int)fertilizerCleanupType.CleanNone + (int) fertilizerCleanupType.ArtificialFertilizer):
                res = FormulaParam.NoneCleanArtificialFert;
                break;
            case ((int)fertilizerCleanupType.CleanNone + (int) fertilizerCleanupType.BioFertilizer):
                res = FormulaParam.NoneCleanBioFert;
                break;
            default: 
                res = 0;
                break;
        }
        return res * ((float)growthArea/(float)totalArea); 
    }

    public static float P3Environment(bool insects, bool birds, bool spiders, bool others,
        int growthArea, int totalArea)
    {
        float growthRatio = (float)growthArea/(float)totalArea;
        float insectVar = Convert.ToUInt16(insects) *FormulaParam.InsectCoefficient*growthRatio;
        float birdVar = Convert.ToUInt16(birds) *FormulaParam.BirdCoefficient*growthRatio;
        float spiderVar = Convert.ToUInt16(spiders) * FormulaParam.SpiderCoefficient *growthRatio;
        float otherVar = Convert.ToUInt16(others) * FormulaParam.OtherCoefficient * growthRatio;
        
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
        float res = (flowerVar + grassVar + bushVar + treeVar);
        if(res >= 10f) res = 10f;
        return res;
    }
    
}

