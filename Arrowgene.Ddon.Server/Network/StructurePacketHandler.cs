using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class StructurePacketHandler<TClient, TReqStruct> : IPacketHandler<TClient>
        where TClient : Client
        where TReqStruct : IPacketStructure
    {
        protected StructurePacketHandler(DdonServer<TClient> server)
        {
            Server = server;
            Database = server.Database;
        }

        public abstract PacketId Id { get; }
        protected DdonServer<TClient> Server { get; }
        protected IDatabase Database { get; }

        public abstract void Handle(TClient client, StructurePacket<TReqStruct> packet);

        public void Handle(TClient client, Packet packet)
        {
            Handle(client, new StructurePacket<TReqStruct>(packet));
        }
    }
}
