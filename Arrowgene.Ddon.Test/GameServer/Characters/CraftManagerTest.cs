using System.Collections.Generic;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Test.Database;
using Xunit;

namespace Arrowgene.Ddon.GameServer.Characters;

public class CraftManagerTest
{
    private readonly DdonGameServer _mockServer;
    private readonly CraftManager _craftManager;

    public CraftManagerTest()
    {
        _mockServer = new DdonGameServer(new GameServerSetting(), new MockDatabase(), new AssetRepository("TestFiles"));
        _craftManager = new CraftManager(_mockServer);
    }

    [Fact]
    public void GetCraftingTimeReductionRate_ShouldReturnCorrectValue()
    {
        List<uint> productionSpeedLevels = new List<uint> { 10, 20, 30 };
        _mockServer.Setting.GameLogicSetting.AdditionalProductionSpeedFactor = 1.0;

        double result = _craftManager.GetCraftingTimeReductionRate(productionSpeedLevels);

        Assert.True(result is > 0 and < 100);
    }

    [Fact]
    public void CalculateRecipeProductionSpeed_ShouldReturnReducedTime()
    {
        List<uint> productionSpeedLevels = new List<uint> { 70, 70, 70, 70 };
        const uint recipeTime = 100;
        _mockServer.Setting.GameLogicSetting.AdditionalProductionSpeedFactor = 1.0;

        uint result = _craftManager.CalculateRecipeProductionSpeed(recipeTime, productionSpeedLevels);

        Assert.True(result < recipeTime*0.6);
    }
    
    [Fact]
    public void CalculateRecipeProductionSpeed_ShouldReturnZeroCraftTime_AdditionalFactorHigh()
    {
        List<uint> productionSpeedLevels = new List<uint> { 70, 70, 70, 70 };
        const uint recipeTime = 100;
        _mockServer.Setting.GameLogicSetting.AdditionalProductionSpeedFactor = 100;

        uint result = _craftManager.CalculateRecipeProductionSpeed(recipeTime, productionSpeedLevels);

        Assert.Equal(0u, result);
    }

    [Fact]
    public void CalculateEquipmentEnhancement_ShouldReturnCorrectEnhancementPoints()
    {
        List<uint> enhancementLevels = new List<uint> { 45, 1, 1, 1 };

        CraftCalculationResult result = _craftManager.CalculateEquipmentEnhancement(enhancementLevels, 0); // not sure what this is used for but I updated this function,
                                                                                                        // so adding a dummy value for calculatedOdds (greatsuccess stuff)
        Assert.True(result.CalculatedValue >= 150);
    }

    [Fact]
    public void CalculatePawnRankUp_ShouldReturnCorrectRankUpsRank70()
    {
        Pawn pawn = new Pawn
        {
            CraftData = new CDataPawnCraftData()
            {
                CraftRank = 1,
                CraftExp = 999999,
                CraftRankLimit = 70
            }
        };

        uint rankUps = CraftManager.CalculatePawnRankUp(pawn);

        Assert.Equal(69u, rankUps);
    }
    
    [Fact]
    public void CanPawnRankUp_ShouldReturnFalse_WhenCraftRankLimitReached()
    {
        var leadPawn = new Pawn
        {
            CraftData = new CDataPawnCraftData
            {
                CraftRank = 20,
                CraftRankLimit = 20,
                CraftExp = 5000
            }
        };

        bool result = CraftManager.CanPawnRankUp(leadPawn);

        Assert.False(result);
    }
    
    [Fact]
    public void CanPawnRankUp_ShouldReturnFalse_WhenInsufficientCraftExp()
    {
        var leadPawn = new Pawn
        {
            CraftData = new CDataPawnCraftData
            {
                CraftRank = 10,
                CraftRankLimit = 20,
                CraftExp = 1000
            }
        };

        bool result = CraftManager.CanPawnRankUp(leadPawn);

        Assert.False(result);
    }
    
    [Fact]
    public void CanPawnRankUp_ShouldReturnTrue_WhenCraftExpIsSufficientAndRankIsBelowLimit()
    {
        var leadPawn = new Pawn
        {
            CraftData = new CDataPawnCraftData
            {
                CraftRank = 10,
                CraftRankLimit = 20,
                CraftExp = 20000
            }
        };

        bool result = CraftManager.CanPawnRankUp(leadPawn);

        Assert.True(result);
    }
    
    [Fact]
    public void CanPawnRankUp_ShouldReturnFalse_WhenRankIsAtLimitEvenWithSufficientExp()
    {
        var leadPawn = new Pawn
        {
            CraftData = new CDataPawnCraftData
            {
                CraftRank = 20,
                CraftRankLimit = 20,
                CraftExp = 100000
            }
        };

        bool result = CraftManager.CanPawnRankUp(leadPawn);

        Assert.False(result);
    }
}
