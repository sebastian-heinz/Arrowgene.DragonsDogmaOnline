using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
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
        //Hub-to-hub transitions seem to have special rules, so this may not be exhaustive.

        public void JoinLobbyContext(GameClient client, uint stageId)
        {
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling) return;

            if (HubMembers.ContainsKey(stageId))
            {
                S2CContextGetLobbyPlayerContextNtc newUserContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(newUserContextNtc, client.Character);
                lock (HubMembers[stageId])
                {
                    HubMembers[stageId].Add(client);
                    foreach (GameClient otherClient in HubMembers[stageId])
                    {
                        if (otherClient == client) continue;
                        else if (otherClient.Character is null) continue;
                        else if (otherClient.Character.Stage.Id != stageId) continue;

                        //Check if we still have their context cached locally, and if not, get a fresh copy.
                        if (!ClientRemembersOtherClient(stageId, client, otherClient))
                        {
                            S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                            GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, otherClient.Character);
                            Logger.Debug($"Sending lobby context {otherClient.Character.CharacterId} -> {client.Character.CharacterId}");
                            client.Send(lobbyPlayerContextNtc);
                            if (client.Character.LocalLobbyContext.TryGetValue(stageId, out var context))
                            {
                                context.Add(otherClient.Character.CharacterId);
                            }
                            else
                            {
                                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_INVALID_STAGE_ID,
                                    $"Lobby context passing failure! {otherClient.Character.CharacterId} -> {client.Character.CharacterId}"
                                );
                            }
                        }

                        //Only send our context to clients who need it.
                        if (!ClientRemembersOtherClient(stageId, otherClient, client))
                        {
                            Logger.Debug($"Sending lobby context {client.Character.CharacterId} -> {otherClient.Character.CharacterId}");
                            otherClient.Send(newUserContextNtc);

                            if (otherClient.Character.LocalLobbyContext.TryGetValue(stageId, out var context))
                            {
                                context.Add(client.Character.CharacterId);
                            }
                            else
                            {
                                throw new ResponseErrorException(ErrorCode.ERROR_CODE_INSTANCE_AREA_INVALID_STAGE_ID,
                                    $"Lobby context passing failure! {otherClient.Character.CharacterId} -> {client.Character.CharacterId}"
                                );
                            }
                        }
                    }
                }
            }
        }

        public void LeaveAllLobbies(GameClient client)
        {
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling) return;
            foreach (uint stageId in HubMembers.Keys)
            {
                HubMembers[stageId].Remove(client);
            }
        }

        private static bool ClientRemembersOtherClient(uint stageId, GameClient client, GameClient otherClient)
        {
            if (!client.Character.LocalLobbyContext.ContainsKey(stageId)) return false;
            return client.Character.LocalLobbyContext[stageId].Contains(otherClient.Character.CharacterId);
        }

        public void TransitionLobbyContext(GameClient client, uint sourceStageId, uint targetStageId)
        {
            if (!Server.Setting.GameLogicSetting.FancyLobbyContextHandling) return;

            if (HubMembers.ContainsKey(sourceStageId))
            {
                lock (HubMembers[sourceStageId])
                {
                    if (HubMembers.ContainsKey(targetStageId)) //If we're moving from hub to hub, we forget everything, but everyone remembers us.
                    {
                        if (client.Character.LocalLobbyContext.TryGetValue(sourceStageId, out var context))
                        {
                            client.Character.LocalLobbyContext[sourceStageId].Clear();
                        }
                    }
                    else //Otherwise, everyone forgets us.
                    {
                        foreach (GameClient otherClient in HubMembers[sourceStageId])
                        {
                            if (otherClient == client) continue;
                            Logger.Debug($"Dropping lobby context {otherClient.Character.CharacterId} -X> {client.Character.CharacterId}");
                            if (otherClient.Character.LocalLobbyContext.TryGetValue(sourceStageId, out var context))
                            {
                                context.Remove(client.Character.CharacterId);
                            }
                        }
                    }
                    HubMembers[sourceStageId].Remove(client);
                }
            }

            JoinLobbyContext(client, targetStageId);
        }
    }
}
