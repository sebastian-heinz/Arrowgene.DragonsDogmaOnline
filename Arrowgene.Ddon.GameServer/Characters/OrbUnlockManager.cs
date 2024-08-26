using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class OrbUnlockManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbUnlockManager));

        private readonly DdonGameServer _Server;

        public OrbUnlockManager(DdonGameServer server)
        {
            _Server = server;
        }

        public List<CDataOrbPageStatus> GetOrbPageStatus(CharacterCommon Character)
        {
            List<CDataOrbPageStatus> Result = new List<CDataOrbPageStatus>();

            Dictionary<uint, CDataOrbPageStatus> PageStatus = new Dictionary<uint, CDataOrbPageStatus>();
            Dictionary<uint, Dictionary<GroupNo, uint>> PageCompletionTotals = new Dictionary<uint, Dictionary<GroupNo, uint>>();

            foreach (var Element in _Server.Database.SelectOrbReleaseElementFromDragonForceAugmentation(Character.CommonId))
            {
                if (!PageStatus.ContainsKey(Element.PageNo))
                {
                    PageStatus.Add(Element.PageNo, new CDataOrbPageStatus() { PageNo = Element.PageNo });
                    PageCompletionTotals.Add(Element.PageNo, new Dictionary<GroupNo, uint>());
                }

                Dictionary<GroupNo, uint> Total = PageCompletionTotals[Element.PageNo];
                if (!Total.ContainsKey((GroupNo)Element.GroupNo))
                {
                    Total.Add((GroupNo)Element.GroupNo, 0);
                }

                Total[(GroupNo)Element.GroupNo] += 1;
            }

            foreach (var PageNo in PageCompletionTotals.Keys)
            {
                CDataOrbPageStatus CurrentPage = PageStatus[PageNo];
                foreach (var GroupNo in PageCompletionTotals[PageNo].Keys)
                {
                    CurrentPage.CategoryStatusList.Add(new CDataOrbCategoryStatus()
                    {
                        CategoryId = (byte)(GroupNo == GroupNo.Group5 ? 0 : GroupNo),
                        ReleaseNum = (byte)PageCompletionTotals[PageNo][GroupNo]
                    });
                }
            }

            foreach (var Value in PageStatus.Values)
            {
                Result.Add(Value);
            }

            return Result;
        }

        public List<CDataItemUpdateResult> GetDragonForceUpgradeUpdateList(CharacterCommon Character)
        {
            List<CDataItemUpdateResult> Results = new List<CDataItemUpdateResult>();

            var Upgrades = _Server.Database.SelectOrbReleaseElementFromDragonForceAugmentation(Character.CommonId);
            foreach (var Upgrade in Upgrades)
            {
                Results.Add(new CDataItemUpdateResult()
                {
                    UpdateItemNum = 1,
                    ItemList = new CDataItemList()
                    {
                        ItemNum = Upgrade.ElementId,
                        SlotNo = (byte)Upgrade.ElementId,
                        EquipCharacterID = Character.CommonId
                    }
                });
            }

            return Results;
        }

        private PacketQueue UpdateExtendedParamData(GameClient client, CharacterCommon character, DragonForceUpgrade upgrade, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();
            CDataOrbGainExtendParam obj = character.ExtendedParams;

            switch (upgrade.GainType)
            {
                case OrbGainParamType.AllJobsHpMax:
                    obj.HpMax += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AllJobsStaminaMax:
                    obj.StaminaMax += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AllJobsPhysicalAttack:
                    obj.Attack += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AllJobsPhysicalDefence:
                    obj.Defence += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AllJobsMagicalAttack:
                    obj.MagicAttack += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AllJobsMagicalDefence:
                    obj.MagicDefence += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AbilityCost:
                    obj.AbilityCost += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.AccessorySlot:
                    obj.JewelrySlot += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.MainPawnSlot:
                    obj.MainPawnSlot += (ushort)upgrade.Amount;
                    // When the player unlocks this, the total number will be increased to 3.
                    client.Character.MyPawnSlotNum += (byte)upgrade.Amount;
                    _Server.Database.UpdateMyPawnSlot(client.Character.CharacterId, client.Character.MyPawnSlotNum, connectionIn);
                    break;
                case OrbGainParamType.SupportPawnSlot:
                    obj.SupportPawnSlot += (ushort)upgrade.Amount;
                    client.Character.RentalPawnSlotNum += (byte)upgrade.Amount;
                    _Server.Database.UpdateRentalPawnSlot(client.Character.CharacterId, client.Character.RentalPawnSlotNum, connectionIn);
                    break;
                case OrbGainParamType.UseItemSlot:
                    obj.UseItemSlot += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.MaterialItemSlot:
                    obj.MaterialItemSlot += (ushort)upgrade.Amount;
                    break;
                case OrbGainParamType.EquipItemSlot:
                    obj.EquipItemSlot += (ushort)upgrade.Amount;
                    break;
                // TODO: OrbGainParamType.PawnAdventureNum Unhandled
                case OrbGainParamType.PawnAdventureNum:
                // TODO: OrbGainParamType.PawnCraftNum Unhandled => this might be a relic from pre season 3
                case OrbGainParamType.PawnCraftNum:
                // TODO: OrbGainParamType.MainPawnLostRate
                case OrbGainParamType.MainPawnLostRate:
                    break;
                case OrbGainParamType.SecretAbility:
                    queue.Enqueue(client, _Server.JobManager.UnlockSecretAbility(client, character, upgrade.SecretAbility, connectionIn));
                    break;
                case OrbGainParamType.Rim:
                    client.Enqueue(_Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.RiftPoints, upgrade.Amount, connectionIn:connectionIn), queue);
                    break;
                case OrbGainParamType.Gold:
                    client.Enqueue(_Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.Gold, upgrade.Amount, connectionIn: connectionIn), queue);
                    break;
                case OrbGainParamType.None:
                default:
                    break;
            }

            _Server.Database.UpdateOrbGainExtendParam(character.CommonId, obj, connectionIn);

            switch (upgrade.GainType)
            {
                case OrbGainParamType.AllJobsHpMax:
                case OrbGainParamType.AllJobsStaminaMax:
                case OrbGainParamType.AllJobsPhysicalAttack:
                case OrbGainParamType.AllJobsPhysicalDefence:
                case OrbGainParamType.AllJobsMagicalAttack:
                case OrbGainParamType.AccessorySlot:
                    queue.AddRange(_Server.CharacterManager.UpdateCharacterExtendedParamsNtc(client, character));
                    break;
                case OrbGainParamType.MainPawnSlot:
                    client.Enqueue(new S2CPawnExtendMainPawnSlotNtc() { TotalNum = client.Character.MyPawnSlotNum, AddNum = (byte)upgrade.Amount }, queue);
                    break;
                case OrbGainParamType.SupportPawnSlot:
                    client.Enqueue(new S2CPawnExtendSupportPawnSlotNtc() { TotalNum = client.Character.RentalPawnSlotNum, AddNum = (byte)upgrade.Amount }, queue);
                    break;
                case OrbGainParamType.AbilityCost:
                    // Handled by S2CSkillGetAbilityCostRes
                    // Handled by S2CProfileGetMyCharacterProfileRes
                    break;
                case OrbGainParamType.MainPawnLostRate:
                case OrbGainParamType.UseItemSlot:
                case OrbGainParamType.MaterialItemSlot:
                case OrbGainParamType.EquipItemSlot:
                    // TODO: Not sure how to notify/update this yet
                    break;
                default:
                    break;
            }

            return queue;
        }

        private DragonForceUpgrade GetPlayerUpgrade(GameClient client, Character character, uint elementId)
        {
            if (!gPlayerDragonForceUpgrades.ContainsKey(elementId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_INVALID_ELEMENT_ID, "Illegal request to unlock 'Dragon Force Augmentation Upgrade' -- Upgrade Doesn't Exist");
            }

            return gPlayerDragonForceUpgrades[elementId];
        }

        private DragonForceUpgrade GetPawnUpgrade(GameClient client, Pawn character, uint elementId)
        {
            if (!gPawnDragonForceUpgrades.ContainsKey(elementId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_INVALID_ELEMENT_ID, "Illegal request to unlock 'Dragon Force Augmentation Upgrade' -- Upgrade Doesn't Exist");
            }

            return gPawnDragonForceUpgrades[elementId];
        }

        public PacketQueue UnlockDragonForceAugmentationUpgrade(GameClient client, CharacterCommon character, uint elementId)
        {
            PacketQueue queue = new();

            DragonForceUpgrade upgrade = null;

            if (character is Character)
            {
                upgrade = GetPlayerUpgrade(client, (Character)character, elementId);
            }
            else
            {
                upgrade = GetPawnUpgrade(client, (Pawn)character, elementId);
            }

            if (upgrade == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_INVALID_ELEMENT_ID);
            }

            // Check for Valid Conditions before continuing
            if (upgrade.IsRestrictedByTotalLevels())
            {
                uint totalLevels = TotalLevelsGained(character);
                if (totalLevels < upgrade.LvlUpCost)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_NOT_COMPLETE_TRUNK);
                }
            }
            else if (upgrade.IsRestrictedByOrbCost())
            {
                if (_Server.WalletManager.GetWalletAmount(client.Character, WalletType.BloodOrbs) < upgrade.LvlUpCost)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_ORB_LACK);
                }
            }
            else if (upgrade.IsRestrictedByFullPageUnlock())
            {
                // TODO: Figure out how to determine this
                // TODO: Probably need to query the DB
                // TODO: Technically, the client is restricting this...
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_INTERNAL_ERROR);
            }

            if (upgrade.IsRestrictedByOrbCost())
            {
                var walletPointUpdate = _Server.WalletManager.RemoveFromWallet(client.Character, WalletType.BloodOrbs, upgrade.LvlUpCost)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ORB_DEVOTE_ORB_LACK);
            }

            _Server.Database.ExecuteInTransaction(connection =>
            {
                queue.AddRange(UpdateExtendedParamData(client, character, upgrade, connection));

                character.OrbRelease.Add(new()
                {
                    ElementId = elementId,
                    PageNo = (byte)upgrade.PageNo,
                    GroupNo = (byte)upgrade.GroupNo,
                    Index = (byte)upgrade.IndexNo,
                });
                _Server.Database.InsertIfNotExistsDragonForceAugmentation(character.CommonId, elementId, upgrade.PageNo, upgrade.GroupNo, upgrade.IndexNo, connection);

                if (character is Character)
                {
                    S2COrbDevoteReleaseOrbElementRes response = new S2COrbDevoteReleaseOrbElementRes()
                    {
                        GainParamType = upgrade.GainType,
                        RestOrb = _Server.WalletManager.GetWalletAmount(client.Character, WalletType.BloodOrbs),
                        GainParamValue = upgrade.Amount
                    };

                    client.Enqueue(response, queue);

                    queue.AddRange(_Server.AchievementManager.HandleOrbDevote(client, upgrade.GainType, connection));
                }
                else
                {
                    S2COrbDevoteReleasePawnOrbElementRes response = new S2COrbDevoteReleasePawnOrbElementRes()
                    {
                        PawnId = ((Pawn)character).PawnId,
                        GainParamType = upgrade.GainType,
                        RestOrb = _Server.WalletManager.GetWalletAmount(client.Character, WalletType.BloodOrbs),
                        GainParamValue = upgrade.Amount
                    };

                    client.Enqueue(response, queue);
                }
            });

            return queue;
        }

        private uint TotalLevelsGained(CharacterCommon Character)
        {
            uint TotalLevels = 0;
            foreach (var JobData in Character.CharacterJobDataList)
            {
                TotalLevels += JobData.Lv;
            }

            return TotalLevels;
        }

        public static Dictionary<OrbGainParamType, int> CountParamType(CharacterCommon character)
        {
            var upgrades = character is Character ? gPlayerDragonForceUpgrades : gPawnDragonForceUpgrades;
            return character.OrbRelease.GroupBy(x => upgrades[x.ElementId].GainType).ToDictionary(g => g.Key, g => g.Count());
        }

        private class DragonForceUpgrade
        {
            public DragonForceUpgrade()
            {
                this.LvlUpRestrictionType = LvlUpRestrictionType.None;
                this.LvlUpCost = 0;
                this.GainType = OrbGainParamType.None;
                this.Amount = 0;
                this.Category = Category.None;
            }

            public OrbGainParamType GainType { get; private set; }
            public uint Amount { get; private set; }
            public LvlUpRestrictionType LvlUpRestrictionType { get; private set; }
            public uint LvlUpCost { get; private set; }
            public AbilityId SecretAbility { get; private set; }
            public uint PageNo { get; private set; }
            public uint GroupNo { get; private set; }
            public uint IndexNo { get; private set; }
            public Category Category { get; private set; }

            public bool IsSecretAbility()
            {
                return this.GainType == OrbGainParamType.SecretAbility;
            }

            public bool IsRestrictedByTotalLevels()
            {
                return this.LvlUpRestrictionType == LvlUpRestrictionType.TotalLevels;
            }

            public bool IsRestrictedByOrbCost()
            {
                return this.LvlUpRestrictionType == LvlUpRestrictionType.Orbs;
            }

            public bool IsRestrictedByFullPageUnlock()
            {
                return this.LvlUpRestrictionType == LvlUpRestrictionType.PageUnlocked;
            }

            public DragonForceUpgrade HasTotalLevelsRestriction(uint amount)
            {
                this.LvlUpRestrictionType = LvlUpRestrictionType.TotalLevels;
                this.LvlUpCost = amount;

                return this;
            }

            public DragonForceUpgrade HasPageUnlockRestriction()
            {
                this.LvlUpRestrictionType = LvlUpRestrictionType.PageUnlocked;
                return this;
            }

            public DragonForceUpgrade HasOrbUnlockRestriction(uint Amount)
            {
                this.LvlUpRestrictionType = LvlUpRestrictionType.Orbs;
                this.LvlUpCost = Amount;
                return this;
            }

            public DragonForceUpgrade Unlocks(OrbGainParamType Type, uint Amount)
            {
                this.GainType = Type;
                this.Amount = Amount;
                return this;
            }

            public DragonForceUpgrade Unlocks(AbilityId Type)
            {
                this.GainType = OrbGainParamType.SecretAbility;
                this.SecretAbility = Type;
                this.Amount = (uint)Type;
                return this;
            }

            public DragonForceUpgrade Location(PageNo PageNo, GroupNo GroupNo, uint IndexNo)
            {
                this.PageNo = (uint)PageNo;
                this.GroupNo = (uint)GroupNo;
                this.IndexNo = IndexNo;
                this.Category = GroupNo2Category(GroupNo);
                return this;
            }

            public CDataReleaseOrbElement AsCDataReleaseOrbElement()
            {
                return new CDataReleaseOrbElement()
                {
                    ElementId = 0
                };
            }

            private Category GroupNo2Category(GroupNo GroupNo)
            {
                switch (GroupNo)
                {
                    case GroupNo.Group1:
                        return Category.Vitality;
                    case GroupNo.Group2:
                        return Category.Adventure;
                    case GroupNo.Group3:
                        return Category.Magick;
                    case GroupNo.Group4:
                        return Category.Combat;
                    case GroupNo.Group5:
                        return Category.Other;
                }

                return Category.None;
            }
        }

        private enum LvlUpRestrictionType
        {
            None = 0,
            Orbs = 1,
            TotalLevels = 2,
            PageUnlocked = 3
        }

        private enum PageNo : byte
        {
            Page1 = 1,
            Page2 = 2,
            Page3 = 3,
            Page4 = 4
        }

        private enum GroupNo : byte
        {
            Group1 = 1,
            Group2 = 2,
            Group3 = 3,
            Group4 = 4,
            Group5 = 5,
        }

        private enum Category : byte
        {
            None = 0,
            Vitality = 1,
            Adventure = 2,
            Other = 3,
            Magick = 4,
            Combat = 5,
        }

        private static readonly Dictionary<uint, DragonForceUpgrade> gPlayerDragonForceUpgrades = new Dictionary<uint, DragonForceUpgrade>()
        {
            #region PAGE1

            [0x01] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x02] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(12)
                .Unlocks(OrbGainParamType.EquipItemSlot, 5),
            [0x03] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(8)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 100),
            [0x04] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(4)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 5),
            [0x05] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(1)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 5),

            [0x15] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x16] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(70)
                .Unlocks(AbilityId.Efficacy),
            [0x17] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x18] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x19] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(70)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 60),
            [0x1a] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 40),
            [0x1b] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 60),
            [0x1c] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),

            [0x1d] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(30)
                .Unlocks(AbilityId.EffectExtension),
            [0x1e] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(75)
                .Unlocks(OrbGainParamType.MaterialItemSlot, 5),
            [0x1f] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.AbilityCost, 5),
            [0x20] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(30)
                .Unlocks(AbilityId.ExtendedSprings),
            [0x21] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(50)
                .Unlocks(OrbGainParamType.EquipItemSlot, 5),
            [0x22] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.Gold, 5000),
            [0x23] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x24] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.Rim, 2000),

            [0x25] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1),
            [0x26] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x27] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0x28] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 35),
            [0x29] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0x2a] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0x2b] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(AbilityId.RainDefense),
            [0x2c] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 3),

            [0x2d] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1),
            [0x2e] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x2f] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x30] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 35),
            [0x31] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x32] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x33] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(AbilityId.RainAttack),
            [0x34] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 3),

            #endregion

            #region PAGE2

            [0x06] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.SupportPawnSlot, 1), // Support Pawn Hires?
            [0x07] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(28)
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x08] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(24)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 100),
            [0x09] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 5),
            [0x0a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(16)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 5),

            [0x35] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(170)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x36] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(220)
                .Unlocks(AbilityId.SoftTouch),
            [0x37] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x38] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x39] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(250)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x3a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),
            [0x3b] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(200)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 40),
            [0x3c] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),

            [0x3d] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(180)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x3e] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(250)
                .Unlocks(OrbGainParamType.MaterialItemSlot, 5),
            [0x3f] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(140)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0x40] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.Gold, 30000),
            [0x41] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(250)
                .Unlocks(AbilityId.Gathering),
            [0x42] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.Rim, 5000),
            [0x43] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0x44] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(200)
                .Unlocks(AbilityId.EnhancedThrow),

            [0x45] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0x46] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(90)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x47] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 3),
            [0x48] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(130)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 25),
            [0x49] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 1),
            [0x4a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0x4b] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(200)
                .Unlocks(AbilityId.NewDefense),
            [0x4c] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),

            [0x4d] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x4e] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(90)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x4f] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 3),
            [0x50] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(130)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 25),
            [0x51] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 1),
            [0x52] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x53] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(200)
                .Unlocks(AbilityId.NewAttack),
            [0x54] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),

            #endregion

            #region PAGE3

            [0x0b] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.MainPawnSlot, 1),
            [0x0c] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(44)
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x0d] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x0e] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(36)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 5),
            [0x0f] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(32)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 5),

            [0x55] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x56] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(1000)
                .Unlocks(AbilityId.Flow),
            [0x57] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(520)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x58] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x59] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 40),
            [0x5a] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(700)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 40),
            [0x5b] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(800)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x5c] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),

            [0x5d] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.Gold, 100000),
            [0x5e] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(1000)
                .Unlocks(OrbGainParamType.MaterialItemSlot, 5),
            [0x5f] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x60] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(750)
                .Unlocks(AbilityId.ExpertExcavator),
            [0x61] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(650)
                .Unlocks(OrbGainParamType.AbilityCost, 5),
            [0x62] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(850)
                .Unlocks(OrbGainParamType.EquipItemSlot, 5),
            [0x63] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(750)
                .Unlocks(AbilityId.Featherfoot),
            [0x64] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.AbilityCost, 4),

            [0x65] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(300)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1),
            [0x66] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(480)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x67] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0x68] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x69] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 3),
            [0x6a] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0x6b] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(750)
                .Unlocks(AbilityId.MoonlightDefense),
            [0x6c] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),

            [0x6d] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(300)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1),
            [0x6e] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(480)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x6f] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x70] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x71] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 3),
            [0x72] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x73] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(750)
                .Unlocks(AbilityId.MoonlightAssault),
            [0x74] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),

            #endregion

            #region PAGE4

            [0x10] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.PawnAdventureNum, 1),
            [0x11] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(60)
                .Unlocks(OrbGainParamType.EquipItemSlot, 5),
            [0x12] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(56)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x13] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(52)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 5),
            [0x14] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(48)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 5),

            [0x75] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x76] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(5000)
                .Unlocks(AbilityId.Willpower),
            [0x77] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),
            [0x78] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x79] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 40),
            [0x7a] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),
            [0x7b] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(2500)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x7c] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),

            [0x7d] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0x7e] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(3000)
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x7f] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(2500)
                .Unlocks(OrbGainParamType.AbilityCost, 5),
            [0x80] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(2700)
                .Unlocks(AbilityId.TreasureEye),
            [0x81] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(2200)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x82] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(3500)
                .Unlocks(AbilityId.SafeLanding),
            [0x83] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(2200)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x84] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(2800)
                .Unlocks(OrbGainParamType.MaterialItemSlot, 5),

            [0x85] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0x86] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(2300)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x87] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 4),
            [0x88] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(3000)
                .Unlocks(AbilityId.Rakshasa),
            [0x89] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0x8a] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 3),
            [0x8b] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x8c] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 3),

            [0x8d] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x8e] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(2300)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x8f] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 4),
            [0x90] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(3000)
                .Unlocks(AbilityId.Yasha),
            [0x91] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x92] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 3),
            [0x93] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x94] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 3)

            #endregion
        };

        private static readonly Dictionary<uint, DragonForceUpgrade> gPawnDragonForceUpgrades = new Dictionary<uint, DragonForceUpgrade>()
        {
            #region PAGE1

            [0x95] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x96] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(12)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 5),
            [0x97] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(8)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 100),
            [0x98] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(4)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 5),
            [0x99] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(1)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 5),

            [0xa9] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0xaa] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xab] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xac] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0xad] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 60),
            [0xae] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),
            [0xaf] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 60),
            [0xb0] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),

            [0xb1] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.Rim, 500),
            [0xb2] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(60)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0xb3] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 3),
            [0xb4] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xb5] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.Rim, 350),
            [0xb6] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xb7] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.Gold, 3000),
            [0xb8] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 3),

            [0xb9] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1),
            [0xba] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xbb] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0xbc] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xbd] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0xbe] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0xbf] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xc0] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 3),

            [0xc1] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1),
            [0xc2] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xc3] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0xc4] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xc5] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0xc6] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0xc7] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xc8] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(25)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 3),

            #endregion

            #region PAGE2

            [0x9a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x9b] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(28)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 5),
            [0x9c] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(24)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 100),
            [0x9d] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(20)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 5),
            [0x9e] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(16)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 5),

            [0xc9] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(80)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0xca] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(180)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xcb] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(70)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xcc] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(85)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0xcd] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0xce] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),
            [0xcf] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 40),
            [0xd0] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(180)
                .Unlocks(OrbGainParamType.AbilityCost, 2),

            [0xd1] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(90)
                .Unlocks(OrbGainParamType.Rim, 2000),
            [0xd2] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(180)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xd3] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(80)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 2),
            [0xd4] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xd5] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(50)
                .Unlocks(OrbGainParamType.Rim, 1000),
            [0xd6] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(180)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xd7] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(OrbGainParamType.Gold, 10000),
            [0xd8] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 2),

            [0xd9] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(70)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0xda] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(80)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0xdb] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 3),
            [0xdc] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(130)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xdd] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(60)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 1),
            [0xde] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0xdf] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(80)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0xe0] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),

            [0xe1] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(70)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0xe2] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(80)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0xe3] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 3),
            [0xe4] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(130)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xe5] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(60)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 1),
            [0xe6] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0xe7] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(80)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0xe8] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),

            #endregion

            #region PAGE3

            [0x9f] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0xa0] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(44)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 5),
            [0xa1] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(40)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0xa2] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(36)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 5),
            [0xa3] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(32)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 5),

            [0xe9] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(350)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0xea] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(520)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0xeb] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(480)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 30),
            [0xec] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(400)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0xed] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 40),
            [0xee] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(380)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xef] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0xf0] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(380)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),

            [0xf1] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0xf2] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.Gold, 30000),
            [0xf3] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(350)
                .Unlocks(OrbGainParamType.Rim, 3000),
            [0xf4] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(380)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0xf5] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(400)
                .Unlocks(OrbGainParamType.Rim, 5000),
            [0xf6] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 3),
            [0xf7] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0xf8] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(450)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 2),

            [0xf9] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(280)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1),
            [0xfa] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(320)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0xfb] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(400)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0xfc] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(380)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0xfd] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(420)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0xfe] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(450)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0xff] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(320)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x100] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 3),

            [0x101] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(280)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1),
            [0x102] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(320)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x103] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(400)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x104] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(380)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x105] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(420)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x106] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(450)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x107] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(320)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x108] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 3),
            #endregion

            #region PAGE4

            [0xa4] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0xa5] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(60)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 5),
            [0xa6] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(56)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 80),
            [0xa7] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(52)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 5),
            [0xa8] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(48)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 5),

            [0x109] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 30),
            [0x10a] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(1750)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0x10b] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 25),
            [0x10c] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0x10d] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 40),
            [0x10e] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(2250)
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0x10f] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsHpMax, 50),
            [0x110] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 25),

            [0x111] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(2750)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 3),
            [0x112] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 2)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.Rim, 10000),
            [0x113] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 3)
                .HasOrbUnlockRestriction(1750)
                .Unlocks(OrbGainParamType.AbilityCost, 2),
            [0x114] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(2500)
                .Unlocks(OrbGainParamType.MainPawnLostRate, 3),
            [0x115] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.Rim, 7500),
            [0x116] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(3000)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x117] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 7)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.Gold, 50000),
            [0x118] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(1750)
                .Unlocks(OrbGainParamType.AbilityCost, 2),

            [0x119] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2),
            [0x11a] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(1000)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x11b] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 4),
            [0x11c] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x11d] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(1300)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 2),
            [0x11e] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 3),
            [0x11f] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x120] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(1600)
                .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 3),

            [0x121] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2),
            [0x122] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(1000)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x123] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 4),
            [0x124] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 20),
            [0x125] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(1300)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2),
            [0x126] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 3),
            [0x127] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(1250)
                .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15),
            [0x128] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(1600)
                .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 3)

            #endregion
        };
    }
}
