using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdatePawnEditParamExHandler : GameStructurePacketHandler<C2SCharacterEditUpdatePawnEditParamExReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdatePawnEditParamExHandler));
        
        public CharacterEditUpdatePawnEditParamExHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterEditUpdatePawnEditParamExReq> packet)
        {
            // TODO: Substract GG
            Pawn pawn = client.Character.PawnBySlotNo(packet.Structure.SlotNo);
            pawn.EditInfo = packet.Structure.EditInfo;
            Server.Database.UpdateEditInfo(pawn);
            pawn.Name = packet.Structure.Name;
            Server.Database.UpdatePawnBaseInfo(pawn);
            client.Send(new S2CCharacterEditUpdatePawnEditParamExRes());
            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamExNtc() {
                    CharacterId = pawn.CharacterId,
                    PawnId = pawn.PawnId,
                    EditInfo = pawn.EditInfo,
                    Name = pawn.Name
                });
            }
        }
    }
}