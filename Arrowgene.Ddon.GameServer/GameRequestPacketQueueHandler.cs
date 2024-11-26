using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer
{
    public abstract class GameRequestPacketQueueHandler<TReqStruct, TResStruct> : RequestPacketQueueHandler<GameClient, TReqStruct, TResStruct>
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {
        protected GameRequestPacketQueueHandler(DdonGameServer server) : base(server)
        {
            Server = server;
        }

        protected new DdonGameServer Server { get; }
    }
}
