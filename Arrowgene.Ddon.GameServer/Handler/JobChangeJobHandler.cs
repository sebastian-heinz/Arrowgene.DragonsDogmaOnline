using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangeJobHandler : StructurePacketHandler<GameClient, C2SJobChangeJobReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangeJobHandler));


        public JobChangeJobHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobChangeJobReq> packet)
        {
            S2CJobGetJobChangeListRes jobChangeList = EntitySerializer.Get<S2CJobGetJobChangeListRes>().Read(InGameDump.data_Dump_52);
            S2CSkillGetCurrentSetSkillListRes getCurrentSetSkillList = EntitySerializer.Get<S2CSkillGetCurrentSetSkillListRes>().Read(InGameDump.data_Dump_54);

            CDataJobChangeInfo requestedJobChangeInfo = null;
            foreach(CDataJobChangeInfo jobChangeInfo in jobChangeList.JobChangeInfo)
            {
                if(jobChangeInfo.JobId == packet.Structure.JobId)
                {
                    requestedJobChangeInfo = jobChangeInfo;
                    break;
                }
            }

            CDataJobPlayPoint requestedJobPlayPoint = null;
            foreach(CDataJobPlayPoint jobPlayPoint in jobChangeList.PlayPointList)
            {
                if(jobPlayPoint.Job == packet.Structure.JobId)
                {
                    requestedJobPlayPoint = jobPlayPoint;
                    break;
                }
            }

            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            response.CharacterEquipList = new List<CDataCharacterEquipInfo>(requestedJobChangeInfo.EquipItemList.Select(x => new CDataCharacterEquipInfo(x)));
            response.PlayPointDataList = new List<CDataPlayPointData> { requestedJobPlayPoint.PlayPoint };
            response.LearnNormalSkillParamList = new List<CDataLearnNormalSkillParam>(getCurrentSetSkillList.NormalSkillList.Where(x => x.Job == packet.Structure.JobId).Select(x => new CDataLearnNormalSkillParam(x)));
            response.SetAbilityParamList = new List<CDataSetAcquirementParam>(getCurrentSetSkillList.SetAbilityList.Where(x => x.Job == packet.Structure.JobId));
            response.SetAcquirementParamList = new List<CDataSetAcquirementParam>(getCurrentSetSkillList.SetCustomSkillList.Where(x => x.Job == packet.Structure.JobId));
            client.Send(response);

            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            notice.CharacterId = client.Character.Id;
            notice.CharacterJobData.Job = packet.Structure.JobId;
            notice.CharacterJobData.Lv = 50;
            notice.EquipItemInfo = requestedJobChangeInfo.EquipItemList;
            notice.LearnNormalSkillParamList = response.LearnNormalSkillParamList;
            notice.SetAbilityParamList = response.SetAbilityParamList;
            notice.SetAcquierementParamList = response.SetAcquirementParamList;
            client.Send(notice);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNotice = new S2CItemUpdateCharacterItemNtc();
            client.Send(updateCharacterItemNotice);
        }
    }
}