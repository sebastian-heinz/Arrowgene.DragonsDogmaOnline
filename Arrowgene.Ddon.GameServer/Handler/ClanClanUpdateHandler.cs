using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanUpdateHandler : GameRequestPacketHandler<C2SClanClanUpdateReq, S2CClanClanUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSettingUpdateHandler));

        public ClanClanUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanUpdateRes Handle(GameClient client, C2SClanClanUpdateReq request)
        {
            Logger.Info($"Motto: {request.CreateParam.Motto}" +
                $"\nActiveDays: {request.CreateParam.ActiveDays}" +
                $"\nActiveTime: {request.CreateParam.ActiveTime}" +
                $"\nCharacteristic: {request.CreateParam.Characteristic}");

            return new S2CClanClanUpdateRes();
        }
    }
}
