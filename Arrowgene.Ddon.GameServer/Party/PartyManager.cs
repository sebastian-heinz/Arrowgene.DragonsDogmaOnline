using System.Collections.Concurrent;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyManager
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyManager));

    private readonly ConcurrentStack<uint> _idPool;
    private readonly ConcurrentDictionary<uint, PartyGroup> _parties;

    public PartyManager()
    {
        _idPool = new ConcurrentStack<uint>();
        for (uint i = 1; i < 100; i++)
        {
            _idPool.Push(i);
        }

        _parties = new ConcurrentDictionary<uint, PartyGroup>();
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

    public PartyGroup NewParty(GameClient creator)
    {
        if (creator.Party != null)
        {
            Logger.Error(creator, "Could not create party, creator already in party (creator.Party != null)");
            return null;
        }

        if (!_idPool.TryPop(out uint partyId))
        {
            Logger.Error("Could not create party, id pool exhausted (!_idPool.TryPop(out uint partyId)");
            return null;
        }

        PartyGroup party = new PartyGroup(partyId, creator);
        if (!_parties.TryAdd(partyId, party))
        {
            Logger.Error("Could not create party, failed to add new party (!_parties.TryAdd(partyId, party))");
            return null;
        }

        return party;
    }
}
