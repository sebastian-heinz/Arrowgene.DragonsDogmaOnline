using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGpEditGetVoiceListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGpEditGetVoiceListHandler));


        public GpGpEditGetVoiceListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_GP_GP_EDIT_GET_VOICE_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CGpGpEditGetVoiceListRes response = EntitySerializer.Get<S2CGpGpEditGetVoiceListRes>().Read(GameFull.data_Dump_703);
            foreach (var voice in response.VoiceList)
            {
                voice.IsValid = true; // Unlock
            }
            client.Send(response);
        }
    }
}