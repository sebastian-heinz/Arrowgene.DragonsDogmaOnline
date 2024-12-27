using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaSupplyHandler : GameRequestPacketHandler<C2SAreaGetAreaSupplyReq, S2CAreaGetAreaSupplyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaSupplyHandler));

        public AreaGetAreaSupplyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaSupplyRes Handle(GameClient client, C2SAreaGetAreaSupplyReq request)
        {
            if (!client.Character.AreaSupply.ContainsKey(request.AreaId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_SUPPLY_NOT_AVAILABLE);
            }
            List<CDataRewardItemInfo> supply = client.Character.AreaSupply[request.AreaId];

            // I don't know what this "StorageType" is actually corresponding to, but there are only two values permitted
            // by the Area Supply UI.
            bool toBag = request.StorageType switch
            {
                19 => true,
                20 => false,
                _ => throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_INVALID_SUPPLY_STORAGE),
            };

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.ReceiveAreaSupply
            };
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var item in request.SelectItemInfoList)
                {
                    CDataRewardItemInfo matchSupply = supply.Find(x => x.Index == item.Index);
                    var result = Server.ItemManager.AddItem(Server, client.Character, toBag, matchSupply.ItemId, item.Num, connectionIn: connection);
                    ntc.UpdateItemList.AddRange(result);

                    matchSupply.Num -= item.Num;
                    Server.Database.UpdateAreaRankSupply(client.Character.CharacterId, request.AreaId, matchSupply.Index, matchSupply.ItemId, matchSupply.Num, connection);
                }
            });

            supply.RemoveAll(x => x.Num == 0);

            if (ntc.UpdateItemList.Any())
            {
                client.Send(ntc);
            }

            return new();
        }
    }
}
