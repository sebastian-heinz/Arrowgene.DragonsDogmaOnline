using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Pawn : IPartyMember
    {
        public Pawn()
        {
            Character = new Character();
            OnlineStatus = OnlineStatus.None;
            PendingInvitedParty = null;
            Party = null;
            PawnReactionList = new List<CDataPawnReaction>();
            SpSkillList = new List<CDataSpSkill>();
        }

        public IPartyMember Owner { get; set; }
        public Character Character { get; set; }
        public byte HmType { get; set; }
        public byte PawnType { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        public Party PendingInvitedParty { get; set; }
        public Party Party { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; }
        public List<CDataSpSkill> SpSkillList { get; set; }

        public CDataPartyMember AsCDataPartyMember()
        {
            CDataPartyMember partyMember = new CDataPartyMember();
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = Owner.Character.Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = Character.FirstName;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Job = Character.Job;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Character.ActiveCharacterJobData.Lv;
            partyMember.MemberType = 2;
            partyMember.MemberIndex = Party.Members.IndexOf(this);
            partyMember.PawnId = Character.Id;
            partyMember.IsLeader = false;
            partyMember.IsPawn = true;
            partyMember.IsPlayEntry = false;
            partyMember.JoinState = JoinState.On;
            partyMember.AnyValueList = new byte[] {0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x1, 0x0, 0x2}; // Taken from pcaps
            partyMember.SessionStatus = 0;
            return partyMember;
        }

        public Packet AsContextPacket()
        {
            S2CContextGetPartyMypawnContextNtc mypawnContextNtc = new S2CContextGetPartyMypawnContextNtc(this);
            mypawnContextNtc.Context.Base.MemberIndex = Party.Members.IndexOf(this);
            StructurePacket<S2CContextGetPartyMypawnContextNtc> packet = new StructurePacket<S2CContextGetPartyMypawnContextNtc>(mypawnContextNtc);
            return packet;
        }

        public void Send(Packet packet)
        {
            // Do nothing
        }

        void IPartyMember.Send<TResStruct>(TResStruct res)
        {
            // Do nothing
        }
    }
}