namespace Arrowgene.Ddon.Shared.Model
{
    public enum OrbGainParamType : byte
    {
        None = 0x0,
        AllJobsHpMax = 1,
        AllJobsStaminaMax = 2,
        AllJobsPhysicalAttack = 3,
        AllJobsPhysicalDefence = 4,
        AllJobsMagicalAttack = 5,
        AllJobsMagicalDefence = 6,
        Rim = 7,
        Gold = 8,
        AbilityCost = 9,
        AccessorySlot = 10,
        PawnAdventureNum = 11,
        PawnCraftNum = 12,
        MainPawnLostRate = 13,
        MainPawnSlot = 14,
        SupportPawnSlot = 15,
        UseItemSlot = 16,
        MaterialItemSlot = 17,
        EquipItemSlot = 18,
        SecretAbility = 19,
        JobHpMax = 20,
        JobStaminaMax = 21,
        JobPhysicalAttack = 22,
        JobPhysicalDefence = 23,
        JobMagicalAttack = 24,
        JobMagicalDefence = 25,
        JobAugment = 26,
        JobCustomSkill = 27,
        BlessingOfAugments = 28, // TODO: Rename this when figuring out what it does
        BlessingOfTheWhiteDragon = 29,
        JobItemSlot = 30,
    }
}
