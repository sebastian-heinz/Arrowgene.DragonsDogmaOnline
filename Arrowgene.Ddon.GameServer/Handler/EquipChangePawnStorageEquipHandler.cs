using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnStorageEquipHandler : GameStructurePacketHandler<C2SEquipChangePawnStorageEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnStorageEquipHandler));
        
        private readonly EquipManager equipManager;

        public EquipChangePawnStorageEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangePawnStorageEquipReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            Server.Database.ExecuteInTransaction(connection =>
            {
                equipManager.HandleChangeEquipList(Server, client, pawn, packet.Structure.ChangeCharacterEquipList, ItemNoticeType.ChangeStoragePawnEquip, ItemManager.BoxStorageTypes, () =>
                {
                    client.Send(new S2CEquipChangePawnStorageEquipRes()
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
