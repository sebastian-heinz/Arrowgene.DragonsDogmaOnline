using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGachaInfo
    {
        public uint Id { get; set; }
        public long Begin { get; set; }
        public long End { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public byte WeightDispType { get; set; }
        public string WeightDispTitle { get; set; }
        public string WeightDispText { get; set; }
        public string ListAddr { get; set; }
        public string ImageAddr { get; set; }
        public List<CDataGachaDrawGroupInfo> DrawGroups { get; set; }

        public CDataGachaInfo()
        {
            DrawGroups = new List<CDataGachaDrawGroupInfo>();
        }

        public class Serializer : EntitySerializer<CDataGachaInfo>
        {
            public override void Write(IBuffer buffer, CDataGachaInfo obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteInt64(buffer, obj.Begin);
                WriteInt64(buffer, obj.End);
                WriteMtString(buffer, obj.Name);
                WriteMtString(buffer, obj.Description);
                WriteMtString(buffer, obj.Detail);
                WriteByte(buffer, obj.WeightDispType);
                WriteMtString(buffer, obj.WeightDispTitle);
                WriteMtString(buffer, obj.WeightDispText);
                WriteMtString(buffer, obj.ListAddr);
                WriteMtString(buffer, obj.ImageAddr);
                WriteEntityList<CDataGachaDrawGroupInfo>(buffer, obj.DrawGroups);
            }

            public override CDataGachaInfo Read(IBuffer buffer)
            {
                CDataGachaInfo obj = new CDataGachaInfo
                {
                    Id = ReadUInt32(buffer),
                    Begin = ReadInt64(buffer),
                    End = ReadInt64(buffer),
                    Name = ReadMtString(buffer),
                    Description = ReadMtString(buffer),
                    Detail = ReadMtString(buffer),
                    WeightDispType = ReadByte(buffer),
                    WeightDispTitle = ReadMtString(buffer),
                    WeightDispText = ReadMtString(buffer),
                    ListAddr = ReadMtString(buffer),
                    ImageAddr = ReadMtString(buffer),
                    DrawGroups = ReadEntityList<CDataGachaDrawGroupInfo>(buffer)
                };

                return obj;
            }
        }
    }
}
