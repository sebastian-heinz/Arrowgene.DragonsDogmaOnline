using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class StageManager
    {
        private readonly static List<CDataStageInfo> StageList = EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19).StageList;

        public static StageNo ConvertIdToStageNo(uint stageId)
        {
            foreach (CDataStageInfo stageInfo in StageList)
            {
                if (stageInfo.Id == stageId)
                    return (StageNo)stageInfo.StageNo;
            }

            return 0; // TODO: Maybe throw an exception?
        }

        public static StageNo ConvertIdToStageNo(StageId stageId)
        {
            return StageManager.ConvertIdToStageNo(stageId.Id);
        }

        // List of "safe" areas, where the context reset NTC will be sent.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> SafeStageIds = new HashSet<uint>(){
            2, // White Dragon Temple
            341, // Dana Centrum
            487, // Fortress City Megado: Residential Level
            4, // Craft Room
            5, // Cave Harbor
            24, // White Deer Inn
            25, // Black Grape Inn
            26, // Sea Dragon Inn
            48, // Singing Winds Inn
            52, // Red Crystal Inn
            53, // Sleeping Wolf Inn
            61, // Golden Tankard Inn
            66, // Gritten Fort
            78, // Pawn Cathedral
            98, // Hobolic Cave
            137, // Mysree Grove Shrine
            139, // Zandora Wastelands Shrine
            141, // Breya Coast (Summer Event Hub Area)
            237, // Mergoda Residential Area
            317, // Expedition Garrison
            339, // Protector's Retreat
            340, // Morfaul Centrum
            347, // Clan Hall
            348, // Arisen's Room
            377, // Glyndwr Centrum
            384, // Hollow of Beginnings: Gathering Area
            400, // Tower of Ivanos
            401, // Spirit Arts Hut
            411, // Manun Village
            467, // Fort Thines
            478, // Lookout Castle
            480, // Bertha's Bandit Group Hideout
            511, // Piremoth Traveler's Inn
            512, // Rothgill Traveler's Inn
            520, // Mephite Traveler's Inn
            549, // Heroic Spirit Sleeping Path: Rathnite Foothills
            557, // Heroic Spirit Sleeping Path: Feryana Wilderness
            558, // Old Heroic Spirit Shrine
            576, // Fort Thines: Great Dining Hall
            580, // Fortress City Megado: Craft Room
            584, // Eli Guard Tower
            594  // Northern Bandit Hideout
        };
        public static bool IsSafeArea(uint stageId)
        {
            return SafeStageIds.Contains(stageId);
        }

        public static bool IsSafeArea(StageId stageId)
        {
            return StageManager.IsSafeArea(stageId.Id);
        }

        // List of lobby areas, where you're supposed to see all other players.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> HubStageIds = new HashSet<uint>(){
            2, // White Dragon Temple
            141, // Breya Coast (Summer Event Hub Area)
            341, // Dana Centrum
            486, // Fortress City Megado: Residential Level
            487, // Fortress City Megado: Residential Level
            488, // Fortress City Megado: Royal Palace Level
        };

        public static bool IsHubArea(uint stageId)
        {
            return HubStageIds.Contains(stageId);
        }

        public static bool IsHubArea(StageId stageId)
        {
            return StageManager.IsHubArea(stageId.Id);
        }
    }
}
