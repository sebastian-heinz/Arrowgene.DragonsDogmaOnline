using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetItemSetListHandler : GameRequestPacketHandler<C2SInstanceGetItemSetListReq, S2CInstanceGetItemSetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetItemSetListHandler));

        public InstanceGetItemSetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetItemSetListRes Handle(GameClient client, C2SInstanceGetItemSetListReq request)
        {
            S2CInstanceGetItemSetListRes res = new()
            {
                LayoutId = request.LayoutId
            };

            // If you don't send the corresponding position, that gathering point doesn't appear.
            // Figuring out what spots are valid is somewhat tricky (spread across multiple subsystems)
            // so just return all possible spots and call it good. This is what we were doing with the old handler.
            for (byte i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                res.SetList.Add(new()
                {
                    PosId = i
                });
            }

            return res;
        }
    }
}
