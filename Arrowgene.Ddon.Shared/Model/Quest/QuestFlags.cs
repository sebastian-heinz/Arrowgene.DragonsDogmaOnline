using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public static class QuestFlags
    {
        private static Dictionary<string, QuestFlagInfo> QuestFlagInfoMap = new Dictionary<string, QuestFlagInfo>();

        static QuestFlags()
        {
            Type outerType = typeof(QuestFlags);

            Type[] nestedTypes = outerType.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var nestedType in nestedTypes)
            {
                FieldInfo[] staticFields = nestedType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo field in staticFields)
                {
                    if (field.FieldType != typeof(QuestFlagInfo))
                    {
                        continue;
                    }

                    var fieldName = field.Name;
                    if (fieldName.Contains("<"))
                    {
                        fieldName = GetNameBetweenBrackets(fieldName);
                    }

                    QuestFlagInfo value = (QuestFlagInfo)field.GetValue(null);
                    QuestFlagInfoMap[$"{nestedType.Name}.{fieldName}"] = value;
                }
            }
        }

        private static string GetNameBetweenBrackets(string input)
        {
            int start = input.IndexOf('<');
            int end = input.IndexOf('>');

            if (start >= 0 && end > start)
            {
                return input.Substring(start + 1, end - start - 1);
            }
            return input; // Return original if no < > found
        }

        public static void InvokeTypeInitializer()
        {
            Type outerType = typeof(QuestFlags);
            outerType.TypeInitializer.Invoke(null, null);
        }

        public static QuestFlagInfo GetFlagInfoFromName(string name)
        {
            if (QuestFlagInfoMap.ContainsKey(name))
            {
                return QuestFlagInfoMap[name];
            }
            return null;
        }

        // NPC flags come from stxxxx_n_gpl.json
        // Search for LayoutFlagNo and QuestNo
        public static class TheWhiteDragonTemple0
        {
            private static StageInfo StageInfo = Stage.TheWhiteDragonTemple0;

            // st0200\scr\st0200\etc\st0200_p.gpl.json
            /// <summary>
            /// Spawns a red/white flags around the stage.
            /// </summary>
            public static QuestFlagInfo RedWhiteFlagDecoration { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1149, QuestId.Q70000001, StageInfo);
            
            /// <summary>
            /// 
            /// </summary>
            public static QuestFlagInfo Q70000001Unk1 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1150, QuestId.Q70000001, StageInfo);

            // st0200\scr\st0200\etc\st0200_n.gpl.json
            public static QuestFlagInfo Q70020001Unk0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3805, QuestId.Q70020001, StageInfo);
            public static QuestFlagInfo Q70034001Unk0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(8486, QuestId.Q70034001, StageInfo);

            /// <summary>
            /// Controls if Mayleaf appears or not in the stage.
            /// </summary>
            public static QuestFlagInfo Mayleaf { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(2245, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Controls if Fabio appears or not in the stage.
            /// </summary>
            public static QuestFlagInfo Fabio { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(2246, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Controls if Heniz appears or not in the stage.
            /// </summary>
            public static QuestFlagInfo Heniz { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(2247, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Controls if Julia appears or not in the stage in Group 6.
            /// @note This one may be the final NPC located when achievments are unlocked.
            /// </summary>
            public static QuestFlagInfo Julia0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3805, QuestId.Q70020001, StageInfo);

            /// <summary>
            /// Controls if Julia appears or not in the stage in Group 0.
            /// @note This one may be used for unlocking the arisens room as it has a movement FSM.
            /// </summary>
            public static QuestFlagInfo Julia1 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(4037, QuestId.Q70020001, StageInfo);
        }

        public static class AudienceChamber
        {
            private static StageInfo StageInfo = Stage.AudienceChamber;

            /// <summary>
            /// Controls if Mysial appears or not in the stage.
            /// </summary>
            public static QuestFlagInfo Mysial { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1215, QuestId.Q70000001, StageInfo);
            
            /// <summary>
            /// Controls if Season 1 Leo appears or not in the stage.
            /// </summary>
            public static QuestFlagInfo Leo { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1218, QuestId.Q70000001, StageInfo);
            
            /// <summary>
            /// Controls if Iris appears or not in the stage.
            /// </summary>
            public static QuestFlagInfo Iris { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1219, QuestId.Q70000001, StageInfo);

            /// Season 1 White Dragon flags

            /// <summary>
            /// The White Dragon is gravely Injured.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1293, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// The White Dragon is slightly healed.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon1 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1294, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// The White Dragon is mostly healed.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon2 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1295, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// The White Dragon is fully healed.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon3 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1296, QuestId.Q70000001, StageInfo);

            /// Season 3 White Dragon Flags

            /// <summary>
            /// The White Dragon is gravely Injured.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon4 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(7387, QuestId.Q70032001, StageInfo);

            /// <summary>
            /// The White Dragon is slightly healed.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon5 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(7388, QuestId.Q70032001, StageInfo);

            /// <summary>
            /// The White Dragon is mostly healed.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon6 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(7389, QuestId.Q70032001, StageInfo);

            /// <summary>
            /// The White Dragon is fully healed.
            /// </summary>
            public static QuestFlagInfo TheWhiteDragon7 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(7390, QuestId.Q70032001, StageInfo);

            /// <summary>
            /// Spawns Elliot, Lise and Gurdolin after season 2 is completed.
            /// </summary>
            public static QuestFlagInfo TheCrewEndSeason2 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3442, QuestId.Q70020001, StageInfo);

            /// <summary>
            /// Spawns Gurdolin, Lise, Elliot after Clearing (クリア後)
            /// </summary>
            public static QuestFlagInfo TheCrewEndSeason32 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(7169, QuestId.Q70032001, StageInfo);

            /// <summary>
            /// Spawns Gurdolin, Lise, Elliot after Clearing
            /// </summary>
            public static QuestFlagInfo TheCrewEndSeason34 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(8630, QuestId.Q70034001, StageInfo);
        }

        public static class CaveHarbor
        {
            private static StageInfo StageInfo = Stage.CaveHarbor;

            /// <summary>
            /// Controls if the portal to Bitterblack Maze is usable or not.
            /// </summary>
            public static QuestFlagInfo BitterblackMazeEntrance { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(4978, QuestId.Q70032001);

            /// <summary>
            /// Controls if the portal in cave harbor can warp to Bloodbane Isle Precipice or not.
            /// </summary>
            public static QuestFlagInfo BloodbaneIslePrecipice { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1433, QuestId.Q70021001);
        }

        public static class Lestania
        {
            private static StageInfo StageInfo = Stage.Lestania;

            /// <summary>
            /// Spawns Gerd and the White Knights outside for the 3rd MSQ.
            /// </summary>
            public static QuestFlagInfo GerdWhiteKnights0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(977, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Changes the entrance of The 1st Ark to teleport the player to st0573.
            /// </summary>
            public static QuestFlagInfo The1stArkRandom { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(2201, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Changes the entrance of The 1st Ark to teleport the player to st0576.
            /// </summary>
            public static QuestFlagInfo The1stArkQuest { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(2202, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Changes the entrance of The 2nd Ark to teleport the player to st0574.
            /// </summary>
            public static QuestFlagInfo The2ndArkRandom { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1263, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Changes the entrance of The 2nd Ark to teleport the player to st0571.
            /// </summary>
            public static QuestFlagInfo The2ndArkQuest { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(2204, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Allows the player to enter Gardnox Fortress when set
            /// </summary>
            public static QuestFlagInfo GardnoxFortress { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(925, QuestId.Q70002001);

            /// <summary>
            /// Allows the player to enter Dreed Castle when set
            /// </summary>
            public static QuestFlagInfo DreedCastle { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(5186, QuestId.Q70034001);

            /// <summary>
            /// Allows the player to enter Mergoda Security District when set
            /// </summary>
            public static QuestFlagInfo MergodaSecurityDistrict { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(5187, QuestId.Q70034001);

            /// <summary>
            /// Allows the player to enter the Knights' Depot Ruins when set
            /// </summary>
            public static QuestFlagInfo KnightsDepotRuins { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1545, QuestId.Q70021001);

            /// <summary>
            /// Allows the player to enter "Temple of Purification: South Chamber" when set
            /// </summary>
            public static QuestFlagInfo TempleOfPurificationSouthChamber { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(926, QuestId.Q70000001);
        }

        public static class GardnoxFortress0
        {
            private static StageInfo StageInfo = Stage.GardnoxFortress0;

            /// <summary>
            /// Floor Lever and Large Door closed?
            /// </summary>
            public static QuestFlagInfo FloorLever { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3859, QuestId.Q70002001, StageInfo);

            /// <summary>
            /// Large door open?
            /// </summary>
            public static QuestFlagInfo LargeDoor { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3860, QuestId.Q70002001, StageInfo);
        }

        public static class ErteDeenan
        {
            private static StageInfo StageInfo = Stage.ErteDeenan;

            /// <summary>
            /// Large Door Closed.
            /// </summary>
            public static QuestFlagInfo LargeDoorClosed { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1113, QuestId.Q70002001, StageInfo);

            /// <summary>
            /// Large Door Open.
            /// </summary>
            public static QuestFlagInfo LargeDoorOpen { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1114, QuestId.Q70002001, StageInfo);
        }

        public static class DreedCastle
        {
            private static StageInfo StageInfo = Stage.DreedCastle;

            /// <summary>
            /// Locks the double doors to the Chapel at (x:51,y:89)
            /// </summary>
            public static QuestFlagInfo LockChapelDoors { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1109, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Unlocks the double doors to the Chapel at (x:51,y:89)
            /// </summary>
            public static QuestFlagInfo UnlockChapelDoors { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1110, QuestId.Q70000001, StageInfo);
        }

        public static class TempleofPurification
        {
            private static StageInfo StageInfo = Stage.TempleofPurification;

            /// <summary>
            /// Locks the door to the water flow control room.
            /// </summary>
            public static QuestFlagInfo LockWaterFlowControlRoomDoor { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1111, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Unlocks the door to the water flow control room.
            /// </summary>
            public static QuestFlagInfo UnlockWaterFlowControlRoomDoor { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1112, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Used to turn on/off the waterfall gimmick in the dungeon.
            /// </summary>
            public static QuestFlagInfo WaterfallGimmick { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1317, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Closed Lever Door (Stone Door, middle)
            /// </summary>
            public static QuestFlagInfo CloseLeverDoor { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1671, QuestId.Q70000001, StageInfo);

            /// <summary>
            /// Open Lever Door  (Stone Door, middle)
            /// </summary>
            public static QuestFlagInfo OpenLeverDoor { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(1672, QuestId.Q70000001, StageInfo);
        }

        public static class ExpeditionGarrison
        {
            private static StageInfo StageInfo = Stage.ExpeditionGarrison;

            /// <summary>
            /// Unlocks the door to Bloodbane Isle Summit and opens the gate at (51,-28)
            /// </summary>
            public static QuestFlagInfo BloodbaneIsleSummit { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1489, QuestId.Q70021001);
        }

        public static class ElanWaterGrove
        {
            private static StageInfo StageInfo = Stage.ElanWaterGrove;

            /// <summary>
            /// Unlocks the Path to Morrow when set.
            /// </summary>
            public static QuestFlagInfo PathToMorrow { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1699, QuestId.Q70022001);
            public static QuestFlagInfo Unk0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3954, QuestId.Q70022001, StageInfo);
            public static QuestFlagInfo Unk1 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(4033, QuestId.Q70022001, StageInfo);
            public static QuestFlagInfo Unk2 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(4968, QuestId.Q70022001, StageInfo);

        }

        public static class FaranaPlains
        {
            private static StageInfo StageInfo = Stage.FaranaPlains0;

            /// <summary>
            /// Opens the gates around Dana when set.
            /// </summary>
            public static QuestFlagInfo DanaGate { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1698, QuestId.Q70022001);

            /// <summary>
            /// Unlocks the "Kingal Canyon Border Checkpoint" when set (from Dana to Glyndwr)
            /// </summary>
            public static QuestFlagInfo KingalCanyonBorderCheckpoint { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1703, QuestId.Q70022001);

            /// <summary>
            /// Unlocks the southern "Vegasa Corridor" entrance when set
            /// </summary>
            public static QuestFlagInfo VegasaCorridorSouth { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1997, QuestId.Q70023001);
        }

        public static class MorrowForest
        {
            private static StageInfo StageInfo = Stage.MorrowForest;

            /// <summary>
            /// Opens the Western Gate in Morfaul when set
            /// </summary>
            public static QuestFlagInfo MorfaulWestGate { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1999, QuestId.Q70022001);

            /// <summary>
            /// Unlocks the western "Vegasa Corridor" entrance when set
            /// Morrow Forest
            /// </summary>
            public static QuestFlagInfo VegasaCorridorWest { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2402, QuestId.Q70023001);
        }

        public static class MorfaulChiefsHome
        {
            private static StageInfo StageInfo = Stage.MorfaulChiefsHome;

            /// <summary>
            /// Placement of Geroid before clearing Season 2
            /// </summary>
            public static QuestFlagInfo Geroid0 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(4053, QuestId.Q70022001, StageInfo);

            /// <summary>
            /// Placement of Geroid after clearing Season 2
            /// </summary>
            public static QuestFlagInfo Geroid1 { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(4066, QuestId.Q70023001, StageInfo);
        }

        public static class KingalCanyon
        {
            private static StageInfo StageInfo = Stage.KingalCanyon;

            /// <summary>
            /// Opens the gates around Glyndwr when set
            /// </summary>
            public static QuestFlagInfo GlyndwrGates { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1998, QuestId.Q70022001);

            /// <summary>
            /// Unlocks "Shadolean Great Temple" when set (st0439).
            /// </summary>
            public static QuestFlagInfo ShadoleanGreatTemple { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(5184, QuestId.Q70034001);

            /// <summary>
            /// Unlocks the eastern "Vegasa Corridor" entrance when set
            /// Kingal Canyon
            /// </summary>
            public static QuestFlagInfo VegasaCorridorEast { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2400, QuestId.Q70023001);
        }

        public static class ManunVillage
        {
            private static StageInfo StageInfo = Stage.ManunVillage;

            /// <summary>
            /// Unlocks the quest board in Manun Village when set.
            /// </summary>
            public static QuestFlagInfo QuestBoard { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2245, QuestId.Q70023001);
        }

        public static class TowerOfIvanos
        {
            private static StageInfo StageInfo = Stage.TowerofIvanos;

            /// <summary>
            /// Unlocks the quest board in the Tower of Ivanos when set.
            /// </summary>
            public static QuestFlagInfo QuestBoard { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2246, QuestId.Q70023001);
        }

        public static class HollowOfBeginnings
        {
            /// <summary>
            /// Unlocks "Hollow Of Beginnings GatheringArea" in Stage.HollowofBeginnings0 when set.
            /// </summary>
            public static QuestFlagInfo HollowOfBeginningsGatheringArea { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2182, QuestId.Q70023001);

            /// <summary>
            /// Unlocks "Valtable Hall" in Stage.HollowofBeginnings0 when set
            /// </summary>
            public static QuestFlagInfo ValtableHall { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2183, QuestId.Q70023001);
        }

        public static class ValtableHall
        {
            /// <summary>
            /// Unlocks "Valtable Hall Upper Area" when set.
            /// </summary>
            public static QuestFlagInfo ValtableHallUpperArea { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2184, QuestId.Q70023001);
        }

        public static class RathniteFoothills
        {
            private static StageInfo StageInfo = Stage.RathniteFoothills;

            /// <summary>
            /// Enables NPCs and Shops in rathnite foothills Orc encampmet
            /// </summary>
            public static QuestFlagInfo OrcEncampmentNpcs { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(3283, QuestId.Q70030001, StageInfo);
        }

        public static class PiremothTravelersInn
        {
            private static StageInfo StageInfo = Stage.PiremothTravelersInn;

            /// <summary>
            /// Spawns the area master Endale when set.
            /// </summary>
            public static QuestFlagInfo Endale { get; private set; } = QuestFlagInfo.WorldManageLayoutFlag(5396, QuestId.Q70030001, StageInfo);
        }

        public static class NpcFunctions
        {
            /// <summary>
            /// Adds "Grand Mission" title and functionality to Travers, Logan, Hancock and Cornelia.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo GrandMission { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(657, QuestId.Q70000001);

            /// <summary>
            /// Adds "Dragon Force Augmentation" functionality to The White Dragon.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo DragonForceAugmentation { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(658, QuestId.Q70000001);

            /// <summary>
            /// Adds "Party Creation" to various NCPCs.
            /// </summary>
            public static QuestFlagInfo PartyCreation { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(659, QuestId.Q70000001);

            /// <summary>
            /// Adds "Area Information" to various NPCs.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo AreaInformation { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(660, QuestId.Q70000001);

            /// <summary>
            /// Adds "Clan Management" to Kibiza.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo ClanManagement { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(661, QuestId.Q70000001);

            /// <summary>
            /// Adds Crafting options to Sonia.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo CraftOfficer { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(662, QuestId.Q70000001);

            /// <summary>
            /// Adds "Vocation/Arts support" options to Archibald.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo VocationArts { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(663, QuestId.Q70000001);

            /// <summary>
            /// Adds "Pawn Contract" options to the "Second Pawn".
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo PawnContracts { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(765, QuestId.Q70000001);

            /// <summary>
            /// Adds "Lestania News" functionality to Seneka.
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo LestaniaNews { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(772, QuestId.Q70000001);

            /// <summary>
            /// Adds "Myrmidons Pledge" to The White Dragon
            /// Found in npc.nll.json
            /// </summary>
            public static QuestFlagInfo MyrmidonsPledge { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(787, QuestId.Q70000001);

            /// <summary>
            /// Adds "Vocation Emblem" option to Renton.
            /// </summary>
            public static QuestFlagInfo VocationEmblem { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2946, QuestId.Q70030001);

            /// <summary>
            /// Adds "Play Point Shop" option to Renton.
            /// </summary>
            public static QuestFlagInfo PlayPointShop { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2477, QuestId.Q70021001);

            /// <summary>
            /// Unlocks the Arisen's Room.
            /// </summary>
            public static QuestFlagInfo ArisensRoom { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1490, QuestId.Q70020001);

            /// <summary>
            /// Unlocks the NPC options "Custom Made Arms" and "Equipment Dissaembly" at the NPC Craig.
            /// </summary>
            public static QuestFlagInfo CustomMadeArmsAndEquipmentDissaembly { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(3592, QuestId.Q70030001);

            /// <summary>
            /// Unlocks the NPC option "Synthesis of Dragon Abilities" at the NPC Craig.
            /// </summary>
            public static QuestFlagInfo SynthesisOfDragonAbilities { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(5274, QuestId.Q70034001);

            /// <summary>
            /// Unlocks the NPC option "Dragon Armor Appraisal" at the NPC Craig.
            /// </summary>
            public static QuestFlagInfo DragonArmorAppraisal { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(5380, QuestId.Q70034001);

            /// <summary>
            /// Unlocks the NPC option "Area Information" for the NPC Bertrand in BBI
            /// </summary>
            public static QuestFlagInfo BloodbaneAreaInfo { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1479, QuestId.Q70020001);

            /// <summary>
            /// Unlocks the NPC option "Area Information" for the NPC Musel in Elan Water Grove (Protector's Retreat)
            /// </summary>
            public static QuestFlagInfo ElanWaterGroveAreaInfo { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1709, QuestId.Q70022001);

            /// <summary>
            /// Unlocks the NPC option "Area Information" for the NPC Arthfael in Morrow Forest (Moarfaul Centrum)
            /// </summary>
            public static QuestFlagInfo MorrowForestAreaInfo { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1710, QuestId.Q70022001);

            /// <summary>
            /// Unlocks the NPC option "Area Information" for the NPC Razanailt in Farna Plains (Dana Centrum)
            /// </summary>
            public static QuestFlagInfo FaranaPlainsAreaInfo { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1711, QuestId.Q70022001);

            /// <summary>
            /// Unlocks the NPC option "Area Information" for the NPC Ciaran in Kingal Canyon (Glyndwr Centrum)
            /// </summary>
            public static QuestFlagInfo KingalCanyonAreaInfo { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1712, QuestId.Q70022001);

            /// <summary>
            /// Adds the NPC option "Extreme Mission" for the NPC Seneka
            /// </summary>
            public static QuestFlagInfo SenekaExm { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(761, QuestId.Q70004001);

            /// <summary>
            /// Adds the NPC option "Extreme Mission" for the NPC Issac
            /// </summary>
            public static QuestFlagInfo IsaacExm { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(1524, QuestId.Q70020001);

            /// <summary>
            /// Adds the NPC option "Extreme Mission" for the NPC Doris
            /// </summary>
            public static QuestFlagInfo DorisExm { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(4410, QuestId.Q70032001);

            /// <summary>
            /// Adds the NPC option "Extreme Mission" for the NPC Endale
            /// @warning the QuestId might be wrong, it is not in the packet capture
            /// </summary>
            public static QuestFlagInfo EndaleExm { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2922, QuestId.Q70031001);

            /// <summary>
            /// Adds the NPC option "Extreme Mission" for the NPC Nayajiku
            /// </summary>
            public static QuestFlagInfo NayajikuExm { get; private set; } = QuestFlagInfo.WorldManageQuestFlag(2923, QuestId.Q70031001);
        }
    }
}
