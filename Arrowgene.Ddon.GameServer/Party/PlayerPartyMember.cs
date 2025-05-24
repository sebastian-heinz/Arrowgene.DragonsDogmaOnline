using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Party;

public class PlayerPartyMember : PartyMember
{
    public PlayerPartyMember(GameClient client, DdonGameServer server)
    {
        Client = client;
        QuestState = new SoloQuestStateManager(this, server);
    }

    public GameClient Client { get; set; }

    public SoloQuestStateManager QuestState { get; set; }

    public override CDataPartyMember GetCDataPartyMember()
    {
        CDataPartyMember obj = new CDataPartyMember();
        GameStructure.CDataPartyMember(obj, this);
        return obj;
    }

    public S2CContextGetPartyPlayerContextNtc GetPartyContext()
    {
        CDataPartyPlayerContext partyPlayerContext = new CDataPartyPlayerContext();
        GameStructure.CDataContextBase(partyPlayerContext.Base, Client.Character);
        GameStructure.CDataContextPlayerInfo(partyPlayerContext.PlayerInfo, Client.Character);
        GameStructure.CDataContextResist(partyPlayerContext.ResistInfo, Client.Character);
        partyPlayerContext.EditInfo = Client.Character.EditInfo;

        S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc();
        partyPlayerContextNtc.CharacterId = Client.Character.CharacterId;
        partyPlayerContextNtc.Context = partyPlayerContext;
        partyPlayerContextNtc.Context.Base.MemberIndex = MemberIndex;
        return partyPlayerContextNtc;
    }
}
