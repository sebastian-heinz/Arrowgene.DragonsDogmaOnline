using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BoxGachaDrawInfoHandler : GameRequestPacketHandler<C2SBoxGachaDrawInfoReq, S2CBoxGachaDrawInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BoxGachaDrawInfoHandler));

        public BoxGachaDrawInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBoxGachaDrawInfoRes Handle(GameClient client, C2SBoxGachaDrawInfoReq request)
        {
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_NOT_IMPLEMENTED);

            S2CBoxGachaDrawInfoRes res = new S2CBoxGachaDrawInfoRes();

            // Reflects received results to user in the appropriate tab
            // TODO: unsure if request's DrawId should be directly sent back or box gacha ID should be figured out based on DrawId
            res.BoxGachaId = 93;
            res.BoxGachaItemList.Add(new CDataBoxGachaItemInfo
            {
                ItemId = 13800,
                ItemNum = 5,
                ItemStock = 13,
                Rank = 2,
                Effect = 0,
                Probability = 0,
                DrawNum = 0
            });

            return res;
        }
    }
}
