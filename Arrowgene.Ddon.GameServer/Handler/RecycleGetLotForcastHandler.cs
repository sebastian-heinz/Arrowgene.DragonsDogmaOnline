using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RecycleGetLotForcastHandler : GameRequestPacketHandler<C2SRecycleGetLotForcastReq, S2CRecycleGetLotForcastRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RecycleGetLotForcastHandler));

        public RecycleGetLotForcastHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRecycleGetLotForcastRes Handle(GameClient client, C2SRecycleGetLotForcastReq request)
        {
            var equipmentRecycleMixin = Server.ScriptManager.MixinModule.Get<IEquipmentRecycleMixin>("equipment_recycle") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_REQUIRED_SERVER_SCRIPT_MISSING, "Couldn't find an object for the script 'equipment_cycle.csx'");

            ClientItemInfo itemInfo = Server.ItemManager.LookupInfoByUID(Server, request.ItemUID) ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CLIENT_ITEM_INFO_MISSING, $"Unable to find item information for '{request.ItemUID}'");

            var item = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.ItemUID)?.Item2.Item2 ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND, $"Unable to find the item '{request.ItemUID}' in the players storage");

            var rewards = equipmentRecycleMixin.GetRecycleRewards(Server.AssetRepository, itemInfo, item);
            return new S2CRecycleGetLotForcastRes()
            {
                NumberOfItemsInLottery = rewards.NumRewards,
                WalletPointList = rewards.WalletRewards.Select(x => new CDataWalletPoint()
                {
                    Type = x.WalletType,
                    Value = x.Amount
                }).ToList(),
                ItemRewardList = rewards.ItemRewards.Select(x => new CDataRecycleItemLot()
                {
                    ItemId = x.ItemId,
                    Amount = x.Amount,
                    Unk2 = false // TODO: Figure out what this is
                }).ToList()
            };
        }
    }
}
