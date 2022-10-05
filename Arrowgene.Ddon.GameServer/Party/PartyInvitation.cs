using System;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyInvitation
{
    public GameClient Client { get; set; }
    public PartyGroup Party { get; set; }
    public DateTime Date { get; set; }
}
