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

using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.GameServer.Chat.Command;
using Arrowgene.Ddon.GameServer.Chat.Log;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Enemy;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class DdonGameServer : DdonServer<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DdonGameServer));

        private readonly List<GameClient> _clients;
        private readonly List<Party> _parties;
        private readonly Dictionary<StageId, Stage> _stages;

        public DdonGameServer(GameServerSetting setting, IDatabase database, AssetRepository assetRepository)
            : base(setting.ServerSetting, database, assetRepository)
        {
            _clients = new List<GameClient>();
            _parties = new List<Party>();
            _stages = new Dictionary<StageId, Stage>();
            Setting = new GameServerSetting(setting);
            Router = new GameRouter();
            ChatManager = new ChatManager(this, Router);
            EnemyManager = new EnemyManager(assetRepository, database);

            S2CStageGetStageListRes stageListPacket = EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19);
            StageList = stageListPacket.StageList;
        }

        public event EventHandler<ClientConnectionChangeArgs> ClientConnectionChangeEvent;

        public GameServerSetting Setting { get; }
        public ChatManager ChatManager { get; }
        public EnemyManager EnemyManager { get; }
        public GameRouter Router { get; }

        /// <summary>
        /// Returns a copy of the client list.
        /// To prevent modifications of affecting the original list.
        /// </summary>
        public override List<GameClient> Clients => new List<GameClient>(_clients);

        /// <summary>
        /// Returns a copy of the party list.
        /// To prevent modifications of affecting the original list.
        /// </summary>
        public List<Party> Parties => new List<Party>(_parties);

        public List<CDataStageInfo> StageList { get; }


        public override void Start()
        {
            LoadStages();
            LoadChatHandler();
            LoadPacketHandler();
            base.Start();
        }

        protected override void ClientConnected(GameClient client)
        {
            client.InitializeChallenge();

            EventHandler<ClientConnectionChangeArgs> connectionChangeEvent = ClientConnectionChangeEvent;
            if (connectionChangeEvent != null)
            {
                ClientConnectionChangeArgs connectionChangeEventArgs
                    = new ClientConnectionChangeArgs(ClientConnectionChangeArgs.EventType.CONNECT, client);
                connectionChangeEvent(this, connectionChangeEventArgs);
            }
        }

        protected override void ClientDisconnected(GameClient client)
        {
            _clients.Remove(client);

            EventHandler<ClientConnectionChangeArgs> connectionChangeEvent = ClientConnectionChangeEvent;
            if (connectionChangeEvent != null)
            {
                ClientConnectionChangeArgs connectionChangeEventArgs
                    = new ClientConnectionChangeArgs(ClientConnectionChangeArgs.EventType.DISCONNECT, client);
                connectionChangeEvent(this, connectionChangeEventArgs);
            }
        }

        public override GameClient NewClient(ITcpSocket socket)
        {
            GameClient newClient = new GameClient(socket, new PacketFactory(Setting.ServerSetting, PacketIdResolver.GamePacketIdResolver));
            _clients.Add(newClient);
            return newClient;
        }

        public Party NewParty()
        {
            Party newParty = new Party();
            _parties.Add(newParty);
            return newParty;
        }

        private void LoadStages()
        {
            _stages.Add(new StageId(0, 0, 0), new Stage(StageId.Invalid));
        }

        private void LoadChatHandler()
        {
            ChatManager.AddHandler(new ChatLogHandler());
            ChatManager.AddHandler(new ChatCommandHandler(this));
        }

        private void LoadPacketHandler()
        {
            AddHandler(new AchievementAchievementGetReceivableRewardListHandler(this));

            AddHandler(new AreaGetAreaBaseInfoListHandler(this));
            AddHandler(new AreaGetAreaQuestHintListHandler(this));
            AddHandler(new AreaGetLeaderAreaReleaseListHandler(this));

            AddHandler(new BattleContentInfoListHandler(this));
            AddHandler(new BinarySaveSetCharacterBinSavedataHandler(this));
            AddHandler(new BlackListGetBlackListHandler(this));

            AddHandler(new ActionSetPlayerActionHistoryHandler(this));

            AddHandler(new CharacterCommunityCharacterStatusGetHandler(this));
            AddHandler(new CharacterDecideCharacterIdHandler(this));
            AddHandler(new CharacterCharacterGoldenReviveHandler(this));
            AddHandler(new CharacterCharacterPenaltyReviveHandler(this));
            AddHandler(new CharacterCharacterPointReviveHandler(this));
            AddHandler(new CharacterPawnGoldenReviveHandler(this));
            AddHandler(new CharacterPawnPointReviveHandler(this));
            AddHandler(new CharacterSetOnlineStatusHandler(this));

            AddHandler(new ClanClanBaseGetInfoHandler(this));
            AddHandler(new ClanClanConciergeGetListHandler(this));
            AddHandler(new ClanClanConciergeUpdateHandler(this));
            AddHandler(new ClanClanGetJoinRequestedListHandler(this));
            AddHandler(new ClanClanGetMyInfoHandler(this));
            AddHandler(new ClanClanGetMyMemberListHandler(this));
            AddHandler(new ClanClanPartnerPawnDataGetHandler(this));
            AddHandler(new ClanClanSettingUpdateHandler(this));
            AddHandler(new ClanGetFurnitureHandler(this));
            AddHandler(new ClanSetFurnitureHandler(this));

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
            AddHandler(new GpGetValidChatComGroupHandler(this));

            AddHandler(new GroupChatGroupChatGetMemberListHandler(this));

            AddHandler(new InnGetStayPriceHandler(this));
            AddHandler(new InnStayInnHandler(this));

            AddHandler(new InstanceEnemyKillHandler(this));
            AddHandler(new InstanceExchangeOmInstantKeyValueHandler(this));
            AddHandler(new InstanceGetEnemySetListHandler(this));
            AddHandler(new InstanceGetGatheringItemHandler(this));
            AddHandler(new InstanceGetGatheringItemListHandler(this));
            AddHandler(new InstanceGetItemSetListHandler(this));
            AddHandler(new InstanceSetOmInstantKeyValueHandler(this));
            AddHandler(new InstanceTreasurePointGetCategoryListHandler(this));
            AddHandler(new InstanceTreasurePointGetListHandler(this));

            AddHandler(new InstanceGetOmInstantKeyValueAllHandler(this));

            AddHandler(new ItemGetStorageItemListHandler(this));
            AddHandler(new ItemSortGetItemSortDataBinHandler(this));
            AddHandler(new ItemSortSetItemSortdataBinHandler(this));
            AddHandler(new ItemUseBagItemHandler(this));

            AddHandler(new JobChangeJobHandler(this));
            AddHandler(new JobGetJobChangeListHandler(this));

            AddHandler(new LoadingInfoLoadingGetInfoHandler(this));

            AddHandler(new LobbyLobbyJoinHandler(this));
            AddHandler(new LobbyLobbyLeaveHandler(this));
            AddHandler(new LobbyLobbyChatMsgHandler(this));
            AddHandler(new LobbyLobbyDataMsgHandler(this));

            AddHandler(new MailMailGetListDataHandler(this));
            AddHandler(new MailMailGetListFootHandler(this));
            AddHandler(new MailMailGetListHeadHandler(this));
            AddHandler(new MailSystemMailGetListDataHandler(this));
            AddHandler(new MailSystemMailGetListFootHandler(this));
            AddHandler(new MailSystemMailGetListHeadHandler(this));

            AddHandler(new MandragoraGetMyMandragoraHandler(this));

            AddHandler(new MyRoomFurnitureListGetHandler(this));
            AddHandler(new MyRoomMyRoomBgmUpdateHandler(this));
            AddHandler(new MyRoomUpdatePlanetariumHandler(this));

            AddHandler(new NpcGetExtendedFacilityHandler(this));

            AddHandler(new OrbDevoteGetOrbGainExtendParamHandler(this));

            AddHandler(new PartnerPawnPawnLikabilityReleasedRewardListGetHandler(this));
            AddHandler(new PartnerPawnPawnLikabilityRewardListGetHandler(this));

            AddHandler(new PartyPartyCreateHandler(this));
            AddHandler(new PartyPartyInviteCharacterHandler(this));
            AddHandler(new PartyPartyInviteEntryHandler(this));
            AddHandler(new PartyPartyInvitePrepareAcceptHandler(this));
            AddHandler(new PartyPartyJoinHandler(this));
            AddHandler(new PartyPartyLeaveHandler(this));
            AddHandler(new PartySendBinaryMsgHandler(this));

            AddHandler(new PawnGetMypawnDataHandler(this));
            AddHandler(new PawnGetMyPawnListHandler(this));
            AddHandler(new PawnGetNoraPawnListHandler(this));
            AddHandler(new PawnGetPawnHistoryListHandler(this));
            AddHandler(new PawnGetRentedPawnListHandler(this));
            AddHandler(new PawnJoinPartyMypawnHandler(this));
            AddHandler(new PawnTrainingGetPreparetionInfoToAdviceHandler(this));

            AddHandler(new PawnGetLostPawnListHandler(this));
            AddHandler(new QuestEndDistributionQuestCancelHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestListHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestNoticeHandler(this));
            AddHandler(new QuestGetAreaBonusListHandler(this));
            AddHandler(new QuestGetCycleContentsStateListHandler(this));
            AddHandler(new QuestGetLevelBonusListHandler(this));
            AddHandler(new QuestGetLotQuestListHandler(this));
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

            AddHandler(new SkillChangeExSkillHandler(this));
            AddHandler(new SkillGetAbilityCostHandler(this));
            AddHandler(new SkillGetAcquirableAbilityListHandler(this));
            AddHandler(new SkillGetAcquirableSkillListHandler(this));
            AddHandler(new SkillGetCurrentSetSkillListHandler(this));
            AddHandler(new SkillGetLearnedAbilityListHandler(this));
            AddHandler(new SkillGetLearnedNormalSkillListHandler(this));
            AddHandler(new SkillGetLearnedSkillListHandler(this));
            AddHandler(new SkillGetPresetAbilityListHandler(this));
            AddHandler(new SkillGetSetAbilityListHandler(this));
            AddHandler(new SkillGetSetSkillListHandler(this));
            AddHandler(new SkillSetOffSkillHandler(this));
            AddHandler(new SkillSetSkillHandler(this));
            AddHandler(new SetShortcutHandler(this));
            AddHandler(new SetCommunicationShortcutHandler(this));

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
            AddHandler(new WarpReleaseWarpPointHandler(this));
            AddHandler(new WarpWarpHandler(this));
        }
    }
}
