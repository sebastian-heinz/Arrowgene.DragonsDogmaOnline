using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;
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

        private DdonGameServer Server;
        public Dictionary<uint, HashSet<GameClient>> HubMembers;

        //The server maintains an authoritative list of who's in each hub stage.
        //Hub stages are defined in StageManager.HubStageIds.
        //The client has a weird way of keeping contexts, which I attempt to replicate here.
        //As long as the client does not see another client leave a hub, it preserves that other client's context
        //in memory and does not need to get it again from the server to see that client's character.

        //TODO/REVIEW: This probably needs locks on the HashSets.

        public bool JoinLobbyContext(GameClient client, uint stageId)
        {
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling) return true;
            //Cleanup any WeakReferences that may have decayed because the clients they point to disconnected.
            client.LocalLobbyContext[stageId].RemoveWhere(x => !x.TryGetTarget(out GameClient target));

            bool res = false;
            if (HubMembers.ContainsKey(stageId))
            {
                S2CContextGetLobbyPlayerContextNtc newUserContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(newUserContextNtc, client.Character);
                lock (HubMembers[stageId])
                {
                    res = HubMembers[stageId].Add(client);
                    foreach (GameClient otherClient in HubMembers[stageId])
                    {
                        if (otherClient == client) continue;
                        if (otherClient.Character is null) continue;
                        else if (otherClient.Character.Stage.Id != stageId) continue;

                        //Check if we still have their context cached locally, and if not, get a fresh copy.
                        if (!ClientRemembersOtherClient(stageId, client, otherClient))
                        {
                            S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                            GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, otherClient.Character);
                            Logger.Info($"Sending lobby context {otherClient.Character.CharacterId} -> {client.Character.CharacterId}");
                            client.Send(lobbyPlayerContextNtc);
                            client.LocalLobbyContext[stageId].Add(new WeakReference<GameClient>(otherClient));
                        }

                        //Only send our context to clients who need it.
                        if (!ClientRemembersOtherClient(stageId, otherClient, client))
                        {
                            Logger.Info($"Sending lobby context {client.Character.CharacterId} -> {otherClient.Character.CharacterId}");
                            otherClient.Send(newUserContextNtc);
                            otherClient.LocalLobbyContext[stageId].Add(new WeakReference<GameClient>(client));
                        }
                    }
                }
                return res;
            }
            return false;
        }

        public bool LeaveLobbyContext(GameClient client, uint stageId)
        {
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling) return true;
            if (HubMembers.ContainsKey(stageId))
            {
                //Everyone who *sees* us leaving drops our context, because the client is dumb.
                //Also use this opportunity to clean up any decayed weak references.
                lock (HubMembers[stageId])
                {
                    foreach (GameClient otherClient in HubMembers[stageId])
                    {
                        Logger.Info($"Dropping lobby context {otherClient.Character.CharacterId} -X> {client.Character.CharacterId}");
                        otherClient.LocalLobbyContext[stageId].RemoveWhere(x => !x.TryGetTarget(out GameClient target) || target == client);
                    }
                    return HubMembers[stageId].Remove(client);
                }
            }
            return false;
        }

        public bool LeaveAllLobbies(GameClient client)
        {
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling) return true;
            bool result = false;
            foreach (uint stageId in HubMembers.Keys)
            {
                result |= HubMembers[stageId].Remove(client);
            }
            return result;
        }

        private static bool ClientRemembersOtherClient(uint stageId, GameClient client, GameClient otherClient)
        {
            if (!client.LocalLobbyContext.ContainsKey(stageId)) return false;
            return client.LocalLobbyContext[stageId].Where(x => x.TryGetTarget(out GameClient target) && target == otherClient).Any();
        }
    }
}
