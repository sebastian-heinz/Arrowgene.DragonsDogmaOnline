using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionReserveServerRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_RESERVE_SERVER_RES;

        public S2CConnectionReserveServerRes()
        {
            ReserveInfoList = new List<CDataCommonU32>();
        }

        public ushort GameServerUniqueID { get; set; }
        public List<CDataCommonU32> ReserveInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionReserveServerRes>
        {
            public override void Write(IBuffer buffer, S2CConnectionReserveServerRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt16(buffer, obj.GameServerUniqueID);
                WriteEntityList<CDataCommonU32>(buffer, obj.ReserveInfoList);
            }

            public override S2CConnectionReserveServerRes Read(IBuffer buffer)
            {
                S2CConnectionReserveServerRes obj = new S2CConnectionReserveServerRes();
                ReadServerResponse(buffer, obj);
                obj.GameServerUniqueID = ReadUInt16(buffer);
                obj.ReserveInfoList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}