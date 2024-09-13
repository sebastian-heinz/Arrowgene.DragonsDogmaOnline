using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGachaBuyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GACHA_GACHA_BUY_RES;

        public uint GachaId { get; set; }
        public List<CDataGachaItemInfo> GachaItemList { get; set; }

        public S2CGachaBuyRes()
        {
            GachaItemList = new List<CDataGachaItemInfo>();
        }

        public class Serializer : PacketEntitySerializer<S2CGachaBuyRes>
        {
            public override void Write(IBuffer buffer, S2CGachaBuyRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteUInt32(buffer, obj.GachaId);
                WriteEntityList<CDataGachaItemInfo>(buffer, obj.GachaItemList);
            }

            public override S2CGachaBuyRes Read(IBuffer buffer)
            {
                S2CGachaBuyRes obj = new S2CGachaBuyRes();

                ReadServerResponse(buffer, obj);

                obj.GachaId = ReadUInt32(buffer);
                obj.GachaItemList = ReadEntityList<CDataGachaItemInfo>(buffer);

                return obj;
            }
        }
    }
}
