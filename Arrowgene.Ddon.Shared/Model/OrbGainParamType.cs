namespace Arrowgene.Ddon.Shared.Model
{
    public enum OrbGainParamType : byte
    {
        None = 0x0,
        HpMax = 0x1,
        StaminaMax = 0x2,
        PhysicalAttack = 0x3,
        PhysicalDefence = 0x4,
        MagicalAttack = 0x5,
        MagicalDefence = 0x6,
        Rim = 0x7,
        Gold = 0x8,
        AbilityCost = 0x9,
        AccessorySlot = 0xa,
        PawnAdventureNum = 0xb,
        PawnCraftNum = 0xc,
        MainPawnLostRate = 0xd,
        MainPawnSlot = 0xe,
        SupportPawnSlot = 0xf,
        UseItemSlot = 0x10,
        MaterialItemSlot = 0x11,
        EquipItemSlot = 0x12,
        SecretAbility = 0x13,
    }
}
