using System;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Server.Network
{
    public interface IPacketHandler<TClient> : IDisposable where TClient : Client
    {
        void Handle(TClient client, IPacket packet);
        PacketId Id { get; }
    }
}
