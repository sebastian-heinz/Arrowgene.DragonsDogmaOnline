using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LoadingInfoLoadingGetInfoHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(LoadingInfoLoadingGetInfoHandler));


        public LoadingInfoLoadingGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_LOADING_INFO_LOADING_GET_INFO_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameDump.Dump_21);
        }
    }
}
