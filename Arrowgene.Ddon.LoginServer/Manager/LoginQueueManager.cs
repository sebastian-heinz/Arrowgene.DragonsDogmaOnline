using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Arrowgene.Ddon.LoginServer.Manager
{
    public class LoginQueueManager : System.Timers.Timer
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LoginQueueManager));

        private readonly DdonLoginServer Server;

        // We need to occasionally pop from the middle to handle leavers, so this can't actually be a queue.
        private readonly List<int> LoginQueue = new();
        private readonly HttpClient _httpClient = new();
        private bool _httpReady = false;

        private static readonly double QUEUE_CHECK_TIME = 10000; // msec
        private static readonly int MAXLOGIN_ADJUSTMENT = 0;

        public LoginQueueManager(DdonLoginServer server) : base(QUEUE_CHECK_TIME)
        {
            Server = server;

            Elapsed += ResolveQueue;
            AutoReset = true;
            Start();
        }

        public int Enqueue(int id)
        {
            lock(LoginQueue)
            {
                LoginQueue.Add(id);
                return LoginQueue.Count;
            }
        }

        public int Remove(int id)
        {
            lock(LoginQueue)
            {
                LoginQueue.RemoveAll(x => x == id);
                return LoginQueue.Count;
            }
        }

        private static void ResolveQueue(object source, ElapsedEventArgs e)
        {
            LoginQueueManager obj = (LoginQueueManager)source;

            lock(obj.LoginQueue)
            {
                if (!obj.LoginQueue.Any())
                {
                    // Nobody in queue, go back to sleep.
                    return;
                }

                var servers = obj.GetServerInfo().GetAwaiter().GetResult();
                var filteredServers = FilterServers(servers);

                Logger.Debug($"Handling a login queue of {obj.LoginQueue.Count} players");

                // Try and hand out login spots to players if possible.
                int i = 0;
                while (filteredServers.Any() && i < obj.LoginQueue.Count)
                {
                    var chosenServer = filteredServers.First();
                    var currentAccountId = obj.LoginQueue[i];
                    var currentClient = obj.Server.ClientLookup.GetClientByAccountId(currentAccountId);

                    if (currentClient is not null)
                    {
                        currentClient.Send(new L2CNextConnectionServerNtc()
                        {
                            ServerList = chosenServer.ToCDataGameServerListInfo(),
                            Counter = 1
                        });

                        chosenServer.LoginNum++;
                        filteredServers = FilterServers(filteredServers);
                    }
                    i++;
                }

                // Clean out any leavers and update anyone who remains about their current status.
                obj.LoginQueue.RemoveAll(x => obj.Server.ClientLookup.GetClientByAccountId(x) is null);
                obj.LoginQueue.ForEach(x => obj.Server.ClientLookup.GetClientByAccountId(x)?.Send(new L2CLoginWaitNumNtc()
                {
                    WaitNum = (uint)obj.LoginQueue.Count,
                }));
            }
        }

        public CDataGameServerListInfo GetBalancedServer()
        {
            var response = GetServerInfo().GetAwaiter().GetResult();
            return GetBalancedServer(response);
        }

        public CDataGameServerListInfo GetBalancedServer(List<ServerInfo> servers)
        {
            var filteredServers = FilterServers(servers);
            if (filteredServers.Any())
            {
                return filteredServers.First().ToCDataGameServerListInfo();
            }
            else
            {
                return null;
            }
        }

        private static IEnumerable<ServerInfo> FilterServers(IEnumerable<ServerInfo> servers)
        {
            return servers.Where(x => !x.PreventLogin).Where(x => (x.LoginNum + MAXLOGIN_ADJUSTMENT) < x.MaxLoginNum).OrderBy(x => x.LoginNum);
        }

        private async Task<List<ServerInfo>> GetServerInfo()
        {
            // Timing issues with loading files vs server process startup.
            ServerInfo serverInfo = Server.AssetRepository.ServerList.Find(x => x.LoginId == Server.Id);
            if (!_httpReady)
            {
                lock (_httpClient)
                {
                    if (serverInfo is null)
                    {
                        Logger.Error($"Login server with ID {Server.Id} was not found in the ServerList asset.");
                        return Server.AssetRepository.ServerList;
                    }

                    // The login server auths as though it was the game server.
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Internal", $"{serverInfo.Id}:{serverInfo.RpcAuthToken}");
                    _httpReady = true;
                }
            }
            string route = $"http://{serverInfo.Addr}:{serverInfo.RpcPort}/rpc/status";
            return await _httpClient.GetFromJsonAsync<List<ServerInfo>>(route);
        }
    }
}
