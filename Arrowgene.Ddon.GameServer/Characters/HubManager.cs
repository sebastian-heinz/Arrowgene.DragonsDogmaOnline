using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class HubManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(HubManager));

        public HubManager(DdonGameServer server)
        {
            Server = server;
            HubMembers = new Dictionary<uint, HashSet<GameClient>>();
            foreach (var stageId in StageManager.HubStageIds)
            {
                HubMembers[stageId] = new HashSet<GameClient>();
            }
        }

        private readonly DdonGameServer Server;
        private readonly Dictionary<uint, HashSet<GameClient>> HubMembers;

        public HashSet<GameClient> GetClientsInHub(StageId stageId)
        {
            return GetClientsInHub(stageId.Id);
        }

        public HashSet<GameClient> GetClientsInHub(uint stageId)
        {
            if (Server.Setting.GameLogicSetting.NaiveLobbyContextHandling)
            {
                return Server.ClientLookup.GetAll().Distinct().ToHashSet();
            }
            else
            {
                if (!HubMembers.ContainsKey(stageId))
                {
                    return new HashSet<GameClient>();
                }
                return HubMembers[stageId];
            }
        }

        // The server maintains an authoritative list of who's in each hub stage.
        // Hub stages are defined in StageManager.HubStageIds.
        // The client has a weird way of keeping contexts, which I attempt to replicate here.
        // As long as the client does not see another client leave a hub, it preserves that other client's context
        // in memory and does not need to get it again from the server to see that client's character.

        public void UpdateLobbyContextOnStageChange(GameClient client, uint previousStageId, uint targetStageId)
        {
            // Fallback to naive method.
            if (Server.Setting.GameLogicSetting.NaiveLobbyContextHandling)
            {
                NaiveLobbyHandling(client, previousStageId);
                return;
            }

            // Transitions that do not involve a hub stage don't concern us.
            if (!HubMembers.ContainsKey(previousStageId) && !HubMembers.ContainsKey(targetStageId))
            {
                return;
            }

            uint id = client.Character.CharacterId;
            HashSet<GameClient> targetClients = new HashSet<GameClient>();
            HashSet<GameClient> gatherClients = new HashSet<GameClient>();

            if (HubMembers.ContainsKey(previousStageId))
            {
                lock (HubMembers[previousStageId])
                {
                    HubMembers[previousStageId].Remove(client);
                    foreach (GameClient otherClient in Server.ClientLookup.GetAll().Where(x => x.Character != null))
                    {
                        if (otherClient == client) continue;

                        if (HubMembers[previousStageId].Contains(otherClient))
                        {
                            // They saw us leave, and do not need to be updated, their clients discard the context automatically.
                            // But the next time they see us, they will need our context back.
                            otherClient.Character.LastSeenLobby.Remove(id);
                        }
                        else if (otherClient.Character.LastSeenLobby.TryGetValue(id, out var lastStage) && lastStage == previousStageId)
                        {
                            // These clients did not see us leave, and so need a new context to remove the phantom lobby member if/when they return.
                            targetClients.Add(otherClient);
                            otherClient.Character.LastSeenLobby[id] = targetStageId; // This syncs with the StageNo in the CDataContextBase we're planning to send.
                        }
                    }
                }
            }

            if (HubMembers.ContainsKey(targetStageId))
            {
                lock (HubMembers[targetStageId])
                {
                    foreach (GameClient otherClient in HubMembers[targetStageId].Where(x => x.Character != null))
                    {
                        if (otherClient == client) continue;

                        uint otherId = otherClient.Character.CharacterId;
                        if (!otherClient.Character.LastSeenLobby.TryGetValue(id, out var lastStage) || lastStage != targetStageId)
                        {
                            // These clients are here, but don't have our context, so plan to send it.
                            targetClients.Add(otherClient);
                            otherClient.Character.LastSeenLobby[id] = targetStageId;
                        }

                        if (!client.Character.LastSeenLobby.TryGetValue(otherId, out lastStage) || lastStage != targetStageId)
                        {
                            // We have no context or an outdated context for the other client, so plan to gather it.
                            gatherClients.Add(otherClient);
                        }
                        client.Character.LastSeenLobby[otherId] = targetStageId;
                    }
                    HubMembers[targetStageId].Add(client);
                }
            }

            if (targetClients.Any())
            {
                SendContext(client, targetClients);
            }
            if (gatherClients.Any())
            {
                GatherContexts(gatherClients, client);
            }
        }

        private static void SendContext(GameClient sourceClient, GameClient targetClient)
        {
            S2CContextGetLobbyPlayerContextNtc contextNtc = new S2CContextGetLobbyPlayerContextNtc();
            GameStructure.S2CContextGetLobbyPlayerContextNtc(contextNtc, sourceClient.Character);
            targetClient.Send(contextNtc);
        }

        private static void SendContext(GameClient sourceClient, IEnumerable<GameClient> targetClients)
        {
            S2CContextGetLobbyPlayerContextNtc contextNtc = new S2CContextGetLobbyPlayerContextNtc();
            GameStructure.S2CContextGetLobbyPlayerContextNtc(contextNtc, sourceClient.Character);
            foreach (GameClient client in targetClients)
            {
                client.Send(contextNtc);
            }
        }

        private static void GatherContexts(IEnumerable<GameClient> sourceClients, GameClient targetClient)
        {
            foreach (GameClient source in sourceClients)
            {
                S2CContextGetLobbyPlayerContextNtc contextNtc = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(contextNtc, source.Character);
                targetClient.Send(contextNtc);
            }
        }

        public void LeaveAllHubs(GameClient client)
        {
            foreach (var otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Character?.LastSeenLobby.Remove(client.Character.CharacterId);
            }

            foreach (var hub in HubMembers.Values)
            {
                hub.Remove(client);
            }
        }

        // A fallback to the existing mechanism; broadcast/gather the entire server on entry.
        private void NaiveLobbyHandling(GameClient client, uint sourceStageId)
        {
            // Designed to only trigger once on joining the server, the only time your StageId == 0.
            if (sourceStageId != 0)
            {
                return;
            } 

            HashSet<GameClient> targetClients = new HashSet<GameClient>();
            HashSet<GameClient> gatherClients = new HashSet<GameClient>();

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                targetClients.Add(otherClient);
                gatherClients.Add(otherClient);
            }

            if (targetClients.Any())
            {
                SendContext(client, targetClients);
            }
            if (gatherClients.Any())
            {
                GatherContexts(gatherClients, client);
            }
        }
    }
}
