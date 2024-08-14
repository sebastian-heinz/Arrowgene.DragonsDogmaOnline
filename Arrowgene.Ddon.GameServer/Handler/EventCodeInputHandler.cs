using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EventCodeInputHandler : GameRequestPacketHandler<C2SEventCodeInputReq, S2CEventCodeInputRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EventCodeInputHandler));

        public EventCodeInputHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEventCodeInputRes Handle(GameClient client, C2SEventCodeInputReq request)
        {
            S2CEventCodeInputRes res = new S2CEventCodeInputRes();

            // TODO: based on event code figure out which items should be sent
            // https://h1g.jp/dd-on/?%E3%82%A4%E3%83%99%E3%83%B3%E3%83%88%E3%82%B3%E3%83%BC%E3%83%89
            // https://dengekionline.com/elem/000/001/138/1138291/
            if (request.Code == "UPCT-YUAW-3K16-8255")
            {
                res.Name = "Silver Tickets and Trial Support Boat Course";
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_EVENT_CODE_INPUT_LOCK);
            }

            // TODO: send mail with items

            return res;
        }
    }
}
