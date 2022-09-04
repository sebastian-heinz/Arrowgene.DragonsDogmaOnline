using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterDecideCharacterIdHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterDecideCharacterIdHandler));


        public CharacterDecideCharacterIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // TODO: Move this to DB
            client.Character.NormalSkills = new List<CDataNormalSkillParam>() {
                    new CDataNormalSkillParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    SkillNo = 1,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    SkillNo = 2,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    SkillNo = 3,
                    Index = 0,
                    PreSkillNo = 0
                }
            };

            // Ditto
            client.Character.Abilities = new List<CDataSetAcquirementParam>() {
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab1Jb,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab1Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab1Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab2Jb,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab2Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab2Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab3Jb,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab3Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab3Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab4Jb,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab4Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab4Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab5Jb,
                    Type = 0,
                    SlotNo = 5,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab5Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab5Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab6Jb,
                    Type = 0,
                    SlotNo = 6,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab6Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab6Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab7Jb,
                    Type = 0,
                    SlotNo = 7,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab7Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab7Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab8Jb,
                    Type = 0,
                    SlotNo = 8,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab8Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab8Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab9Jb,
                    Type = 0,
                    SlotNo = 9,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab9Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab9Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab10Jb,
                    Type = 0,
                    SlotNo = 10,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab10Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab10Lv
                }
            };

            S2CCharacterDecideCharacterIdRes res = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(GameDump.data_Dump_13);
            res.CharacterId = client.Character.Id;
            res.CharacterInfo = client.Character.CharacterInfo;
            client.Send(res);
            
            // Unlocks menu options such as inventory, warping, etc.
            S2CCharacterContentsReleaseElementNotice contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNotice>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
