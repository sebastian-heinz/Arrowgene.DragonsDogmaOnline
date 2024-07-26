using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPartyPawnDataHandler : GameStructurePacketHandler<C2SPawnGetPartyPawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPartyPawnDataHandler));

        private readonly CharacterManager _CharacterManager;
        private readonly OrbUnlockManager _OrbUnlockManager;

        public PawnGetPartyPawnDataHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
            _OrbUnlockManager = server.OrbUnlockManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetPartyPawnDataReq> packet)
        {
            // var owner = Server.CharacterManager.SelectCharacter(packet.Structure.CharacterId);
            GameClient owner = this.Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            // TODO: Move this to a function or lookup class
            List<Pawn> pawns = owner.Character.Pawns.Concat(client.Character.RentedPawns).ToList();
            Pawn pawn = pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).First();

            var res = new S2CPawnGetPartyPawnDataRes();
            res.CharacterId = pawn.CharacterId;
            res.PawnId = pawn.PawnId;

            // TODO: This static function is not flexible enough to accept
            // TODO: things like the CharacterManager. We should create a
            // TODO: proper mechanism for these conversions between server object and CData objects.
            GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
            res.PawnInfo.AbilityCostMax = _CharacterManager.GetMaxAugmentAllocation(pawn);

            S2CPawnGetPawnOrbDevoteInfoNtc ntc = new S2CPawnGetPawnOrbDevoteInfoNtc()
            {
                CharacterId = packet.Structure.CharacterId,
                PawnId = pawn.PawnId,
                OrbPageStatusList = _OrbUnlockManager.GetOrbPageStatus(pawn)
            };
            client.Send(ntc);

            client.Send(res);
        }
    }
}
