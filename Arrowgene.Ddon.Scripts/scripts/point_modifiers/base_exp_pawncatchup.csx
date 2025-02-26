#load "libs/PartyUtils.csx"

public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.ExperiencePoints;
    public override PointModifierType ModifierType => PointModifierType.BaseModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Multiplicative;
    public override RewardSource Source => RewardSource.Enemy;
    public override PlayerType PlayerTypes => PlayerType.MyPawn;

    public override bool IsEnabled()
    {
        return LibDdon.GetSetting<bool>("PointModifierSettings", "EnablePawnCatchup");
    }

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        if (gameMode == GameMode.BitterblackMaze || characterCommon is not Pawn)
        {
            return 1.0;
        }

        Pawn pawn = (Pawn)characterCommon;

        var client = PartyUtils.GetPawnOwner(pawn, party);
        if (client == null)
        {
            return 1.0;
        }

        var playerLevel = client.Character.ActiveCharacterJobData.Lv;
        var pawnLevel = pawn.ActiveCharacterJobData.Lv;

        if (playerLevel <= pawnLevel)
        {
            return 1.0;
        }

        var lvDiff = playerLevel - pawnLevel;
        if (LibDdon.GetSetting<uint>("PointModifierSettings", "PawnCatchupLvDiff") > lvDiff)
        {
            return 1.0;
        }

        return LibDdon.GetSetting<double>("PointModifierSettings", "PawnCatchupMultiplier");
    }
}

return new PointModifier();
