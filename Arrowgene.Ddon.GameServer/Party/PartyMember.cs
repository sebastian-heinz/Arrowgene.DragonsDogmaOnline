using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Party;

public abstract class PartyMember
{
    public byte MemberType { get; set; }
    public int MemberIndex { get; set; }
    public uint CommonId { get; set; }
    public uint PawnId { get; set; }
    public bool IsLeader { get; set; }
    public bool IsPawn { get; set; }
    public bool IsPlayEntry { get; set; }
    public JoinState JoinState { get; set; }
    public byte[] AnyValueList { get; set; }
    public byte SessionStatus { get; set; }

    public abstract CDataPartyMember GetCDataPartyMember();
}
