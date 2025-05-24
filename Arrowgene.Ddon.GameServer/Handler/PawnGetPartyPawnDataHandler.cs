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

        private readonly DdonGameServer Server;

        public PawnGetPartyPawnDataHandler(DdonGameServer server) : base(server)
        {
            Server = server;
        }

        public override S2CPawnGetPartyPawnDataRes Handle(GameClient client, C2SPawnGetPartyPawnDataReq packet)
        {
            // var owner = Server.CharacterManager.SelectCharacter(packet.Structure.CharacterId);
            GameClient owner = Server.ClientLookup.GetClientByCharacterId(packet.CharacterId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PARAM_NOT_FOUND);
            // TODO: Move this to a function or lookup class
            List<Pawn> pawns = owner.Character.Pawns.Concat(client.Character.RentedPawns).ToList();

            Pawn pawn = pawns
                .Find(pawn => pawn.PawnId == packet.PawnId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED,
                $"Couldn't find pawn ID {packet.PawnId}.");

            var res = new S2CPawnGetPartyPawnDataRes();
            res.CharacterId = pawn.CharacterId;
            res.PawnId = pawn.PawnId;

            // TODO: This static function is not flexible enough to accept
            // TODO: things like the CharacterManager. We should create a
            // TODO: proper mechanism for these conversions between server object and CData objects.
            GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
            res.PawnInfo.AbilityCostMax = Server.CharacterManager.GetMaxAugmentAllocation(pawn);

            S2CPawnGetPawnOrbDevoteInfoNtc ntc = new S2CPawnGetPawnOrbDevoteInfoNtc()
            {
                CharacterId = packet.CharacterId,
                PawnId = pawn.PawnId,
                OrbPageStatusList = Server.OrbUnlockManager.GetOrbPageStatus(pawn),
                JobOrbTreeStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(owner.Character),
            };
            client.Send(ntc);

            return res;
        }
    }
}
