using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : StructurePacketHandler<GameClient, C2SStageAreaChangeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageAreaChangeHandler));

        // List of "safe" areas, where the context reset NTC will be sent.
        // TODO: Complete with all the safe areas.
        private static readonly HashSet<uint> SafeStageIds = new HashSet<uint>(){
            // Lobbies
            2, // White Dragon Temple
            3, // Audience Chamber
            4, // Craft Room
            5, // Cave Harbor
            347, // Clan Hall
            348, // Arisen Room
            340, // Morfaul Centrum
            341, // Dana Centrum
            377, // Glyndwr Centrum
            467, // Fort Thines
            487, // Fortress City Megado: Residential Level
            595, // Firefall Mountain Campsite

            // Inns, Job Masters, other safe areas
            24, // White Deer Inn
            25, // Black Grape Inn
            26, // Sea Dragon Inn
            48, // Singing Winds Inn
            52, // Red Crystal Inn
            53, // Sleeping Wolf Inn
            66, // Gritten Fort
            78, // Pawn Cathedral
            95, // Hobolic Cave
            137, // Mysree Grove Shrine
            139, // Zandora Wastelands Shrine
            237, // Mergoda Residential Area
            61, // Golden Tankard Inn
            384, // Hollow of Beginnings: Gathering Area
            511, // Piremoth Traveler's Inn
            512, // Rothgill Traveler's Inn
            520, // Mephite Traveler's Inn
            317, // Expedition Garrison
            339, // Protector's Retreat
            400, // Tower of Ivanos
            401, // Spirit Arts Hut
            411, // Manun Village
            478, // Lookout Castle
            480, // Bertha's Bandit Group Hideout
            584, // Eli Guard Tower
            594, // Northern Bandit Hideout
            549, // Heroic Spirit Sleeping Path: Rathnite Foothills
            557, // Heroic Spirit Sleeping Path: Feryana Wilderness
            558, // Old Heroic Spirit Shrine
            578, // Fort Thines: Great Dining Hall
        };

        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
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
