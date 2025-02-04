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
            Character targetCharacter = Server.ClientLookup.GetClientByCharacterId(request.CharacterId)?.Character
                ?? Server.CharacterManager.SelectCharacter(request.CharacterId, fetchPawns:false)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_DATA_INVALID_CHARACTER_ID);

            S2CCharacterGetCharacterStatusNtc ntc = new S2CCharacterGetCharacterStatusNtc();
            ntc.CharacterId = targetCharacter.CharacterId;
            ntc.StatusInfo = targetCharacter.StatusInfo;
            ntc.JobParam = targetCharacter.ActiveCharacterJobData;
            GameStructure.CDataCharacterLevelParam(ntc.CharacterParam, targetCharacter);
            ntc.EditInfo = targetCharacter.EditInfo;
            ntc.EquipDataList = targetCharacter.Equipment.AsCDataEquipItemInfo(EquipType.Performance);
            ntc.VisualEquipDataList = targetCharacter.Equipment.AsCDataEquipItemInfo(EquipType.Visual);
            ntc.EquipJobItemList = targetCharacter.EquipmentTemplate.JobItemsAsCDataEquipJobItem(targetCharacter.Job);
            ntc.HideHead = targetCharacter.HideEquipHead;
            ntc.HideLantern = targetCharacter.HideEquipLantern;
            ntc.JewelryNum = targetCharacter.ExtendedParams.JewelrySlot;

            client.Send(ntc);

            S2CProfileGetCharacterProfileRes res = new S2CProfileGetCharacterProfileRes();
            res.CharacterId = targetCharacter.CharacterId;
            GameStructure.CDataCharacterName(res.CharacterName, targetCharacter);
            res.JobId = targetCharacter.Job;
            res.JobLevel = (byte)targetCharacter.ActiveCharacterJobData.Lv;
            res.ClanParam = Server.ClanManager.GetClan(targetCharacter.ClanId);

            var (clanId, memberInfo) = Server.ClanManager.ClanMembership(targetCharacter.CharacterId);
            if (memberInfo != null)
            {
                res.ClanMemberRank = (uint)memberInfo.Rank;
            }

            res.JobLevelList = targetCharacter.CharacterJobDataList.Select(jobData => new CDataJobBaseInfo()
            {
                Job = jobData.Job,
                Level = (byte)jobData.Lv
            }).ToList();
            res.MatchingProfile = targetCharacter.MatchingProfile;
            res.ArisenProfile = targetCharacter.ArisenProfile;
            // TODO: OnlineId

            return res;
        }
    }
}
