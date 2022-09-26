using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClient : Client
    {
        public GameClient(ITcpSocket socket, PacketFactory packetFactory) : base(socket, packetFactory)
        {
            UpdateIdentity();
        }

        public void UpdateIdentity()
        {
            string newIdentity = $"[GameClient@{Socket.Identity}]";
            if (Account != null)
            {
                newIdentity += $"[Acc:({Account.Id}){Account.NormalName}]";
            }

            if (Character != null)
            {
                newIdentity += $"[Cha:({Character.Id}){Character.FirstName} {Character.LastName}]";
            }

            Identity = newIdentity;
        }

        public Account Account { get; set; }

        public Character Character { get; set; }

        
        
        
        public PartyGroup PendingInvitedParty { get; set; } // Maybe its more clean to store this in the handlers ?
        public PartyGroup Party { get; set; }

        public CDataPartyMember AsCDataPartyMember()
        {
            CDataPartyMember partyMember = new CDataPartyMember();
            partyMember.CharacterListElement.ServerId = Character.Server.Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = Character.Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = Character.FirstName;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = Character.LastName;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Job = Character.Job;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Character.ActiveCharacterJobData.Lv;
            partyMember.CharacterListElement.OnlineStatus = Character.OnlineStatus;
            partyMember.CharacterListElement.unk2 = 1;
            partyMember.MemberType = 1;
            partyMember.MemberIndex = (byte) Party.Members.IndexOf(this);
            partyMember.IsLeader = Character.Id == Party.Leader.Character.Id;
            partyMember.JoinState = JoinState.On;
            return partyMember;
        }        

        public Packet AsContextPacket()
        {
            S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc(Character);
            partyPlayerContextNtc.Context.Base.MemberIndex = Party.Members.IndexOf(this);
            StructurePacket<S2CContextGetPartyPlayerContextNtc> packet = new StructurePacket<S2CContextGetPartyPlayerContextNtc>(partyPlayerContextNtc);
            return packet;
        }
    }
}
