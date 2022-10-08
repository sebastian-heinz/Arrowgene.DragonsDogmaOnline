using System;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyInvitation
{
    public GameClient Host { get; set; }
    public GameClient Invitee { get; set; }
    public PartyGroup Party { get; set; }
    public DateTime Date { get; set; }
}
