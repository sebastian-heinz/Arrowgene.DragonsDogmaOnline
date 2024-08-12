using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Deferred;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

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
            DeferredOperations = new List<DeferredOperation>();
        }

        public PacketId Id { get; }
        protected DdonServer<TClient> Server { get; }
        protected IDatabase Database { get; }
        protected List<DeferredOperation> DeferredOperations { get; }

        public abstract void Handle(TClient client, StructurePacket<TReqStruct> packet);

        public void Handle(TClient client, IPacket packet)
        {
            DeferredOperations.Clear();
            Handle(client, new StructurePacket<TReqStruct>(packet));
            Server.Database.ExecuteDeferred(DeferredOperations);
        }
    }
}
