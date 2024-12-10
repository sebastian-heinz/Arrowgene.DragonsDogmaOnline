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
using System.IO;

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

            return 0;
        }

        public static StageNo ConvertIdToStageNo(StageId stageId)
        {
            return StageManager.ConvertIdToStageNo(stageId.Id);
        }

        // List of "safe" areas, where the context reset NTC will be sent.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> SafeStageIds = new HashSet<uint>()
        {
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
            95, // Hobolic Cave
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
            578, // Bonus Dungeon Lobby
            580, // Fortress City Megado: Craft Room
            584, // Eli Guard Tower
            594, // Northern Bandit Hideout
            602, // Bitterblack Maze Cove
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
        public static readonly HashSet<uint> HubStageIds = new HashSet<uint>(){
            2, // White Dragon Temple
            141, // Breya Coast (Summer Event Hub Area)
            341, // Dana Centrum
            347, // Clan Hall (has special handling)
            486, // Fortress City Megado: Residential Level
            487, // Fortress City Megado: Residential Level
            488, // Fortress City Megado: Royal Palace Level
            602, // Bitterblack Maze Cove
        };

        public static bool IsHubArea(uint stageId)
        {
            return HubStageIds.Contains(stageId);
        }

        public static bool IsHubArea(StageId stageId)
        {
            return StageManager.IsHubArea(stageId.Id);
        }

        public static readonly uint BitterblackCove = 602;

        private static readonly HashSet<uint> BitterBlackStageIds = new HashSet<uint>()
        {
            602, // Bitterblack Maze Cove
            603, // Garden of Ignominy
            604, // Duskmoon Tower
            605, // Rotunda of Dread
            610, // Netherworld 1
            611, // Netherworld 1
            612, // Netherworld 1
            614, // Netherworld 2
            615, // Netherworld 2
            616, // Netherworld 2
            617, // Netherworld 3
            618, // Netherworld 3
            619, // Netherworld 3
            620, // Netherworld 3
            621, // Netherworld 3
            622, // Netherworld 3
            623, // Rift
            624, // Rift
            682, // Noxious Cathedral
            683, // Traitors Cathedral
            684, // Fallen City
            685, // Altar of the Black Curse
            686, // Netherworld 1
            687, // Netherworld 1
            688, // Netherworld 1
            689, // Netherworld 2
            690, // Netherworld 2
            691, // Netherworld 2
            692, // Netherworld 3
            693, // Netherworld 3
            694, // Netherworld 3
            695, // Netherworld 4
            696, // Netherworld 4
            697, // Netherworld 4
            698, // Rift
            699, // Rift
            700, // Rift
            715, // Netherworld 4
            716, // Netherworld 4
            717, // Netherworld 4
        };

        public static bool IsBitterBlackMazeStageId(uint stageId)
        {
            return BitterBlackStageIds.Contains(stageId);
        }

        public static bool IsBitterBlackMazeStageId(StageId stageId)
        {
            return StageManager.IsBitterBlackMazeStageId(stageId.Id);
        }

        private static readonly HashSet<uint> BitterBlackNormalBossStageIds = new HashSet<uint>()
        {
            603, // Garden of Ignominy
            604, // Duskmoon Tower
            605, // Rotunda of Dread
        };

        public static bool IsBitterBlackMazeBossStageId(uint stageId)
        {
            return BitterBlackNormalBossStageIds.Contains(stageId);
        }

        public static bool IsBitterBlackMazeBossStageId(StageId stageId)
        {
            return StageManager.IsBitterBlackMazeBossStageId(stageId.Id);
        }

        private static readonly HashSet<uint> BitterBlackAbyssBossStageIds = new HashSet<uint>()
        {
            682, // Noxious Cathedral
            683, // Traitors Tower
            684, // Fallen City
            685, // Alter of the Black Curse
        };

        public static bool IsBitterBlackMazeAbyssBossStageId(uint stageId)
        {
            return BitterBlackAbyssBossStageIds.Contains(stageId);
        }

        public static bool IsBitterBlackMazeAbyssBossStageId(StageId stageId)
        {
            return StageManager.IsBitterBlackMazeAbyssBossStageId(stageId.Id);
        }

        // Hubs
        // 549 Sleeping Path : Rathnite Foothills (Pehr)
        // 557 Sleeping Path : Feryana (Ira)
        // 558 Old Heroic Spirit Shrine (Morgan -> Memory of Megadosys, Selim -> Memory of Urteca)

        private static readonly HashSet<uint> EpitaphRoadStageIds = new HashSet<uint>()
        {
            // 3.0
            550, // Heroic Spirit Sleeping Path: Shrine
            551, // Heroic Spirit Sleeping Path: Cave
            // Cave Depth??
            553, // Heroic Spirit Sleeping Path: Waterway
            // 3.1
            552, // Heroic Spirit Sleeping Path: Ruins
            554, // Heroic Spirit Sleeping Path: Well
            555, // Heroic Spirit Sleeping Path: Tomb
            556, // Heroic Spirit Sleeping Path Ruins: Deepest Level
            // 3.2
            559, // Memory of Megadosys
            560, // Memory of Megadosys: Old Road
            561, // Memory of Megadosys: War God Space
            // 3.3
            563, // Memory of Urteca
            564, // Memories of the Earth: Sacred Flame Path
            565, // Memory of Urteca: War God Space
            566, // Memory of Royal Family Mausoleum
            567, // Memory of Firefall Mountain Campsite
        };

        public static bool IsEpitaphRoadStageId(uint stageId)
        {
            return EpitaphRoadStageIds.Contains(stageId);
        }

        public static bool IsEpitaphRoadStageId(StageId stageId)
        {
            return StageManager.IsEpitaphRoadStageId(stageId.Id);
        }

        private static readonly HashSet<uint> LegacyEpitaphRoadStageIds = new HashSet<uint>
        {
            // 3.0
            550, // Heroic Spirit Sleeping Path: Shrine
            551, // Heroic Spirit Sleeping Path: Cave
            // Cave Depth??
            553, // Heroic Spirit Sleeping Path: Waterway
            // 3.1
            552, // Heroic Spirit Sleeping Path: Ruins
            554, // Heroic Spirit Sleeping Path: Well
            555, // Heroic Spirit Sleeping Path: Tomb
            556, // Heroic Spirit Sleeping Path Ruins: Deepest Level
        };

        public static bool IsLegacyEpitaphRoadStageId(uint stageId)
        {
            return LegacyEpitaphRoadStageIds.Contains(stageId);
        }

        public static bool IsLegacyEpitaphRoadStageId(StageId stageId)
        {
            return StageManager.IsLegacyEpitaphRoadStageId(stageId.Id);
        }

        private static readonly HashSet<uint> EpitaphHubArea = new HashSet<uint>
        {
            549, // Heroic Spirit Sleeping Path: Rathnite Foothills
            1148, // Heroic Spirit Sleeping Path: Feryana Wilderness
            1149, // Memory of Megadosys/Memory of Urteca
        };

        public static bool IsEpitaphHubArea(uint stageId)
        {
            return EpitaphHubArea.Contains(stageId);
        }

        public static bool IsEpitaphHubArea(StageId stageId)
        {
            return StageManager.IsEpitaphHubArea(stageId.Id);
        }

        private static readonly HashSet<uint> LegacyEpitaphHubArea = new HashSet<uint>
        {
            549, // Heroic Spirit Sleeping Path: Rathnite Foothills
            1148, // Heroic Spirit Sleeping Path: Feryana Wilderness
        };

        public static bool IsLegacyEpitaphHubArea(uint stageId)
        {
            return LegacyEpitaphHubArea.Contains(stageId);
        }

        public static bool IsLegacyEpitaphHubArea(StageId stageId)
        {
            return StageManager.IsLegacyEpitaphHubArea(stageId.Id);
        }
    }
}
