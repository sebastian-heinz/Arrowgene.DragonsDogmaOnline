using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaRankUpReadNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_AREA_RANK_UP_READY_NTC;

        public List<CDataAreaRank> AreaRankList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaRankUpReadNtc>
        {
            public override void Write(IBuffer buffer, S2CAreaRankUpReadNtc obj)
            {
                WriteEntityList(buffer, obj.AreaRankList);
            }

            public override S2CAreaRankUpReadNtc Read(IBuffer buffer)
            {
                S2CAreaRankUpReadNtc obj = new S2CAreaRankUpReadNtc();
                obj.AreaRankList = ReadEntityList<CDataAreaRank>(buffer);
                return obj;
            }
        }
    }
}
