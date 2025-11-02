using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGpEditGetVoiceListHandler : GameRequestPacketHandler<C2SGpGpEditGetVoiceListReq, S2CGpGpEditGetVoiceListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGpEditGetVoiceListHandler));

        public GpGpEditGetVoiceListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGpEditGetVoiceListRes Handle(GameClient client, C2SGpGpEditGetVoiceListReq request)
        {
            S2CGpGpEditGetVoiceListRes response = EntitySerializer.Get<S2CGpGpEditGetVoiceListRes>().Read(GameFull.data_Dump_703);
            foreach (var voice in response.VoiceList)
            {
                voice.IsValid = true; // Unlock
            }
            return response;
        }
    }
}
