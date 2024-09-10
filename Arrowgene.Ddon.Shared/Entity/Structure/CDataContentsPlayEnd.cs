using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContentsPlayEnd
    {
        public CDataContentsPlayEnd()
        {
            RewardItemDetailList = new List<CDataRewardItemDetail>();
        }

        public uint Gold {  get; set; }
        public uint Exp {  get; set; }
        public uint Rim { get; set; }
        public uint PlayTimeMillSec { get; set; }
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public List<CDataRewardItemDetail> RewardItemDetailList {  get; set; }

        public class Serializer : EntitySerializer<CDataContentsPlayEnd>
        {
            public override void Write(IBuffer buffer, CDataContentsPlayEnd obj)
            {
                WriteUInt32(buffer, obj.Gold);
                WriteUInt32(buffer, obj.Exp);
                WriteUInt32(buffer, obj.Rim);
                WriteUInt32(buffer, obj.PlayTimeMillSec);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteEntityList<CDataRewardItemDetail>(buffer, obj.RewardItemDetailList);
            }

            public override CDataContentsPlayEnd Read(IBuffer buffer)
            {
                CDataContentsPlayEnd obj = new CDataContentsPlayEnd();
                obj.Gold = ReadUInt32(buffer);
                obj.Exp = ReadUInt32(buffer);
                obj.Rim = ReadUInt32(buffer);
                obj.PlayTimeMillSec = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.RewardItemDetailList = ReadEntityList<CDataRewardItemDetail>(buffer);
                return obj;
            }
        }
    }
}
