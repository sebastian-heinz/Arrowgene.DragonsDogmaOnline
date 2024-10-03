using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : GameRequestPacketHandler<C2SStageAreaChangeReq, S2CStageAreaChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageAreaChangeHandler));

        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageAreaChangeRes Handle(GameClient client, C2SStageAreaChangeReq packet)
        {
            S2CStageAreaChangeRes res = new S2CStageAreaChangeRes();
            res.StageNo = (uint) StageManager.ConvertIdToStageNo(packet.StageId);
            res.IsBase = false; // This is set true for audience chamber and WDT for exmaple

            uint previousStageId = client.Character.Stage.Id;

            ContextManager.DelegateAllMasters(client);

            client.Character.StageNo = res.StageNo;
            client.Character.Stage = new StageId(packet.StageId, 0, 0);

            // For shared spaces, deal with all the context updating required for characters to be visible.
            // Must be done after Character.StageNo is set because of how the context is structured.
            Server.HubManager.UpdateLobbyContextOnStageChange(client, previousStageId, packet.StageId);

            foreach (var pawn in client.Character.Pawns)
            {
                pawn.StageNo = res.StageNo;
            }

            if (StageManager.IsSafeArea(client.Character.Stage))
            {
                res.IsBase = true;
                client.Character.LastSafeStageId = packet.StageId;

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
                    shouldReset &= StageManager.IsSafeArea(((PlayerPartyMember)member).Client.Character.Stage);
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
                    // Next two packets seem to be send when transitioning to a safe area in all pcaps
                    // not sure what they do though
                    client.Party.SendToAll(new S2C_SEASON_62_38_16_NTC());
                    // client.Party.SendToAll(new S2C_SEASON_62_39_16_NTC) ??? Does this go to all, it has a character ID
                }
            }

            if (client.Party.ContentInProgress && BoardManager.BoardIdIsExm(client.Party.ContentId))
            {
                var quest = QuestManager.GetQuestByBoardId(client.Party.ContentId);
                if (quest != null)
                {
                    quest.HandleAreaChange(client, client.Character.Stage);
                }
            }

            return res;
        }
    }
}
