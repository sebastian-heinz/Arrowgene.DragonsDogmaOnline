using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftTimeSaveHandler : GameStructurePacketHandler<C2SCraftTimeSaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftCancelCraftHandler));

        public CraftTimeSaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftTimeSaveReq> packet)
        {
            // TODO: consume GG

            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, packet.Structure.PawnID);
            craftProgress.RemainTime = 0;
            Server.Database.UpdatePawnCraftProgress(craftProgress);

            client.Send(new S2CCraftTimeSaveRes { PawnID = packet.Structure.PawnID, RemainTime = 0 });
            client.Send(new S2CCraftFinishCraftNtc { PawnId = packet.Structure.PawnID });
        }
    }
}
