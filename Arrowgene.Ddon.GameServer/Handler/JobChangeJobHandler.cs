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
            client.Character.Job = packet.Structure.JobId;

            Server.Database.UpdateCharacterBaseInfo(client.Character);

            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            notice.CharacterId = client.Character.Id;
            notice.CharacterJobData = client.Character.ActiveCharacterJobData;
            notice.EquipItemInfo = client.Character.CharacterEquipViewDataListDictionary[client.Character.Job]
                .Union(client.Character.CharacterEquipDataListDictionary[client.Character.Job])
                .SelectMany(x => x.Equips)
                .ToList();
            notice.SetAcquirementParamList = client.Character.CustomSkills
                .Where(x => x.Job == packet.Structure.JobId)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            notice.SetAbilityParamList = client.Character.Abilities
                .Where(x => x.EquippedToJob == packet.Structure.JobId)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            notice.LearnNormalSkillParamList = client.Character.NormalSkills
                .Select(x => new CDataLearnNormalSkillParam(x))
                .ToList();
            notice.EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job];

            foreach(GameClient otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(notice); // This does the change itself (it does work)
            }

            // TODO: Replace pcap data with DB data
            S2CJobGetJobChangeListRes jobChangeList = EntitySerializer.Get<S2CJobGetJobChangeListRes>().Read(InGameDump.data_Dump_52);
            CDataJobPlayPoint requestedJobPlayPoint = jobChangeList.PlayPointList.Where(x => x.Job == packet.Structure.JobId).FirstOrDefault();

            S2CEquipGetCharacterEquipListRes getCharacterEquipListRes = EntitySerializer.Get<S2CEquipGetCharacterEquipListRes>().Read(InGameDump.data_Dump_48);

            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            response.CharacterJobData = client.Character.ActiveCharacterJobData;
            response.CharacterEquipList = getCharacterEquipListRes.CharacterEquipList;
            notice.SetAcquirementParamList = client.Character.CustomSkills
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            notice.SetAbilityParamList = client.Character.Abilities
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            notice.LearnNormalSkillParamList = client.Character.NormalSkills
                .Select(x => new CDataLearnNormalSkillParam(x))
                .ToList();
            response.EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job];
            response.PlayPointData = requestedJobPlayPoint.PlayPoint;
            response.Unk0.Unk0 = (byte) packet.Structure.JobId;
            response.Unk0.Unk1 = client.Character.CharacterItemSlotInfoList;
            
            // I don't know whats the purpose of this carrying so much data since the job change itself is done by the NTC
            client.Send(response);
        }
    }
}