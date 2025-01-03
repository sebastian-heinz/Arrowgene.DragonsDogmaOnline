using Arrowgene.Ddon.Server.Scripting.interfaces;
using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using Arrowgene.Ddon.Test.Database;
using System.Collections.Generic;
using Xunit;

namespace Arrowgene.Ddon.GameServer.Characters;

public class CraftManagerTest
{
    private readonly DdonGameServer _mockServer;
    private readonly CraftManager _craftManager;
    private readonly ScriptableSettings _scriptableSettings;

    public CraftManagerTest()
    {
        var settings = new GameServerSetting();

        _scriptableSettings = new ScriptableSettings();
        _scriptableSettings.Set<uint>("GameLogicSettings", "GameClockTimescale", 90);
        _scriptableSettings.Set<uint>("GameLogicSettings", "WeatherSequenceLength", 20);
        _scriptableSettings.Set("GameLogicSettings", "WeatherStatistics", new List<(uint MeanLength, uint Weight)>()
        {
            (60 * 30, 1), // Fair
            (60 * 30, 1), // Cloudy
            (60 * 30, 1), // Rainy
        });
        _scriptableSettings.Set("GameLogicSettings", "WalletLimits", new Dictionary<WalletType, uint>()
        {
            {WalletType.Gold, 999999999},
            {WalletType.RiftPoints, 999999999},
            {WalletType.BloodOrbs, 500000},
            {WalletType.SilverTickets, 999999999},
            {WalletType.GoldenGemstones, 99999},
            {WalletType.RentalPoints, 99999},
            {WalletType.ResetJobPoints, 99},
            {WalletType.ResetCraftSkills, 99},
            {WalletType.HighOrbs, 5000},
            {WalletType.DominionPoints, 999999999},
            {WalletType.AdventurePassPoints, 80},
            {WalletType.UnknownTickets, 999999999},
            {WalletType.BitterblackMazeResetTicket, 3},
            {WalletType.GoldenDragonMark, 30},
            {WalletType.SilverDragonMark, 150},
            {WalletType.RedDragonMark, 99999},
        });
        _scriptableSettings.Set<uint>("Crafting", "EquipmentEnhancementMinimumTotal", 150);
        _scriptableSettings.Set<double>("Crafting", "EquipmentEnhancementIncrementPerLevel", 1.0);
        _scriptableSettings.Set<int>("Crafting", "GreatSuccessOddsDefault", 10);
        _scriptableSettings.Set("Crafting", "CraftRankExpLimit", new List<uint>
        {
            0, 8, 120, 240, 400, 800, 1400, 2150, 3050, 4100, 5300, 6600, 8000, 9500, 11000, 13000, 15000, 17500, 20000, 23000, 27000, 29500, 32500, 36000, 39500, 43500, 47000,
            51000, 55000, 60000, 63000, 67461, 72101, 76927, 81946, 87165, 92593, 98239, 104110, 110216, 116566, 123170, 130039, 137182, 144611, 152337, 160372, 168728, 177419,
            186457, 195857, 205633, 215800, 226374, 237370, 248807, 260701, 273070, 285935, 299314, 313228, 327699, 342748, 358400, 374678, 391606, 409212, 427522, 446565, 466369
        });

        var gameLogicSetting = new GameLogicSetting(_scriptableSettings);
        _mockServer = new DdonGameServer(settings, gameLogicSetting, new MockDatabase(), new AssetRepository("TestFiles"));
        _craftManager = new CraftManager(_mockServer);
    }

    /*[Fact]
    public void CalculateRecipeProductionSpeed_ShouldReturnReducedTime()
    {
        List<CraftPawn> craftPawns = new()
        {
            new(50, 50, 50, 50, 50, CraftPosition.Leader),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant)
        };
        const uint recipeTime = 100;
        _scriptableSettings.Set<double>("GameLogicSettings", "AdditionalProductionSpeedFactor", 1.0);

        uint result = _craftManager.CalculateRecipeProductionSpeed(recipeTime, new(), craftPawns);

        Assert.True(result < recipeTime*0.6);
    }
    
    [Fact]
    public void CalculateRecipeProductionSpeed_ShouldReturnZeroCraftTime_AdditionalFactorHigh()
    {
        List<CraftPawn> craftPawns = new()
        {
            new(50, 50, 50, 50, 50, CraftPosition.Leader),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant)
        };

        const uint recipeTime = 100;
        _scriptableSettings.Set<double>("GameLogicSettings", "AdditionalProductionSpeedFactor", 100.0);

        uint result = _craftManager.CalculateRecipeProductionSpeed(recipeTime, new(), craftPawns);

        Assert.Equal(0u, result);
    }*/

    [Fact]
    public void CalculateEquipmentEnhancement_ShouldReturnCorrectEnhancementPoints()
    {
        List<CraftPawn> craftPawns = new()
        {
            new(50, 50, 50, 50, 50, CraftPosition.Leader),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant),
            new(50, 50, 50, 50, 50, CraftPosition.Assistant)
        };

        CraftCalculationResult result = _craftManager.CalculateEquipmentEnhancement(craftPawns, 0); // not sure what this is used for but I updated this function,
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

        uint rankUps = _craftManager.CalculatePawnRankUp(pawn);

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

        bool result = _craftManager.CanPawnRankUp(leadPawn);

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

        bool result = _craftManager.CanPawnRankUp(leadPawn);

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

        bool result = _craftManager.CanPawnRankUp(leadPawn);

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

        bool result = _craftManager.CanPawnRankUp(leadPawn);

        Assert.False(result);
    }
}
