using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionPingReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_PING_REQ;

        public List<CDataCheatInfo> Info { get; set; } = [];

        public class Serializer : PacketEntitySerializer<C2SConnectionPingReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionPingReq obj)
            {
                WriteEntityList(buffer, obj.Info);
            }

            public override C2SConnectionPingReq Read(IBuffer buffer)
            {
                C2SConnectionPingReq obj = new C2SConnectionPingReq();
                obj.Info = ReadEntityList<CDataCheatInfo>(buffer);
                return obj;
            }
        }

    }
}
