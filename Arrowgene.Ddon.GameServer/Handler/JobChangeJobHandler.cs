using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;

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

            Server.Database.UpdateCharacterCommonBaseInfo(client.Character);

            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            notice.CharacterId = client.Character.CharacterId;
            notice.CharacterJobData = client.Character.ActiveCharacterJobData;
            notice.EquipItemInfo = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Performance)
                .Union(client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Visual))
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

            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            response.CharacterJobData = client.Character.ActiveCharacterJobData;
            // TODO: Figure out if it should send all equips or just the ones for the current job
            response.CharacterEquipList = client.Character.Equipment.getEquipmentAsCDataCharacterEquipInfo(client.Character.Job, EquipType.Performance)
                .Union(client.Character.Equipment.getEquipmentAsCDataCharacterEquipInfo(client.Character.Job, EquipType.Visual))
                .ToList();
            response.SetAcquirementParamList = client.Character.CustomSkills
                .Where(x => x.Job == packet.Structure.JobId)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            response.SetAbilityParamList = client.Character.Abilities
                .Where(x => x.EquippedToJob == packet.Structure.JobId)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            response.LearnNormalSkillParamList = client.Character.NormalSkills
                .Select(x => new CDataLearnNormalSkillParam(x))
                .ToList();
            response.EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job];
            response.PlayPointData = client.Character.PlayPointList
                .Where(x => x.Job == packet.Structure.JobId)
                .Select(x => x.PlayPoint)
                .FirstOrDefault(new CDataPlayPointData());
            response.Unk0.Unk0 = (byte) packet.Structure.JobId;
            response.Unk0.Unk1 = client.Character.Storage.getAllStoragesAsCDataCharacterItemSlotInfoList();

            foreach(GameClient otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(notice);
            }

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0x28;
            // TODO: Move previous job equipment to storage box, and move new job equipment from storage box
            client.Send(updateCharacterItemNtc);

            // I don't know whats the purpose of this carrying so much data since the job change itself is done by the NTC
            client.Send(response);
        }
    }
}