namespace Arrowgene.Ddon.Shared.Model
{
    // Check nItem::E_STORAGE_TYPE in the PS4 debug symbols for IDs?
    public enum StorageType : byte
    {
        ItemBagConsumable = 0x1,
        ItemBagMaterial = 0x2,
        ItemBagEquipment = 0x3, 
        ItemBagJob = 0x4,
        Unk5 = 0x5,
        Unk6 = 0x6,
        Unk7 = 0x7,
        Unk8 = 0x8,
        Unk9 = 0x9,
        Unk10 = 0xA,
        Unk11 = 0xB,
        Unk12 = 0xC,
        Unk13 = 0xD,
        Unk14 = 0xE,
        Unk15 = 0xF,
        Unk16 = 0x10,
        Unk17 = 0x11,
    }
}