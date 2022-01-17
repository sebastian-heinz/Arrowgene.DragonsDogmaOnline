/*
 * This file is part of Arrowgene.Ddon.LoginServer
 *
 * Arrowgene.Ddon.LoginServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2022 DDON Team
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

using Arrowgene.Ddon.LoginServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.LoginServer
{
    public class DdonLoginServer : DdonServer<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(DdonLoginServer));

        public DdonLoginServer(LoginServerSetting setting, IServerProvider provider) : base(setting.ServerSetting, provider)
        {
            Setting = new LoginServerSetting(setting);
            LoadPacketHandler();
        }

        public LoginServerSetting Setting { get; }


        protected override void ClientConnected(LoginClient client)
        {
            client.InitializeChallenge();
        }

        protected override void ClientDisconnected(LoginClient client)
        {
        }

        public override LoginClient NewClient(ITcpSocket socket)
        {
            return new LoginClient(socket, new PacketFactory(Setting.ServerSetting, PacketIdResolver.LoginPacketIdResolver));
        }

        private void LoadPacketHandler()
        {
            AddHandler(new ClientChallengeHandler(this));
            AddHandler(new ClientLoginHandler(this));
            AddHandler(new ClientPingHandler(this));
            AddHandler(new GetErrorMessageListHandler(this));
            AddHandler(new GetLoginSettingHandler(this));
            AddHandler(new GpCourseGetInfoHandler(this));
            AddHandler(new GetCharacterListHandler(this));
            AddHandler(new DeleteCharacterHandler(this));
            AddHandler(new ClientDecideCharacterIdHandler(this));
            AddHandler(new ClientGetGameSessionKeyHandler(this));
            AddHandler(new ClientLogoutHandler(this));
            AddHandler(new CreateCharacterHandler(this));
        }
    }
}
