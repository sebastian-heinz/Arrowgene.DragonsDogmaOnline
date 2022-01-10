/*
 * This file is part of Arrowgene.Ddon.GameServer
 *
 * Arrowgene.Ddon.GameServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2022 DDON Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddon.GameServer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddon.GameServer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddon.GameServer. If not, see <https://www.gnu.org/licenses/>.
 */

using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class DdonGameServer : DdonServer<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(DdonGameServer));

        public DdonGameServer(GameServerSetting setting) : base(setting.ServerSetting)
        {
            Setting = new GameServerSetting(setting);
            LoadPacketHandler();
        }

        public GameServerSetting Setting { get; }

        protected override void ClientConnected(GameClient client)
        {
            client.InitializeChallenge();
        }

        protected override void ClientDisconnected(GameClient client)
        {
        }

        public override GameClient NewClient(ITcpSocket socket)
        {
            return new GameClient(socket, new PacketFactory(Setting.ServerSetting, PacketIdResolver.GamePacketIdResolver));
        }

        private void LoadPacketHandler()
        {
            AddHandler(new CharacterDecideCharacterIdHandler(this));
            AddHandler(new ClanClanSettingUpdateHandler(this));
            AddHandler(new ClientChallengeHandler(this));
            AddHandler(new ConnectionLoginHandler(this));
            AddHandler(new ConnectionMoveOutServerHandler(this));
            AddHandler(new ConnectionPingHandler(this));
            AddHandler(new ConnectionReserveServerHandler(this));
            AddHandler(new LoadingInfoLoadingGetInfoHandler(this));
            AddHandler(new ServerGetGameSettingHandler(this));
            AddHandler(new ServerGetRealTimeHandler(this));
            AddHandler(new ServerGetServerListHandler(this));
            AddHandler(new StageGetStageListHandler(this));
            AddHandler(new WarpGetStartPointListHandler(this));
        }
    }
}
