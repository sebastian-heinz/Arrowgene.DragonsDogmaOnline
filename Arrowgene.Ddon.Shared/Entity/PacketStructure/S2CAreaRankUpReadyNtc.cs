using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaRankUpReadyNtc : IPacketStructure
    { 
        public PacketId Id => PacketId.S2C_AREA_RANK_UP_READY_NTC;

        public List<CDataAreaRank> AreaRankList { get; set; }
        public S2CAreaRankUpReadyNtc()
        {
            AreaRankList = new();
        }

        public class Serializer : PacketEntitySerializer<S2CAreaRankUpReadyNtc>
        {
            public override void Write(IBuffer buffer, S2CAreaRankUpReadyNtc obj)
            {
                WriteEntityList(buffer, obj.AreaRankList);
            }

            public override S2CAreaRankUpReadyNtc Read(IBuffer buffer)
            {
                S2CAreaRankUpReadyNtc obj = new S2CAreaRankUpReadyNtc();
                obj.AreaRankList = ReadEntityList<CDataAreaRank>(buffer);
                return obj;
            }
        }
    }
}
