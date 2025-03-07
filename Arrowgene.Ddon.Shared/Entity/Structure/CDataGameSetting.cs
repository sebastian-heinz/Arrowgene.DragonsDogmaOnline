using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGameSetting
    {
        public uint MainPawnMax { get; set; }
        public uint SupportPawnMax { get; set; }
        public uint JobLevelMax { get; set; }
        public uint CraftLevelMax { get; set; }
        public uint CraftSkillLevelMax { get; set; }
        public uint UserListMax { get; set; }
        public uint ClanLvMax { get; set; }
        public uint ClanMemberMax { get; set; }
        public uint ClanLeaveIntervalTime { get; set; } // In seconds
        public byte CharacterNumMax { get; set; }
        public List<CDataCharacterItemSlotInfo> GlobalItemSlotNumMaxList { get; set; } = new();
        public uint PawnCreateItemID { get; set; }
        public byte PawnCreateItemNum { get; set; }
        public bool EnableVisualEquip { get; set; }
        public byte EquipColorChangeGrade { get; set; }
        public uint FriendListMax { get; set; }
        public uint RecentPlayerMax { get; set; }
        public uint BlackListMax { get; set; }
        public uint HistoryListMax { get; set; }
        public uint CharacterReviveGP { get; set; }
        public uint PawnReviveGP { get; set; }
        public uint LostPawnReviveGP { get; set; }
        public List<CDataURLInfo> UrlInfoList { get; set; } = new();
        public List<CDataPartyMemberMaxNum> PartyMemberMaxNumList { get; set; } = new();
        public uint GroupChatMemberMax { get; set; }
        public uint EventCodeInputLockFailNum { get; set; }
        public uint EventCodeLockTime { get; set; }
        public uint PawnPresentItemID { get; set; }
        public byte PawnPresentItemNum { get; set; }
        public List<CDataJewelryEquipLimit> JewelryEquipLimitList { get; set; } = new();
        public uint JobPointMax { get; set; }
        public uint PlayPointMax { get; set; }
        public uint PlayPointLevelMin { get; set; }
        public uint BazaarSearchTime { get; set; }
        public uint BazaarSearchCautionTime { get; set; }
        public uint BazaarSearchCautionCount { get; set; }
        public List<CDataCommonU32> ClanBaseStageList { get; set; } = new();
        public uint MailCacheExpireTime { get; set; }
        public bool Unk0 { get; set; } // Default false
        public uint Unk1 { get; set; } // Default 0
        public uint Unk2 { get; set; } // Default 10000
        public uint Unk3 { get; set; } // Default 80
        public List<CDataWalletLimit> WalletLimits { get; set; } = new();
        public ushort Unk5 { get; set; } // Default 150
        public List<CDataExpSetting> ExpRequiredPerLevel { get; set; } = new();

        public class Serializer : EntitySerializer<CDataGameSetting>
        {
            public override void Write(IBuffer buffer, CDataGameSetting obj)
            {
                WriteUInt32(buffer, obj.MainPawnMax);
                WriteUInt32(buffer, obj.SupportPawnMax);
                WriteUInt32(buffer, obj.JobLevelMax);
                WriteUInt32(buffer, obj.CraftLevelMax);
                WriteUInt32(buffer, obj.CraftSkillLevelMax);
                WriteUInt32(buffer, obj.UserListMax);
                WriteUInt32(buffer, obj.ClanLvMax);
                WriteUInt32(buffer, obj.ClanMemberMax);
                WriteUInt32(buffer, obj.ClanLeaveIntervalTime);
                WriteByte(buffer, obj.CharacterNumMax);
                WriteEntityList<CDataCharacterItemSlotInfo>(buffer, obj.GlobalItemSlotNumMaxList);
                WriteUInt32(buffer, obj.PawnCreateItemID);
                WriteByte(buffer, obj.PawnCreateItemNum);
                WriteBool(buffer, obj.EnableVisualEquip);
                WriteByte(buffer, obj.EquipColorChangeGrade);
                WriteUInt32(buffer, obj.FriendListMax);
                WriteUInt32(buffer, obj.RecentPlayerMax);
                WriteUInt32(buffer, obj.BlackListMax);
                WriteUInt32(buffer, obj.HistoryListMax);
                WriteUInt32(buffer, obj.CharacterReviveGP);
                WriteUInt32(buffer, obj.PawnReviveGP);
                WriteUInt32(buffer, obj.LostPawnReviveGP);
                WriteEntityList<CDataURLInfo>(buffer, obj.UrlInfoList);
                WriteEntityList<CDataPartyMemberMaxNum>(buffer, obj.PartyMemberMaxNumList);
                WriteUInt32(buffer, obj.GroupChatMemberMax);
                WriteUInt32(buffer, obj.EventCodeInputLockFailNum);
                WriteUInt32(buffer, obj.EventCodeLockTime);
                WriteUInt32(buffer, obj.PawnPresentItemID);
                WriteByte(buffer, obj.PawnPresentItemNum);
                WriteEntityList<CDataJewelryEquipLimit>(buffer, obj.JewelryEquipLimitList);
                WriteUInt32(buffer, obj.JobPointMax);
                WriteUInt32(buffer, obj.PlayPointMax);
                WriteUInt32(buffer, obj.PlayPointLevelMin);
                WriteUInt32(buffer, obj.BazaarSearchTime);
                WriteUInt32(buffer, obj.BazaarSearchCautionTime);
                WriteUInt32(buffer, obj.BazaarSearchCautionCount);
                WriteEntityList<CDataCommonU32>(buffer, obj.ClanBaseStageList);
                WriteUInt32(buffer, obj.MailCacheExpireTime);
                WriteBool(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteEntityList<CDataWalletLimit>(buffer, obj.WalletLimits);
                WriteUInt16(buffer, obj.Unk5);
                WriteEntityList<CDataExpSetting>(buffer, obj.ExpRequiredPerLevel);
            }

            public override CDataGameSetting Read(IBuffer buffer)
            {
                CDataGameSetting obj = new CDataGameSetting();
                obj.MainPawnMax = ReadUInt32(buffer);
                obj.SupportPawnMax = ReadUInt32(buffer);
                obj.JobLevelMax = ReadUInt32(buffer);
                obj.CraftLevelMax = ReadUInt32(buffer);
                obj.CraftSkillLevelMax = ReadUInt32(buffer);
                obj.UserListMax = ReadUInt32(buffer);
                obj.ClanLvMax = ReadUInt32(buffer);
                obj.ClanMemberMax = ReadUInt32(buffer);
                obj.ClanLeaveIntervalTime = ReadUInt32(buffer);
                obj.CharacterNumMax = ReadByte(buffer);
                obj.GlobalItemSlotNumMaxList = ReadEntityList<CDataCharacterItemSlotInfo>(buffer);
                obj.PawnCreateItemID = ReadUInt32(buffer);
                obj.PawnCreateItemNum = ReadByte(buffer);
                obj.EnableVisualEquip = ReadBool(buffer);
                obj.EquipColorChangeGrade = ReadByte(buffer);
                obj.FriendListMax = ReadUInt32(buffer);
                obj.RecentPlayerMax = ReadUInt32(buffer);
                obj.BlackListMax = ReadUInt32(buffer);
                obj.HistoryListMax = ReadUInt32(buffer);
                obj.CharacterReviveGP = ReadUInt32(buffer);
                obj.PawnReviveGP = ReadUInt32(buffer);
                obj.LostPawnReviveGP = ReadUInt32(buffer);
                obj.UrlInfoList = ReadEntityList<CDataURLInfo>(buffer);
                obj.PartyMemberMaxNumList = ReadEntityList<CDataPartyMemberMaxNum>(buffer);
                obj.GroupChatMemberMax = ReadUInt32(buffer);
                obj.EventCodeInputLockFailNum = ReadUInt32(buffer);
                obj.EventCodeLockTime = ReadUInt32(buffer);
                obj.PawnPresentItemID = ReadUInt32(buffer);
                obj.PawnPresentItemNum = ReadByte(buffer);
                obj.JewelryEquipLimitList = ReadEntityList<CDataJewelryEquipLimit>(buffer);
                obj.JobPointMax = ReadUInt32(buffer);
                obj.PlayPointMax = ReadUInt32(buffer);
                obj.PlayPointLevelMin = ReadUInt32(buffer);
                obj.BazaarSearchTime = ReadUInt32(buffer);
                obj.BazaarSearchCautionTime = ReadUInt32(buffer);
                obj.BazaarSearchCautionCount = ReadUInt32(buffer);
                obj.ClanBaseStageList = ReadEntityList<CDataCommonU32>(buffer);
                obj.MailCacheExpireTime = ReadUInt32(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.WalletLimits = ReadEntityList<CDataWalletLimit>(buffer);
                obj.Unk5 = ReadUInt16(buffer);
                obj.ExpRequiredPerLevel = ReadEntityList<CDataExpSetting>(buffer);
                return obj;
            }
        }
    }
}
