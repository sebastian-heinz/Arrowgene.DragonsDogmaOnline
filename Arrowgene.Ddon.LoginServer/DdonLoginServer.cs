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

using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.LoginServer.Handler;
using Arrowgene.Ddon.LoginServer.Manager;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Handler;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.LoginServer
{
    public class DdonLoginServer : DdonServer<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DdonLoginServer));

        public DdonLoginServer(LoginServerSetting setting, GameSettings gameSetting, IDatabase database, AssetRepository assetRepository)
            : base(ServerType.Login, setting.ServerSetting, database, assetRepository)
        {
            Setting = new LoginServerSetting(setting);
            GameSetting = gameSetting;
            ClientLookup = new LoginClientLookup();
            LoginQueueManager = new(this);
            LoadPacketHandler();
        }

        public LoginServerSetting Setting { get; }
        public GameSettings GameSetting { get; }
        public LoginQueueManager LoginQueueManager { get; }

        public override LoginClientLookup ClientLookup { get; }

        protected override void ClientConnected(LoginClient client)
        {
            client.InitializeChallenge();
            ClientLookup.Add(client);
        }

        protected override void ClientDisconnected(LoginClient client)
        {
            ClientLookup.Remove(client);

            Account account = client.Account;
            if (account != null)
            {
                LoginQueueManager.Remove(client.Account.Id);
                Database.DeleteConnection(Id, client.Account.Id);
            }
        }

        public override LoginClient NewClient(ITcpSocket socket)
        {
            return new LoginClient(socket,
                new PacketFactory(Setting.ServerSetting, PacketIdResolver.LoginPacketIdResolver));
        }


        private void LoadPacketHandler()
        {
            SetFallbackHandler(new FallbackHandler<LoginClient>(this));

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
            AddHandler(new ClientDecideCancelCharacterHandler(this));
        }
    }
}
