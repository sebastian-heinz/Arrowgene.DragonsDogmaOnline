using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTimeGainQuestList
    {
        public CDataTimeGainQuestList()
        {
            Param = new CDataQuestList();
            RewardItemDetailList = new List<CDataRewardItemDetail>();
            Restrictions = new CDataTimeGainQuestRestrictions();
            RequiredItemsList = new List<CDataTimeGainQuestUnk2>();
        }

        public CDataQuestList Param { get; set; }
        public uint PlayTimeInSec { get; set; }
        public bool IsNoTimeup {  get; set; }
        public bool Unk0 { get; set; }
        public bool IsJoinCharacter { get; set; }
        public bool IsJoinPawn {  get; set; }
        public bool Unk1 { get; set; }
        public byte JoinPawnNum { get; set; }
        public List<CDataRewardItemDetail> RewardItemDetailList {  get; set; }
        public CDataTimeGainQuestRestrictions Restrictions {  get; set; }
        public List<CDataTimeGainQuestUnk2> RequiredItemsList {  get; set; }

        public class Serializer : EntitySerializer<CDataTimeGainQuestList>
        {
            public override void Write(IBuffer buffer, CDataTimeGainQuestList obj)
            {
                WriteEntity(buffer, obj.Param);
                WriteUInt32(buffer, obj.PlayTimeInSec);
                WriteBool(buffer, obj.IsNoTimeup);
                WriteBool(buffer, obj.Unk0);
                WriteBool(buffer, obj.IsJoinCharacter);
                WriteBool(buffer, obj.IsJoinPawn);
                WriteBool(buffer, obj.Unk1);
                WriteByte(buffer, obj.JoinPawnNum);
                WriteEntityList(buffer, obj.RewardItemDetailList);
                WriteEntity(buffer, obj.Restrictions);
                WriteEntityList(buffer, obj.RequiredItemsList);
            }

            public override CDataTimeGainQuestList Read(IBuffer buffer)
            {
                CDataTimeGainQuestList obj = new CDataTimeGainQuestList();
                obj.Param = ReadEntity<CDataQuestList>(buffer);
                obj.PlayTimeInSec = ReadUInt32(buffer);
                obj.IsNoTimeup = ReadBool(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.IsJoinCharacter = ReadBool(buffer);
                obj.IsJoinPawn = ReadBool(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.JoinPawnNum = ReadByte(buffer);
                obj.RewardItemDetailList = ReadEntityList<CDataRewardItemDetail>(buffer);
                obj.Restrictions = ReadEntity<CDataTimeGainQuestRestrictions>(buffer);
                obj.RequiredItemsList = ReadEntityList<CDataTimeGainQuestUnk2>(buffer);
                return obj;
            }
        }
    }
}
