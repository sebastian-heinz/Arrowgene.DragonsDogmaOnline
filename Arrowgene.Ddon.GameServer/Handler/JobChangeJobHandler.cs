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
using Arrowgene.Buffers;

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
            CDataCharacterJobData characterJobData = new CDataCharacterJobData();
            characterJobData.Job = packet.Structure.JobId;
            characterJobData.Exp = client.Character.ActiveCharacterJobData.Exp;
            characterJobData.JobPoint = client.Character.ActiveCharacterJobData.JobPoint;
            characterJobData.Lv = client.Character.ActiveCharacterJobData.Lv;
            characterJobData.Atk = client.Character.ActiveCharacterJobData.Atk;
            characterJobData.Def = client.Character.ActiveCharacterJobData.Def;
            characterJobData.MAtk = client.Character.ActiveCharacterJobData.MAtk;
            characterJobData.MDef = client.Character.ActiveCharacterJobData.MDef;
            characterJobData.Strength = client.Character.ActiveCharacterJobData.Strength;
            characterJobData.DownPower = client.Character.ActiveCharacterJobData.DownPower;
            characterJobData.ShakePower = client.Character.ActiveCharacterJobData.ShakePower;
            characterJobData.StunPower = client.Character.ActiveCharacterJobData.StunPower;
            characterJobData.Consitution = client.Character.ActiveCharacterJobData.Consitution;
            characterJobData.Guts = client.Character.ActiveCharacterJobData.Guts;
            characterJobData.FireResist = client.Character.ActiveCharacterJobData.FireResist;
            characterJobData.IceResist = client.Character.ActiveCharacterJobData.IceResist;
            characterJobData.ThunderResist = client.Character.ActiveCharacterJobData.ThunderResist;
            characterJobData.HolyResist = client.Character.ActiveCharacterJobData.HolyResist;
            characterJobData.DarkResist = client.Character.ActiveCharacterJobData.DarkResist;
            characterJobData.SpreadResist = client.Character.ActiveCharacterJobData.SpreadResist;
            characterJobData.FreezeResist = client.Character.ActiveCharacterJobData.FreezeResist;
            characterJobData.ShockResist = client.Character.ActiveCharacterJobData.ShockResist;
            characterJobData.AbsorbResist = client.Character.ActiveCharacterJobData.AbsorbResist;
            characterJobData.DarkElmResist = client.Character.ActiveCharacterJobData.DarkElmResist;
            characterJobData.PoisonResist = client.Character.ActiveCharacterJobData.PoisonResist;
            characterJobData.SlowResist = client.Character.ActiveCharacterJobData.SlowResist;
            characterJobData.SleepResist = client.Character.ActiveCharacterJobData.SleepResist;
            characterJobData.StunResist = client.Character.ActiveCharacterJobData.StunResist;
            characterJobData.WetResist = client.Character.ActiveCharacterJobData.WetResist;
            characterJobData.OilResist = client.Character.ActiveCharacterJobData.OilResist;
            characterJobData.SealResist = client.Character.ActiveCharacterJobData.SealResist;
            characterJobData.CurseResist = client.Character.ActiveCharacterJobData.CurseResist;
            characterJobData.SoftResist = client.Character.ActiveCharacterJobData.SoftResist;
            characterJobData.StoneResist = client.Character.ActiveCharacterJobData.StoneResist;
            characterJobData.GoldResist = client.Character.ActiveCharacterJobData.GoldResist;
            characterJobData.FireReduceResist = client.Character.ActiveCharacterJobData.FireReduceResist;
            characterJobData.IceReduceResist = client.Character.ActiveCharacterJobData.IceReduceResist;
            characterJobData.ThunderReduceResist = client.Character.ActiveCharacterJobData.ThunderReduceResist;
            characterJobData.HolyReduceResist = client.Character.ActiveCharacterJobData.HolyReduceResist;
            characterJobData.DarkReduceResist = client.Character.ActiveCharacterJobData.DarkReduceResist;
            characterJobData.AtkDownResist = client.Character.ActiveCharacterJobData.AtkDownResist;
            characterJobData.DefDownResist = client.Character.ActiveCharacterJobData.DefDownResist;
            characterJobData.MAtkDownResist = client.Character.ActiveCharacterJobData.MAtkDownResist;
            characterJobData.MDefDownResist = client.Character.ActiveCharacterJobData.MDefDownResist;

            //client.Character.CharacterInfo.CharacterJobDataList.Add(characterJobData);
            //client.Character.CharacterInfo.Job = packet.Structure.JobId;

            // TODO: Replace pcap data with DB data
            S2CJobGetJobChangeListRes jobChangeList = EntitySerializer.Get<S2CJobGetJobChangeListRes>().Read(InGameDump.data_Dump_52);
            S2CEquipGetCharacterEquipListRes getCharacterEquipListRes = EntitySerializer.Get<S2CEquipGetCharacterEquipListRes>().Read(InGameDump.data_Dump_48);

            CDataJobPlayPoint requestedJobPlayPoint = jobChangeList.PlayPointList.Where(x => x.Job == packet.Structure.JobId).FirstOrDefault();
            CDataJobChangeInfo requestedJobChangeInfo = jobChangeList.JobChangeInfo.Where(x => x.JobId == packet.Structure.JobId).FirstOrDefault();

            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            response.CharacterJobData = characterJobData;
            response.CharacterEquipList = getCharacterEquipListRes.CharacterEquipList;
            response.SetAcquirementParamList = new List<CDataSetAcquirementParam>();
            response.SetAbilityParamList = new List<CDataSetAcquirementParam>();
            response.LearnNormalSkillParamList = new List<CDataLearnNormalSkillParam>();
            response.EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job];
            response.PlayPointData = requestedJobPlayPoint.PlayPoint;
            response.Unk0.Unk0 = (byte) packet.Structure.JobId;
            response.Unk0.Unk1 = client.Character.CharacterItemSlotInfoList;
            
            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            notice.CharacterId = client.Character.Id;
            notice.CharacterJobData = characterJobData;
            notice.EquipItemInfo = requestedJobChangeInfo.EquipItemList;
            notice.SetAcquirementParamList = new List<CDataSetAcquirementParam>();
            notice.SetAbilityParamList = new List<CDataSetAcquirementParam>();
            notice.LearnNormalSkillParamList = new List<CDataLearnNormalSkillParam>();
            notice.EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job];
            notice.Unk0.Unk0 = (byte) packet.Structure.JobId;
            notice.Unk0.Unk1 = client.Character.CharacterItemSlotInfoList;
            
            // I don't know whats the purpose of this carrying so much data since the job change itself is done by the NTC
            client.Send(response);

            foreach(GameClient otherClient in Server.Clients)
            {
                otherClient.Send(notice); // This does the change itself (it does work)
            }
        }
    }
}