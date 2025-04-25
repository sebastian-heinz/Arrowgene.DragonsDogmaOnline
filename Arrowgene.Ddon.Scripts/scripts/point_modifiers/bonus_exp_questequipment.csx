public class PointModifier : IPointModifier
{
    public override PointType PointType => PointType.ExperiencePoints;
    public override PointModifierType ModifierType => PointModifierType.BonusModifier;
    public override PointModifierAction ModifierAction => PointModifierAction.Additive;
    public override RewardSource Source => RewardSource.Quest;
    public override PlayerType PlayerTypes => (PlayerType.Player | PlayerType.MyPawn);

    private double GetRookiesRingBonus(CharacterCommon characterCommon, QuestType questType)
    {
        if (!LibDdon.GetSetting<bool>("RookiesRing", "EnableRookiesRing"))
        {
            return 0.0;
        }

        if (questType != QuestType.World)
        {
            return 0.0;
        }

        if (!LibDdon.Character.HasEquipped(characterCommon, EquipType.Performance, ItemId.RookiesRingOfBlessing))
        {
            return 0.0;
        }

        var rookiesRing = LibDdon.Item.GetItemInterface(ItemId.RookiesRingOfBlessing);
        if (rookiesRing == null)
        {
            return 0.0;
        }

        return rookiesRing.GetBonusMultiplier(characterCommon);
    }

    public override double GetMultiplier(GameMode gameMode, CharacterCommon characterCommon, PartyGroup party, InstancedEnemy enemy, QuestType questType)
    {
        if (characterCommon == null)
        {
            return 0.0;
        }
        return GetRookiesRingBonus(characterCommon, questType);
    }
}

return new PointModifier();
