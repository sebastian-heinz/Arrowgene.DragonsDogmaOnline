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
using Arrowgene.Ddon.GameServer.Party;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : GameStructurePacketHandler<C2SStageAreaChangeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageAreaChangeHandler));

        // List of "safe" areas, where the context reset NTC will be sent.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> SafeStageIds = new HashSet<uint>(){
            2 // White Dragon Temple
        };

        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SStageAreaChangeReq> packet)
        {
            S2CStageAreaChangeRes res = new S2CStageAreaChangeRes();
            res.StageNo = ConvertIdToStageNo(packet.Structure.StageId);
            res.IsBase = false; // This is set true for audience chamber and WDT for exmaple

            client.Character.StageNo = res.StageNo;
            client.Character.Stage = new StageId(packet.Structure.StageId, 0, 0);

            foreach (var pawn in client.Character.Pawns)
            {
                pawn.StageNo = res.StageNo;
            }

            Logger.Info($"StageNo:{client.Character.StageNo} StageId{packet.Structure.StageId}");

            if (SafeStageIds.Contains(packet.Structure.StageId))
            {
                res.IsBase = true;

                bool shouldReset = true;
                // Check to see if all player members are in a safe area.
                foreach (var member in client.Party.Members)
                {
                    if (member == null || member.IsPawn)
                    {
                        continue;
                    }

                    // TODO: Is it safe to iterate over player party members this way?
                    // TODO: Can this logic be made part of the party object instead?
                    shouldReset &= SafeStageIds.Contains(((PlayerPartyMember)member).Client.Character.Stage.Id);
                    if (!shouldReset)
                    {
                        // No need to loop over rest of party members
                        break;
                    }
                }

                if (shouldReset)
                {
                    client.Party.ResetInstance();
                    client.Party.SendToAll(new S2CInstanceAreaResetNtc());
                }
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
