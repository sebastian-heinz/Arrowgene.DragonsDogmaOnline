using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum ItemSubCategory //For serverside use only.
    {
        UseNone = 1,
        UseThrowing = 2,
        UseMine = 3,
        UseLumber = 4,
        UseLockpick = 5,
        UseJobItem = 6,
        UseDoorKey = 8,

        //MaterialCategory, offset by 100
        MaterialInorganicMetal = 101,
        MaterialInorganicOre = 102,
        MaterialInorganicSand = 103,
        MaterialSewingCloth = 104,
        MaterialSewingString = 105,
        MaterialSewingFur = 106,
        MaterialAnimalSkin = 107,
        MaterialAnimalBone = 108,
        MaterialAnimalFang = 109,
        MaterialAnimalHorn = 110,
        MaterialUnusedShell = 111,
        MaterialAnimalFeather = 112,
        MaterialInorganicGem = 113,
        MaterialPlantGrass = 114,
        MaterialPlantMushroom = 117,
        MaterialPlantLumber = 118,
        MaterialInorganicLiquid = 119,
        MaterialSpecialScroll = 120,
        MaterialAnimalMeat = 122,
        MaterialSpecialOther = 123,
        MaterialCrestWeapon = 124,
        MaterialCrestArmor = 125,
        MaterialRefiningWeapon = 126,
        MaterialRefiningArmor = 127,
        MaterialSpecialDye = 128,
        MaterialAppraisedItem = 129,
        MaterialRegionalMaterial = 130,
        MaterialSlayerStone = 131,
        MaterialPawnInspiration = 132,
        MaterialDragonAbility = 133,

        //EquipSlot, offset by 200
        EquipArmorHelm = 203,
        EquipArmorBody = 204,
        EquipClothingBody = 205,
        EquipArmorArm = 206,
        EquipArmorLeg = 207,
        EquipClothingLeg = 208,
        EquipOverwear = 209,
        EquipJewelry = 210,
        EquipLantern = 211,
        EquipEnsemble = 212,

        //JewelrySubCategory, offset by 300
        JewelryCommon = 326,
        JewelryRing = 342,
        JewelryBracelet = 358,
        JewelryPierce = 374,
        EmblemStone = 390,
        
        //WeaponCategory, offset by 400
        WeaponHand = 400,
        WeaponSword = 401,
        WeaponShield = 402,
        WeaponGreatsword = 403,
        WeaponGreatshield = 404,
        WeaponRod = 405,
        WeaponDagger = 406,
        WeaponBow = 407,
        WeaponGauntlet = 408,
        WeaponMagickBow = 409,
        WeaponStaff = 411,
        WeaponArchistaff = 412,
        WeaponLance = 413,
        WeaponMagickSword = 415,
    }
}
