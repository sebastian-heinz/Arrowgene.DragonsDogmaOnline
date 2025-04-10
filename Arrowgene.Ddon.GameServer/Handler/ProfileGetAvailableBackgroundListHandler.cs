using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileGetAvailableBackgroundListHandler : GameRequestPacketHandler<C2SProfileGetAvailableBackgroundListReq, S2CProfileGetAvailableBackgroundListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileGetAvailableBackgroundListHandler));

        public ProfileGetAvailableBackgroundListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CProfileGetAvailableBackgroundListRes Handle(GameClient client, C2SProfileGetAvailableBackgroundListReq request)
        {
            uint count = (uint)client.Character.AchievementStatus.Count;
            S2CProfileGetAvailableBackgroundListRes res = new();

            res.BackgroundIdList.AddRange(Server.AssetRepository.AchievementBackgroundAsset.DefaultBackgrounds.Select(x => new CDataCommonU32(x)));
            res.BackgroundIdList.AddRange(client.Character.UnlockableItems.Where(x => x.Category == UnlockableItemCategory.ArisenCardBackground).Select(x => new CDataCommonU32(x.Id)));

            return res;
        }
    }
}
