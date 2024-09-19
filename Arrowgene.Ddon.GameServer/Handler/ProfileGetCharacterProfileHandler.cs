using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileGetCharacterProfileHandler : GameRequestPacketHandler<C2SProfileGetCharacterProfileReq, S2CProfileGetCharacterProfileRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileGetCharacterProfileHandler));

        public ProfileGetCharacterProfileHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CProfileGetCharacterProfileRes Handle(GameClient client, C2SProfileGetCharacterProfileReq request)
        {
            GameClient targetClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId);

            if (targetClient is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_DATA_INVALID_CHARACTER_ID);
            }

            S2CCharacterGetCharacterStatusNtc ntc = new S2CCharacterGetCharacterStatusNtc();
            ntc.CharacterId = targetClient.Character.CharacterId;
            ntc.StatusInfo = targetClient.Character.StatusInfo;
            ntc.JobParam = targetClient.Character.ActiveCharacterJobData;
            GameStructure.CDataCharacterLevelParam(ntc.CharacterParam, client.Character);
            ntc.EditInfo = targetClient.Character.EditInfo;
            ntc.EquipDataList = targetClient.Character.Equipment.AsCDataEquipItemInfo(EquipType.Performance);
            ntc.VisualEquipDataList = targetClient.Character.Equipment.AsCDataEquipItemInfo(EquipType.Visual);
            ntc.EquipJobItemList = targetClient.Character.EquipmentTemplate.JobItemsAsCDataEquipJobItem(targetClient.Character.Job);
            ntc.HideHead = targetClient.Character.HideEquipHead;
            ntc.HideLantern = targetClient.Character.HideEquipLantern;
            ntc.JewelryNum = targetClient.Character.ExtendedParams.JewelrySlot;

            client.Send(ntc);

            S2CProfileGetCharacterProfileRes res = new S2CProfileGetCharacterProfileRes();
            res.CharacterId = targetClient.Character.CharacterId;
            GameStructure.CDataCharacterName(res.CharacterName, targetClient.Character);
            res.JobId = targetClient.Character.Job;
            res.JobLevel = (byte)targetClient.Character.ActiveCharacterJobData.Lv;
            res.ClanParam = Server.ClanManager.GetClan(targetClient.Character.ClanId);
            // TODO: ClanParam
            // TODO: ClanMemberRank
            res.JobLevelList = targetClient.Character.CharacterJobDataList.Select(jobData => new CDataJobBaseInfo()
            {
                Job = jobData.Job,
                Level = (byte)jobData.Lv
            }).ToList();
            res.MatchingProfile = targetClient.Character.MatchingProfile;
            res.ArisenProfile = targetClient.Character.ArisenProfile;
            // TODO: OnlineId

            return res;
        }
    }
}
