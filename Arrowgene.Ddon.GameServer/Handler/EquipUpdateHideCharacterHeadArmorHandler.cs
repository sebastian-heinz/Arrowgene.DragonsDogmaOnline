using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipUpdateHideCharacterHeadArmorHandler : GameRequestPacketQueueHandler<C2SEquipUpdateHideCharacterHeadArmorReq, S2CEquipUpdateHideCharacterHeadArmorRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipUpdateHideCharacterHeadArmorHandler));

        public EquipUpdateHideCharacterHeadArmorHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SEquipUpdateHideCharacterHeadArmorReq request)
        {
            PacketQueue queue = new();

            client.Character.HideEquipHead = request.Hide;
            Database.UpdateCharacterCommonBaseInfo(client.Character);
            client.Enqueue(new S2CEquipUpdateHideCharacterHeadArmorRes()
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
