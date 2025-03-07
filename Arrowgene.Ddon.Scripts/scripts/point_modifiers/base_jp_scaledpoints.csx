public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.JobPoints;
    public override PointModifierType ModifierType => PointModifierType.BaseModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Multiplicative;
    public override RewardSource Source => (RewardSource.None | RewardSource.Enemy | RewardSource.Quest);
    public override PlayerType PlayerTypes => (PlayerType.Player | PlayerType.MyPawn);

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        if (gameMode == GameMode.BitterblackMaze)
        {
            return 0.0;
        }

        return LibDdon.GetSetting<double>("GameServerSettings", "JpModifier");
    }
}

return new PointModifier();
