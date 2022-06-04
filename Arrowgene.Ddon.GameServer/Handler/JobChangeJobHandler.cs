using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
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

            CDataCharacterJobData characterJobData = new CDataCharacterJobData();
            characterJobData.Job = packet.Structure.JobId;
            characterJobData.Exp = 100;
            characterJobData.JobPoint = 100;
            characterJobData.Lv = 100;
            characterJobData.Atk = 100;
            characterJobData.Def = 100;
            characterJobData.MAtk = 100;
            characterJobData.MDef = 100;
            characterJobData.Strength = 100;
            characterJobData.DownPower = 100;
            characterJobData.ShakePower = 100;
            characterJobData.StunPower = 100;
            characterJobData.Consitution = 100;
            characterJobData.Guts = 100;
            characterJobData.FireResist = 100;
            characterJobData.IceResist = 100;
            characterJobData.ThunderResist = 100;
            characterJobData.HolyResist = 100;
            characterJobData.DarkResist = 100;
            characterJobData.SpreadResist = 100;
            characterJobData.FreezeResist = 100;
            characterJobData.ShockResist = 100;
            characterJobData.AbsorbResist = 100;
            characterJobData.DarkElmResist = 100;
            characterJobData.PoisonResist = 100;
            characterJobData.SlowResist = 100;
            characterJobData.SleepResist = 100;
            characterJobData.StunResist = 100;
            characterJobData.WetResist = 100;
            characterJobData.OilResist = 100;
            characterJobData.SealResist = 100;
            characterJobData.CurseResist = 100;
            characterJobData.SoftResist = 100;
            characterJobData.StoneResist = 100;
            characterJobData.GoldResist = 100;
            characterJobData.FireReduceResist = 100;
            characterJobData.IceReduceResist = 100;
            characterJobData.ThunderReduceResist = 100;
            characterJobData.HolyReduceResist = 100;
            characterJobData.DarkReduceResist = 100;
            characterJobData.AtkDownResist = 100;
            characterJobData.DefDownResist = 100;
            characterJobData.MAtkDownResist = 100;
            characterJobData.MDefDownResist = 100;

            CDataJobChangeInfo requestedJobChangeInfo = jobChangeList.JobChangeInfo.Where(x => x.JobId == packet.Structure.JobId).FirstOrDefault();
            CDataJobPlayPoint requestedJobPlayPoint = jobChangeList.PlayPointList.Where(x => x.Job == packet.Structure.JobId).FirstOrDefault();

            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            if(requestedJobChangeInfo == null || requestedJobPlayPoint == null) {                
                response.CharacterJobData.Job = packet.Structure.JobId;

                notice.CharacterId = client.Character.Id;
                notice.CharacterJobData.Job = packet.Structure.JobId;
            } else {
                List<CDataCharacterEquipInfo> characterEquipList = requestedJobChangeInfo.EquipItemList.Select(x => new CDataCharacterEquipInfo(x)).ToList();
                List<CDataSetAcquirementParam> setAcquirementParamList = getCurrentSetSkillList.SetCustomSkillList.Where(x => x.Job == packet.Structure.JobId).ToList();
                List<CDataSetAcquirementParam> setAbilityParamList = getCurrentSetSkillList.SetAbilityList.Where(x => x.Job == packet.Structure.JobId).ToList();
                List<CDataLearnNormalSkillParam> learnNormalSkillParamList = getCurrentSetSkillList.NormalSkillList.Where(x => x.Job == packet.Structure.JobId).Select(x => new CDataLearnNormalSkillParam(x)).ToList();

                response.CharacterJobData = characterJobData;
                response.CharacterEquipList = characterEquipList;
                response.SetAcquirementParamList = setAcquirementParamList;
                response.SetAbilityParamList = setAbilityParamList;
                response.LearnNormalSkillParamList = learnNormalSkillParamList;
                // response.EquipJobItemList?
                response.PlayPointData = requestedJobPlayPoint.PlayPoint;
                // response.Unk0?
                
                notice.CharacterId = client.Character.Id;
                notice.EquipItemInfo = requestedJobChangeInfo.EquipItemList;
                notice.CharacterJobData = characterJobData;
                notice.LearnNormalSkillParamList = learnNormalSkillParamList;
                notice.SetAbilityParamList = setAbilityParamList;
                notice.SetAcquirementParamList = setAcquirementParamList;
                // notice.EquipJobItemList?
                // notice.Unk0?
            }

            client.Send(response);
            foreach(GameClient otherClient in Server.Clients)
            {
                if(otherClient.Character.Id != client.Character.Id)
                {
                    otherClient.Send(notice);
                }
            }
        }
    }
}