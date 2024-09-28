public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.AreaPoints;
    public override PointModifierType ModifierType => PointModifierType.BonusModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Additive;
    public override RewardSource Source => RewardSource.Quest;
    public override PlayerType PlayerTypes => PlayerType.Player;

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        return LibDdon.GetCourseManager().QuestApBonus(questType);
    }
}

return new PointModifier();
