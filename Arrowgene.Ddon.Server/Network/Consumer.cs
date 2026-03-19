using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arrowgene.Ddon.Metrics;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Metrics;
using Arrowgene.Networking.SAEAServer;
using Arrowgene.Networking.SAEAServer.Consumer.BlockingQueueConsumption;

namespace Arrowgene.Ddon.Server.Network
{
    public class Consumer<TClient> : ThreadedBlockingQueue, IDisposable, IMetricsCapture<DdonConsumerMetricsSnapshot> where TClient : Client
    {
        private readonly ServerLogger Logger;
        private readonly Dictionary<PacketId, IPacketHandler<TClient>> _packetHandlerLookup;
        private readonly Dictionary<long, TClient> _clients;
        private readonly DdonConsumerMetricsState _ddonConsumerMetricsState;
        private readonly object _lock;
        private readonly IClientFactory<TClient> _clientFactory;

        private IPacketHandler<TClient> _fallbackPacketHandler;

        public Action<TClient> ClientDisconnected;
        public Action<TClient> ClientConnected;


        public Consumer(
            int maxUnitOfOrder,
            int queueCapacityPerLane,
            string identity,
            IClientFactory<TClient> clientFactory,
            ServerLogger logger = null
        ) : base(maxUnitOfOrder, queueCapacityPerLane, identity)
        {
            Logger = logger ?? LogProvider.Logger<ServerLogger>(GetType());
            _clientFactory = clientFactory;
            _lock = new object();
            _clients = new Dictionary<long, TClient>();
            _packetHandlerLookup = new Dictionary<PacketId, IPacketHandler<TClient>>();
            _ddonConsumerMetricsState = new DdonConsumerMetricsState();
        }

        public void Clear()
        {
            _packetHandlerLookup.Clear();
        }

        public void AddHandler(IPacketHandler<TClient> packetHandler)
        {
            if (_packetHandlerLookup.ContainsKey(packetHandler.Id))
            {
                Logger.Error($"PacketHandlerId: {packetHandler.Id.Name} already exists");
            }
            else
            {
                _packetHandlerLookup.Add(packetHandler.Id, packetHandler);
            }
        }

        public void SetFallbackHandler(IPacketHandler<TClient> packetHandler)
        {
            _fallbackPacketHandler = packetHandler;
        }

        protected override void HandleReceived(ClientHandle clientHandle, byte[] data)
        {
            long receivedTimestamp = Stopwatch.GetTimestamp();

            if (!clientHandle.IsAlive)
            {
                return;
            }

            TClient client;
            lock (_lock)
            {
                if (!_clients.TryGetValue(clientHandle.UniqueId, out client))
                {
                    Logger.Error(clientHandle, "Client does not exist in lookup");
                    return;
                }
            }

            List<IPacket> packets = client.Receive(data);
            foreach (IPacket packet in packets)
            {
                HandlePacket(client, packet, receivedTimestamp);
            }
        }

        private void HandlePacket(TClient client, IPacket packet, long receivedTimestamp)
        {
            if (!_packetHandlerLookup.TryGetValue(packet.Id, out IPacketHandler<TClient> packetHandler))
            {
                Logger.LogUnhandledPacket(client, packet);
                if (_fallbackPacketHandler != null)
                {
                    ExecutePacketHandler(client, packet, _fallbackPacketHandler, receivedTimestamp);
                }

                return;
            }

            ExecutePacketHandler(client, packet, packetHandler, receivedTimestamp);
        }

        private void ExecutePacketHandler(TClient client, IPacket packet, IPacketHandler<TClient> packetHandler, long receivedTimestamp)
        {
            _ddonConsumerMetricsState.RecordParseDuration(receivedTimestamp);
            long startTimestamp = Stopwatch.GetTimestamp();
            try
            {
                packetHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
                Logger.LogPacketError(client, packet);
                _ddonConsumerMetricsState.IncrementHandlerErrors(
                    packetHandler.Id.ToString(),
                    packetHandler.Id.Name);
            }
            finally
            {
                _ddonConsumerMetricsState.RecordHandlerExecution(
                    packetHandler.Id.ToString(),
                    packetHandler.Id.Name,
                    startTimestamp);
            }
        }

        protected override void HandleDisconnected(ClientSnapshot clientSnapshot)
        {
            TClient client;
            lock (_lock)
            {
                if (!_clients.Remove(clientSnapshot.UniqueId, out client))
                {
                    Logger.Error(clientSnapshot, "Disconnected client does not exist in lookup");
                    return;
                }
            }

            Action<TClient> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }

            Logger.Info($"Disconnected: {client.Identity}");
        }

        protected override void HandleConnected(ClientHandle clientHandle)
        {
            TClient client = _clientFactory.NewClient(clientHandle);
            lock (_lock)
            {
                _clients.Add(clientHandle.UniqueId, client);
            }

            Logger.Info($"Connected: {client.Identity}");

            Action<TClient> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }
        }

        protected override void HandleError(ClientSnapshot clientSnapshot, Exception exception, string message)
        {
            Logger.Error(clientSnapshot, message);
            Logger.Exception(clientSnapshot, exception);
        }

        public DdonConsumerMetricsSnapshot CreateSnapshot(double elapsedSeconds)
        {
            long currentExecuted = _ddonConsumerMetricsState.GetHandlersExecuted();
            long currentErrors = _ddonConsumerMetricsState.GetHandlerErrors();

            long[] durationBuckets = new long[_ddonConsumerMetricsState.HandlerDurationBucketsCount];
            _ddonConsumerMetricsState.CopyHandlerDurationBuckets(durationBuckets);

            long[] parseBuckets = new long[_ddonConsumerMetricsState.HandlerDurationBucketsCount];
            _ddonConsumerMetricsState.CopyParseDurationBuckets(parseBuckets);

            var handlerEntries = _ddonConsumerMetricsState.GetHandlerEntries();
            var handlerMetrics = new Dictionary<string, DdonConsumerMetricsSnapshot.HandlerMetrics>(handlerEntries.Count);
            foreach (var kvp in handlerEntries)
            {
                var entry = kvp.Value;
                handlerMetrics[kvp.Key] = new DdonConsumerMetricsSnapshot.HandlerMetrics(
                    entry.HandlerName,
                    entry.GetExecutionCount(),
                    entry.GetErrorCount(),
                    entry.GetTotalDurationTicks(),
                    entry.GetMinDurationTicks(),
                    entry.GetMaxDurationTicks());
            }

            return new DdonConsumerMetricsSnapshot(
                currentExecuted, currentErrors, durationBuckets, parseBuckets, handlerMetrics);
        }

        void IMetricsCapture.EnableCapture()
        {
            _ddonConsumerMetricsState.EnableCapture();
        }

        void IMetricsCapture.DisableCapture()
        {
            _ddonConsumerMetricsState.DisableCapture();
        }

        public void Dispose()
        {
            foreach (var handler in _packetHandlerLookup.Values)
            {
                handler.Dispose();
            }

            _fallbackPacketHandler?.Dispose();
        }

    }
}
