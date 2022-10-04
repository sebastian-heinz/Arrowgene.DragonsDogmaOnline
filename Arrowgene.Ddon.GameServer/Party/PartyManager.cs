using System.Collections.Concurrent;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyManager
{
    public const uint MaxNumParties = 100;
    public const uint InvalidPartyId = 0;

    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyManager));

    private readonly ConcurrentStack<uint> _idPool;
    private readonly ConcurrentDictionary<uint, PartyGroup> _parties;
    private readonly ConcurrentDictionary<GameClient, PartyGroup> _invites;

    public PartyManager()
    {
        _idPool = new ConcurrentStack<uint>();
        for (uint i = 1; i < MaxNumParties + 1; i++)
        {
            _idPool.Push(i);
        }

        _parties = new ConcurrentDictionary<uint, PartyGroup>();
        _invites = new ConcurrentDictionary<GameClient, PartyGroup>();
    }

    public bool AddInvitedParty(GameClient client, PartyGroup party)
    {
        if (!_invites.TryAdd(client, party))
        {
            Logger.Error(client, $"Already has pending invite)");
            return false;
        }

        return true;
    }
    
    public PartyGroup GetInvitedParty(GameClient client)
    {
        if (!_invites.TryGetValue(client, out PartyGroup party))
        {
            Logger.Error(client, $"invite not found, for get");
            return null;
        }

        return party;
    }
    
    public PartyGroup RemoveInvitedParty(GameClient client)
    {
        if (!_invites.TryRemove(client, out PartyGroup party))
        {
            Logger.Error(client, $"invite not found for remove");
            return null;
        }

        return party;
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
