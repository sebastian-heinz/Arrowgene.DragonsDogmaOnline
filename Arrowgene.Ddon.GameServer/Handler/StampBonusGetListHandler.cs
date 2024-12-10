using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusGetListHandler : GameRequestPacketHandler<C2SStampBonusGetListReq, S2CStampBonusGetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusGetListHandler));

        private readonly DdonGameServer _gameServer;

        public StampBonusGetListHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override S2CStampBonusGetListRes Handle(GameClient client, C2SStampBonusGetListReq request)
        {
            //This handler gets called twice in a row; once for the daily bonus list and then once for the total bonus list.
            //The sequence normally goes CHECK -> GET_LIST -> RECIEVE_DAILY -> GET_LIST -> RECIEVE_TOTAL
            //Character.StampBonus is incremented in RECIEVE_DAILY, so the daily list portions need an offset and the total list portions don't.

            ushort totalStampNum = (ushort)(client.Character.StampBonus.TotalStamp);

            var res = new S2CStampBonusGetListRes();
            res.TotalStamp.TotalStampNum = totalStampNum;
            res.TotalStamp.AssetList = _gameServer.StampManager.GetTotalStampAssetsWindow(totalStampNum);
            res.DailyStamp.Add(new CDataStampBonusDaily()
            {
                Unk0 = 1,
                Unk1 = 77,
                AssetList = _gameServer.StampManager.GetDailyStampAssets()
            });

            //If you missed a day, reset stamp to 0 (to be incremented up in RECIEVE_DAILY)
            if (StampManager.CanResetConsecutiveStamp(client.Character.StampBonus)) client.Character.StampBonus.ConsecutiveStamp = 0;
            //If you've finished the 8-stamp sequence and you're going to roll over to 9, reset stamp to 0.
            if (client.Character.StampBonus.ConsecutiveStamp >= StampManager.MAX_DAILY_STAMP) client.Character.StampBonus.ConsecutiveStamp = 0;

            foreach (var item in res.DailyStamp.First().AssetList)
            {
                if (item.StampNum < client.Character.StampBonus.ConsecutiveStamp + 1) item.RecieveState = (byte)StampRecieveState.Claimed;
                else if (item.StampNum == client.Character.StampBonus.ConsecutiveStamp + 1) item.RecieveState = (byte)StampRecieveState.ToBeClaimed;
                else item.RecieveState = (byte)StampRecieveState.Unearned;
            }

            foreach (var item in res.TotalStamp.AssetList)
            {
                if (item.StampNum < totalStampNum) item.RecieveState = (byte)StampRecieveState.Claimed;
                else if (item.StampNum == totalStampNum) item.RecieveState = (byte)StampRecieveState.ToBeClaimed;
                else item.RecieveState = (byte)StampRecieveState.Unearned;
            }

            return res;
        }
    }
} 
