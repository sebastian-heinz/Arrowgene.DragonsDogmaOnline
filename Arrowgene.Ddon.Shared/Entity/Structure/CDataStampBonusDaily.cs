using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampBonusDaily
    {
        public CDataStampBonusDaily()
        {
            AssetList = new List<CDataStampBonusAsset>();
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataStampBonusAsset> AssetList { get; set; }

        public class Serializer : EntitySerializer<CDataStampBonusDaily>
        {
            public override void Write(IBuffer buffer, CDataStampBonusDaily obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList<CDataStampBonusAsset>(buffer, obj.AssetList);
            }

            public override CDataStampBonusDaily Read(IBuffer buffer)
            {
                CDataStampBonusDaily obj = new CDataStampBonusDaily();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.AssetList = ReadEntityList<CDataStampBonusAsset>(buffer);

                return obj;
            }
        }
    }
}
