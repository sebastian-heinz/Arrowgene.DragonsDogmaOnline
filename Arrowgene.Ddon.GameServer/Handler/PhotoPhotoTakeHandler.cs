using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PhotoPhotoTakeHandler : GameStructurePacketHandler<C2SPhotoPhotoTakeNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PhotoPhotoTakeHandler));

        public PhotoPhotoTakeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPhotoPhotoTakeNtc> packet)
        {
            Server.AchievementManager.HandleTakePhoto(client).Send();
        }
    }
}
