using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class StructurePacketHandler<TClient, TReqStruct> : IPacketHandler<TClient>
        where TClient : Client
        where TReqStruct : class, IPacketStructure, new()
    {
        protected StructurePacketHandler(DdonServer<TClient> server)
        {
            Server = server;
            Database = server.Database;
            // Create a instance to obtain PacketId information.
            Id = new TReqStruct().Id;
        }

        public PacketId Id { get; }
        protected DdonServer<TClient> Server { get; }
        protected IDatabase Database { get; }

        public abstract void Handle(TClient client, StructurePacket<TReqStruct> packet);

        public void Handle(TClient client, IPacket packet)
        {
            Handle(client, new StructurePacket<TReqStruct>(packet));
        }
    }
}
