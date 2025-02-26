public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.PlayPoints;
    public override PointModifierType ModifierType => PointModifierType.BaseModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Multiplicative;
    public override RewardSource Source => RewardSource.Enemy;
    public override PlayerType PlayerTypes => PlayerType.Player;

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        return LibDdon.GetSetting<double>("GameServerSettings", "PpModifier");
    }
}

return new PointModifier();
