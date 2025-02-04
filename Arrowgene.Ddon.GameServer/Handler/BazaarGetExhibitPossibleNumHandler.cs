using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BazaarGetExhibitPossibleNumHandler : GameRequestPacketHandler<C2SBazaarGetExhibitPossibleNumReq, S2CBazaarGetExhibitPossibleNumRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BazaarGetExhibitPossibleNumHandler));
        
        public BazaarGetExhibitPossibleNumHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBazaarGetExhibitPossibleNumRes Handle(GameClient client, C2SBazaarGetExhibitPossibleNumReq request)
        {
            return new S2CBazaarGetExhibitPossibleNumRes()
            {
                Num = client.Character.MaxBazaarExhibits + Server.GpCourseManager.BazaarExhibitExtend(),
                Add = Server.GpCourseManager.BazaarExhibitExtend() // Not sure what the purpose of this value is
            };
        }
    }
}