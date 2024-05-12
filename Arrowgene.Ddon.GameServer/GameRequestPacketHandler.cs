using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer
{
    public abstract class GameRequestPacketHandler<TReqStruct, TResStruct> : RequestPacketHandler<GameClient, TReqStruct, TResStruct>
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {
        protected GameRequestPacketHandler(DdonGameServer server) : base(server)
        {
            Server = server;
        }

        protected new DdonGameServer Server { get; }
    }
}
