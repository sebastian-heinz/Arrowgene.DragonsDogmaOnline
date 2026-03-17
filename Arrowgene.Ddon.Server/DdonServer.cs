/*
 * This file is part of Arrowgene.Ddon.LoginServer
 *
 * Arrowgene.Ddon.LoginServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2026 DDON Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddon.LoginServer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddon.LoginServer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddon.LoginServer. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Metrics;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.SAEAServer;
using Arrowgene.Networking.SAEAServer.Metric;

namespace Arrowgene.Ddon.Server
{
    public abstract class DdonServer<TClient> : IClientFactory<TClient>
        where TClient : Client
    {
        private readonly ServerLogger Logger;

        private readonly Consumer<TClient> _consumer;
        private readonly TcpServer _server;
        private readonly ServerSetting _setting;
        private readonly DdonServerMetricsState _ddonMetricsState;
        private readonly DdonServerMetricsCollector _ddonMetricsCollector;

        public readonly ServerType Type;

        public DdonServer(ServerType type, ServerSetting setting, IDatabase database, AssetRepository assetRepository)
        {
            LogProvider.ConfigureNamespace(GetType().Namespace, setting);
            Logger = LogProvider.Logger<ServerLogger>(GetType());

            Type = type;

            _setting = setting;
            AssetRepository = assetRepository;
            Database = database;

            _consumer = new Consumer<TClient>(
                _setting.TcpServerSettings.OrderingLaneCount,
                _setting.ConsumerQueueCapacityPerLane,
                _setting.TcpServerSettings.Identity,
                this,
                Logger
            );
            _consumer.ClientConnected += ClientConnected;
            _consumer.ClientDisconnected += ClientDisconnected;

            _server = new TcpServer(
                _setting.ListenIpAddress,
                _setting.ServerPort,
                _consumer,
                _setting.TcpServerSettings
            );

            _ddonMetricsState = new DdonServerMetricsState(_consumer.MetricsState);
            _ddonMetricsCollector = new DdonServerMetricsCollector(_ddonMetricsState);
        }

        public int Id => _setting.Id;
        public string Name => _setting.Name;
        public string ServerIdentity => _setting.TcpServerSettings.Identity;

        public AssetRepository AssetRepository { get; }
        public IDatabase Database { get; }

        public virtual void Start()
        {
            Database.DeleteConnectionsByServerId(Id);
            Logger.Info($"[{_setting.TcpServerSettings.Identity}] Listening: {_server.IpAddress}:{_server.Port}");
            EnableMetricsCapture();
            string metricsThreadName = $"{_setting.TcpServerSettings.Identity}.DdonMetrics";
            _ddonMetricsCollector.Start(metricsThreadName);
            _consumer.Start();
            _server.Start();
        }

        public void Stop()
        {
            DisableMetricsCapture();
            _ddonMetricsCollector.Stop();
            _consumer.Stop();
            _server.Stop();
            _consumer.Dispose();
        }

        protected void AddHandler(IPacketHandler<TClient> packetHandler)
        {
            _consumer.AddHandler(packetHandler);
        }

        protected void SetFallbackHandler(IPacketHandler<TClient> packetHandler)
        {
            _consumer.SetFallbackHandler(packetHandler);
        }

        protected abstract void ClientConnected(TClient client);
        protected abstract void ClientDisconnected(TClient client);
        public abstract TClient NewClient(ClientHandle clientHandle);
        public abstract ClientLookup<TClient> ClientLookup { get; }

        public TcpServerMetricsSnapshot GetTcpServerMetricsSnapshot()
        {
            return _server.GetMetricsSnapshot();
        }

        public TcpServerMetricsSnapshot GetTcpServerPublishedMetricsSnapshot()
        {
            return _server.GetPublishedMetricsSnapshot();
        }

        public DdonServerMetricsSnapshot GetDdonServerMetricsSnapshot()
        {
            if (IsMetricsCaptureEnabled())
            {
                try
                {
                    _ddonMetricsCollector.CaptureSnapshot();
                }
                catch (ObjectDisposedException)
                {
                }
            }

            return _ddonMetricsCollector.GetSnapshot();
        }

        public DdonServerMetricsSnapshot GetDdonServerPublishedMetricsSnapshot()
        {
            return _ddonMetricsCollector.GetSnapshot();
        }

        private void EnableMetricsCapture()
        {
            _ddonMetricsState.EnableCapture();
            _consumer.MetricsState.EnableCapture();
        }

        private void DisableMetricsCapture()
        {
            _ddonMetricsState.DisableCapture();
            _consumer.MetricsState.DisableCapture();
        }

        private bool IsMetricsCaptureEnabled()
        {
            return _ddonMetricsState.IsCaptureEnabled();
        }
    }
}
