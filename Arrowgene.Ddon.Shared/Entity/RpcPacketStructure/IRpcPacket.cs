using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    public interface IRpcPacket
    {
        public void Handle(Character character, RpcPacketHeader packetHeader, IBuffer buffer);
    }
}
