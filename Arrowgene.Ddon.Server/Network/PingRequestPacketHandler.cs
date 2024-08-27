using System;
using System.Timers;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class PingRequestPacketHandler<TClient, TReqStruct, TResStruct> : RequestPacketHandler<TClient, TReqStruct, TResStruct>
        where TClient : Client
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {

        private static readonly double TIMER_INTERVAL_MS = 60000; // 1 minute

        private readonly Timer TimeoutCheckTimer;

        protected PingRequestPacketHandler(DdonServer<TClient> server) : base(server)
        {
            TimeoutCheckTimer = new Timer(TIMER_INTERVAL_MS);
            TimeoutCheckTimer.AutoReset = true;
            TimeoutCheckTimer.Elapsed += (sender, e) =>
            {
                DateTime now = DateTime.UtcNow;
                foreach (var client in server.ClientLookup.GetAll())
                {
                    if ((now - client.PingTime).TotalMilliseconds > TIMER_INTERVAL_MS * 2)
                    {
                        // Try messaging the client to ensure it is still alive
                        // If the client is dead, the send will fail and it'll be cleaned up
                        var pingRes = BuildPingResponse(client, now);
                        client.Send(pingRes);
                    }
                }
            };
            TimeoutCheckTimer.Start();

            // TODO: Find out a good place to dispose TimeoutCheckTimer
        }

        public override TResStruct Handle(TClient client, TReqStruct request)
        {
            DateTime now = DateTime.UtcNow;
            client.PingTime = now;
            return BuildPingResponse(client, now);
        }

        public abstract TResStruct BuildPingResponse(TClient client, DateTime now);

        public override void Dispose()
        {
            TimeoutCheckTimer.Dispose();
        }
    }

}
