using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyCreateHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));


        public PartyPartyCreateHandler(DdonGameServer server) : base(server)
        {
                              
        }

        public override PacketId Id => PacketId.C2S_PARTY_PARTY_CREATE_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CParty_6_8_16_Ntc ntc_6_8_16 = new S2CParty_6_8_16_Ntc();
            CDataPartyMember partyMember = new CDataPartyMember();
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = client.Character.FirstName;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = client.Character.LastName;
            ntc_6_8_16.PartyMembers.Add(partyMember);
            client.Send(ntc_6_8_16);
           
            //client.Send(InGameDump.Dump_103);
            
          //  client.Send(InGameDump.Dump_104);
            client.Send(InGameDump.Dump_105);
        }
    }
}
