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

using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.GameServer.Chat.Command;
using Arrowgene.Ddon.GameServer.Chat.Log;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Handler;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Server.Scripting.interfaces;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer
{
    public class DdonGameServer : DdonServer<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DdonGameServer));

        public DdonGameServer(GameServerSetting setting, GameLogicSetting gameLogicSettings, IDatabase database, AssetRepository assetRepository)
            : base(ServerType.Game, setting.ServerSetting, database, assetRepository)
        {
            ServerSetting = new GameServerSetting(setting);
            GameLogicSettings = gameLogicSettings;
            ScriptManager = new GameServerScriptManager(this);
            ClientLookup = new GameClientLookup();
            ChatLogHandler = new ChatLogHandler();
            ChatManager = new ChatManager(this);
            ItemManager = new ItemManager(this);
            CraftManager = new CraftManager(this);
            PartyManager = new PartyManager(this);
            ExpManager = new ExpManager(this, ClientLookup);
            PPManager = new PlayPointManager(this);
            JobManager = new JobManager(this);
            EquipManager = new EquipManager();
            ShopManager = new ShopManager(assetRepository, database);
            WalletManager = new WalletManager(this);
            CharacterManager = new CharacterManager(this);
            BazaarManager = new BazaarManager(this);
            RewardManager = new RewardManager(this);
            StampManager = new StampManager(this);
            HubManager = new HubManager(this);
            GpCourseManager = new GpCourseManager(this);
            WeatherManager = new WeatherManager(this);
            PartyQuestContentManager = new PartyQuestContentManager(this);
            DungeonManager = new DungeonManager(this);
            BoardManager = new BoardManager(this);
            TimerManager = new TimerManager(this);
            ClanManager = new ClanManager(this);
            RpcManager = new RpcManager(this);
            EpitaphRoadManager = new EpitaphRoadManager(this);
            ScheduleManager = new ScheduleManager(this);
            AreaRankManager = new AreaRankManager(this);

            // Orb Management is slightly complex and requires updating fields across multiple systems
            OrbUnlockManager = new OrbUnlockManager(database, WalletManager, JobManager, CharacterManager);

            S2CStageGetStageListRes stageListPacket =
                EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19);
            StageList = stageListPacket.StageList;
        }

        public event EventHandler<ClientConnectionChangeArgs> ClientConnectionChangeEvent;
        public GameServerSetting ServerSetting { get; }
        public GameLogicSetting GameLogicSettings { get; }
        public GameServerScriptManager ScriptManager { get; }
        public ChatManager ChatManager { get; }
        public ItemManager ItemManager { get; }
        public CraftManager CraftManager { get; }
        public PartyManager PartyManager { get; }
        public ExpManager ExpManager { get; }
        public PlayPointManager PPManager { get; }
        public ShopManager ShopManager { get; }
        public JobManager JobManager { get; }
        public OrbUnlockManager OrbUnlockManager { get; }
        public WalletManager WalletManager { get; }
        public CharacterManager CharacterManager { get; }
        public EquipManager EquipManager { get; }
        public BazaarManager BazaarManager { get; }
        public RewardManager RewardManager { get; }
        public StampManager StampManager { get; }
        public HubManager HubManager { get; }
        public GpCourseManager GpCourseManager { get; }
        public WeatherManager WeatherManager { get; }
        public PartyQuestContentManager PartyQuestContentManager { get; }
        public DungeonManager DungeonManager { get; }
        public BoardManager BoardManager { get; }
        public TimerManager TimerManager { get; }
        public ClanManager ClanManager { get; }
        public RpcManager RpcManager { get; }
        public EpitaphRoadManager EpitaphRoadManager { get; }
        private ScheduleManager ScheduleManager { get; }
        public AreaRankManager AreaRankManager { get; }

        public ChatLogHandler ChatLogHandler { get; }

        public List<CDataStageInfo> StageList { get; }

        public override GameClientLookup ClientLookup { get; }

        // TODO: Maybe place somewhere else
        public readonly Dictionary<uint, DateTime> LastRevivalPowerRechargeTime = new Dictionary<uint, DateTime>();

        public override void Start()
        {
            ScriptManager.Initialize();

            QuestManager.LoadQuests(this);
            GpCourseManager.EvaluateCourses();

            if (ServerUtils.IsHeadServer(this))
            {
                ScheduleManager.StartServerTasks();
            }

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
            ClientLookup.Remove(client);

            Account account = client.Account;
            if (account != null)
            {
                Database.DeleteConnection(Id, client.Account.Id);
            }

            PartyGroup party = client.Party;
            if (party != null)
            {
                party.Leave(client);
            }

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
            GameClient newClient = new GameClient(socket,
                new PacketFactory(ServerSetting.ServerSetting, PacketIdResolver.GamePacketIdResolver), this);
            ClientLookup.Add(newClient);
            return newClient;
        }

        private void LoadChatHandler()
        {
            ChatManager.AddHandler(ChatLogHandler);
            ChatManager.AddHandler(new ChatCommandHandler(this));
        }

        private void LoadPacketHandler()
        {
            SetFallbackHandler(new FallbackHandler<GameClient>(this));
            
            AddHandler(new AchievementGetReceivableRewardListHandler(this));
            AddHandler(new AchievementGetProgressListHandler(this));
            AddHandler(new AchievementGetRewardListHandler(this));
            AddHandler(new AchievementGetFurnitureRewardListHandler(this));
            AddHandler(new AchievementRewardReceiveHandler(this));
            AddHandler(new AchievementGetCategoryProgressListHandler(this));

            AddHandler(new AreaGetAreaBaseInfoListHandler(this));
            AddHandler(new AreaGetAreaMasterInfoHandler(this));
            AddHandler(new AreaGetAreaQuestHintListHandler(this));
            AddHandler(new AreaGetAreaSupplyInfoHandler(this));
            AddHandler(new AreaGetLeaderAreaReleaseListHandler(this));
            AddHandler(new AreaGetSpotInfoListHandler(this));
            AddHandler(new AreaAreaRankUpHandler(this));
            AddHandler(new AreaGetAreaSupplyHandler(this));

            AddHandler(new BattleContentInfoListHandler(this));
            AddHandler(new BattleContentGetContentStatusFromOmHandler(this));
            AddHandler(new BattleContentContentEntryHandler(this));
            AddHandler(new BattleContentInstantClearInfoHandler(this));
            AddHandler(new BattleContentPartyMemberInfoHandler(this));
            AddHandler(new BattleContentPartyMemberInfoUpdateHandler(this));
            AddHandler(new BattleContentContentFirstPhaseChangeHandler(this));
            AddHandler(new BattleContentCharacterInfoHandler(this));
            AddHandler(new BattleContentRewardListHandler(this));
            AddHandler(new BattleContentResetInfoHandler(this));
            AddHandler(new BattleContentGetRewardHandler(this));
            AddHandler(new BattleContentContentResetHandler(this));

            AddHandler(new BazaarCancelHandler(this));
            AddHandler(new BazaarExhibitHandler(this));
            AddHandler(new BazaarGetCharacterListHandler(this));
            AddHandler(new BazaarGetExhibitPossibleNumHandler(this));
            AddHandler(new BazaarGetItemHistoryInfoHandler(this));
            AddHandler(new BazaarGetItemInfoHandler(this));
            AddHandler(new BazaarGetItemListHandler(this));
            AddHandler(new BazaarGetItemPriceLimitHandler(this));
            AddHandler(new BazaarProceedsHandler(this));
            AddHandler(new BazaarReceiveProceedsHandler(this));
            AddHandler(new BazaarReExhibitHandler(this));

            AddHandler(new BinarySaveSetCharacterBinSavedataHandler(this));
            AddHandler(new BlackListGetBlackListHandler(this));

            AddHandler(new ActionSetPlayerActionHistoryHandler(this));

            AddHandler(new CharacterCommunityCharacterStatusGetHandler(this));
            AddHandler(new CharacterDecideCharacterIdHandler(this));
            AddHandler(new CharacterGetReviveChargeableTimeHandler(this));
            AddHandler(new CharacterCharacterGoldenReviveHandler(this));
            AddHandler(new CharacterCharacterPenaltyReviveHandler(this));
            AddHandler(new CharacterCharacterPointReviveHandler(this));
            AddHandler(new CharacterCharacterSearchHandler(this));
            AddHandler(new CharacterChargeRevivePointHandler(this));
            AddHandler(new CharacterPawnGoldenReviveHandler(this));
            AddHandler(new CharacterPawnPointReviveHandler(this));
            AddHandler(new CharacterSetOnlineStatusHandler(this));
			AddHandler(new CharacterEditGetShopPriceHandler(this));
			AddHandler(new CharacterEditUpdateCharacterEditParamHandler(this));
            AddHandler(new CharacterEditUpdateCharacterEditParamExHandler(this));
			AddHandler(new CharacterEditUpdatePawnEditParamHandler(this));
            AddHandler(new CharacterEditUpdatePawnEditParamExHandler(this));
            AddHandler(new CharacterCharacterDeadHandler(this));
            AddHandler(new CharacterCharacterDownCancelHandler(this));
            AddHandler(new CharacterCharacterDownHandler(this));
            AddHandler(new CharacterPawnDeadHandler(this));
            AddHandler(new CharacterPawnDownCancelHandler(this));
            AddHandler(new CharacterPawnDownHandler(this));
            AddHandler(new CharacterSwitchGameModeHandler(this));
            AddHandler(new CharacterCreateModeCharacterEditHandler(this));

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
            AddHandler(new ClanClanScoutEntryGetMyHandler(this));
            AddHandler(new ClanClanScoutEntryGetInvitedListHandler(this));
            AddHandler(new ClanClanGetMyJoinRequestListHandler(this));
            AddHandler(new ClanClanCreateHandler(this));
            AddHandler(new ClanClanGetHistoryHandler(this));
            AddHandler(new ClanClanUpdateHandler(this));
            AddHandler(new ClanClanInviteHandler(this));
            AddHandler(new ClanClanGetInfoHandler(this));
            AddHandler(new ClanClanInviteAcceptHandler(this));
            AddHandler(new ClanClanScoutEntrySearchHandler(this));
            AddHandler(new ClanClanSearchHandler(this));
            AddHandler(new ClanClanScoutEntryGetInviteListHandler(this));
            AddHandler(new ClanClanLeaveMemberHandler(this));
            AddHandler(new ClanClanGetMemberListHandler(this));
            AddHandler(new ClanClanExpelMemberHandler(this));
            AddHandler(new ClanClanSetMemberRankHandler(this));
            AddHandler(new ClanClanNegotiateMasterHandler(this));
            AddHandler(new ClanClanBaseReleaseHandler(this));
            AddHandler(new ClanClanShopGetFunctionItemListHandler(this));
            AddHandler(new ClanClanShopGetBuffItemListHandler(this));
            AddHandler(new ClanClanShopBuyFunctionItemHandler(this));
            AddHandler(new ClanClanShopBuyBuffItemHandler(this));

            AddHandler(new ClientChallengeHandler(this));

            AddHandler(new ConnectionGetLoginAnnouncementHandler(this));
            AddHandler(new ConnectionLoginHandler(this));
            AddHandler(new ConnectionLogoutHandler(this));
            AddHandler(new ConnectionMoveInServerHandler(this));
            AddHandler(new ConnectionMoveOutServerHandler(this));
            AddHandler(new ConnectionPingHandler(this));
            AddHandler(new ConnectionReserveServerHandler(this));

            AddHandler(new ContextGetSetContextHandler(this));
            AddHandler(new ContextMasterThrowHandler(this));
            AddHandler(new ContextSetContextHandler(this));
            AddHandler(new Context_35_5_16_Handler(this));

            AddHandler(new CraftGetCraftIRCollectionValueListHandler(this));
            AddHandler(new CraftGetCraftProgressListHandler(this));
            AddHandler(new CraftGetCraftSettingHandler(this));
            AddHandler(new CraftRecipeGetCraftRecipeHandler(this));
            AddHandler(new CraftStartCraftHandler(this));
            AddHandler(new CraftSkillUpHandler(this));
            AddHandler(new CraftGetCraftProductInfoHandler(this));
            AddHandler(new CraftCancelCraftHandler(this));
            AddHandler(new CraftTimeSaveHandler(this));
            AddHandler(new CraftGetCraftProductHandler(this));
            AddHandler(new CraftResetCraftpointHandler(this));
            AddHandler(new CraftStartEquipGradeUpHandler(this));
            AddHandler(new CraftStartQualityUpHandler(this));
            AddHandler(new CraftSkillAnalyzeHandler(this));
            AddHandler(new CraftRecipeGetGradeupRecipeHandler(this));
            AddHandler(new CraftStartEquipColorChangeHandler(this));
            AddHandler(new CraftStartAttachElementHandler(this));
            AddHandler(new CraftStartDetachElementHandler(this));

            AddHandler(new DailyMissionListGetHandler(this));
            AddHandler(new DispelGetDispelItemSettingsHandler(this));
            AddHandler(new DispelGetDispelItemListHandler(this));
            AddHandler(new DispelExchangeDispelItemHandler(this));

            AddHandler(new EquipChangeCharacterEquipHandler(this));
            AddHandler(new EquipChangeCharacterEquipJobItemHandler(this));
            AddHandler(new EquipChangeCharacterStorageEquipHandler(this));
            AddHandler(new EquipChangePawnEquipHandler(this));
            AddHandler(new EquipChangePawnEquipJobItemHandler(this));
            AddHandler(new EquipChangePawnStorageEquipHandler(this));
            AddHandler(new EquipEnhancedGetPacks(this));
            AddHandler(new EquipGetCharacterEquipListHandler(this));
            AddHandler(new EquipGetCraftLockedElementListHandler(this));
            AddHandler(new EquipUpdateHideCharacterHeadArmorHandler(this));
            AddHandler(new EquipUpdateHideCharacterLanternHandler(this));
            AddHandler(new EquipUpdateHidePawnHeadArmorHandler(this));
            AddHandler(new EquipUpdateHidePawnLanternHandler(this));

            AddHandler(new EventStartHandler(this));
            AddHandler(new EventEndHandler(this));

            AddHandler(new FriendGetFriendListHandler(this));
            AddHandler(new FriendApplyFriendListHandler(this));
            AddHandler(new FriendApproveFriendListHandler(this));
            AddHandler(new FriendRemoveFriendHandler(this));
            AddHandler(new FriendRegisterFavoriteFriendHandler(this));
            AddHandler(new FriendCancelFriendApplicationHandler(this));
            AddHandler(new FriendGetRecentCharacterListHandler(this));

            AddHandler(new GpGetGpDetailHandler(this));
            AddHandler(new GpGetGpPeriodHandler(this));
            AddHandler(new GpGetUpdateAppCourseBonusFlagHandler(this));
            AddHandler(new GpGetValidChatComGroupHandler(this));
            AddHandler(new GpGpEditGetVoiceListHandler(this));
            AddHandler(new GpGetGpHandler(this));
            AddHandler(new GpGpCourseGetAvailableListHandler(this));

            AddHandler(new GroupChatGroupChatGetMemberListHandler(this));

            AddHandler(new InnGetPenaltyHealStayPrice(this));
            AddHandler(new InnGetStayPriceHandler(this));
            AddHandler(new InnStayInnHandler(this));
            AddHandler(new InnStayPenaltyHealInn(this));
            AddHandler(new InnHpRecoveryCompleteHandler(this));

            AddHandler(new InstanceEnemyGroupEntryHandler(this));
            AddHandler(new InstanceEnemyGroupLeaveHandler(this));
            AddHandler(new InstanceEnemyKillHandler(this));
            AddHandler(new InstanceExchangeOmInstantKeyValueHandler(this));
            AddHandler(new InstanceGetDropItemListHandler(this));
            AddHandler(new InstanceGetDropItemHandler(this));
            AddHandler(new InstanceGetEnemySetListHandler(this));
            AddHandler(new InstanceGetGatheringItemHandler(this));
            AddHandler(new InstanceGetGatheringItemListHandler(this));
            AddHandler(new InstanceGetItemSetListHandler(this));
            AddHandler(new InstanceSetOmInstantKeyValueHandler(this));
            AddHandler(new InstanceTreasurePointGetCategoryListHandler(this));
            AddHandler(new InstanceTreasurePointGetListHandler(this));
            AddHandler(new InstanceCharacterEndBadStatusHandler(this));
            AddHandler(new InstanceCharacterStartBadStatusHandler(this));
            AddHandler(new InstancePlTouchOmHandler(this));
            AddHandler(new InstanceGetOmInstantKeyValueAllHandler(this));
            AddHandler(new InstanceTraningRoomGetEnemyListHandler(this));
            AddHandler(new InstanceTraningRoomSetEnemyHandler(this));
            AddHandler(new InstanceEnemyBadStatusStartHandler(this));
            AddHandler(new InstanceEnemyBadStatusEndHandler(this));

            AddHandler(new ItemConsumeStorageItemHandler(this));
            AddHandler(new ItemGetStorageItemListHandler(this));
            AddHandler(new ItemMoveItemHandler(this));
            AddHandler(new ItemSellItemHandler(this));
            AddHandler(new ItemSortGetItemSortDataBinHandler(this));
            AddHandler(new ItemSortSetItemSortDataBinHandler(this));
            AddHandler(new ItemUseBagItemHandler(this));
            AddHandler(new ItemUseJobItemsHandler(this));
            AddHandler(new ItemGetValuableItemListHandler(this));
            AddHandler(new ItemGetPostItemListHandler(this));
            AddHandler(new ItemGetDefaultStorageEmptySlotNumHandler(this));
            AddHandler(new ItemEmbodyPayCostHandler(this));
            AddHandler(new ItemGetSpecifiedHavingItemListHandler(this));
            AddHandler(new ItemEmbodyItemsHandler(this));
            AddHandler(new ItemChangeAttrDiscardHandler(this));

            AddHandler(new JobChangeJobHandler(this));
            AddHandler(new JobChangePawnJobHandler(this));
            AddHandler(new JobGetJobChangeListHandler(this));
            AddHandler(new JobUpdateExpModeHandler(this));
            AddHandler(new JobGetPlayPointListHandler(this));
            AddHandler(new JobJobValueShopGetLineupHandler(this));
            AddHandler(new JobJobValueShopBuyItemHandler(this));

            AddHandler(new JobOrbTreeGetJobOrbTreeStatusListHandler(this));
            AddHandler(new JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler(this));

            AddHandler(new LoadingInfoLoadingGetInfoHandler(this));

            AddHandler(new ChatSendTellMsgHandler(this));
            
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
            AddHandler(new MailSystemMailGetTextHandler(this));
            AddHandler(new MailSystemMailGetAllItemHandler(this));
            AddHandler(new MailSystemMailDeleteHandler(this));

            AddHandler(new MandragoraGetMyMandragoraHandler(this));

            AddHandler(new MyRoomFurnitureListGetHandler(this));
            AddHandler(new MyRoomMyRoomBgmUpdateHandler(this));
            AddHandler(new MyRoomUpdatePlanetariumHandler(this));

            AddHandler(new NpcGetExtendedFacilityHandler(this));

            AddHandler(new OrbDevoteGetOrbGainExtendParamHandler(this));
            AddHandler(new OrbDevoteGetReleaseOrbElementListHandler(this));
            AddHandler(new OrbDevoteReleaseOrbElementHandler(this));
            AddHandler(new OrbDevoteGetPawnReleaseOrbElementListHandler(this));
            AddHandler(new OrbDevoteReleasePawnOrbElementHandler(this));

            AddHandler(new PartnerPawnPawnLikabilityReleasedRewardListGetHandler(this));
            AddHandler(new PartnerPawnPawnLikabilityRewardListGetHandler(this));

            AddHandler(new PartyMemberSetValueHandler(this));
            AddHandler(new PartyPartyBreakupHandler(this));
            AddHandler(new PartyPartyChangeLeaderHandler(this));
            AddHandler(new PartyPartyCreateHandler(this));
            AddHandler(new PartyPartyInviteCancelHandler(this));
            AddHandler(new PartyPartyInviteCharacterHandler(this));
            AddHandler(new PartyPartyInviteEntryHandler(this));
            AddHandler(new PartyPartyInvitePrepareAcceptHandler(this));
            AddHandler(new PartyPartyInviteRefuseHandler(this));
            AddHandler(new PartyPartyJoinHandler(this));
            AddHandler(new PartyPartyLeaveHandler(this));
            AddHandler(new PartyPartyMemberKickHandler(this));
            AddHandler(new PartySendBinaryMsgAllHandler(this));
            AddHandler(new PartySendBinaryMsgHandler(this));

            AddHandler(new PawnGetLostPawnListHandler(this));
            AddHandler(new PawnGetMypawnDataHandler(this));
            AddHandler(new PawnGetMyPawnListHandler(this));
            AddHandler(new PawnGetNoraPawnListHandler(this));
            AddHandler(new PawnGetPartyPawnDataHandler(this));
            AddHandler(new PawnGetPawnHistoryListHandler(this));
            AddHandler(new PawnGetPawnTotalScoreHandler(this));
            AddHandler(new PawnGetRegisteredPawnDataHandler(this));
            AddHandler(new PawnGetRentedPawnDataHandler(this));
            AddHandler(new PawnGetRentedPawnListHandler(this));
            AddHandler(new PawnJoinPartyMypawnHandler(this));
            AddHandler(new PawnLostPawnGoldenReviveHandler(this));
            AddHandler(new PawnLostPawnPointReviveHandler(this));
            AddHandler(new PawnLostPawnReviveHandler(this));
            AddHandler(new PawnLostPawnWalletReviveHandler(this));
            AddHandler(new PawnPawnLostHandler(this));
            AddHandler(new PawnRentalPawnLostHandler(this));
            AddHandler(new PawnSpSkillDeleteStockSkillHandler(this));
            AddHandler(new PawnSpSkillGetActiveSkillHandler(this));
            AddHandler(new PawnSpSkillGetStockSkillHandler(this));
            AddHandler(new PawnSpSkillSetActiveSkillHandler(this));
            AddHandler(new PawnTrainingGetPreparetionInfoToAdviceHandler(this));
            AddHandler(new PawnTrainingGetTrainingStatusHandler(this));
            AddHandler(new PawnTrainingSetTrainingStatusHandler(this));
            AddHandler(new PawnCreatePawnHandler(this));
            AddHandler(new PawnDeleteMyPawnHandler(this));
            AddHandler(new PawnGetRegisteredPawnListHandler(this));
            AddHandler(new PawnGetOfficialPawnListHandler(this));
            AddHandler(new PawnRentRegisteredPawnHandler(this));
            AddHandler(new PawnJoinPartyRentedPawnHandler(this));
            AddHandler(new PawnReturnRentedPawnHandler(this));
            AddHandler(new PawnUpdatePawnReactionListHandler(this));

            AddHandler(new PawnExpeditionGetSallyInfoHandler(this));

            AddHandler(new PhotoPhotoTakeHandler(this));

            AddHandler(new ProfileGetCharacterProfileHandler(this));
            AddHandler(new ProfileGetMyCharacterProfileHandler(this));

            AddHandler(new QuestCancelPriorityQuestHandler(this));
            AddHandler(new QuestEndDistributionQuestCancelHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestListHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestNoticeHandler(this));
            AddHandler(new QuestGetAreaBonusListHandler(this));
            AddHandler(new QuestGetAreaInfoListHandler(this));
            AddHandler(new QuestGetCycleContentsStateListHandler(this));
            AddHandler(new QuestGetLevelBonusListHandler(this));
            AddHandler(new QuestGetLightQuestList(this));
            AddHandler(new QuestGetLotQuestListHandler(this));
            AddHandler(new QuestGetMainQuestListHandler(this));
            AddHandler(new QuestGetPackageQuestListHandler(this));
            AddHandler(new QuestGetPartyQuestProgressInfoHandler(this));
            AddHandler(new QuestGetPriorityQuestHandler(this));
            AddHandler(new QuestGetQuestCompletedListHandler(this));
            AddHandler(new QuestGetQuestPartyBonusListHandler(this));
            AddHandler(new QuestGetRewardBoxListHandler(this));
            AddHandler(new QuestGetRewardBoxItemHandler(this));
            AddHandler(new QuestGetSetQuestInfoListHandler(this));
            AddHandler(new QuestGetSetQuestListHandler(this));
            AddHandler(new QuestGetTutorialQuestListHandler(this));
            AddHandler(new QuestGetWorldManageQuestListHandler(this));
            AddHandler(new QuestLeaderQuestProgressRequestHandler(this));
			AddHandler(new QuestGetEndContentsGroupHandler(this));
			AddHandler(new QuestGetCycleContentsNewsListHandler(this));
            AddHandler(new QuestQuestOrderHandler(this));
            AddHandler(new QuestQuestProgressHandler(this));
            AddHandler(new QuestSendLeaderQuestOrderConditionInfoHandler(this));
            AddHandler(new QuestSendLeaderWaitOrderQuestListHandler(this));
            AddHandler(new QuestSetPriorityQuestHandler(this));
            AddHandler(new QuestQuestLogInfoHandler(this));
            AddHandler(new QuestQuestCompleteFlagClearHandler(this));
            AddHandler(new Quest_11_60_16_Handler(this));
            AddHandler(new QuestDeliverItemHandler(this));
            AddHandler(new QuestDecideDeliveryItemHandler(this));
            AddHandler(new QuestCancelHandler(this));
            AddHandler(new QuestGetPartyBonusListHandler(this));
            AddHandler(new QuestGetMobHuntQuestListHandler(this));
            AddHandler(new QuestPlayEntryHandler(this));
            AddHandler(new QuestPlayEntryCancelHandler(this));
            AddHandler(new QuestPlayStartHandler(this));
            AddHandler(new QuestPlayStartTimerHandler(this));
            AddHandler(new QuestPlayEndHandler(this));
            AddHandler(new QuestPlayInterruptHandler(this));
            AddHandler(new QuestPlayInterruptAnswerHandler(this));
            AddHandler(new QuestGetEndContentsRecruitListHandler(this));
            AddHandler(new QuestGetQuestScheduleInfoHandler(this));

			AddHandler(new EntryBoardEntryBoardListHandler(this));
			AddHandler(new EntryBoardEntryBoardItemCreateHandler(this));
			AddHandler(new EntryBoardEntryBoardItemForceStartHandler(this));
			AddHandler(new EntryBoardEntryBoardItemInfoMyselfHandler(this));
            AddHandler(new EntryBoardEntryBoardItemReadyHandler(this));
            AddHandler(new EntryBoardEntryBoardItemLeaveHandler(this));
            AddHandler(new EntryBoardEntryItemInfoChangeHandler(this));
            AddHandler(new EntryBoardEntryBoardItemInviteHandler(this));
            AddHandler(new EntryBoardEntryBoardItemInfoHandler(this));
            AddHandler(new EntryBoardEntryBoardItemEntryHandler(this));
            AddHandler(new EntryBoardEntryBoardItemListHandler(this));
            AddHandler(new EntryBoardEntryRecreateHandler(this));
            AddHandler(new EntryBoardItemKickHandler(this));
            AddHandler(new EntryBoardEntryBoardItemExtendTimeoutHandler(this));
            AddHandler(new EntryBoardPartyRecruitCategoryListHandler(this));

            AddHandler(new SeasonDungeon62_40_16_Handler(this));
            AddHandler(new SeasonDungeonGetIdFromNpcIdHandler(this));
            AddHandler(new SeasonDungeonGetInfoHandler(this));
            AddHandler(new SeasonDungeonGetExtendableBlockadeListFromNpcIdHandler(this));
            AddHandler(new SeasonDungeonGetSoulOrdealListFromOmHandler(this));
            AddHandler(new SeasonDungeonSoulOrdealReadyHandler(this));
            AddHandler(new SeasonDungeonExecuteSoulOrdealHandler(this));
            AddHandler(new SeasonDungeonGetSoulOrdealRewardListHandler(this));
            AddHandler(new SeasonDungeonReceiveSoulOrdealBuffHandler(this));
            AddHandler(new SeasonDungeonReceiveSoulOrdealRewardHandler(this));
            AddHandler(new SeasonDungeon62_12_16_Handler(this));
            AddHandler(new SeasonDungeonGetBlockadeIdFromOmHandler(this));
            AddHandler(new SeasonDungeonGetExRequiredItemHandler(this));
            AddHandler(new SeasonDungeonDeliverItemForExHandler(this));
            AddHandler(new SeasonDungeonGetBlockadeIdFromNpcIdHandler(this));
            AddHandler(new SeasonDungeonGetStatueStateHandler(this));
            AddHandler(new SeasonDungeonUpdateKeyPointDoorStatusHandler(this));
            AddHandler(new SeasonDungeonGetSoulOrdealRewardListForViewHandler(this));
            AddHandler(new SeasonDungeonInterruptSoulOrdealHandler(this));
            AddHandler(new SeasonDungeonSoulOrdealCancelReadyHandler(this));

            AddHandler(new ServerGameTimeGetBaseinfoHandler(this));
            AddHandler(new ServerGetGameSettingHandler(this));
            AddHandler(new ServerGetRealTimeHandler(this));
            AddHandler(new ServerGetServerListHandler(this));
            AddHandler(new ServerWeatherForecastGetHandler(this));
            AddHandler(new ServerGetScreenShotCategoryHandler(this));

            AddHandler(new SkillChangeExSkillHandler(this));
            AddHandler(new SkillGetAbilityCostHandler(this));
            AddHandler(new SkillGetAcquirableAbilityListHandler(this));
            AddHandler(new SkillGetAcquirableSkillListHandler(this));
            AddHandler(new SkillGetCurrentSetSkillListHandler(this));
            AddHandler(new SkillGetLearnedAbilityListHandler(this));
            AddHandler(new SkillGetLearnedNormalSkillListHandler(this));
            AddHandler(new SkillGetLearnedSkillListHandler(this));
            AddHandler(new SkillGetPawnAbilityCostHandler(this));
            AddHandler(new SkillGetPawnLearnedAbilityListHandler(this));
            AddHandler(new SkillGetPawnLearnedNormalSkillListHandler(this));
            AddHandler(new SkillGetPawnLearnedSkillListHandler(this));
            AddHandler(new SkillGetPawnSetAbilityListHandler(this));
            AddHandler(new SkillGetPawnSetSkillListHandler(this));
            AddHandler(new SkillGetPresetAbilityListHandler(this));
            AddHandler(new SkillGetSetAbilityListHandler(this));
            AddHandler(new SkillGetSetSkillListHandler(this));
            AddHandler(new SkillLearnAbilityHandler(this));
            AddHandler(new SkillLearnNormalSkillHandler(this));
            AddHandler(new SkillLearnPawnAbilityHandler(this));
            AddHandler(new SkillLearnPawnNormalSkillHandler(this));
            AddHandler(new SkillLearnPawnSkillHandler(this));
            AddHandler(new SkillLearnSkillHandler(this));
            AddHandler(new SkillSetAbilityHandler(this));
            AddHandler(new SkillSetOffAbilityHandler(this));
            AddHandler(new SkillSetOffPawnAbilityHandler(this));
            AddHandler(new SkillSetOffPawnSkillHandler(this));
            AddHandler(new SkillSetOffSkillHandler(this));
            AddHandler(new SkillSetPawnAbilityHandler(this));
            AddHandler(new SkillSetPawnSkillHandler(this));
            AddHandler(new SkillSetSkillHandler(this));
            AddHandler(new SkillRegisterPresetAbilityHandler(this));
            AddHandler(new SkillSetPresetAbilityNameHandler(this));
            AddHandler(new SkillSetPresetAbilityListHandler(this));

            AddHandler(new SetShortcutHandler(this));
            AddHandler(new ShopBuyShopGoodsHandler(this));
            AddHandler(new ShopGetShopGoodsListHandler(this));
            AddHandler(new SetCommunicationShortcutHandler(this));

            AddHandler(new StageAreaChangeHandler(this));
            AddHandler(new StageGetStageListHandler(this));
            AddHandler(new StageGetTicketDungeonCategoryListHandler(this));
            AddHandler(new StageGetTicketDungeonInfoListHandler(this));
            AddHandler(new StageUnisonAreaChangeBeginRecruitmentHandler(this));
            AddHandler(new StageUnisonAreaChangeGetRecruitmentStateHandler(this));
            AddHandler(new StageUnisonAreaChangeReadyHandler(this));
            AddHandler(new StageUnisonAreaChangeReadyCancelHandler(this));
            AddHandler(new StageGetSpAreaChangeIdFromNpcIdHandler(this));
            AddHandler(new StageGetSpAreaChangeInfoHandler(this));

            AddHandler(new StampBonusCheckHandler(this));
			AddHandler(new StampBonusGetListHandler(this));
			AddHandler(new StampBonusReceiveDailyHandler(this));
            AddHandler(new StampBonusReceiveTotalHandler(this));

            AddHandler(new WarpAreaWarpHandler(this));
            AddHandler(new WarpGetAreaWarpPointListHandler(this));
            AddHandler(new WarpGetFavoriteWarpPointListHandler(this));
            AddHandler(new WarpGetReleaseWarpPointListHandler(this));
            AddHandler(new WarpGetReturnLocationHandler(this));
            AddHandler(new WarpGetStartPointListHandler(this));
            AddHandler(new WarpGetWarpPointListHandler(this));
            AddHandler(new WarpPartyWarpHandler(this));
            AddHandler(new WarpRegisterFavoriteWarpHandler(this));
            AddHandler(new WarpReleaseWarpPointHandler(this));
            AddHandler(new WarpWarpHandler(this));
            AddHandler(new WarpWarpStartHandler(this));
            AddHandler(new WarpWarpEndHandler(this));
        }
    }
}
