public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.ExperiencePoints;
    public override PointModifierType ModifierType => PointModifierType.BaseModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Multiplicative;
    public override RewardSource Source => RewardSource.Enemy;
    public override PlayerType PlayerTypes => (PlayerType.Player | PlayerType.MyPawn);

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        double multiplier = 1.0;
        switch (gameMode)
        {
            case GameMode.Normal:
                multiplier = LibDdon.GetSetting<double>("GameServerSettings", "EnemyExpModifier");
                break;
            case GameMode.BitterblackMaze:
                multiplier = LibDdon.GetSetting<double>("GameServerSettings", "BBMEnemyExpModifier");
                break;
            default:
                break;
        }

        return multiplier;
    }
}

return new PointModifier();
