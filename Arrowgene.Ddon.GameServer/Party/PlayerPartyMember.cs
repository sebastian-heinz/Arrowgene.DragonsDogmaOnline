using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Party;

public class PlayerPartyMember : PartyMember
{
    public GameClient Client { get; set; }

    public override Packet GetS2CContextGetParty_ContextNtc()
    {
        CDataPartyPlayerContext partyPlayerContext = new CDataPartyPlayerContext();
        partyPlayerContext.Base = new CDataContextBase(Character);
        partyPlayerContext.PlayerInfo = new CDataContextPlayerInfo(Character);
        partyPlayerContext.ResistInfo = new CDataContextResist(Character);
        partyPlayerContext.EditInfo = Character.EditInfo;

        S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc();
        partyPlayerContextNtc.CharacterId = Character.Id;
        partyPlayerContextNtc.Context = partyPlayerContext;
        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;

        return new StructurePacket<S2CContextGetPartyPlayerContextNtc>(partyPlayerContextNtc);
    }
}
