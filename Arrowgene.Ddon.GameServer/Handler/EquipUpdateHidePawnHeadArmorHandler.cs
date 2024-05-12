using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipUpdateHidePawnHeadArmorHandler : StructurePacketHandler<GameClient, C2SEquipUpdateHidePawnHeadArmorReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipUpdateHidePawnHeadArmorHandler));

        public EquipUpdateHidePawnHeadArmorHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipUpdateHidePawnHeadArmorReq> packet)
        {
            client.Character.HideEquipHeadPawn = packet.Structure.Hide;
            Database.UpdateCharacterBaseInfo(client.Character);

            foreach (Pawn pawn in client.Character.Pawns)
            {
                pawn.HideEquipHead = packet.Structure.Hide;
                Database.UpdateCharacterCommonBaseInfo(pawn);
            }

            client.Send(new S2CEquipUpdateHidePawnHeadArmorRes()
            {
                Hide = packet.Structure.Hide
            });

            S2CEquipUpdateEquipHideNtc ntc = new S2CEquipUpdateEquipHideNtc()
            {
                CharacterId = client.Character.CharacterId,
                HideHead = client.Character.HideEquipHead,
                HideLantern = client.Character.HideEquipLantern,
                HidePawnHead = client.Character.HideEquipHeadPawn,
                HidePawnLantern = client.Character.HideEquipLanternPawn
            };
            foreach(Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(ntc);
            }
        }
    }
}