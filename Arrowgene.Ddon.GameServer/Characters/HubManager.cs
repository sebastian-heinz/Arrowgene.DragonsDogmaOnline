using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
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

        //The server maintains an authoritative list of who's in each hub stage.
        //Hub stages are defined in StageManager.HubStageIds.
        //The client has a weird way of keeping contexts, which I attempt to replicate here.
        //As long as the client does not see another client leave a hub, it preserves that other client's context
        //in memory and does not need to get it again from the server to see that client's character.

        public void MoveStageUpdateLastSeen(GameClient client, uint sourceStageId, uint targetStageId)
        {
            //Fallback to classic method.
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling)
            {
                ClassicLobbyHandling(client, sourceStageId);
                return;
            }

            //Transitions that do not involve a hub stage don't concern us.
            if (!HubMembers.ContainsKey(sourceStageId) && !HubMembers.ContainsKey(targetStageId)) return;

            uint id = client.Character.CharacterId;
            HashSet<GameClient> targetClients = new HashSet<GameClient>();
            HashSet<GameClient> gatherClients = new HashSet<GameClient>();

            if (HubMembers.ContainsKey(sourceStageId))
            {
                lock (HubMembers[sourceStageId])
                {
                    HubMembers[sourceStageId].Remove(client);
                    foreach (GameClient otherClient in Server.ClientLookup.GetAll())
                    {
                        if (HubMembers[sourceStageId].Contains(otherClient))
                        {
                            //They saw us leave, and do not need to be updated, their clients discard the context automatically.
                            //But the next time they see us, they will need our context back.
                            otherClient.Character.LastSeenLobby.Remove(id);
                        }
                        else if (otherClient.Character.LastSeenLobby.TryGetValue(id, out var lastStage) && lastStage == sourceStageId)
                        {
                            //These clients did not see us leave, and so need a new context to remove the phantom lobby member if/when they return.
                            targetClients.Add(otherClient);
                            otherClient.Character.LastSeenLobby[id] = targetStageId; //This syncs with the StageNo in the CDataContextBase we're planning to send.
                        }
                    }
                }
            }

            if (HubMembers.ContainsKey(targetStageId))
            {
                lock (HubMembers[targetStageId])
                {
                    foreach (GameClient otherClient in HubMembers[targetStageId])
                    {
                        uint otherId = otherClient.Character.CharacterId;
                        if (otherClient.Character.LastSeenLobby.TryGetValue(id, out var lastStage) && lastStage == targetStageId)
                        {
                            //These clients saw us here last, so they don't need our context again.
                        }
                        else
                        {
                            //These clients are here, but don't have our context, so plan to send it.
                            targetClients.Add(otherClient);
                            otherClient.Character.LastSeenLobby[id] = targetStageId;
                        }
                        
                        //Where did we last see them?
                        if (client.Character.LastSeenLobby.TryGetValue(otherId, out lastStage) && lastStage == targetStageId)
                        {
                            //We last saw them in this spot, so we have the proper context.
                        }
                        else
                        {
                            //We have no context or an outdated context, so queue them up to gather it.
                            gatherClients.Add(otherClient);
                        }
                        client.Character.LastSeenLobby[otherId] = targetStageId;
                    }
                    HubMembers[targetStageId].Add(client);
                }
            }

            if (targetClients.Any()) SendContext(client, targetClients);
            if (gatherClients.Any()) GatherContexts(gatherClients, client);
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
                Logger.Info($"Sending lobby context {sourceClient.Character.CharacterId} -> {client.Character.CharacterId}");
                client.Send(contextNtc);
            }
        }

        private static void GatherContexts(IEnumerable<GameClient> sourceClients, GameClient targetClient)
        {
            foreach (GameClient source in sourceClients)
            {
                Logger.Info($"Sending lobby context {source.Character.CharacterId} -> {targetClient.Character.CharacterId}");
                S2CContextGetLobbyPlayerContextNtc contextNtc = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(contextNtc, source.Character);
                targetClient.Send(contextNtc);
            }
        }

        public void LeaveAllHubs(GameClient client)
        {
            foreach (var hub in HubMembers.Values)
            {
                hub.Remove(client);
            }
        }

        //A fallback to the existing mechanism; broadcast/gather the entire server on entry.
        private void ClassicLobbyHandling(GameClient client, uint sourceStageId)
        {
            if (sourceStageId != 0) return; //Designed to only trigger once on joining the server, the only time your StageId == 0.

            HashSet<GameClient> targetClients = new HashSet<GameClient>();
            HashSet<GameClient> gatherClients = new HashSet<GameClient>();

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                targetClients.Add(otherClient);
                gatherClients.Add(otherClient);
            }

            if (targetClients.Any()) SendContext(client, targetClients);
            if (gatherClients.Any()) GatherContexts(gatherClients, client);
        }
    }
}
