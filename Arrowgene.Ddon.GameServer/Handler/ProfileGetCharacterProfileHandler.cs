using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileGetCharacterProfileHandler : GameStructurePacketHandler<C2SProfileGetCharacterProfileReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileGetCharacterProfileHandler));
        
        public ProfileGetCharacterProfileHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SProfileGetCharacterProfileReq> packet)
        {
            GameClient targetClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);

            S2CProfileGetCharacterProfileRes res = new S2CProfileGetCharacterProfileRes();
            res.CharacterId = targetClient.Character.Id;
            GameStructure.CDataCharacterName(res.CharacterName, targetClient.Character);
            res.JobId = targetClient.Character.Job;
            res.JobLevel = (byte) targetClient.Character.ActiveCharacterJobData.Lv;
            // TODO: ClanParam
            // TODO: ClanMemberRank
            res.JobLevelList = targetClient.Character.CharacterJobDataList.Select(jobData => new CDataJobBaseInfo() {
                Job = jobData.Job,
                Level = (byte) jobData.Lv
            }).ToList();
            res.MatchingProfile = targetClient.Character.MatchingProfile;
            res.ArisenProfile = targetClient.Character.ArisenProfile;
            // TODO: OnlineId
            
            client.Send(res);
        }
    }
}