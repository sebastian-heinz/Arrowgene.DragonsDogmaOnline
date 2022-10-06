using System;
using System.Collections.Concurrent;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyManager
{
    public const uint MaxNumParties = 100;
    public const uint InvalidPartyId = 0;
    public const ushort InvitationTimeoutSec = 30;


    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyManager));

    private readonly ConcurrentStack<uint> _idPool;
    private readonly ConcurrentDictionary<uint, PartyGroup> _parties;
    private readonly ConcurrentDictionary<GameClient, PartyInvitation> _invites;

    public PartyManager()
    {
        _idPool = new ConcurrentStack<uint>();
        for (uint i = 1; i < MaxNumParties + 1; i++)
        {
            _idPool.Push(i);
        }

        _parties = new ConcurrentDictionary<uint, PartyGroup>();
        _invites = new ConcurrentDictionary<GameClient, PartyInvitation>();
    }

    public bool InviteParty(GameClient invitee, GameClient host, PartyGroup party)
    {
        PartyInvitation invitation = new PartyInvitation();
        invitation.Invitee = invitee;
        invitation.Host = host;
        invitation.Party = party;
        invitation.Date = DateTime.Now;
        if (!_invites.TryAdd(invitee, invitation))
        {
            Logger.Error(invitee, $"Already has pending invite)");
            return false;
        }

        return true;
    }

    public PartyInvitation GetPartyInvitation(GameClient client)
    {
        if (!_invites.TryGetValue(client, out PartyInvitation partyInvitation))
        {
            Logger.Error(client, $"invite not found, for get");
            return null;
        }

        return partyInvitation;
    }

    public PartyInvitation RemovePartyInvitation(GameClient client)
    {
        if (!_invites.TryRemove(client, out PartyInvitation partyInvitation))
        {
            Logger.Error(client, $"invite not found for remove");
            return null;
        }

        return partyInvitation;
    }

    public PartyGroup GetParty(uint partyId)
    {
        if (!_parties.TryGetValue(partyId, out PartyGroup party))
        {
            Logger.Error(
                $"Could not find party by partyId {partyId} (!_parties.TryGetValue(partyId, out PartyGroup party)");
            return null;
        }

        return party;
    }

    public bool DisbandParty(uint partyId)
    {
        if (!_parties.TryRemove(partyId, out PartyGroup party))
        {
            Logger.Error($"Failed to remove partyId:{partyId} (!_parties.TryRemove(partyId, out PartyGroup party)");
            return false;
        }

        _idPool.Push(party.Id);
        return true;
    }

    public PartyGroup NewParty()
    {
        if (!_idPool.TryPop(out uint partyId))
        {
            Logger.Error("Could not create party, id pool exhausted (!_idPool.TryPop(out uint partyId)");
            return null;
        }

        PartyGroup party = new PartyGroup(partyId, this);
        if (!_parties.TryAdd(partyId, party))
        {
            Logger.Error("Could not create party, failed to add new party (!_parties.TryAdd(partyId, party))");
            return null;
        }

        return party;
    }
}
