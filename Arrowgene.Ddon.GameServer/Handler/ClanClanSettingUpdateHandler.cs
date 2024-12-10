using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanSettingUpdateHandler : GameRequestPacketHandler<C2SClanClanSettingUpdateReq, S2CClanClanSettingUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSettingUpdateHandler));


        public ClanClanSettingUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanSettingUpdateRes Handle(GameClient client, C2SClanClanSettingUpdateReq request)
        {
            // TODO: Figure out what fires this and implement.
            return new S2CClanClanSettingUpdateRes()
            {
                IsMemberNotice = false
            };
        }
    }
}
