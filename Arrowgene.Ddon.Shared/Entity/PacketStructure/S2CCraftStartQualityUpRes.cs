using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftStartQualityUpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_START_QUALITY_UP_RES;

        public S2CCraftStartQualityUpRes()
        {
            Unk0 = new CDataS2CCraftStartQualityUpResUnk0();
            AddStatusDataList = new List<CDataAddStatusParam>();
            CurrentEquip = new CDataCurrentEquipInfo();            
        }

        public CDataS2CCraftStartQualityUpResUnk0 Unk0 { get; set; } // I'm thinking this is AddStatus
        public List<CDataAddStatusParam> AddStatusDataList { get; set; } // I think this is Craigs Limit break?
        public CDataCurrentEquipInfo CurrentEquip { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftStartQualityUpRes>
        {
            public override void Write(IBuffer buffer, S2CCraftStartQualityUpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataS2CCraftStartQualityUpResUnk0>(buffer, obj.Unk0);
                WriteEntityList<CDataAddStatusParam>(buffer, obj.AddStatusDataList);
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquip);
            }

            public override S2CCraftStartQualityUpRes Read(IBuffer buffer)
            {
                S2CCraftStartQualityUpRes obj = new S2CCraftStartQualityUpRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadEntity<CDataS2CCraftStartQualityUpResUnk0>(buffer);
                obj.AddStatusDataList = ReadEntityList<CDataAddStatusParam>(buffer);
                obj.CurrentEquip = ReadEntity<CDataCurrentEquipInfo>(buffer);
                return obj;
            }
        }
    }
}