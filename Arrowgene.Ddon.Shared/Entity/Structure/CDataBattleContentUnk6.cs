using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk6
    {
        public CDataBattleContentUnk6()
        {
            BattleContentSituationData = new CDataBattleContentSituationData();
            BattleContentAvailableRewardsList = new List<CDataBattleContentAvailableRewards>();
        }

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public CDataBattleContentSituationData BattleContentSituationData {  get; set; }
        public List<CDataBattleContentAvailableRewards> BattleContentAvailableRewardsList {  get; set; }
        public bool Unk4 {  get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk6>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk6 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteEntity(buffer, obj.BattleContentSituationData);
                WriteEntityList(buffer, obj.BattleContentAvailableRewardsList);
                WriteBool(buffer, obj.Unk4);
            }

            public override CDataBattleContentUnk6 Read(IBuffer buffer)
            {
                CDataBattleContentUnk6 obj = new CDataBattleContentUnk6();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.BattleContentSituationData = ReadEntity<CDataBattleContentSituationData>(buffer);
                obj.BattleContentAvailableRewardsList = ReadEntityList<CDataBattleContentAvailableRewards>(buffer);
                obj.Unk4 = ReadBool(buffer);
                return obj;
            }
        }
    }
}




