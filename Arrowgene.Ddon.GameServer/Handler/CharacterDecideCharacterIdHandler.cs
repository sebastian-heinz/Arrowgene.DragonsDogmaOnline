using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterDecideCharacterIdHandler : GameRequestPacketQueueHandler<C2SCharacterDecideCharacterIdReq, S2CCharacterDecideCharacterIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterDecideCharacterIdHandler));

        public CharacterDecideCharacterIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SCharacterDecideCharacterIdReq request)
        {
            PacketQueue packetQueue = new PacketQueue();
            S2CCharacterDecideCharacterIdRes res = new S2CCharacterDecideCharacterIdRes();
            res.CharacterId = client.Character.CharacterId;
            res.CharacterInfo = new CDataCharacterInfo(client.Character);
            res.BinaryData = client.Character.BinaryData;
            client.Enqueue(res, packetQueue);

            // Unlocks menu options such as inventory, warping, etc.
            // S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNtc>().Read(GameFull.data_Dump_20);
            S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = new S2CCharacterContentsReleaseElementNtc()
            {
                CharacterReleaseElements = client.Character.GetReleasedContent()
            };
            client.Enqueue(contentsReleaseElementNotice, packetQueue);

            ulong now = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (var (id, course) in Server.AssetRepository.GPCourseInfoAsset.ValidCourses)
            {
                if (now >= course.StartTime && now <= course.EndTime)
                {
                    client.Enqueue(new S2CGPCourseStartNtc()
                    {
                        CourseID = course.Id,
                        ExpiryTimestamp = course.EndTime
                    }, packetQueue);
                }
            }

            return packetQueue;
        }

        private static List<CDataCharacterReleaseElement> gContentsReleaseIds = new List<CDataCharacterReleaseElement>()
        {
            // NOTE: Items currently reflect what was in the original static packet
            // NOTE: Look at ContentsRelease enum for more elements not originally
            // NOTE: in the packet sent
            new CDataCharacterReleaseElement(ContentsRelease.SpecialSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.RiftTeleport),
            new CDataCharacterReleaseElement(ContentsRelease.SorcererVocationEmblem),
            new CDataCharacterReleaseElement(ContentsRelease.PartyPlayers),
            new CDataCharacterReleaseElement(ContentsRelease.PawnAndPartyPlay),
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.GrandMissions),
            new CDataCharacterReleaseElement(ContentsRelease.ExtremeMissions),
            new CDataCharacterReleaseElement(ContentsRelease.RathniteFoothillsWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.DressEquipment),
            new CDataCharacterReleaseElement(ContentsRelease.FeryanaWildernessWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.LestaniaNews),
            //new CDataCharacterReleaseElement(ContentsRelease.MandragoraBreeding),
            new CDataCharacterReleaseElement(ContentsRelease.JobTrainingLog),
            new CDataCharacterReleaseElement(ContentsRelease.YourRoomsTerrace),
            new CDataCharacterReleaseElement(ContentsRelease.DragonForceAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.Craft),
            new CDataCharacterReleaseElement(ContentsRelease.HighScepterJobTraining),
            new CDataCharacterReleaseElement(ContentsRelease.Riftstone),
            new CDataCharacterReleaseElement(ContentsRelease.MegadosysPlateauWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.QuestBoard),
            new CDataCharacterReleaseElement(ContentsRelease.ChangetoHighScepter),
            new CDataCharacterReleaseElement(ContentsRelease.AreaMaster),
            new CDataCharacterReleaseElement(ContentsRelease.PawnsNewSpecialSkills),
            new CDataCharacterReleaseElement(ContentsRelease.AreaMastersWorldQuestInfo),
            new CDataCharacterReleaseElement(ContentsRelease.ExtremeMission1),
            new CDataCharacterReleaseElement(ContentsRelease.ChangeVocations),
            new CDataCharacterReleaseElement(ContentsRelease.ExtremeMission2),
            new CDataCharacterReleaseElement(ContentsRelease.CreateandJoinClans),
            new CDataCharacterReleaseElement(ContentsRelease.ExtremeMission3),
            new CDataCharacterReleaseElement(ContentsRelease.MainMenu),
            new CDataCharacterReleaseElement(ContentsRelease.AppraisalExchangeofDragonArmor),
            new CDataCharacterReleaseElement(ContentsRelease.MyrmidonsPledge),
            new CDataCharacterReleaseElement(ContentsRelease.AdventureBroker), // This was called "PartyMenu" or "冒険仲介係"
            new CDataCharacterReleaseElement(ContentsRelease.MatchingProfile),
            new CDataCharacterReleaseElement(ContentsRelease.QuickParty), // Also unlocks board for EM
            new CDataCharacterReleaseElement(ContentsRelease.OrbEnemy),
            new CDataCharacterReleaseElement(ContentsRelease.WarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.FighterWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.HunterWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.PriestWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.ShieldSageWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.SeekerWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.SorcererWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.WarriorWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.ElementArcherWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.AlchemistWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.SpiritLancerWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.HighScepterWarSkillAugmentation),
            new CDataCharacterReleaseElement(ContentsRelease.BloodbaneIsleWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.ChangeBgmIsland),
            new CDataCharacterReleaseElement(ContentsRelease.MyRoom),
            new CDataCharacterReleaseElement(ContentsRelease.Baggage),
            new CDataCharacterReleaseElement(ContentsRelease.MorrowForestWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.ElanWaterGroveWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.FaranaPlainsWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.KingalCanyonWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.PlayPoints),
            new CDataCharacterReleaseElement(ContentsRelease.AreaInvestigation), // This was labeled as "TreasurePoint"
            new CDataCharacterReleaseElement(ContentsRelease.PawnTacticalTraining),
            new CDataCharacterReleaseElement(ContentsRelease.ShieldSageVocationEmblem),
            new CDataCharacterReleaseElement(ContentsRelease.UrtecaMountainsWorldQuests),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown85), // Was in packet capture but no mention anywhere
            new CDataCharacterReleaseElement(ContentsRelease.WildHunt),
            new CDataCharacterReleaseElement(ContentsRelease.DismantlingofDragonArms),
            new CDataCharacterReleaseElement(ContentsRelease.SynthesisofDragonAbilities),
            new CDataCharacterReleaseElement(ContentsRelease.ExtremeMission0),
        };
    }
}
