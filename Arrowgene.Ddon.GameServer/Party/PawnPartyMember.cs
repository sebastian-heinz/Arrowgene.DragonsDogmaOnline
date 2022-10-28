using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Party;

public class PawnPartyMember : PartyMember
{
    public Pawn Pawn { get; set; }

    public override Packet GetS2CContextGetParty_ContextNtc()
    {
        CDataPartyContextPawn partyContextPawn = new CDataPartyContextPawn();
        partyContextPawn.Base = new CDataContextBase(Character);
        partyContextPawn.Base.PawnId = PawnId;
        partyContextPawn.Base.CharacterId = Character.Id;
        partyContextPawn.Base.PawnType = Pawn.PawnType;
        partyContextPawn.Base.HmType = Pawn.HmType;
        partyContextPawn.PlayerInfo = new CDataContextPlayerInfo(Character);
        partyContextPawn.PawnReactionList = Pawn.PawnReactionList;
        partyContextPawn.Unk0 = new byte[64];
        partyContextPawn.SpSkillList = Pawn.SpSkillList;
        partyContextPawn.ResistInfo = new CDataContextResist(Character);
        partyContextPawn.EditInfo = Character.EditInfo;

        S2CContextGetPartyMypawnContextNtc partyPlayerContextNtc = new S2CContextGetPartyMypawnContextNtc();
        partyPlayerContextNtc.PawnId = PawnId;
        partyPlayerContextNtc.Context = partyContextPawn;
        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;
        return new StructurePacket<S2CContextGetPartyMypawnContextNtc>(partyPlayerContextNtc);
    }
}
