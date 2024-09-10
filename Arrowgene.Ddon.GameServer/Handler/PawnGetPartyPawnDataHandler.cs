using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPartyPawnDataHandler : GameRequestPacketHandler<C2SPawnGetPartyPawnDataReq, S2CPawnGetPartyPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPartyPawnDataHandler));

        private readonly CharacterManager _CharacterManager;
        private readonly OrbUnlockManager _OrbUnlockManager;

        public PawnGetPartyPawnDataHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
            _OrbUnlockManager = server.OrbUnlockManager;
        }

        public override S2CPawnGetPartyPawnDataRes Handle(GameClient client, C2SPawnGetPartyPawnDataReq packet)
        {
            // var owner = Server.CharacterManager.SelectCharacter(packet.Structure.CharacterId);
            GameClient owner = this.Server.ClientLookup.GetClientByCharacterId(packet.CharacterId);
            // TODO: Move this to a function or lookup class
            List<Pawn> pawns = owner.Character.Pawns.Concat(client.Character.RentedPawns).ToList();

            if (!pawns.Any())
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED);
            }

            Pawn pawn = pawns.Where(pawn => pawn.PawnId == packet.PawnId).First();

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
                CharacterId = packet.CharacterId,
                PawnId = pawn.PawnId,
                OrbPageStatusList = _OrbUnlockManager.GetOrbPageStatus(pawn)
            };
            client.Send(ntc);

            return res;
        }
    }
}
