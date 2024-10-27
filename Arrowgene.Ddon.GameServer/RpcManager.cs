using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer
{
    public class RpcManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RpcManager));

        private static readonly string[] TRAFFIC_LABELS = new string[] {
            "Empty", "Light", "Good", "Normal", "Busy", "Heavy"
        };
        private static readonly uint COUNT_PER_TRAFFIC = 10;

        private readonly HttpClient HttpClient = new HttpClient();

        private readonly DdonGameServer Server;
        private readonly Dictionary<ushort, ServerInfo> ChannelInfo;
        private readonly Dictionary<ushort, Dictionary<uint, CharacterSummary>> CharacterTrackingMap;

        public class RpcWrappedObject
        {
            public RpcInternalCommand Command { get; set; }
            public ushort Origin { get; set; }
            public object Data { get; set; }
        }

        public class RpcUnwrappedObject
        {
            public RpcInternalCommand Command { get; set; }
            public ushort Origin { get; set; }

            [JsonConverter(typeof(DataJsonConverter))]
            public string Data { get; set; }
            public T GetData<T>()
            {
                return JsonSerializer.Deserialize<T>(Data);
            }

            // Hack to deserialize nested objects.
            internal class DataJsonConverter : JsonConverter<string>
            {
                public override string Read(
                    ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                    {
                        return jsonDoc.RootElement.GetRawText();
                    }
                }

                public override void Write(
                    Utf8JsonWriter writer, string value, JsonSerializerOptions options)
                {
                    throw new NotImplementedException();
                }
            }
        }


        public RpcManager(DdonGameServer server)
        {
            Server = server;
            ChannelInfo = Server.AssetRepository.ServerList.ToDictionary(x => x.Id,
                x => new ServerInfo()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Brief = x.Brief,
                    TrafficName = x.TrafficName,
                    TrafficLevel = x.TrafficLevel,
                    MaxLoginNum = x.MaxLoginNum,
                    LoginNum = x.LoginNum,
                    Addr = x.Addr,
                    Port = x.Port,
                    IsHide = x.IsHide,
                    RpcPort = x.RpcPort,
                    RpcAuthToken = x.RpcAuthToken,
                    IsHead = x.IsHead,
                });
            CharacterTrackingMap = new();
            foreach (var info in ChannelInfo.Values)
            {
                CharacterTrackingMap[info.Id] = new();
            }

            var authToken = ChannelInfo[(ushort) Server.Id].RpcAuthToken;
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Internal", $"{server.Id}:{authToken}");
        }

        #region Server List
        public List<CDataGameServerListInfo> ServerListInfo()
        {
            return ChannelInfo.Keys.Select(x => ServerListInfo(x)).ToList();
        }

        public CDataGameServerListInfo ServerListInfo(ushort channelId)
        {
            var info = ChannelInfo[channelId].ToCDataGameServerListInfo();
            lock (CharacterTrackingMap[channelId])
            {
                info.LoginNum = (uint)CharacterTrackingMap[channelId].Keys.Count;
            }
            info.TrafficName = GetTrafficName(info.LoginNum);
            return info;
        }

        private static string GetTrafficName(uint count)
        {
            uint index = 0;
            if (count > 0)
            {
                index = count / COUNT_PER_TRAFFIC + 1;
                index = (uint) Math.Min(index, TRAFFIC_LABELS.Length);
            }
            return $"{TRAFFIC_LABELS[index]} ({count})";
        }
        #endregion

        #region RPC Machinery
        public bool Auth(ushort channelId, string token)
        {
            return ChannelInfo.Values.Where(x => x.Id == channelId && x.RpcAuthToken == token).Any();
        }

        private string Route(ushort channelId, string route)
        {
            var channel = ChannelInfo[channelId];
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
            _ = HttpClient.PostAsync(Route(channelId, route), new StringContent(json));
        }

        public void AnnounceAll(string route, RpcInternalCommand command, object data)
        {
            foreach (var id in ChannelInfo.Keys)
            {
                Announce(id, route, command, data);
            }
        }

        public void AnnounceOthers(string route, RpcInternalCommand command, object data)
        {
            foreach (var id in ChannelInfo.Keys)
            {
                if (id == Server.Id) continue;
                Announce(id, route, command, data);
            }
        }
        #endregion

        #region Player Tracking
        public ushort FindPlayerByName(string firstName, string lastName)
        {
            foreach (var channel in CharacterTrackingMap)
            {
                lock(channel.Value)
                {
                    foreach (var player in channel.Value.Values)
                    {
                        if (player.FirstName == firstName && player.LastName == lastName)
                        {
                            return channel.Key;
                        }
                    }
                }
            }
            return 0;
        }

        public ushort FindPlayerById(uint characterId)
        {
            foreach (var channel in CharacterTrackingMap)
            {
                lock (channel.Value)
                {
                    foreach (var player in channel.Value.Values)
                    {
                        if (player.CharacterId == characterId)
                        {
                            return channel.Key;
                        }
                    }
                }
            }
            return 0;
        }
    
        public void RemovePlayerSummary(ushort channelId, uint characterId)
        {
            lock (CharacterTrackingMap[channelId])
            {
                CharacterTrackingMap[channelId].Remove(characterId);
            }
        }

        public void AddPlayerSummary(ushort channelId, CharacterSummary characterSummary)
        {
            lock (CharacterTrackingMap[channelId])
            {
                CharacterTrackingMap[channelId][characterSummary.CharacterId] = characterSummary;
            }
        }

        public void AnnouncePlayerLeave(Character character)
        {
            var characterSummary = new CharacterSummary(character);
            lock (CharacterTrackingMap[(ushort)Server.Id])
            {
                if (!CharacterTrackingMap[(ushort)Server.Id].ContainsKey(character.CharacterId))
                {
                    // We don't actually have this player, so we don't have the authority to announce his leaving.
                    return;
                }
            }
            RemovePlayerSummary((ushort)Server.Id, character.CharacterId);
            AnnounceOthers("internal/tracking", RpcInternalCommand.NotifyPlayerLeave, characterSummary);
        }

        public void AnnouncePlayerJoin(Character character)
        {
            var characterSummary = new CharacterSummary(character);
            AddPlayerSummary((ushort)Server.Id, characterSummary);
            AnnounceOthers("internal/tracking", RpcInternalCommand.NotifyPlayerJoin, characterSummary);
        }

        #endregion
    }
}
