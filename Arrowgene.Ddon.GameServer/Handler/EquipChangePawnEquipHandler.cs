using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnEquipHandler : GameStructurePacketHandler<C2SEquipChangePawnEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnEquipHandler));
        
        private readonly EquipManager equipManager;

        public EquipChangePawnEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangePawnEquipReq> packet)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangePawnEquipNtc equipNtc) equipResult = (null, null);

            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangePawnEquipNtc))
                equipManager.HandleChangeEquipList(
                    Server, 
                    client, 
                    pawn, 
                    packet.Structure.ChangeCharacterEquipList, 
                    ItemNoticeType.ChangePawnEquip, 
                    new List<StorageType>() { StorageType.ItemBagEquipment },  
                    connection);
            });

            client.Send(equipResult.itemNtc);

            //Only the party needs to be updated, because only they can see pawns.
            client.Party.SendToAllExcept(equipResult.equipNtc, client);

            client.Send(new S2CEquipChangePawnEquipRes()
            {
                PawnId = packet.Structure.PawnId,
                CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                // TODO: Unk0
            });
        }
    }
}
