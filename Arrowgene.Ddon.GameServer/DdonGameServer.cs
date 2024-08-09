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
using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Handler;
using Arrowgene.Ddon.Server.Network;
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

        // TODO: Maybe place somewhere else
        public static readonly TimeSpan RevivalPowerRechargeTimeSpan = TimeSpan.FromDays(1);

        public DdonGameServer(GameServerSetting setting, IDatabase database, AssetRepository assetRepository)
            : base(ServerType.Game, setting.ServerSetting, database, assetRepository)
        {
            Setting = new GameServerSetting(setting);
            Router = new GameRouter();
            ClientLookup = new GameClientLookup();
            ChatLogHandler = new ChatLogHandler();
            ChatManager = new ChatManager(this, Router);
            ItemManager = new ItemManager();
            PartyManager = new PartyManager(assetRepository);
            ExpManager = new ExpManager(this, ClientLookup);
            PPManager = new PlayPointManager(database);
            JobManager = new JobManager(this);
            EquipManager = new EquipManager();
            ShopManager = new ShopManager(assetRepository, database);
            WalletManager = new WalletManager(database);
            CharacterManager = new CharacterManager(this);
            BazaarManager = new BazaarManager(this);
            RewardManager = new RewardManager(this);
            StampManager = new StampManager(this);

            // Orb Management is slightly complex and requires updating fields across multiple systems
            OrbUnlockManager = new OrbUnlockManager(database, WalletManager, JobManager, CharacterManager);

            S2CStageGetStageListRes stageListPacket =
                EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19);
            StageList = stageListPacket.StageList;
        }

        public event EventHandler<ClientConnectionChangeArgs> ClientConnectionChangeEvent;
        public GameServerSetting Setting { get; }
        public ChatManager ChatManager { get; }
        public ItemManager ItemManager { get; }
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
        public GameRouter Router { get; }

        public ChatLogHandler ChatLogHandler { get; }

        public List<CDataStageInfo> StageList { get; }

        public override GameClientLookup ClientLookup { get; }

        // TODO: Maybe place somewhere else
        public readonly Dictionary<uint, DateTime> LastRevivalPowerRechargeTime = new Dictionary<uint, DateTime>();

        public override void Start()
        {
            QuestManager.LoadQuests(this.AssetRepository);
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
                new PacketFactory(Setting.ServerSetting, PacketIdResolver.GamePacketIdResolver), ShopManager, AssetRepository);
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
            
            AddHandler(new AchievementAchievementGetReceivableRewardListHandler(this));

            AddHandler(new AreaGetAreaBaseInfoListHandler(this));
            AddHandler(new AreaGetAreaMasterInfoHandler(this));
            AddHandler(new AreaGetAreaQuestHintListHandler(this));
            AddHandler(new AreaGetAreaSupplyInfoHandler(this));
            AddHandler(new AreaGetLeaderAreaReleaseListHandler(this));
            AddHandler(new AreaGetSpotInfoListHandler(this));

            AddHandler(new BattleContentInfoListHandler(this));
            
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
            AddHandler(new ContextMasterThrowHandler(this));
            AddHandler(new ContextSetContextHandler(this));
            AddHandler(new CraftGetCraftIRCollectionValueListHandler(this));
            AddHandler(new CraftGetCraftProgressListHandler(this));
            AddHandler(new CraftGetCraftSettingHandler(this));
            AddHandler(new CraftRecipeGetCraftRecipeHandler(this));
            AddHandler(new CraftStartCraftHandler(this));
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

            AddHandler(new InstanceGetOmInstantKeyValueAllHandler(this));

            AddHandler(new ItemConsumeStorageItemHandler(this));
            AddHandler(new ItemGetStorageItemListHandler(this));
            AddHandler(new ItemMoveItemHandler(this));
            AddHandler(new ItemSellItemHandler(this));
            AddHandler(new ItemSortGetItemSortDataBinHandler(this));
            AddHandler(new ItemSortSetItemSortDataBinHandler(this));
            AddHandler(new ItemUseBagItemHandler(this));
            AddHandler(new ItemUseJobItemsHandler(this));

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
            AddHandler(new PawnPawnLostHandler(this));
            AddHandler(new PawnSpSkillDeleteStockSkillHandler(this));
            AddHandler(new PawnSpSkillGetActiveSkillHandler(this));
            AddHandler(new PawnSpSkillGetStockSkillHandler(this));
            AddHandler(new PawnSpSkillSetActiveSkillHandler(this));
            AddHandler(new PawnTrainingGetPreparetionInfoToAdviceHandler(this));
            AddHandler(new PawnTrainingGetTrainingStatusHandler(this));
            AddHandler(new PawnTrainingSetTrainingStatusHandler(this));
            AddHandler(new PawnCreatePawnHandler(this));

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
			AddHandler(new QuestGetEndContentsGroup(this));
			AddHandler(new QuestGetCycleContentsNewsList(this));
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

			AddHandler(new EntryBoardEntryBoardList(this));
			AddHandler(new EntryBoardEntryBoardItemCreate(this));
			AddHandler(new EntryBoardEntryBoardItemForceStart(this));
			AddHandler(new EntryBoardEntryBoardItemInfoMyself(this));

            AddHandler(new ServerGameTimeGetBaseinfoHandler(this));
            AddHandler(new ServerGetGameSettingHandler(this));
            AddHandler(new ServerGetRealTimeHandler(this));
            AddHandler(new ServerGetServerListHandler(this));
            AddHandler(new ServerWeatherForecastGetHandler(this));

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

            AddHandler(new SetShortcutHandler(this));
            AddHandler(new ShopBuyShopGoodsHandler(this));
            AddHandler(new ShopGetShopGoodsListHandler(this));
            AddHandler(new SetCommunicationShortcutHandler(this));

            AddHandler(new StageAreaChangeHandler(this));
            AddHandler(new StageGetStageListHandler(this));

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
        }
    }
}
