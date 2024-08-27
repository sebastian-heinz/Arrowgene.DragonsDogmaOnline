using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnListHandler
        : GameRequestPacketHandler<C2SPawnGetRegisteredPawnListReq, S2CPawnGetRegisteredPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(
            typeof(PawnGetRegisteredPawnListHandler)
        );

        public PawnGetRegisteredPawnListHandler(DdonGameServer server)
            : base(server) { }

        public override S2CPawnGetRegisteredPawnListRes Handle(
            GameClient client,
            C2SPawnGetRegisteredPawnListReq request
        )
        {
            var result = new S2CPawnGetRegisteredPawnListRes();
            result.RegisterdPawnList = Server.Database.SelectRegisteredPawns(
                client.Character,
                request.SearchParam
            );
            return result;
        }
    }
}
