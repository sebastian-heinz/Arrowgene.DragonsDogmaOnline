namespace Arrowgene.Ddon.Shared.Model
{
    public enum EquipCategory : byte
    {
        WepMain = 0x1,
        WepSub = 0x2,
        ArmorHelm = 0x3,
        ArmorBody = 0x4,
        WearBody = 0x5,
        ArmorArm = 0x6,
        ArmorLeg = 0x7,
        WearLeg = 0x8,
        Accessory = 0x9,
        Jewelry = 0xA,
        Lantern = 0xB,
        Costume = 0xC,
        // JobItem = 0xD // This is just speculation try it out if you think you need it
    }
}
