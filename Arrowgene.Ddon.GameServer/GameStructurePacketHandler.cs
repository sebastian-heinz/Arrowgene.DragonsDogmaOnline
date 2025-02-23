using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.GameServer
{
    public abstract class GameStructurePacketHandler<TReqStruct> : StructurePacketHandler<GameClient, TReqStruct>
        where TReqStruct : class, IPacketStructure, new()
    {
        protected GameStructurePacketHandler(DdonGameServer server) : base(server)
        {
            Server = server;
        }

        protected new DdonGameServer Server { get; }
    }
}
