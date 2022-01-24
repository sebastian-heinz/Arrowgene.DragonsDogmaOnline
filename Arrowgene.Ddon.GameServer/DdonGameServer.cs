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

using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class DdonGameServer : DdonServer<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DdonGameServer));

        private readonly List<GameClient> _clients;

        public DdonGameServer(GameServerSetting setting, IDatabase database, AssetRepository assetRepository)
            : base(setting.ServerSetting, database, assetRepository)
        {
            Setting = new GameServerSetting(setting);
            _clients = new List<GameClient>();
            LoadPacketHandler();
        }

        public GameServerSetting Setting { get; }

        public override List<GameClient> Clients => new List<GameClient>(_clients);

        protected override void ClientConnected(GameClient client)
        {
            client.InitializeChallenge();
        }

        protected override void ClientDisconnected(GameClient client)
        {
            _clients.Remove(client);
        }

        public override GameClient NewClient(ITcpSocket socket)
        {
            GameClient newClient = new GameClient(socket, new PacketFactory(Setting.ServerSetting, PacketIdResolver.GamePacketIdResolver));
            _clients.Add(newClient);
            return newClient;
        }
        
        private void LoadPacketHandler()
        {
            AddHandler(new AchievementAchievementGetReceivableRewardListHandler(this));

            AddHandler(new AreaGetAreaBaseInfoListHandler(this));
            AddHandler(new AreaGetAreaQuestHintListHandler(this));
            AddHandler(new AreaGetLeaderAreaReleaseListHandler(this));

            AddHandler(new BattleContentInfoListHandler(this));
            AddHandler(new BlackListGetBlackListHandler(this));

            AddHandler(new ActionSetPlayerActionHistoryHandler(this));

            AddHandler(new CharacterCommunityCharacterStatusGetHandler(this));
            AddHandler(new CharacterDecideCharacterIdHandler(this));
            AddHandler(new CharacterCharacterGoldenReviveHandler(this));
            AddHandler(new CharacterCharacterPenaltyReviveHandler(this));
            AddHandler(new CharacterCharacterPointReviveHandler(this));

            AddHandler(new ClanClanGetJoinRequestedListHandler(this));
            AddHandler(new ClanClanGetMyInfoHandler(this));
            AddHandler(new ClanClanGetMyMemberListHandler(this));
            AddHandler(new ClanClanSettingUpdateHandler(this));

            AddHandler(new ClientChallengeHandler(this));

            AddHandler(new ConnectionGetLoginAnnouncementHandler(this));
            AddHandler(new ConnectionLoginHandler(this));
            AddHandler(new ConnectionLogoutHandler(this));
            AddHandler(new ConnectionMoveInServerHandler(this));
            AddHandler(new ConnectionMoveOutServerHandler(this));
            AddHandler(new ConnectionPingHandler(this));
            AddHandler(new ConnectionReserveServerHandler(this));

            AddHandler(new ContextGetSetContextHandler(this));
            AddHandler(new DailyMissionListGetHandler(this));
            AddHandler(new EquipGetCharacterEquipListHandler(this));

            AddHandler(new FriendGetFriendListHandler(this));
            AddHandler(new FriendGetRecentCharacterListHandler(this));


            AddHandler(new Gp_28_2_1_Handler(this));
            AddHandler(new GpGetUpdateAppCourseBonusFlagHandler(this));

            AddHandler(new GroupChatGroupChatGetMemberListHandler(this));

            AddHandler(new InstanceGetEnemySetListHandler(this));
            AddHandler(new InstanceGetItemSetListHandler(this));

            AddHandler(new InstanceGetOmInstantKeyValueAllHandler(this));

            AddHandler(new ItemGetStorageItemListHandler(this));
            AddHandler(new ItemSortGetItemSortDataBinHandler(this));

            AddHandler(new JobGetJobChangeListHandler(this));
            AddHandler(new LoadingInfoLoadingGetInfoHandler(this));

            AddHandler(new LobbyLobbyChatMsgHandler(this));
            AddHandler(new LobbyLobbyJoinHandler(this));

            AddHandler(new MailMailGetListDataHandler(this));
            AddHandler(new MailMailGetListFootHandler(this));
            AddHandler(new MailMailGetListHeadHandler(this));
            AddHandler(new MailSystemMailGetListDataHandler(this));
            AddHandler(new MailSystemMailGetListFootHandler(this));
            AddHandler(new MailSystemMailGetListHeadHandler(this));

            AddHandler(new OrbDevoteGetOrbGainExtendParamHandler(this));

            AddHandler(new PartnerPawnPawnLikabilityReleasedRewardListGetHandler(this));

            AddHandler(new PartyPartyCreateHandler(this));

            AddHandler(new PawnGetMyPawnListHandler(this));
            AddHandler(new PawnGetNoraPawnListHandler(this));
            AddHandler(new PawnGetRentedPawnListHandler(this));

            AddHandler(new QuestGetAdventureGuideQuestListHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestNoticeHandler(this));
            AddHandler(new QuestGetAreaBonusListHandler(this));
            AddHandler(new QuestGetCycleContentsStateListHandler(this));
            AddHandler(new QuestGetLevelBonusListHandler(this));
            AddHandler(new QuestGetMainQuestListHandler(this));
            AddHandler(new QuestGetPackageQuestListHandler(this));
            AddHandler(new QuestGetPartyQuestProgressInfoHandler(this));
            AddHandler(new QuestGetPriorityQuestHandler(this));
            AddHandler(new QuestGetQuestCompletedListHandler(this));
            AddHandler(new QuestGetQuestPartyBonusListHandler(this));
            AddHandler(new QuestGetSetQuestListHandler(this));
            AddHandler(new QuestGetTutorialQuestListHandler(this));
            AddHandler(new QuestGetWorldManageQuestListHandler(this));
            AddHandler(new QuestQuestProgressHandler(this));

            AddHandler(new ServerGameTimeGetBaseinfoHandler(this));
            AddHandler(new ServerGetGameSettingHandler(this));
            AddHandler(new ServerGetRealTimeHandler(this));
            AddHandler(new ServerGetServerListHandler(this));

            AddHandler(new SkillGetCurrentSetSkillListHandler(this));

            AddHandler(new StageAreaChangeHandler(this));
            AddHandler(new StageGetStageListHandler(this));

            AddHandler(new StampBonusCheckHandler(this));

            AddHandler(new WarpAreaWarpHandler(this));
            AddHandler(new WarpGetFavoriteWarpPointListHandler(this));
            AddHandler(new WarpGetReleaseWarpPointListHandler(this));
            AddHandler(new WarpGetReturnLocationHandler(this));
            AddHandler(new WarpGetStartPointListHandler(this));
            AddHandler(new WarpGetWarpPointListHandler(this));
            AddHandler(new WarpRegisterFavoriteWarpHandler(this));
            AddHandler(new WarpWarpHandler(this));
        }
    }
}
