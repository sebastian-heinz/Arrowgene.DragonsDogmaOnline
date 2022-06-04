using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetCurrentSetSkillListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetCurrentSetSkillListHandler));

        public SkillGetCurrentSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            //client.Send(InGameDump.Dump_54);

            S2CSkillGetCurrentSetSkillListRes res = new S2CSkillGetCurrentSetSkillListRes();
            res.NormalSkillList = new List<CDataNormalSkillParam>() {
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
            res.SetCustomSkillList = client.Character.CustomSkills; // TODO: Filter so only the current job skills are sent?
            res.SetAbilityList = new List<CDataSetAcquirementParam>() {
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
            client.Send(res);
        }
    }
}
