using System.Collections.Generic;
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