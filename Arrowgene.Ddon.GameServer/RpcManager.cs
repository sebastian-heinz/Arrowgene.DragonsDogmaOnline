using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer
{
    public class RpcManager
    {
        private class RpcTrackingMap : Dictionary<uint, RpcCharacterData>
        {
            public readonly DateTime TimeStamp;
            public readonly ushort ChannelId;

            public RpcTrackingMap(ushort channelId) : base() 
            {
                ChannelId = channelId;
                TimeStamp = DateTime.UtcNow;
            }

            public RpcTrackingMap(ushort channelId, List<RpcCharacterData> characterData) 
                : base(characterData.ToDictionary(key => key.CharacterId, val => val))
            {
                ChannelId = channelId;
                TimeStamp = DateTime.UtcNow;
            }

            public RpcTrackingMap(ushort channelId, List<RpcCharacterData> characterData, DateTime timeStamp)
                : base(characterData.ToDictionary(key => key.CharacterId, val => val))
            {
                ChannelId = channelId;
                TimeStamp = timeStamp;
            }
        }

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RpcManager));

        private static readonly string[] TRAFFIC_LABELS = new string[] {
            "Empty", "Light", "Good", "Normal", "Busy", "Heavy"
        };

        private readonly HttpClient HttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) };
        private static readonly double PING_TIMEOUT = 3000; // msec.

        private readonly DdonGameServer Server;
        private ConcurrentDictionary<ushort, RpcTrackingMap> CharacterTrackingMap { get; set; }

        public RpcManager(DdonGameServer server)
        {
            Server = server;
          
            CharacterTrackingMap = new();
            foreach (var info in server.AssetRepository.ServerList)
            {
                CharacterTrackingMap[info.Id] = new(info.Id);
            }

            string authToken = GetServer((ushort) Server.Id)?.RpcAuthToken ?? 
                throw new Exception("Failed to internally authenticate RPC Manager; ensure your GameServerList is correctly set up.");

            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Internal", $"{server.Id}:{authToken}");
        }

        #region Server List

        public ServerInfo GetServer(ushort id)
        {
            return Server.AssetRepository.ServerList.FirstOrDefault(x => x.Id == id);
        }

        public List<CDataGameServerListInfo> ServerListInfo()
        {
            return Server.AssetRepository.ServerList.Select(x => ServerListInfo(x)).ToList();
        }

        public ServerInfo HeadServer()
        {
            return Server.AssetRepository.ServerList.ToList().OrderBy(x => x.Id).FirstOrDefault();
        }

        public CDataGameServerListInfo ServerListInfo(ushort channelId)
        {
            return ServerListInfo(GetServer(channelId));
        }

        public CDataGameServerListInfo ServerListInfo(ServerInfo info)
        {
            var cdata = info.ToCDataGameServerListInfo();
            if (cdata.Id == Server.Id)
            {
                cdata.LoginNum = (uint)Server.ClientLookup.GetAll().Where(x => x.Character != null).Count();
            }
            else
            {
                cdata.LoginNum = (uint)(CharacterTrackingMap.GetValueOrDefault(cdata.Id)?.Count ?? 0);
            }

            cdata.TrafficName = GetTrafficName(cdata.LoginNum, cdata.MaxLoginNum);
            return cdata;
        }

        public static string GetTrafficName(uint count, uint maxLoginNum)
        {
            uint index = 0;
            if (count >= maxLoginNum)
            {
                return $"Full ({count})";
            }
            else if (count > 0)
            {
                uint countPerTraffic = (uint)(maxLoginNum / (TRAFFIC_LABELS.Length-1));
                index = count / countPerTraffic + 1;
                index = (uint) Math.Min(index, TRAFFIC_LABELS.Length - 1);
            }
            return $"{TRAFFIC_LABELS[index]} ({count})";
        }

        public bool DoesGameServerExist(ushort channelId)
        {
            return GetServer(channelId) is not null;
        }

        #endregion

        #region RPC Machinery
        public bool Auth(ushort channelId, string token)
        {
            return GetServer(channelId)?.RpcAuthToken == token;
        }

        private string Route(ushort channelId, string route)
        {
            var channel = GetServer(channelId);
            return $"http://{channel.Addr}:{channel.RpcPort}/rpc/{route}";
        }

        public async Task<T> Post<T>(ushort channelId, string route, RpcInternalCommand command, object data)
        {
            var response = await HttpClient.PostAsJsonAsync(Route(channelId, route), new RpcWrappedObject()
            {
                Command = command,
                Origin = (ushort) Server.Id,
                Data = data
            });

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public void Announce(ushort channelId, string route, RpcInternalCommand command, object data)
        {
            var wrappedObject = new RpcWrappedObject()
            {
                Command = command,
                Origin = (ushort)Server.Id,
                Data = data
            };

            var json = JsonSerializer.Serialize(wrappedObject);

            try
            {
                _ = HttpClient.PostAsync(Route(channelId, route), new StringContent(json));
            }
            catch (HttpRequestException ex)
            {
                Logger.Error($"RPC announce {command} > server {channelId} failed: {ex.Message}");
            }
        }

        public void AnnounceAll(string route, RpcInternalCommand command, object data)
        {
            foreach (var channel in Server.AssetRepository.ServerList)
            {
                Announce(channel.Id, route, command, data);
            }
        }

        public void AnnounceOthers(string route, RpcInternalCommand command, object data)
        {
            foreach (var channel in Server.AssetRepository.ServerList)
            {
                if (channel.Id == Server.Id) continue;
                Announce(channel.Id, route, command, data);
            }
        }

        public void AnnounceClan(uint clanId, string route, RpcInternalCommand command, object data)
        {
            foreach (var channel in CharacterTrackingMap)
            {
                if (channel.Key == Server.Id)
                {
                    continue;
                }

                if (channel.Value.Any(x => x.Value.ClanId == clanId))
                {
                    Announce(channel.Key, route, command, data);
                }
            }
        }

        public bool PingServer(ushort serverId)
        {
            return PingServer(GetServer(serverId)).GetAwaiter().GetResult();
        }

        private async Task<bool> PingServer(ServerInfo targetServer)
        {
            try
            {
                // This is probably not the correct way to do this.
                var route = $"http://{targetServer.Addr}:{targetServer.RpcPort}/rpc/internal/command";
                var wrappedObject = new RpcWrappedObject()
                {
                    Command = RpcInternalCommand.Ping,
                };
                var json = JsonSerializer.Serialize(wrappedObject);
                var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(PING_TIMEOUT));
                var response = await HttpClient.PostAsync(route, new StringContent(json), cts.Token);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex is OperationCanceledException || ex is HttpRequestException)
            {
                Logger.Error($"Ping on server {targetServer.Id} failed; {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Player Tracking
        public ushort FindPlayerByName(string firstName, string lastName)
        {
            lock (CharacterTrackingMap)
            {
                foreach ((ushort channelId, var channelMembers) in CharacterTrackingMap)
                {
                    foreach (var player in channelMembers.Values)
                    {
                        if (player.FirstName == firstName && player.LastName == lastName)
                        {
                            return channelId;
                        }
                    }
                }
            }
            return 0;
        }

        public ushort FindPlayerById(uint characterId)
        {
            lock (CharacterTrackingMap)
            {
                foreach ((ushort channelId, var channelMembers) in CharacterTrackingMap)
                {
                    if (channelMembers.ContainsKey(characterId))
                    {
                        return channelId;
                    }
                }
            }
            return 0;
        }
    
        public void AnnouncePlayerList(Character exception = null)
        {
            List<RpcCharacterData> rpcCharacterDatas = new List<RpcCharacterData>();
            foreach (var character in Server.ClientLookup.GetAllCharacter())
            {
                if (character == exception) continue;
                rpcCharacterDatas.Add(new(character));
            }
            Logger.Info($"Announcing player list for channel {Server.Id} with {rpcCharacterDatas.Count} players over RPC.");
            AnnounceOthers("internal/command", RpcInternalCommand.NotifyPlayerList, rpcCharacterDatas);
            CharacterTrackingMap[(ushort) Server.Id] = new RpcTrackingMap((ushort) Server.Id, rpcCharacterDatas);
        }

        public void ReceivePlayerList(ushort channelId, DateTime timestamp, List<RpcCharacterData> characterDatas)
        {
            Logger.Info($"Recieving player list from channel {channelId} with {characterDatas.Count} players.");
            if (CharacterTrackingMap.ContainsKey(channelId))
            {
                if (timestamp > CharacterTrackingMap[channelId].TimeStamp)
                {
                    CharacterTrackingMap[channelId] = new RpcTrackingMap(channelId, characterDatas, timestamp);
                }
                else
                {
                    Logger.Error($"Out of date character list discarded for channel ID {channelId}");
                }
            }
        }

        public void UpdatePlayerSummaryClan(uint characterId, uint clanId)
        {
            var clan = Server.ClanManager.GetClan(clanId);
            foreach ((ushort channelId, var channelMembers) in CharacterTrackingMap)
            {
                lock (channelMembers)
                {
                    if (channelMembers.ContainsKey(characterId))
                    {
                        channelMembers[characterId].ClanId = clan.ClanServerParam.ID;
                        channelMembers[characterId].ClanName = clan.ClanUserParam.Name;
                        channelMembers[characterId].ClanShortName = clan.ClanUserParam.ShortName;
                    }
                }
            }
        }
        #endregion

        #region Chat
        public void AnnounceClanChat(GameClient client, ChatResponse chatResponse)
        {
            if (client.Character.ClanId == 0) return;

            RpcChatData chatData = new RpcChatData()
            {
                HandleId = 0,
                Type = LobbyChatMsgType.Clan,
                MessageFlavor = chatResponse.MessageFlavor,
                PhrasesCategory = chatResponse.PhrasesCategory,
                PhrasesIndex = chatResponse.PhrasesIndex,
                Message = chatResponse.Message,
                Deliver = false,
                SourceData = new RpcCharacterData(client.Character)
            };

            AnnounceClan(client.Character.ClanId, "internal/chat", RpcInternalCommand.SendClanMessage, chatData);
        }

        public void AnnounceTellChat(GameClient client, C2SChatSendTellMsgReq request)
        {
            var targetServer = FindPlayerByName(request.CharacterInfo.CharacterName.FirstName, 
                request.CharacterInfo.CharacterName.LastName);

            if (targetServer == 0) throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHAT_TELL_CHARACTER_OFFLINE);
            if (targetServer == Server.Id) throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHAT_TELL_SESSION_LOST);

            RpcChatData chatData = new RpcChatData()
            {
                HandleId = 0,
                Type = LobbyChatMsgType.Tell,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex,
                Message = request.Message,
                Deliver = false,
                SourceData = new RpcCharacterData(client.Character),
                TargetData = new RpcCharacterData()
                {
                    FirstName = request.CharacterInfo.CharacterName.FirstName,
                    LastName = request.CharacterInfo.CharacterName.LastName,
                    CharacterId = request.CharacterInfo.CharacterId
                }
            };

            Announce(targetServer, "internal/chat", RpcInternalCommand.SendTellMessage, chatData);
        }
        #endregion

        public void AnnounceAllPacket<T>(T packet, uint characterId = 0)
            where T : class, IPacketStructure, new()
        {
            RpcPacketData data = new RpcPacketData()
            {
                GroupId = packet.Id.GroupId,
                HandlerId = packet.Id.HandlerId,
                HandlerSubId = packet.Id.HandlerSubId,
                CharacterId = characterId,
                Data = EntitySerializer.Get<T>().Write(packet)
            };
            AnnounceAll("internal/packet", RpcInternalCommand.AnnouncePacketAll, data);
        }

        public void AnnounceClanPacket<T>(uint clanId, T packet, uint characterId = 0)
            where T : class, IPacketStructure, new()
        {
            if (clanId == 0) return;

            RpcPacketData data = new RpcPacketData()
            {
                GroupId = packet.Id.GroupId,
                HandlerId = packet.Id.HandlerId,
                HandlerSubId = packet.Id.HandlerSubId,
                ClanId = clanId,
                CharacterId = characterId,
                Data = EntitySerializer.Get<T>().Write(packet)
            };

            if (ClanManager.INTERNAL_IMPORTANT_PACKETS.Contains(packet.Id))
            {
                // This needs to be sent to all channels so they'll update their internal tracking of the clan, even if nobody is there to recieve the packet.
                AnnounceOthers("internal/packet", RpcInternalCommand.AnnouncePacketClan, data);
            }
            else
            {
                AnnounceClan(clanId, "internal/packet", RpcInternalCommand.AnnouncePacketClan, data);
            }
        }
    }
}
