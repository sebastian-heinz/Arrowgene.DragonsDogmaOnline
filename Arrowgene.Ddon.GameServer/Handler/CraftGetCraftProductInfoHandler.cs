using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProductInfoHandler : GameStructurePacketHandler<C2SCraftGetCraftProductInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProductInfoHandler));

        public CraftGetCraftProductInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftGetCraftProductInfoReq> packet)
        {
            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, packet.Structure.CraftMainPawnID);

            C2SCraftGetCraftProductInfoRes craftProductInfoRes = new C2SCraftGetCraftProductInfoRes();
            // TODO: PlusValue, ExtraBonus, IsGreatSuccess are not tracked right now
            CDataCraftProductInfo craftProductInfo = new CDataCraftProductInfo()
            {
                ItemID = craftProgress.ItemId,
                ItemNum = craftProgress.CreateCount,
                Unk0 = craftProgress.Unk0,
                PlusValue = 0,
                Exp = craftProgress.Exp,
                ExtraBonus = 0,
                IsGreatSuccess = false
            };
            craftProductInfoRes.CraftProductInfo = craftProductInfo;
            
            client.Send(craftProductInfoRes);
        }
    }
}
