using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipUpdateHideCharacterHeadArmorHandler : StructurePacketHandler<GameClient, C2SEquipUpdateHideCharacterHeadArmorReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipUpdateHideCharacterHeadArmorHandler));

        public EquipUpdateHideCharacterHeadArmorHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipUpdateHideCharacterHeadArmorReq> packet)
        {
            client.Character.HideEquipHead = packet.Structure.Hide;
            Database.UpdateCharacterBaseInfo(client.Character);
            client.Send(new S2CEquipUpdateHideCharacterHeadArmorRes()
            {
                Hide = packet.Structure.Hide
            });

            S2CEquipUpdateEquipHideNtc ntc = new S2CEquipUpdateEquipHideNtc()
            {
                CharacterId = client.Character.Id,
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