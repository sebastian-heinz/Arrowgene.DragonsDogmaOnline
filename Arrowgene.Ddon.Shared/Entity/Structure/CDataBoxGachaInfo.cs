using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBoxGachaInfo
    {
        public uint Id { get; set; }
        public long Begin { get; set; }
        public long End { get; set; }
        public bool Unk1 { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public byte WeightDispType { get; set; }
        public string FreeSpaceText { get; set; } = string.Empty;
        public string ListAddr { get; set; } = string.Empty;
        public string ImageAddr { get; set; } = string.Empty;
        public List<CDataBoxGachaSettlementInfo> SettlementList { get; set; }
        public List<CDataBoxGachaItemInfo> BoxGachaSets { get; set; }

        public CDataBoxGachaInfo()
        {
            SettlementList = new List<CDataBoxGachaSettlementInfo>();
            BoxGachaSets = new List<CDataBoxGachaItemInfo>();
        }

        public class Serializer : EntitySerializer<CDataBoxGachaInfo>
        {
            public override void Write(IBuffer buffer, CDataBoxGachaInfo obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteInt64(buffer, obj.Begin);
                WriteInt64(buffer, obj.End);
                WriteBool(buffer, obj.Unk1);
                WriteMtString(buffer, obj.Name);
                WriteMtString(buffer, obj.Description);
                WriteMtString(buffer, obj.Detail);
                WriteByte(buffer, obj.WeightDispType);
                WriteMtString(buffer, obj.FreeSpaceText);
                WriteMtString(buffer, obj.ListAddr);
                WriteMtString(buffer, obj.ImageAddr);
                WriteEntityList<CDataBoxGachaSettlementInfo>(buffer, obj.SettlementList);
                WriteEntityList<CDataBoxGachaItemInfo>(buffer, obj.BoxGachaSets);
            }

            public override CDataBoxGachaInfo Read(IBuffer buffer)
            {
                CDataBoxGachaInfo obj = new CDataBoxGachaInfo
                {
                    Id = ReadUInt32(buffer),
                    Begin = ReadInt64(buffer),
                    End = ReadInt64(buffer),
                    Unk1 = ReadBool(buffer),
                    Name = ReadMtString(buffer),
                    Description = ReadMtString(buffer),
                    Detail = ReadMtString(buffer),
                    WeightDispType = ReadByte(buffer),
                    FreeSpaceText = ReadMtString(buffer),
                    ListAddr = ReadMtString(buffer),
                    ImageAddr = ReadMtString(buffer),
                    SettlementList = ReadEntityList<CDataBoxGachaSettlementInfo>(buffer),
                    BoxGachaSets = ReadEntityList<CDataBoxGachaItemInfo>(buffer)
                };

                return obj;
            }
        }
    }
}
