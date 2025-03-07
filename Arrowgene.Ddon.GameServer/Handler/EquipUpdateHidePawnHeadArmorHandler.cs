using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipUpdateHidePawnHeadArmorHandler : GameRequestPacketQueueHandler<C2SEquipUpdateHidePawnHeadArmorReq, S2CEquipUpdateHidePawnHeadArmorRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipUpdateHidePawnHeadArmorHandler));

        public EquipUpdateHidePawnHeadArmorHandler(DdonGameServer server) : base(server)
        {
        }


        public override PacketQueue Handle(GameClient client, C2SEquipUpdateHidePawnHeadArmorReq request)
        {
            PacketQueue queue = new();

            client.Character.HideEquipHeadPawn = request.Hide;
            Database.UpdateCharacterBaseInfo(client.Character);

            foreach (Pawn pawn in client.Character.Pawns)
            {
                pawn.HideEquipHead = request.Hide;
                Database.UpdateCharacterCommonBaseInfo(pawn);
            }

            client.Enqueue(new S2CEquipUpdateHidePawnHeadArmorRes()
            {
                Hide = request.Hide
            }, queue);

            S2CEquipUpdateEquipHideNtc ntc = new S2CEquipUpdateEquipHideNtc()
            {
                CharacterId = client.Character.CharacterId,
                HideHead = client.Character.HideEquipHead,
                HideLantern = client.Character.HideEquipLantern,
                HidePawnHead = client.Character.HideEquipHeadPawn,
                HidePawnLantern = client.Character.HideEquipLanternPawn
            };
            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Enqueue(ntc, queue);
            }

            return queue;
        }
    }
}
