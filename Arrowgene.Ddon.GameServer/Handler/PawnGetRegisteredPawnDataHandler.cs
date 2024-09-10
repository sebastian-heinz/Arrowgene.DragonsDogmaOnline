using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnDataHandler : GameRequestPacketHandler<C2SPawnGetRegisteredPawnDataReq, S2CPawnGetRegisteredPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRegisteredPawnDataHandler));

        public PawnGetRegisteredPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetRegisteredPawnDataRes Handle(GameClient client, C2SPawnGetRegisteredPawnDataReq request)
        {
            S2CPawnGetRegisteredPawnDataRes res = new S2CPawnGetRegisteredPawnDataRes();
            res.PawnId = (uint)request.PawnId;

            Pawn pawn = null;
            uint ownerCharacterId = Server.Database.GetPawnOwnerCharacterId((uint) request.PawnId);
            if (ownerCharacterId == 0)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PAWN_PARAM_NOT_FOUND);
            }

            var ownerCharacter = Server.CharacterManager.SelectCharacter(ownerCharacterId);
            for (int i = 0; i < ownerCharacter.Pawns.Count; i++)
            {
                if (ownerCharacter.Pawns[i].PawnId != request.PawnId)
                {
                    continue;
                }

                pawn = ownerCharacter.Pawns[i];
                break;
            }

            GameStructure.CDataPawnInfo(res.PawnInfo, pawn);

            return res;
        }
    }
}
