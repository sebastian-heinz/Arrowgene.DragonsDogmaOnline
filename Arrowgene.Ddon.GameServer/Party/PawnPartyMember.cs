using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Party;

public class PawnPartyMember : PartyMember
{
    public Pawn Pawn { get; set; }

    public override CDataPartyMember GetCDataPartyMember()
    {
        CDataPartyMember obj = new CDataPartyMember();
        GameStructure.CDataPartyMember(obj, this);
        return obj;
    }

    public override Packet GetS2CContextGetParty_ContextNtc()
    {
        CDataPartyContextPawn partyContextPawn = new CDataPartyContextPawn();
        GameStructure.CDataContextBase(partyContextPawn.Base, Pawn);
        partyContextPawn.Base.PawnId = PawnId;
        partyContextPawn.Base.CharacterId = Pawn.CharacterId;
        partyContextPawn.Base.PawnType = Pawn.PawnType;
        partyContextPawn.Base.HmType = Pawn.HmType;
        GameStructure.CDataContextPlayerInfo(partyContextPawn.PlayerInfo, Pawn);
        partyContextPawn.PawnReactionList = Pawn.PawnReactionList;
        partyContextPawn.Unk0 = new byte[64];
        partyContextPawn.SpSkillList = Pawn.SpSkillList;
        GameStructure.CDataContextResist(partyContextPawn.ResistInfo, Pawn);
        partyContextPawn.EditInfo = Pawn.EditInfo;

        S2CContextGetPartyMypawnContextNtc partyPlayerContextNtc = new S2CContextGetPartyMypawnContextNtc();
        partyPlayerContextNtc.PawnId = PawnId;
        partyPlayerContextNtc.Context = partyContextPawn;
        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;
        return new StructurePacket<S2CContextGetPartyMypawnContextNtc>(partyPlayerContextNtc);
    }
}
