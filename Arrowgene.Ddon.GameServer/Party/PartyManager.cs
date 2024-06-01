using System;
using System.Collections.Concurrent;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyManager
{
    public const uint MaxNumParties = 100;
    public const uint InvalidPartyId = 0;
    public const ushort InvitationTimeoutSec = 30;


    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyManager));

    public readonly AssetRepository assetRepository;

    private readonly ConcurrentStack<uint> _idPool;
    private readonly ConcurrentDictionary<uint, PartyGroup> _parties;
    private readonly ConcurrentDictionary<GameClient, PartyInvitation> _invites;

    public PartyManager(AssetRepository assetRepository)
    {
        this.assetRepository = assetRepository;

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
        invitation.Date = DateTime.UtcNow;
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
        LogPartyIdCount();
        return true;
    }

    public PartyGroup NewParty()
    {
        if (!_idPool.TryPop(out uint partyId))
        {
            Logger.Info("Id pool seemingly exhausted. Cleaning up (!_idPool.TryPop(out uint partyId)");
            RecalculateIdPool();
            // Try again a second time and error if the problem persists
            if (!_idPool.TryPop(out partyId))
            {
                Logger.Error("Could not create party, id pool exhausted (!_idPool.TryPop(out partyId)");
                return null;
            }
        }

        PartyGroup party = new PartyGroup(partyId, this);
        if (!_parties.TryAdd(partyId, party))
        {
            Logger.Error("Could not create party, failed to add new party (!_parties.TryAdd(partyId, party))");
            return null;
        }

        LogPartyIdCount();
        
        return party;
    }

    private void LogPartyIdCount()
    {
        Logger.Info($"Free party IDs: {_idPool.Count}/{MaxNumParties}");
    }

    private void RecalculateIdPool()
    {
        // TODO: Thread safety, logs, error handling
        foreach (KeyValuePair<uint, PartyGroup> pair in _parties)
        {
            if(pair.Value.MemberCount() == 0)
            {
                _idPool.Push(pair.Key);
                _parties.TryRemove(pair);
            }
        }

        Logger.Info($"Free party IDs: {_idPool.Count}/{MaxNumParties}");
    }

    public bool ClientsInSameParty(GameClient clientA, GameClient clientB)
    {
        if (clientA.Party == null || clientB.Party == null)
        {
            return false;
        }

        return (clientA.Party.Id == clientB.Party.Id);
    }

    public void CleanupOnExit(GameClient client)
    {
        if (client.Party != null)
        {
            client.Party.Leave(client);

            Logger.Info(client, $"Left PartyId:{client.Party.Id}");

            S2CPartyPartyLeaveNtc partyLeaveNtc = new S2CPartyPartyLeaveNtc();
            partyLeaveNtc.CharacterId = client.Character.CharacterId;
            client.Party.SendToAllExcept(partyLeaveNtc, client);

            client.Send(new S2CPartyPartyLeaveRes());
        }
    }
}
