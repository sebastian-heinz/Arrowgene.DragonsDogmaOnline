using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnJoinPartyMypawnHandler : StructurePacketHandler<GameClient, C2SPawnJoinPartyMypawnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnJoinPartyMypawnHandler));


        public PawnJoinPartyMypawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnJoinPartyMypawnReq> req)
        {                
                // Oh no
                //client.Party.Members.Add(pawn);

                MyPawnCsv myPawnCsvData = Server.AssetRepository.MyPawnAsset[req.Structure.PawnNumber-1];

                S2CPawnJoinPartyPawnNtc joinPartyPawnNtc = new S2CPawnJoinPartyPawnNtc();
                joinPartyPawnNtc.PartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
                joinPartyPawnNtc.PartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = myPawnCsvData.Name;
                joinPartyPawnNtc.PartyMember.CharacterListElement.CurrentJobBaseInfo.Job = myPawnCsvData.Job;
                joinPartyPawnNtc.PartyMember.CharacterListElement.CurrentJobBaseInfo.Level = myPawnCsvData.JobLv;
                joinPartyPawnNtc.PartyMember.MemberType = 2;
                joinPartyPawnNtc.PartyMember.MemberIndex = client.Party.Members.Count;
                joinPartyPawnNtc.PartyMember.PawnId = myPawnCsvData.PawnId;
                joinPartyPawnNtc.PartyMember.IsLeader = false;
                joinPartyPawnNtc.PartyMember.IsPawn = true;
                joinPartyPawnNtc.PartyMember.IsPlayEntry = false;
                joinPartyPawnNtc.PartyMember.JoinState = 2;
                joinPartyPawnNtc.PartyMember.AnyValueList = new byte[] {0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x1, 0x0, 0x2};
                joinPartyPawnNtc.PartyMember.SessionStatus = 0;
                client.Send(joinPartyPawnNtc);

                S2CContextGetPartyMypawnContextNtc mypawnContextNtc = new S2CContextGetPartyMypawnContextNtc(Server.AssetRepository.MyPawnAsset, req.Structure);
                mypawnContextNtc.CharacterId = client.Character.Id;
                client.Send(mypawnContextNtc);

                S2CPawnJoinPartyMypawnRes res = new S2CPawnJoinPartyMypawnRes();
                client.Send(res);
        }
    }
}
