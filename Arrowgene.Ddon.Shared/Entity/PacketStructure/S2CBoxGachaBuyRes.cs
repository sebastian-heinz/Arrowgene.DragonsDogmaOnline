using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBoxGachaBuyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BOX_GACHA_BOX_GACHA_BUY_RES;

        public uint BoxGachaId { get; set; }
        public List<CDataBoxGachaItemInfo> BoxGachaItemList { get; set; }

        public S2CBoxGachaBuyRes()
        {
            BoxGachaItemList = new List<CDataBoxGachaItemInfo>();
        }

        public class Serializer : PacketEntitySerializer<S2CBoxGachaBuyRes>
        {
            public override void Write(IBuffer buffer, S2CBoxGachaBuyRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteUInt32(buffer, obj.BoxGachaId);
                WriteEntityList<CDataBoxGachaItemInfo>(buffer, obj.BoxGachaItemList);
            }

            public override S2CBoxGachaBuyRes Read(IBuffer buffer)
            {
                S2CBoxGachaBuyRes obj = new S2CBoxGachaBuyRes();

                ReadServerResponse(buffer, obj);

                obj.BoxGachaId = ReadUInt32(buffer);
                obj.BoxGachaItemList = ReadEntityList<CDataBoxGachaItemInfo>(buffer);

                return obj;
            }
        }
    }
}
