public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.ExperiencePoints;
    public override PointModifierType ModifierType => PointModifierType.BonusModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Additive;
    public override RewardSource Source => RewardSource.Enemy;
    public override PlayerType PlayerTypes => PlayerType.Player;

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        return LibDdon.GetCourseManager().EnemyExpBonus(characterCommon);
    }
}

return new PointModifier();