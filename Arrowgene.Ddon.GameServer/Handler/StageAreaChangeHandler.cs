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

        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SStageAreaChangeReq> packet)
        {
            S2CStageAreaChangeRes res = new S2CStageAreaChangeRes();
            res.StageNo = (uint) StageManager.ConvertIdToStageNo(packet.Structure.StageId);
            res.IsBase = false; // This is set true for audience chamber and WDT for exmaple

            client.Character.StageNo = res.StageNo;
            client.Character.Stage = new StageId(packet.Structure.StageId, 0, 0);

            foreach (var pawn in client.Character.Pawns)
            {
                pawn.StageNo = res.StageNo;
            }

            Logger.Info($"StageNo:{client.Character.StageNo} StageId{packet.Structure.StageId}");

            if (StageManager.IsSafeArea(client.Character.Stage))
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
                }
            }

            client.Send(res);
        }
    }
}
