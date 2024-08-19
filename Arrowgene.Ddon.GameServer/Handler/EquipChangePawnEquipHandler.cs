using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            Server.Database.ExecuteInTransaction(connection =>
            {
                equipManager.HandleChangeEquipList(Server, client, pawn, packet.Structure.ChangeCharacterEquipList, ItemNoticeType.ChangePawnEquip, new List<StorageType>() { StorageType.ItemBagEquipment }, () => {
                    client.Send(new S2CEquipChangePawnEquipRes()
                    {
                        PawnId = packet.Structure.PawnId,
                        CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                        // TODO: Unk0
                    });
                }, connection);
            });
        }
    }
}
