using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanScoutEntryGetMyHandler : GameRequestPacketHandler<C2SClanClanScoutEntryGetMyReq, S2CClanClanScoutEntryGetMyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanScoutEntryGetMyHandler));


        public ClanClanScoutEntryGetMyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanScoutEntryGetMyRes Handle(GameClient client, C2SClanClanScoutEntryGetMyReq request)
        {
            return new S2CClanClanScoutEntryGetMyRes()
            {
                //TODO: Actually return the list.
            };
        }
    }
}
