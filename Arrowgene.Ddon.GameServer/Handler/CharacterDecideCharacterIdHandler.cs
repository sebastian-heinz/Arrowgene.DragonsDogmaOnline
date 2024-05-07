using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterDecideCharacterIdHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterDecideCharacterIdHandler));

        private readonly AssetRepository _AssetRepo;

        public CharacterDecideCharacterIdHandler(DdonGameServer server) : base(server)
        {
            _AssetRepo = server.AssetRepository;
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CCharacterDecideCharacterIdRes pcap = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(GameDump.data_Dump_13);
            S2CCharacterDecideCharacterIdRes res = new S2CCharacterDecideCharacterIdRes();
            res.CharacterId = client.Character.CharacterId;
            res.CharacterInfo = new CDataCharacterInfo(client.Character);
            res.Unk0 = pcap.Unk0; // Removing this makes tons of tutorials pop up
            client.Send(res);

            // Unlocks menu options such as inventory, warping, etc.
            // S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNtc>().Read(GameFull.data_Dump_20);
            S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = new S2CCharacterContentsReleaseElementNtc()
            {
                CharacterReleaseElements = gContentsReleaseIds
            };
            client.Send(contentsReleaseElementNotice);

            foreach (var ValidCourse in _AssetRepo.GPCourseInfoAsset.ValidCourses)
            {
                client.Send(new S2CGPCourseStartNtc()
                {
                    CourseID = ValidCourse.Value.Id,
                    ExpiryTimestamp = ValidCourse.Value.EndTime
                });
            }
        }

        private static List<CDataCharacterReleaseElement> gContentsReleaseIds = new List<CDataCharacterReleaseElement>()
        {
            // NOTE: Items currently reflect what was in the original static packet
            // NOTE: Look at ContentsRelease enum for more elements not originally
            // NOTE: in the packet sent
            new CDataCharacterReleaseElement(ContentsRelease.Unknown73),
            new CDataCharacterReleaseElement(ContentsRelease.RimWarp),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown65),
            new CDataCharacterReleaseElement(ContentsRelease.PartyPlayWithPlayer),
            new CDataCharacterReleaseElement(ContentsRelease.PartyPlayWithPawn),
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuest),
            new CDataCharacterReleaseElement(ContentsRelease.GrandMission),
            new CDataCharacterReleaseElement(ContentsRelease.ExtremeMission),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown70),
            new CDataCharacterReleaseElement(ContentsRelease.VisualEquip),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown71),
            new CDataCharacterReleaseElement(ContentsRelease.LestaniaNews),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown72),
            new CDataCharacterReleaseElement(ContentsRelease.MasterHundbook),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown74),
            new CDataCharacterReleaseElement(ContentsRelease.BloodOrbTrade),
            new CDataCharacterReleaseElement(ContentsRelease.PawnCraft),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown76),
            new CDataCharacterReleaseElement(ContentsRelease.RimStone),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown77),
            new CDataCharacterReleaseElement(ContentsRelease.QuestBoard),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown79),
            new CDataCharacterReleaseElement(ContentsRelease.AreaMaster),
            new CDataCharacterReleaseElement(ContentsRelease.SpecialSkillPawnTraining),
            new CDataCharacterReleaseElement(ContentsRelease.QuestHint),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown90),
            new CDataCharacterReleaseElement(ContentsRelease.JobChange),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown91),
            new CDataCharacterReleaseElement(ContentsRelease.Clan),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown92),
            new CDataCharacterReleaseElement(ContentsRelease.MainMenu),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown93),
            new CDataCharacterReleaseElement(ContentsRelease.CreateSecondPawn),
            new CDataCharacterReleaseElement(ContentsRelease.PartyMenu),
            new CDataCharacterReleaseElement(ContentsRelease.MainMenuArisenProfile),
            new CDataCharacterReleaseElement(ContentsRelease.MainMenuQuickParty),
            new CDataCharacterReleaseElement(ContentsRelease.OrbEnemy),
            new CDataCharacterReleaseElement(ContentsRelease.JobOrbTree), // Skill Augmentation option at white dragon
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeFighter),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeHunter),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreePriest),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeShieldSage),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeSeeker),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeSorcerer),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeWarrior),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeElementArcher),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbTreeAlchemist),
                new CDataCharacterReleaseElement(ContentsRelease.JobOrbtreeSpiritLancer),
                // Is there an element for the last job?
            // World Quest Progress in Bloodbane Isle?
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuestKOTOU), 
            new CDataCharacterReleaseElement(ContentsRelease.ChangeBgmIsland),
            new CDataCharacterReleaseElement(ContentsRelease.MyRoom),
            new CDataCharacterReleaseElement(ContentsRelease.Baggage),
            // World quest in the "deep woods"?
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuestSINSYOKUMORI),
            // World quest in the other "deep woods"?
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuestKIREIMORI),
            // World quest progress in "Hidell Plains"?
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuestHEIGEN),
            new CDataCharacterReleaseElement(ContentsRelease.WorldQuestKEIKOKU),
            new CDataCharacterReleaseElement(ContentsRelease.PlayPoint),
            new CDataCharacterReleaseElement(ContentsRelease.TreasurePoint),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown59),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown63),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown84),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown85),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown86),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown87),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown88),
            new CDataCharacterReleaseElement(ContentsRelease.Unknown89),
        };
    }
}
