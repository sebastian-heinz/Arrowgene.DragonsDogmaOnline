using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

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
            
            // TODO: Send these notices to all players
            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            notice.CharacterId = client.Character.Id;
            notice.EquipItemInfo = requestedJobChangeInfo.EquipItemList;
            notice.CharacterJobData.Job = packet.Structure.JobId;
            notice.CharacterJobData.Lv = 50;
            notice.LearnNormalSkillParamList = new List<CDataLearnNormalSkillParam>(getCurrentSetSkillList.NormalSkillList.Where(x => x.Job == packet.Structure.JobId).Select(x => new CDataLearnNormalSkillParam(x)));
            notice.SetAbilityParamList = new List<CDataSetAcquirementParam>(getCurrentSetSkillList.SetAbilityList.Where(x => x.Job == packet.Structure.JobId));
            notice.SetAcquirementParamList = new List<CDataSetAcquirementParam>(getCurrentSetSkillList.SetCustomSkillList.Where(x => x.Job == packet.Structure.JobId));
            client.Send(notice);

            S2CChangeCharacterEquipLobbyNotice changeCharacterEquipLobbyNotice = new S2CChangeCharacterEquipLobbyNotice();
            changeCharacterEquipLobbyNotice.CharacterId = client.Character.Id;
            changeCharacterEquipLobbyNotice.Job = packet.Structure.JobId;
            changeCharacterEquipLobbyNotice.EquipItemList = requestedJobChangeInfo.EquipItemList;

            S2CItemUpdateCharacterItemNtc updateCharacterItemNotice = new S2CItemUpdateCharacterItemNtc();
            client.Send(updateCharacterItemNotice);

            IBuffer packet_S2C_JOB_33_20_16_NTC_buffer = new StreamBuffer();
            packet_S2C_JOB_33_20_16_NTC_buffer.WriteByte(packet.Structure.JobId);
            packet_S2C_JOB_33_20_16_NTC_buffer.WriteUInt32(0x24, Endianness.Big);
            packet_S2C_JOB_33_20_16_NTC_buffer.WriteUInt32(0, Endianness.Big);
            packet_S2C_JOB_33_20_16_NTC_buffer.WriteUInt32(0x333, Endianness.Big);
            packet_S2C_JOB_33_20_16_NTC_buffer.WriteByte(0x4);
            Packet packet_S2C_JOB_33_20_16_NTC = new Packet(PacketId.S2C_JOB_33_11_16_NTC, packet_S2C_JOB_33_20_16_NTC_buffer.GetAllBytes());
            client.Send(packet_S2C_JOB_33_20_16_NTC);

            client.Send(GameFull.Dump_20);

            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            response.CharacterJobData = response.CharacterJobData;
            response.LearnNormalSkillParamList = response.LearnNormalSkillParamList;
            response.SetAbilityParamList = response.SetAbilityParamList;
            response.SetAcquirementParamList = response.SetAcquirementParamList;
            response.CharacterEquipList = new List<CDataCharacterEquipInfo>(requestedJobChangeInfo.EquipItemList.Select(x => new CDataCharacterEquipInfo(x)));
            response.PlayPointDataList = new List<CDataPlayPointData> { requestedJobPlayPoint.PlayPoint };
            client.Send(response);
        }
    }
}