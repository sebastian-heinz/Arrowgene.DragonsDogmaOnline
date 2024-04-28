using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using YamlDotNet.Serialization.NodeDeserializers;
using static Arrowgene.Ddon.Server.Network.Challenge;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class OrbUnlockManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbUnlockManager));

        private readonly IDatabase _Database;
        private readonly WalletManager _WalletManager;
        private readonly JobManager _JobManager;
        private readonly CharacterManager _CharacterManager;

        public OrbUnlockManager(IDatabase Database, WalletManager WalletManager, JobManager JobManager, CharacterManager CharacterManager)
        {
            _Database = Database;
            _WalletManager = WalletManager;
            _CharacterManager = CharacterManager;
            _JobManager = JobManager;
        }

        public List<CDataOrbPageStatus> GetOrbPageStatus(CharacterCommon Character)
        {
            List<CDataOrbPageStatus> Result = new List<CDataOrbPageStatus>();

            Dictionary<uint, CDataOrbPageStatus> PageStatus = new Dictionary<uint, CDataOrbPageStatus>();
            Dictionary<uint, Dictionary<GroupNo, uint>> PageCompletionTotals = new Dictionary<uint, Dictionary<GroupNo, uint>>();

            foreach (var Element in _Database.SelectOrbReleaseElementFromDragonForceAugmentation(Character.CommonId))
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
                        CategoryId = (byte) (GroupNo == GroupNo.Group5 ? 0 : GroupNo),
                        ReleaseNum = (byte) PageCompletionTotals[PageNo][GroupNo]
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

            var Upgrades = _Database.SelectOrbReleaseElementFromDragonForceAugmentation(Character.CommonId);
            foreach (var Upgrade in Upgrades)
            {
                Results.Add(new CDataItemUpdateResult() {
                    UpdateItemNum = 1,
                    ItemList = new CDataItemList() {
                        ItemNum = Upgrade.ElementId,
                        SlotNo = (byte)Upgrade.ElementId,
                        EquipCharacterID = Character.CommonId
                    }
                });
            }

            return Results;
        }

        private CDataOrbGainExtendParam UpdateExtendedParamData(GameClient Client, CharacterCommon Character, DragonForceUpgrade Upgrade)
        {
            CDataOrbGainExtendParam obj = Character.ExtendedParams;

            switch (Upgrade.GainType)
            {
                case OrbGainParamType.HpMax:
                    obj.HpMax += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.StaminaMax:
                    obj.StaminaMax += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.PhysicalAttack:
                    obj.Attack += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.PhysicalDefence:
                    obj.Defence += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.MagicalAttack:
                    obj.MagicAttack += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.MagicalDefence:
                    obj.MagicDefence += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.AbilityCost:
                    obj.AbilityCost += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.AccessorySlot:
                    obj.JewelrySlot += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.MainPawnSlot:
                    obj.MainPawnSlot += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.SupportPawnSlot:
                    obj.SupportPawnSlot += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.UseItemSlot:
                    obj.UseItemSlot += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.MaterialItemSlot:
                    obj.MaterialItemSlot += (ushort) Upgrade.Amount;
                    break;
                case OrbGainParamType.EquipItemSlot:
                    obj.EquipItemSlot += (ushort) Upgrade.Amount;
                    break;
                // Unhandeled
                case OrbGainParamType.PawnAdventureNum:
                case OrbGainParamType.PawnCraftNum:
                case OrbGainParamType.MainPawnLostRate:
                    break;
                case OrbGainParamType.SecretAbility:
                    _JobManager.UnlockSecretAbility(Character, Upgrade.SecretAbility);
                    break;
                case OrbGainParamType.Rim:
                    _WalletManager.AddToWalletNtc(Client, (Character)Character, WalletType.RiftPoints, Upgrade.Amount);
                    break;
                case OrbGainParamType.Gold:
                    _WalletManager.AddToWalletNtc(Client, (Character)Character, WalletType.Gold, Upgrade.Amount);
                    break;
                case OrbGainParamType.None:
                default:
                    break;
            }

            _Database.UpdateOrbGainExtendParam(Character.CommonId, obj);

            switch (Upgrade.GainType)
            {
                case OrbGainParamType.HpMax:
                case OrbGainParamType.StaminaMax:
                case OrbGainParamType.PhysicalAttack:
                case OrbGainParamType.PhysicalDefence:
                case OrbGainParamType.MagicalAttack:
                case OrbGainParamType.AccessorySlot:
                case OrbGainParamType.MainPawnSlot:
                case OrbGainParamType.SupportPawnSlot:
                    if (Character is Character)
                    {
                        _CharacterManager.UpdateCharacterExtendedParamsNtc(Client, (Character)Character);
                    }
                    break;
                case OrbGainParamType.AbilityCost:
                    // Handeled by S2CSkillGetAbilityCostRes
                    // Handeled by S2CProfileGetMyCharacterProfileRes
                    break;
                case OrbGainParamType.UseItemSlot:
                case OrbGainParamType.MaterialItemSlot:
                case OrbGainParamType.EquipItemSlot:
                    // TODO: Not sure how to notify/update this yet
                    break;
                default:
                    break;
            }

            return obj;
        }

        public void UnlockDragonForceAugmentationUpgrade(GameClient Client, CharacterCommon Character, uint ElementId)
        {
            if (!gDragonForceUpgrades.ContainsKey(ElementId))
            {
                Logger.Error("Illegal request to unlock 'Dragon Force Augmentation Upgrade' -- Skill Doesn't Exist");
                S2COrbDevoteReleaseHandlerRes Error = new S2COrbDevoteReleaseHandlerRes()
                {
                    Error = 0x1baddeed
                };
                Client.Send(Error);
                return;
            }

            var Upgrade = gDragonForceUpgrades[ElementId];

            // Check for Valid Conditions before continuing
            bool CheckPassed = true;
            if (Upgrade.IsRestrictedByTotalLevels())
            {
                uint TotalLevels = TotalLevelsGained(Character);
                if (TotalLevels < Upgrade.LvlUpCost)
                {
                    CheckPassed = false;
                }
            }
            else if (Upgrade.IsRestrictedByOrbCost())
            {
                if (_WalletManager.GetWalletAmount((Character)Character, WalletType.BloodOrbs) < Upgrade.LvlUpCost)
                {
                    CheckPassed = false;
                }
            }
            else if (Upgrade.IsRestrictedByFullPageUnlock())
            {
                // TODO: Figure out how to determine this
                // TODO: Probably need to query the DB
                // TODO: Technically, the client is restricting this...
            }
            else
            {
                CheckPassed = false;
            }

            if (!CheckPassed)
            {
                Logger.Error("Illegal request to unlock 'Dragon Force Augmentation Upgrade' -- Constraint checks failed");
                S2COrbDevoteReleaseHandlerRes Error = new S2COrbDevoteReleaseHandlerRes()
                {
                    Error = 0x1baddeed
                };
                Client.Send(Error);
                return;
            }

            if (Upgrade.IsRestrictedByOrbCost())
            {
                _WalletManager.RemoveFromWallet((Character)Character, WalletType.BloodOrbs, Upgrade.LvlUpCost);
            }

            CDataOrbGainExtendParam ExtendParam = UpdateExtendedParamData(Client, Character, Upgrade);

            _Database.InsertIfNotExistsDragonForceAugmentation(Character.CommonId, ElementId, Upgrade.PageNo, Upgrade.GroupNo, Upgrade.IndexNo);

            S2COrbDevoteReleaseHandlerRes Response = new S2COrbDevoteReleaseHandlerRes()
            {
                GainParamType = Upgrade.GainType,
                RestOrb = _WalletManager.GetWalletAmount((Character)Character, WalletType.BloodOrbs),
                GainParamValue = Upgrade.Amount
            };

            Client.Send(Response);
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
            public uint Amount { get; private set;}
            public LvlUpRestrictionType LvlUpRestrictionType { get; private set; }
            public uint LvlUpCost { get; private set; }
            public SecretAbility SecretAbility { get; private set; }
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

            public DragonForceUpgrade Unlocks(SecretAbility Type)
            {
                this.GainType = OrbGainParamType.SecretAbility;
                this.SecretAbility = Type;
                this.Amount = (uint) Type;
                return this;
            }

            public DragonForceUpgrade Location(PageNo PageNo, GroupNo GroupNo, uint IndexNo)
            {
                this.PageNo = (uint) PageNo;
                this.GroupNo = (uint) GroupNo;
                this.IndexNo = IndexNo;
                this.Category = GroupNo2Category(GroupNo);
                return this;
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

        private static readonly Dictionary<uint, DragonForceUpgrade> gDragonForceUpgrades = new Dictionary<uint, DragonForceUpgrade>()
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
                .Unlocks(OrbGainParamType.HpMax, 100),
            [0x04] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(4)
                .Unlocks(OrbGainParamType.MagicalAttack, 5),
            [0x05] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(1)
                .Unlocks(OrbGainParamType.PhysicalAttack, 5),

            [0x15] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x16] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(70)
                .Unlocks(SecretAbility.Efficacy),
            [0x17] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(10)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x18] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.HpMax, 50),
            [0x19] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(70)
                .Unlocks(OrbGainParamType.HpMax, 60),
            [0x1a] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.StaminaMax, 40),
            [0x1b] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(OrbGainParamType.HpMax, 60),
            [0x1c] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.StaminaMax, 30),

            [0x1d] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group2, 1)
                .HasOrbUnlockRestriction(30)
                .Unlocks(SecretAbility.EffectExtension),
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
                .Unlocks(SecretAbility.ExtendedSprings),
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
                .Unlocks(OrbGainParamType.MagicalAttack, 1),
            [0x26] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x27] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.MagicalDefence, 2),
            [0x28] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.StaminaMax, 35),
            [0x29] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.MagicalDefence, 2),
            [0x2a] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.MagicalAttack, 2),
            [0x2b] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(SecretAbility.RainDefense),
            [0x2c] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.MagicalDefence, 3),

            [0x2d] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.PhysicalAttack, 1),
            [0x2e] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x2f] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(15)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            [0x30] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(40)
                .Unlocks(OrbGainParamType.StaminaMax, 35),
            [0x31] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(20)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            [0x32] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.PhysicalAttack, 2),
            [0x33] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(50)
                .Unlocks(SecretAbility.RainAttack),
            [0x34] = new DragonForceUpgrade()
                .Location(PageNo.Page1, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(30)
                .Unlocks(OrbGainParamType.PhysicalDefence, 3),
            #endregion

            #region PAGE2
            [0x06] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.SupportPawnSlot, 1), // Support Pawn Hires?
            [0x07] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(12)
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x08] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(24)
                .Unlocks(OrbGainParamType.HpMax, 100),
            [0x09] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(20)
                .Unlocks(OrbGainParamType.MagicalDefence, 5),
            [0x0a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(16)
                .Unlocks(OrbGainParamType.PhysicalDefence, 5),

            [0x35] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(170)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x36] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(220)
                .Unlocks(SecretAbility.SoftTouch),
            [0x37] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(100)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x38] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x39] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(250)
                .Unlocks(OrbGainParamType.HpMax, 50),
            [0x3a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.StaminaMax, 30),
            [0x3b] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(200)
                .Unlocks(OrbGainParamType.HpMax, 40),
            [0x3c] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.StaminaMax, 20),

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
                .Unlocks(SecretAbility.Gathering),
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
                .Unlocks(SecretAbility.EnhancedThrow),

            [0x45] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.MagicalAttack, 2),
            [0x46] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(90)
                .Unlocks(OrbGainParamType.StaminaMax, 15),
            [0x47] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.MagicalAttack, 3),
            [0x48] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(130)
                .Unlocks(OrbGainParamType.StaminaMax, 25),
            [0x49] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.MagicalDefence, 1),
            [0x4a] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.MagicalAttack, 2),
            [0x4b] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(200)
                .Unlocks(SecretAbility.NewDefense),
            [0x4c] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.MagicalDefence, 2),

            [0x4d] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.PhysicalAttack, 2),
            [0x4e] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(90)
                .Unlocks(OrbGainParamType.StaminaMax, 15),
            [0x4f] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.PhysicalAttack, 3),
            [0x50] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(130)
                .Unlocks(OrbGainParamType.StaminaMax, 25),
            [0x51] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(120)
                .Unlocks(OrbGainParamType.PhysicalDefence, 1),
            [0x52] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(150)
                .Unlocks(OrbGainParamType.PhysicalAttack, 2),
            [0x53] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(200)
                .Unlocks(SecretAbility.NewAttack),
            [0x54] = new DragonForceUpgrade()
                .Location(PageNo.Page2, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(160)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            #endregion

            #region PAGE3
            [0x0b] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.PawnAdventureNum, 1),
            [0x0c] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(44)
                .Unlocks(OrbGainParamType.AccessorySlot, 1),
            [0x0d] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(40)
                .Unlocks(OrbGainParamType.HpMax, 50),
            [0x0e] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(36)
                .Unlocks(OrbGainParamType.MagicalAttack, 5),
            [0x0f] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(32)
                .Unlocks(OrbGainParamType.PhysicalAttack, 5),

            [0x55] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x56] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(1000)
                .Unlocks(SecretAbility.Flow),
            [0x57] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(520)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x58] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x59] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.HpMax, 40),
            [0x5a] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(700)
                .Unlocks(OrbGainParamType.StaminaMax, 40),
            [0x5b] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(800)
                .Unlocks(OrbGainParamType.HpMax, 50),
            [0x5c] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.StaminaMax, 20),

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
                .Unlocks(OrbGainParamType.AbilityCost, 3),
            [0x60] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 4)
                .HasOrbUnlockRestriction(750)
                .Unlocks(SecretAbility.ExpertExcavator),
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
                .Unlocks(SecretAbility.Featherfoot),
            [0x64] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group2, 8)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.AbilityCost, 4),

            [0x65] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 1)
                .HasOrbUnlockRestriction(300)
                .Unlocks(OrbGainParamType.MagicalAttack, 1),
            [0x66] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(480)
                .Unlocks(OrbGainParamType.StaminaMax, 15),
            [0x67] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.MagicalDefence, 2),
            [0x68] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x69] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.MagicalDefence, 3),
            [0x6a] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.MagicalAttack, 2),
            [0x6b] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(750)
                .Unlocks(SecretAbility.MoonlightDefense),
            [0x6c] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.MagicalDefence, 2),

            [0x6d] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(300)
                .Unlocks(OrbGainParamType.PhysicalAttack, 1),
            [0x6e] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(480)
                .Unlocks(OrbGainParamType.StaminaMax, 15),
            [0x6f] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            [0x70] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(580)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x71] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(600)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            [0x72] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(500)
                .Unlocks(OrbGainParamType.PhysicalAttack, 2),
            [0x73] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(750)
                .Unlocks(SecretAbility.MoonlightAssault),
            [0x74] = new DragonForceUpgrade()
                .Location(PageNo.Page3, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(550)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            #endregion

            #region PAGE4
            [0x10] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 1)
                .HasPageUnlockRestriction()
                .Unlocks(OrbGainParamType.MainPawnSlot, 1),
            [0x11] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 2)
                .HasTotalLevelsRestriction(60)
                .Unlocks(OrbGainParamType.EquipItemSlot, 5),
            [0x12] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 3)
                .HasTotalLevelsRestriction(56)
                .Unlocks(OrbGainParamType.HpMax, 50),
            [0x13] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 4)
                .HasTotalLevelsRestriction(52)
                .Unlocks(OrbGainParamType.MagicalDefence, 5),
            [0x14] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group5, 5)
                .HasTotalLevelsRestriction(48)
                .Unlocks(OrbGainParamType.PhysicalDefence, 5),

            [0x75] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 1)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x76] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 2)
                .HasOrbUnlockRestriction(5000)
                .Unlocks(SecretAbility.Willpower),
            [0x77] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 3)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.StaminaMax, 30),
            [0x78] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 4)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.HpMax, 30),
            [0x79] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 5)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.HpMax, 40),
            [0x7a] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 6)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.StaminaMax, 30),
            [0x7b] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 7)
                .HasOrbUnlockRestriction(2500)
                .Unlocks(OrbGainParamType.HpMax, 50),
            [0x7c] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group1, 8)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.StaminaMax, 20),

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
                .Unlocks(SecretAbility.TreasureEye),
            [0x81] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 5)
                .HasOrbUnlockRestriction(2200)
                .Unlocks(OrbGainParamType.AbilityCost, 4),
            [0x82] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group2, 6)
                .HasOrbUnlockRestriction(3500)
                .Unlocks(SecretAbility.SafeLanding),
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
                .Unlocks(OrbGainParamType.MagicalAttack, 2),
            [0x86] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 2)
                .HasOrbUnlockRestriction(2300)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x87] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 3)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.MagicalAttack, 4),
            [0x88] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 4)
                .HasOrbUnlockRestriction(3000)
                .Unlocks(SecretAbility.Rakshasa),
            [0x89] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 5)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.MagicalDefence, 2),
            [0x8a] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 6)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.MagicalAttack, 3),
            [0x8b] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 7)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.StaminaMax, 15),
            [0x8c] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group3, 8)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.MagicalDefence, 3),

            [0x8d] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 1)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.PhysicalAttack, 2),
            [0x8e] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 2)
                .HasOrbUnlockRestriction(2300)
                .Unlocks(OrbGainParamType.StaminaMax, 20),
            [0x8f] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 3)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.PhysicalAttack, 4),
            [0x90] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 4)
                .HasOrbUnlockRestriction(3000)
                .Unlocks(SecretAbility.Yasha),
            [0x91] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 5)
                .HasOrbUnlockRestriction(1500)
                .Unlocks(OrbGainParamType.PhysicalDefence, 2),
            [0x92] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 6)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.PhysicalAttack, 3),
            [0x93] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 7)
                .HasOrbUnlockRestriction(2000)
                .Unlocks(OrbGainParamType.StaminaMax, 15),
            [0x94] = new DragonForceUpgrade()
                .Location(PageNo.Page4, GroupNo.Group4, 8)
                .HasOrbUnlockRestriction(1800)
                .Unlocks(OrbGainParamType.PhysicalDefence, 3)
            #endregion
        };
    }
}
