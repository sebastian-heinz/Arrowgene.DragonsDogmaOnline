#load "libs/PartyUtils.csx"

public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.ExperiencePoints;
    public override PointModifierType ModifierType => PointModifierType.BaseModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Multiplicative;
    public override RewardSource Source => RewardSource.Enemy;
    public override PlayerType PlayerTypes => (PlayerType.Player | PlayerType.MyPawn);

    public override bool IsEnabled()
    {
        return LibDdon.GetSetting<bool>("PointModifierSettings", "EnableAdjustPartyEnemyExp");
    }

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        if (gameMode == GameMode.BitterblackMaze)
        {
            return 1.0;
        }

        if (LibDdon.GetSetting<bool>("PointModifierSettings", "DisableExpCorrectionForMyPawn") && PartyUtils.AllMembersOwnedByPartyLeader(party))
        {
            return 1.0;
        }

        var range = PartyUtils.LevelRange(party);

        double multiplier = 0;
        foreach (var tier in LibDdon.GetSetting<List<(uint MinLv, uint MaxLv, double ExpMultiplier)>>("PointModifierSettings", "AdjustPartyEnemyExpTiers"))
        {
            if (range >= tier.MinLv && range <= tier.MaxLv)
            {
                multiplier = tier.ExpMultiplier;
                break;
            }
        }

        return multiplier;
    }
}

return new PointModifier();
