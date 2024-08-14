using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGachaDrawInfo
    {
        public uint Num { get; set; }
        public bool IsBonus { get; set; }
        public List<CDataGachaItemInfo> GachaItemInfo { get; set; }

        public CDataGachaDrawInfo()
        {
        }

        public class Serializer : EntitySerializer<CDataGachaDrawInfo>
        {
            public override void Write(IBuffer buffer, CDataGachaDrawInfo obj)
            {
                WriteUInt32(buffer, obj.Num);
                WriteBool(buffer, obj.IsBonus);
                WriteEntityList<CDataGachaItemInfo>(buffer, obj.GachaItemInfo);
            }

            public override CDataGachaDrawInfo Read(IBuffer buffer)
            {
                CDataGachaDrawInfo obj = new CDataGachaDrawInfo
                {
                    Num = ReadUInt32(buffer),
                    IsBonus = ReadBool(buffer),
                    GachaItemInfo = ReadEntityList<CDataGachaItemInfo>(buffer),
                };

                return obj;
            }
        }
    }
}
