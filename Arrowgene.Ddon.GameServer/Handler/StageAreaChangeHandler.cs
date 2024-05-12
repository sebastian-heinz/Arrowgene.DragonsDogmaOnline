using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : StructurePacketHandler<GameClient, C2SStageAreaChangeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageAreaChangeHandler));
        private readonly CharacterManager _CharacterManager;

        // List of "safe" areas, where the context reset NTC will be sent.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> SafeStageIds = new HashSet<uint>(){
            2 // White Dragon Temple
        };

        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SStageAreaChangeReq> packet)
        {
            S2CStageAreaChangeRes res = new S2CStageAreaChangeRes();
            res.StageNo = ConvertIdToStageNo(packet.Structure.StageId);
            res.IsBase = false;

            client.Character.StageNo = res.StageNo;
            client.Character.Stage = new StageId(packet.Structure.StageId, 0, 0);

            foreach (var pawn in client.Character.Pawns)
            {
                pawn.StageNo = res.StageNo;
            }

            Logger.Info($"StageNo:{client.Character.StageNo} StageId{packet.Structure.StageId}");

            if(client.Party.GetPlayerPartyMember(client).IsLeader && SafeStageIds.Contains(packet.Structure.StageId))
            {
                _CharacterManager.ResetAllOmData(client.Character);
                client.Party.SendToAll(new S2CInstanceAreaResetNtc());
                client.Party.ResetInstance();
            }

            client.Send(res);
        }

        private uint ConvertIdToStageNo(uint stageId)
        {
            foreach(CDataStageInfo stageInfo in (Server as DdonGameServer).StageList)
            {
                if(stageInfo.Id == stageId)
                    return stageInfo.StageNo;
            }

            Logger.Error($"No stage found with Id:{stageId}");
            return 0; // TODO: Maybe throw an exception?
        }
    }
}
