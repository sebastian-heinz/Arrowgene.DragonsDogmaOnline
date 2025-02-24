using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class StructurePacketHandler<TClient, TReqStruct> : PacketHandler<TClient>
        where TClient : Client
        where TReqStruct : class, IPacketStructure, new()
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StructurePacketHandler<TClient, TReqStruct>));

        protected StructurePacketHandler(DdonServer<TClient> server) : base(server)
        {
            // Create a instance to obtain PacketId information.
            Id = new TReqStruct().Id;

#if DEBUG
            if (!EntitySerializer.Contains(typeof(TReqStruct)))
            {
                Logger.Error($"StructurePacketHandler missing serializer for {typeof(TReqStruct).Name}");
            }
#endif
        }

        public override PacketId Id { get; }
        
        public abstract void Handle(TClient client, StructurePacket<TReqStruct> packet);

        public override void Handle(TClient client, IPacket packet)
        {
            Handle(client, new StructurePacket<TReqStruct>(packet));
        }
    }
}
