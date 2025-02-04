using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LoadingInfoLoadingGetInfoHandler : GameRequestPacketHandler<C2SConnectionGetLoginAnnouncementReq, S2CLoadingInfoLoadingGetInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LoadingInfoLoadingGetInfoHandler));

        public LoadingInfoLoadingGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_LOADING_INFO_LOADING_GET_INFO_REQ;

        public override S2CLoadingInfoLoadingGetInfoRes Handle(GameClient client, C2SConnectionGetLoginAnnouncementReq request)
        {
            return new()
            {
                Info = Server.AssetRepository.LoadingInfoAsset
            };
        }
    }
}
