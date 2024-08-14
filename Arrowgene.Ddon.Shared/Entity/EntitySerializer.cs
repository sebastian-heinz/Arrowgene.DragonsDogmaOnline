using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Entity
{
    public abstract class EntitySerializer
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(EntitySerializer));

        private static readonly Dictionary<PacketId, EntitySerializer> LoginPacketSerializers;
        private static readonly Dictionary<PacketId, EntitySerializer> GamePacketSerializers;
        private static readonly Dictionary<Type, EntitySerializer> Serializers;
        private static readonly Dictionary<PacketId, IStructurePacketFactory> LoginStructurePacketFactories;
        private static readonly Dictionary<PacketId, IStructurePacketFactory> GameStructurePacketFactories;

        static EntitySerializer()
        {
            LoginPacketSerializers = new Dictionary<PacketId, EntitySerializer>();
            GamePacketSerializers = new Dictionary<PacketId, EntitySerializer>();
            Serializers = new Dictionary<Type, EntitySerializer>();
            LoginStructurePacketFactories = new Dictionary<PacketId, IStructurePacketFactory>();
            GameStructurePacketFactories = new Dictionary<PacketId, IStructurePacketFactory>();

            // Data structure serializers
            Create(new C2SActionSetPlayerActionHistoryReqElement.Serializer()); // TODO naming convention C2S -> not a packet
            Create(new CData_772E80.Serializer());
            Create(new CDataSpSkill.Serializer());
            Create(new CDataAbilityLevelParam.Serializer());
            Create(new CDataAbilityParam.Serializer());
            Create(new CDataAchieveCategoryStatus.Serializer());
            Create(new CDataAchievementIdentifierSerializer());
            Create(new CDataAllPlayerContext.Serializer());
            Create(new CDataAreaBaseInfo.Serializer());
            Create(new CDataAreaRank.Serializer());
            Create(new CDataArisenProfileSerializer());
            Create(new CDataCharacterEquipDataSerializer());
            Create(new CDataCharacterEquipInfo.Serializer());
            Create(new CDataCharacterInfoSerializer());
            Create(new CDataCharacterJobDataSerializer());
            Create(new CDataCharacterLevelParam.Serializer());
            Create(new CDataCharacterListElement.Serializer());
            Create(new CDataCharacterListInfoSerializer());
            Create(new CDataCharacterMessageSerializer());
            Create(new CDataCharacterMsgSetSerializer());
            Create(new CDataCharacterName.Serializer());
            Create(new CDataCharacterReleaseElement.Serializer());
            Create(new CDataCharacterSearchParam.Serializer());
            Create(new CDataClanMemberInfo.Serializer());
            Create(new CDataClanParam.Serializer());
            Create(new CDataClanServerParam.Serializer());
            Create(new CDataClanUserParam.Serializer());
            Create(new CDataCommonU32.Serializer());
            Create(new CDataCommonU8.Serializer());
            Create(new CDataCommunicationShortCut.Serializer());
            Create(new CDataCommunityCharacterBaseInfo.Serializer());
            Create(new CDataContextAcquirementData.Serializer());
            Create(new CDataContextBase.Serializer());
            Create(new CDataContextBaseUnk0.Serializer());
            Create(new CDataContextEquipData.Serializer());
            Create(new CDataContextEquipJobItemData.Serializer());
            Create(new CDataContextJobData.Serializer());
            Create(new CDataContextNormalSkillData.Serializer());
            Create(new CDataContextPlayerInfo.Serializer());
            Create(new CDataContextResist.Serializer());
            Create(new CDataContextSetAdditional.Serializer());
            Create(new CDataContextSetBase.Serializer());
            Create(new CDataCraftMaterial.Serializer());
            Create(new CDataCraftPawnInfo.Serializer());
            Create(new CDataCraftPawnList.Serializer());
            Create(new CDataCraftProgress.Serializer());
            Create(new CDataCraftSkillRate.Serializer());
            Create(new CDataCraftSupportPawnID.Serializer());
            Create(new CDataCraftTimeSaveCost.Serializer());
            Create(new CDataCraftSkillAnalyzeResult.Serializer());
            Create(new CDataCraftElement.Serializer());
            Create(new CDataCraftProductInfo.Serializer());
            Create(new CDataCraftProduct.Serializer());
            Create(new S2CCraftCraftRankUpNtc.Serializer());
            Create(new CDataCurrentEquipInfo.Serializer());
            Create(new CDataEquipSlot.Serializer());
            Create(new CDataDeliveredItem.Serializer());
            Create(new CDataDeliveredItemRecord.Serializer());
            Create(new CDataDeliveryItem.Serializer());
            Create(new CDataDispelItemCategoryInfo.Serializer());
            Create(new CDataDispelBaseItem.Serializer());
            Create(new CDataDispelBaseItemData.Serializer());
            Create(new CDataDispelLotData.Serializer());
            Create(new CDataDispelLotItem.Serializer());
            Create(new CDataDispelLotCrest.Serializer());
            Create(new CDataDispelLotCrestUnk2.Serializer());
            Create(new CDataDispelLotColor.Serializer());
            Create(new CDataDispelLotPlus.Serializer());
            Create(new CDataGetDispelItem.Serializer());
            Create(new CDataDispelResultInfo.Serializer());
            Create(new CDataDropItemSetInfo.Serializer());
            Create(new CDataEditInfoSerializer());
            Create(new CDataEditParam.Serializer());
            Create(new CDataEditParamPalette.Serializer());
            Create(new CDataEquipElementParam.Serializer());
            Create(new CDataAddStatusParam.Serializer());
            Create(new CDataEquipItemInfoUnk2.Serializer());
            Create(new CDataBazaarCharacterInfo.Serializer());
            Create(new CDataBazaarItemBaseInfo.Serializer());
            Create(new CDataBazaarItemHistoryInfo.Serializer());
            Create(new CDataBazaarItemInfo.Serializer());
            Create(new CDataBazaarItemNumOfExhibitionInfo.Serializer());
            Create(new CDataChangeEquipJobItem.Serializer());
            Create(new CDataCharacterEditUpdatePawnEditParamReqUnk0.Serializer());
            Create(new CDataEquipItemInfo.Serializer());
            Create(new CDataEquipJobItem.Serializer());
            Create(new CDataErrorMessage.Serializer());
            Create(new CDataFavoriteWarpPoint.Serializer());
            Create(new CDataGameServerListInfoSerializer());
            Create(new CDataGameTimeBaseInfo.Serializer());
            Create(new CDataGatheringItemGetRequest.Serializer());
            Create(new CDataGatheringItemListUnk1.Serializer());
            Create(new CDataGatheringItemListUnk1Unk2.Serializer());
            Create(new CDataGetRewardBoxItem.Serializer());
            Create(new CDataGoodsParam.Serializer());
            Create(new CDataGoodsParamUnk7.Serializer());
            Create(new CDataGatheringItemElement.Serializer());
            Create(new CDataGPDetail.Serializer());
            Create(new CDataGPPeriod.Serializer());
            Create(new CDataGPCourseValidSerializer());
            Create(new CDataGPCourseInfoSerializer());
            Create(new CDataGPCourseEffectParamSerializer());
            Create(new CDataGPCourseAvailableSerializer());
            Create(new CDataHistoryElement.Serializer());
            Create(new CDataItemEquipElement.Serializer());
            Create(new CDataItemEquipElementParam.Serializer());
            Create(new CDataItemList.Serializer());
            Create(new CDataItemSort.Serializer());
            Create(new CDataItemStorageIndicateNum.Serializer());
            Create(new CDataItemUIdList.Serializer());
            Create(new CDataItemUpdateResult.Serializer());
            Create(new CDataJobChangeInfo.Serializer());
            Create(new CDataJobChangeJobResUnk0.Serializer());
            Create(new CDataCharacterItemSlotInfo.Serializer());
            Create(new CDataJobBaseInfo.Serializer());
            Create(new CDataJobOrbTreeStatus.Serializer());
            Create(new CDataJobOrbDevoteElement.Serializer());
            Create(new CDataReleaseOrbElement.Serializer());
            Create(new CDataJobExpMode.Serializer());
            Create(new CDataJobPlayPointSerializer());
            Create(new CDataJobValueShopItem.Serializer());
            Create(new CDataJumpLocationSerializer());
            Create(new CDataLayoutEnemyData.Serializer());
            Create(new CDataLearnedSetAcquirementParam.Serializer());
            Create(new CDataLearnNormalSkillParam.Serializer());
            Create(new CDataLightQuestDetail.Serializer());
            Create(new CDataLightQuestList.Serializer());
            Create(new CDataLightQuestOrderList.Serializer());
            Create(new CDataQuestOrderListUnk8.Serializer());
            Create(new CDataLobbyContextPlayer.Serializer());
            Create(new CDataLobbyMemberInfoSerializer());
            Create(new CDataLoginSettingSerializer());
            Create(new CDataLotQuestOrderList.Serializer());
            Create(new CDataMailInfo.Serializer());
            Create(new CDataMailTextInfo.Serializer());
            Create(new CDataMailAttachmentList.Serializer());
            Create(new CDataMailAttachmentInfo.Serializer());
            Create(new CDataMailItemInfo.Serializer());
            Create(new CDataMailGPInfo.Serializer());
            Create(new CDataMailOptionCourseInfo.Serializer());
            Create(new CDataMailLegendPawnInfo.Serializer());


            Create(new CDataMainQuestList.Serializer());
            Create(new CDataMainQuestOrderList.Serializer());
            Create(new CDataMasterInfo.Serializer());
            Create(new CDataMatchingProfileSerializer());
            Create(new CDataMDataCraftMaterial.Serializer());
            Create(new CDataMDataCraftRecipe.Serializer());
            Create(new CDataMDataCraftGradeupRecipe.Serializer());;
            Create(new CDataCraftStartEquipGradeUpUnk0.Serializer());
            Create(new CDataCraftStartEquipGradeUpUnk0Unk0.Serializer());
            Create(new CDataCraftColorant.Serializer());
            Create(new CDataMoonSchedule.Serializer());
            Create(new CDataMoveItemUIDFromTo.Serializer());
            Create(new CDataNamedEnemyParamClient.Serializer());
            Create(new CDataNormalSkillParam.Serializer());
            Create(new CDataOcdActive.Serializer());
            Create(new CDataOrbCategoryStatusSerializer());
            Create(new CDataOrbGainExtendParam.Serializer());
            Create(new CDataOrbPageStatusSerializer());
            Create(new CDataOrderConditionInfo.Serializer());
            Create(new CDataPartnerPawnInfo.Serializer());
            Create(new CDataPartyContextPawn.Serializer());
            Create(new CDataPartyListInfo.Serializer());
            Create(new CDataPartyMember.Serializer());
            Create(new CDataPartyMemberMinimum.Serializer());
            Create(new CDataPartyPlayerContext.Serializer());
            Create(new CDataPartyQuestProgressInfo.Serializer());
            Create(new CDataPawnCraftData.Serializer());
            Create(new CDataPawnCraftSkill.Serializer());
            Create(new CDataPawnEquipInfo.Serializer());
            Create(new CDataPawnFeedback.Serializer());
            Create(new CDataPawnHistory.Serializer());
            Create(new CDataPawnHp.Serializer());
            Create(new CDataPawnJobChangeInfo.Serializer());
            Create(new CDataPawnInfo.Serializer());
            Create(new CDataPawnList.Serializer());
            Create(new CDataPawnListData.Serializer());
            Create(new CDataPawnReaction.Serializer());
            Create(new CDataPawnTotalScore.Serializer());
            Create(new CDataPawnTrainingPreparationInfoToAdvice.Serializer());
            Create(new CDataPlayPointDataSerializer());
            Create(new CDataPresetAbilityParam.Serializer());
            Create(new CDataPriorityQuest.Serializer());
            Create(new CDataPriorityQuestSetting.Serializer());
            Create(new CDataQuestAnnounce.Serializer());
            Create(new CDataQuestProcessState.MtTypedArrayCDataQuestCommand.Serializer());
            Create(new CDataQuestCommand.Serializer());
            Create(new CDataQuestContents.Serializer());
            Create(new CDataQuestDefine.Serializer());
            Create(new CDataQuestEnemyInfo.Serializer());
            Create(new CDataQuestFlag.Serializer());
            Create(new CDataQuestId.Serializer());
            Create(new CDataQuestIdScheduleId.Serializer());
            Create(new CDataQuestKeyItemPoint.Serializer());
            Create(new CDataQuestKeyItemPointRecord.Serializer());
            Create(new CDataQuestLayoutFlag.Serializer());
            Create(new CDataQuestLayoutFlagSetInfo.Serializer());
            Create(new CDataQuestList.Serializer());
            Create(new CDataQuestExp.Serializer());
            Create(new CDataQuestListUnk7.Serializer());
            Create(new CDataQuestLog.Serializer());
            Create(new CDataQuestOrderConditionParam.Serializer());
            Create(new CDataQuestOrderList.Serializer());
            Create(new CDataQuestProcessState.Serializer());
            Create(new CDataQuestProgressWork.Serializer());
            Create(new CDataQuestSetInfo.Serializer());
            Create(new CDataQuestTalkInfo.Serializer());
            Create(new CDataRewardItem.Serializer());
            Create(new CDataRewardBoxRecord.Serializer());
            Create(new CDataRewardBoxItem.Serializer());
            Create(new CDataS2CCraftGetCraftSettingResUnk0.Serializer());
            Create(new CDataS2CCraftGetCraftSettingResUnk0Unk6.Serializer());
            Create(new CDataS2CCraftGetCraftSettingResUnk3.Serializer());
            Create(new CDataS2CCraftStartQualityUpResUnk0.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk6.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk9.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk10.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1.Serializer());
            Create(new CDataS2CQuestJoinLobbyQuestInfoNtcUnk0.Serializer());
            Create(new CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1.Serializer());
            Create(new CDataWildHuntQuestOrderList.Serializer());
            Create(new CDataSetAcquirementParam.Serializer());
            Create(new CDataSetQuestDetail.Serializer());
            Create(new CDataSetQuestList.Serializer());
            Create(new CDataSetQuestOrderList.Serializer());
            Create(new CDataShortCut.Serializer());
            Create(new CDataSkillLevelParam.Serializer());
            Create(new CDataSkillParam.Serializer());
            Create(new CDataStageAttribute.Serializer());
            Create(new CDataStageInfo.Serializer());
            Create(new CDataStageLayoutEnemyPresetEnemyInfoClient.Serializer());
            Create(new CDataStatusInfoSerializer());
            Create(new CDataStorageItemUIDList.Serializer());

            Create(new CDataStampBonus.Serializer());
            Create(new CDataStampBonusAsset.Serializer());

            Create(new CDataTimeLimitedQuestOrderList.Serializer());
            Create(new CDataTraningRoomEnemyHeader.Serializer());
            Create(new CDataTutorialQuestOrderList.Serializer());
            Create(new CDataUpdateMatchingProfileInfo.Serializer());
            Create(new CDataUpdateWalletPoint.Serializer());
            Create(new CDataURLInfoSerializer());
            Create(new CDataWalletPoint.Serializer());
            Create(new CDataWarpPoint.Serializer());
            Create(new CDataWeatherLoop.Serializer());
            Create(new CDataWeatherSchedule.Serializer());
            Create(new CDataWorldManageQuestOrderList.Serializer());
            Create(new CDataWorldManageQuestList.Serializer());
            Create(new CDataStageLayoutId.Serializer());
            Create(new CDataFriendInfo.Serializer());
            Create(new UnkownCharacterData0Serializer());
            Create(new CDataOmData.Serializer());
            Create(new CDataReleaseAreaInfoSet.Serializer());

            // Packet structure serializers
            Create(new C2LCreateCharacterDataReq.Serializer());
            Create(new C2LDecideCharacterIdReq.Serializer());
            Create(new C2LGetErrorMessageListReq.Serializer());
            Create(new C2LLoginReq.Serializer());
            Create(new C2SActionSetPlayerActionHistoryReq.Serializer());
            Create(new C2SAreaGetAreaBaseInfoListReq.Serializer());

            Create(new C2SBazaarCancelReq.Serializer());
            Create(new C2SBazaarExhibitReq.Serializer());
            Create(new C2SBazaarGetCharacterListReq.Serializer());
            Create(new C2SBazaarGetExhibitPossibleNumReq.Serializer());
            Create(new C2SBazaarGetItemHistoryInfoReq.Serializer());
            Create(new C2SBazaarGetItemInfoReq.Serializer());
            Create(new C2SBazaarGetItemListReq.Serializer());
            Create(new C2SBazaarGetItemPriceLimitReq.Serializer());
            Create(new C2SBazaarProceedsReq.Serializer());
            Create(new C2SBazaarReceiveProceedsReq.Serializer());
            Create(new C2SBazaarReExhibitReq.Serializer());

            Create(new C2SCharacterCharacterGoldenReviveReq.Serializer());
            Create(new C2SCharacterCharacterPenaltyReviveReq.Serializer());
            Create(new C2SCharacterCharacterPointReviveReq.Serializer());
            Create(new C2SCharacterCharacterSearchReq.Serializer());
            Create(new C2SCharacterEditUpdateCharacterEditParamReq.Serializer());
            Create(new C2SCharacterEditUpdateCharacterEditParamExReq.Serializer());
            Create(new C2SCharacterEditUpdatePawnEditParamReq.Serializer());
            Create(new C2SCharacterEditUpdatePawnEditParamExReq.Serializer());
            Create(new C2SCharacterChargeRevivePointReq.Serializer());
            Create(new C2SCharacterGetReviveChargeableTimeReq.Serializer());
            Create(new C2SCharacterPawnGoldenReviveReq.Serializer());
            Create(new C2SCharacterPawnPointReviveReq.Serializer());
            Create(new C2SCharacterSetOnlineStatusReq.Serializer());

            Create(new C2SClanClanBaseGetInfoReq.Serializer());
            Create(new C2SClanClanConciergeGetListReq.Serializer());
            Create(new C2SClanClanConciergeUpdateReq.Serializer());
            Create(new C2SClanClanPartnerPawnDataGetReq.Serializer());
            Create(new C2SClanGetFurnitureReq.Serializer());
            Create(new C2SClanSetFurnitureReq.Serializer());

            Create(new C2SConnectionLoginReq.Serializer());
            Create(new C2SConnectionMoveInServerReq.Serializer());
            Create(new C2SConnectionMoveOutServerReq.Serializer());

            Create(new C2SContextGetSetContextReq.Serializer());
            Create(new C2SContextMasterThrowReq.Serializer());
            Create(new C2SContextSetContextNtc.Serializer());
            Create(new C2SCraftGetCraftIRCollectionValueListReq.Serializer());
            Create(new C2SCraftGetCraftProgressListReq.Serializer());
            Create(new C2SCraftGetCraftSettingReq.Serializer());
            Create(new C2SCraftRecipeGetCraftRecipeReq.Serializer());
            Create(new C2SCraftRecipeGetCraftGradeupRecipeReq.Serializer());
            Create(new C2SCraftStartEquipGradeUpReq.Serializer());
            Create(new C2SCraftStartQualityUpReq.Serializer());
            Create(new C2SCraftStartCraftReq.Serializer());
            Create(new C2SCraftSkillUpReq.Serializer());
            Create(new C2SCraftGetCraftProductInfoReq.Serializer());
            Create(new C2SCraftCancelCraftReq.Serializer());
            Create(new C2SCraftTimeSaveReq.Serializer());
            Create(new C2SCraftGetCraftProductReq.Serializer());
            Create(new C2SCraftResetCraftpointReq.Serializer());
            Create(new C2SCraftSkillAnalyzeReq.Serializer());
            Create(new C2SCraftStartEquipColorChangeReq.Serializer());
            Create(new S2CCraftStartEquipColorChangeRes.Serializer());
            Create(new C2SCraftStartAttachElementReq.Serializer());
            Create(new C2SCraftStartDetachElementReq.Serializer());

            Create(new C2SDispelGetDispelItemSettingsReq.Serializer());
            Create(new C2SDispelGetDispelItemListReq.Serializer());
            Create(new C2SDispelExchangeDispelItemReq.Serializer());

            Create(new C2SEquipChangeCharacterEquipJobItemReq.Serializer());
            Create(new C2SEquipChangeCharacterEquipReq.Serializer());
            Create(new C2SEquipChangeCharacterStorageEquipReq.Serializer());
            Create(new C2SEquipChangePawnEquipJobItemReq.Serializer());
            Create(new C2SEquipChangePawnEquipReq.Serializer());
            Create(new C2SEquipChangePawnStorageEquipReq.Serializer());
            Create(new C2SEquipEnhancedGetPacksReq.Serializer());
            Create(new C2SEquipGetCharacterEquipListReq.Serializer());
            Create(new C2SEquipGetCraftLockedElementListReq.Serializer());
            Create(new C2SEquipUpdateHideCharacterHeadArmorReq.Serializer());
            Create(new C2SEquipUpdateHideCharacterLanternReq.Serializer());
            Create(new C2SEquipUpdateHidePawnHeadArmorReq.Serializer());
            Create(new C2SEquipUpdateHidePawnLanternReq.Serializer());
            Create(new C2SGpGetGpReq.Serializer());
            Create(new C2SGpGetGpDetailReq.Serializer());
            Create(new C2SGpGetGpPeriodReq.Serializer());
            Create(new C2SGpGetValidChatComGroupReq.Serializer());
            Create(new C2SGpGpEditGetVoiceListReq.Serializer());
            Create(new C2SInnGetPenaltyHealStayPriceReq.Serializer());
            Create(new C2SInnGetStayPriceReq.Serializer());
            Create(new C2SInnStayInnReq.Serializer());
            Create(new C2SInnStayPenaltyHealInnReq.Serializer());

            Create(new C2SInstanceEnemyGroupEntryNtc.Serializer());
            Create(new C2SInstanceEnemyGroupLeaveNtc.Serializer());
            Create(new C2SInstanceEnemyKillReq.Serializer());
            Create(new C2SInstanceExchangeOmInstantKeyValueReq.Serializer());
            Create(new C2SInstanceGetDropItemListReq.Serializer());
            Create(new C2SInstanceGetDropItemReq.Serializer());
            Create(new C2SInstanceGetEnemySetListReq.Serializer());
            Create(new C2SInstanceGetGatheringItemListReq.Serializer());
            Create(new C2SInstanceGetGatheringItemReq.Serializer());
            Create(new C2SInstanceGetItemSetListReq.Serializer());
            Create(new C2SInstanceSetOmInstantKeyValueReq.Serializer());
            Create(new C2SInstanceTreasurePointGetCategoryListReq.Serializer());
            Create(new C2SInstanceTreasurePointGetListReq.Serializer());
            Create(new C2SInstancePlTouchOmNtc.Serializer());

            Create(new C2SItemConsumeStorageItemReq.Serializer());
            Create(new C2SItemGetStorageItemListReq.Serializer());
            Create(new C2SItemMoveItemReq.Serializer());
            Create(new C2SItemSellItemReq.Serializer());
            Create(new C2SItemSortGetItemSortDataBinReq.Serializer());
            Create(new C2SItemSortSetItemSortDataBinReq.Serializer());
            Create(new C2SItemUseBagItemReq.Serializer());
            Create(new C2SItemUseJobItemsReq.Serializer());

            Create(new C2SJobChangeJobReq.Serializer());
            Create(new C2SJobChangePawnJobReq.Serializer());
            Create(new C2SJobGetJobChangeListReq.Serializer());
            Create(new C2SJobUpdateExpModeReq.Serializer());
            Create(new C2SJobGetPlayPointListReq.Serializer());
            Create(new C2SJobJobValueShopGetLineupReq.Serializer());
            Create(new C2SJobJobValueShopBuyItemReq.Serializer());

            Create(new C2SLobbyChatMsgReq.Serializer());
            Create(new C2SLobbyJoinReq.Serializer());
            Create(new C2SLobbyLeaveReq.Serializer());
            Create(new C2SLobbyLobbyDataMsgReq.Serializer());

            Create(new C2SMailMailGetListHeadReq.Serializer());
            Create(new C2SMailMailGetListFootReq.Serializer());
            Create(new C2SMailMailGetListDataReq.Serializer());
            Create(new C2SMailSystemMailGetListHeadReq.Serializer());
            Create(new C2SMailSystemMailGetListFootReq.Serializer());
            Create(new C2SMailSystemMailGetListDataReq.Serializer());
            Create(new C2SMailSystemMailGetTextReq.Serializer());
            Create(new C2SMailSystemMailGetAllItemReq.Serializer());
            Create(new C2SMailSystemMailDeleteReq.Serializer());

            Create(new C2SMandragoraGetMyMandragoraReq.Serializer());
            Create(new C2SMyRoomFurnitureListGetReq.Serializer());
            Create(new C2SMyRoomMyRoomBgmUpdateReq.Serializer());
            Create(new C2SMyRoomUpdatePlanetariumReq.Serializer());
            Create(new C2SPartnerPawnPawnLikabilityRewardListGetReq.Serializer());

            Create(new C2SPartyPartyBreakupReq.Serializer());
            Create(new C2SPartyPartyChangeLeaderReq.Serializer());
            Create(new C2SPartyPartyCreateReq.Serializer());
            Create(new C2SPartyPartyInviteCancelReq.Serializer());
            Create(new C2SPartyPartyInviteCharacterReq.Serializer());
            Create(new C2SPartyPartyInviteEntryReq.Serializer());
            Create(new C2SPartyPartyInvitePrepareAcceptReq.Serializer());
            Create(new C2SPartyPartyInviteRefuseReq.Serializer());
            Create(new C2SPartyPartyJoinReq.Serializer());
            Create(new C2SPartyPartyLeaveReq.Serializer());
            Create(new C2SPartyPartyMemberKickReq.Serializer());
            Create(new C2SPartySendBinaryMsgAllNtc.Serializer());
            Create(new C2SPartySendBinaryMsgNtc.Serializer());

            Create(new C2SPartyPartyMemberSetValueReq.Serializer());
            Create(new S2CPartyPartyMemberSetValueRes.Serializer());
            Create(new S2CPartyPartyMemberSetValueNtc.Serializer());
            Create(new S2CPartyPartyQuestProgressNtc.Serializer());

            Create(new C2SPawnCreatePawnReq.Serializer());
            Create(new C2SPawnGetLostPawnListReq.Serializer());
            Create(new C2SPawnGetMypawnDataReq.Serializer());
            Create(new C2SPawnGetMyPawnListReq.Serializer());
            Create(new C2SPawnGetPartyPawnDataReq.Serializer());
            Create(new C2SPawnGetPawnHistoryListReq.Serializer());
            Create(new C2SPawnGetPawnTotalScoreReq.Serializer());
            Create(new C2SPawnGetRegisteredPawnDataReq.Serializer());
            Create(new C2SPawnJoinPartyMypawnReq.Serializer());
            Create(new C2SPawnPawnLostReq.Serializer());
            Create(new C2SPawnSpSkillDeleteStockSkillReq.Serializer());
            Create(new C2SPawnSpSkillGetActiveSkillReq.Serializer());
            Create(new C2SPawnSpSkillGetStockSkillReq.Serializer());
            Create(new C2SPawnSpSkillSetActiveSkillReq.Serializer());
            Create(new C2SPawnTrainingGetPreparetionInfoToAdviceReq.Serializer());
            Create(new C2SPawnTrainingGetTrainingStatusReq.Serializer());
            Create(new C2SPawnTrainingSetTrainingStatusReq.Serializer());
            Create(new C2SProfileGetCharacterProfileReq.Serializer());
            Create(new C2SProfileGetMyCharacterProfileReq.Serializer());

            Create(new C2SQuestCancelPriorityQuestReq.Serializer());
            Create(new C2SQuestGetLightQuestListReq.Serializer());
            Create(new C2SQuestGetLotQuestListReq.Serializer());
            Create(new C2SQuestGetPackageQuestListReq.Serializer());
            Create(new C2SQuestGetRewardBoxItemReq.Serializer());
            Create(new C2SQuestGetTutorialQuestListReq.Serializer());
            Create(new C2SQuestQuestOrderReq.Serializer());
            Create(new C2SQuestLeaderQuestProgressRequestReq.Serializer());
            Create(new C2SQuestSendLeaderQuestOrderConditionInfoReq.Serializer());
            Create(new C2SQuestSendLeaderWaitOrderQuestListReq.Serializer());
            Create(new C2SQuestSetPriorityQuestReq.Serializer());
            Create(new C2SQuestGetSetQuestListReq.Serializer());
            Create(new C2SQuestQuestCompleteFlagClearReq.Serializer());
            Create(new C2SQuestDeliverItemReq.Serializer());
            Create(new C2SQuestQuestProgressReq.Serializer());
            Create(new C2SQuestDecideDeliveryItemReq.Serializer());
            Create(new C2SQuestGetRewardBoxListReq.Serializer());
            Create(new C2SQuestGetQuestCompleteListReq.Serializer());
            Create(new C2SQuestQuestCancelReq.Serializer());

            Create(new C2SServerGameTimeGetBaseInfoReq.Serializer());
            Create(new C2SServerGetRealTimeReq.Serializer());

            Create(new C2SSkillChangeExSkillReq.Serializer());
            Create(new C2SSkillGetAbilityCostReq.Serializer());
            Create(new C2SSkillGetAcquirableAbilityListReq.Serializer());
            Create(new C2SSkillGetAcquirableSkillListReq.Serializer());
            Create(new C2SSkillGetLearnedAbilityListReq.Serializer());
            Create(new C2SSkillGetLearnedNormalSkillListReq.Serializer());
            Create(new C2SSkillGetLearnedSkillListReq.Serializer());
            Create(new C2SSkillGetPawnAbilityCostReq.Serializer());
            Create(new C2SSkillGetPawnLearnedAbilityListReq.Serializer());
            Create(new C2SSkillGetPawnLearnedNormalSkillListReq.Serializer());
            Create(new C2SSkillGetPawnLearnedSkillListReq.Serializer());
            Create(new C2SSkillGetPawnSetAbilityListReq.Serializer());
            Create(new C2SSkillGetPawnSetSkillListReq.Serializer());
            Create(new C2SSkillGetPresetAbilityListReq.Serializer());
            Create(new C2SSkillGetSetAbilityListReq.Serializer());
            Create(new C2SSkillGetSetSkillListReq.Serializer());
            Create(new C2SSkillLearnAbilityReq.Serializer());
            Create(new C2SSkillLearnNormalSkillReq.Serializer());
            Create(new C2SSkillLearnPawnAbilityReq.Serializer());
            Create(new C2SSkillLearnPawnNormalSkillReq.Serializer());
            Create(new C2SSkillLearnPawnSkillReq.Serializer());
            Create(new C2SSkillLearnSkillReq.Serializer());
            Create(new C2SSkillSetAbilityReq.Serializer());
            Create(new C2SSkillSetOffAbilityReq.Serializer());
            Create(new C2SSkillSetOffPawnAbilityReq.Serializer());
            Create(new C2SSkillSetOffPawnSkillReq.Serializer());
            Create(new C2SSkillSetOffSkillReq.Serializer());
            Create(new C2SSkillSetPawnAbilityReq.Serializer());
            Create(new C2SSkillSetPawnSkillReq.Serializer());
            Create(new C2SSkillSetSkillReq.Serializer());
            Create(new C2SSkillRegisterPresetAbilityReq.Serializer());
            Create(new C2SSkillSetPresetAbilityNameReq.Serializer());
            Create(new C2SSkillSetPresetAbilityListReq.Serializer());

            Create(new C2SSetShortcutReq.Serializer());
            Create(new C2SShopBuyShopGoodsReq.Serializer());
            Create(new C2SShopGetShopGoodsListReq.Serializer());
            Create(new C2SSetCommunicationShortcutReq.Serializer());
            Create(new C2SStageAreaChangeReq.Serializer());
            Create(new C2SStageGetStageListReq.Serializer());
            Create(new C2STraningRoomGetEnemyListReq.Serializer());
            Create(new C2STrainingRoomSetEnemyReq.Serializer());
            Create(new C2SWarpAreaWarpReq.Serializer());
            Create(new C2SWarpGetFavoriteWarpPointListReq.Serializer());
            Create(new C2SWarpGetReleaseWarpPointListReq.Serializer());
            Create(new C2SWarpGetReturnLocationReq.Serializer());
            Create(new C2SWarpGetStartPointListReq.Serializer());
            Create(new C2SWarpGetWarpPointListReq.Serializer());
            Create(new C2SWarpPartyWarpReq.Serializer());
            Create(new C2SWarpRegisterFavoriteWarpReq.Serializer());
            Create(new C2SWarpReleaseWarpPointReq.Serializer());
            Create(new C2SWarpWarpEndNtc.Serializer());
            Create(new C2SWarpWarpReq.Serializer());
            Create(new C2SWarpWarpStartNtc.Serializer());

            Create(new C2SFriendApplyFriendReq.Serializer());
            Create(new C2SFriendApproveFriendReq.Serializer());
            Create(new C2SFriendRemoveFriendReq.Serializer());
            Create(new C2SFriendRegisterFavoriteFriendReq.Serializer());
            Create(new C2SFriendCancelFriendApplicationReq.Serializer());
            Create(new C2SCharacterCommunityCharacterStatusGetReq.Serializer());

            Create(new L2CCreateCharacterDataNtc.Serializer());
            Create(new L2CCreateCharacterDataRes.Serializer());
            Create(new L2CGetErrorMessageListNtc.Serializer());
            Create(new L2CGetErrorMessageListRes.Serializer());
            Create(new L2CDecideCharacterIdRes.Serializer());
            Create(new L2CGetGameSessionKeyRes.Serializer());
            Create(new L2CGetLoginSettingsRes.Serializer());
            Create(new L2CLoginRes.Serializer());
            Create(new L2CLoginWaitNumNtc.Serializer());
            Create(new L2CNextConnectionServerNtc.Serializer());
            Create(new L2CGpCourseGetInfoRes.Serializer());

            Create(new S2CActionSetPlayerActionHistoryRes.Serializer());
            Create(new S2CAreaGetAreaBaseInfoListRes.Serializer());

            Create(new S2CBazaarCancelRes.Serializer());
            Create(new S2CBazaarExhibitRes.Serializer());
            Create(new S2CBazaarGetCharacterListRes.Serializer());
            Create(new S2CBazaarGetExhibitPossibleNumRes.Serializer());
            Create(new S2CBazaarGetItemHistoryInfoRes.Serializer());
            Create(new S2CBazaarGetItemInfoRes.Serializer());
            Create(new S2CBazaarGetItemListRes.Serializer());
            Create(new S2CBazaarGetItemPriceLimitRes.Serializer());
            Create(new S2CBazaarProceedsNtc.Serializer());
            Create(new S2CBazaarProceedsRes.Serializer());
            Create(new S2CBazaarReceiveProceedsRes.Serializer());
            Create(new S2CBazaarReExhibitRes.Serializer());

            Create(new S2CEquipChangeCharacterEquipLobbyNtc.Serializer());

            Create(new S2CCharacterCharacterGoldenReviveRes.Serializer());
            Create(new S2CCharacterCharacterPenaltyReviveRes.Serializer());
            Create(new S2CCharacterCharacterPointReviveRes.Serializer());
            Create(new S2CCharacterCharacterSearchRes.Serializer());
            Create(new S2CCharacterChargeRevivePointRes.Serializer());
            Create(new S2CCharacterCommunityCharacterStatusUpdateNtc.Serializer());
            Create(new S2CCharacterDecideCharacterIdRes.Serializer());
            Create(new S2CCharacterEditUpdateCharacterEditParamRes.Serializer());
            Create(new S2CCharacterEditUpdateCharacterEditParamExRes.Serializer());
            Create(new S2CCharacterEditUpdateEditParamNtc.Serializer());
            Create(new S2CCharacterEditUpdateEditParamExNtc.Serializer());
            Create(new S2CCharacterEditUpdatePawnEditParamRes.Serializer());
            Create(new S2CCharacterEditUpdatePawnEditParamExRes.Serializer());
            Create(new S2CCharacterFinishDeathPenaltyNtc.Serializer());
            Create(new S2CCharacterGetCharacterStatusNtc.Serializer());
            Create(new S2CCharacterGetReviveChargeableTimeRes.Serializer());
            Create(new S2CCharacterContentsReleaseElementNtc.Serializer());
            Create(new S2CCharacterPawnGoldenReviveRes.Serializer());
            Create(new S2CCharacterPawnPointReviveRes.Serializer());
            Create(new S2CCharacterSetOnlineStatusRes.Serializer());
            Create(new S2CCharacterStartDeathPenaltyNtc.Serializer());
            Create(new S2CCharacterUpdateRevivePointNtc.Serializer());

            Create(new S2CClanClanBaseGetInfoRes.Serializer());
            Create(new S2CClanClanConciergeGetListRes.Serializer());
            Create(new S2CClanClanConciergeUpdateRes.Serializer());
            Create(new S2CClanClanGetMyInfoRes.Serializer());
            Create(new S2CClanClanGetMyMemberListRes.Serializer());
            Create(new S2CClanClanPartnerPawnDataGetRes.Serializer());
            Create(new S2CClanGetFurnitureRes.Serializer());
            Create(new S2CClanSetFurnitureRes.Serializer());
            Create(new S2CConnectionLoginRes.Serializer());
            Create(new S2CConnectionLogoutRes.Serializer());
            Create(new S2CConnectionMoveInServerRes.Serializer());
            Create(new S2CConnectionMoveOutServerRes.Serializer());
            Create(new S2CContextGetAllPlayerContextNtc.Serializer());
            Create(new S2CContextGetLobbyPlayerContextNtc.Serializer());
            Create(new S2CContextGetPartyMypawnContextNtc.Serializer());
            Create(new S2CContextGetPartyPlayerContextNtc.Serializer());
            Create(new S2CContextGetSetContextRes.Serializer());
            Create(new S2CContextMasterChangeNtc.Serializer());
            Create(new S2CContextMasterInfoNtc.Serializer());
            Create(new S2CContextMasterThrowNtc.Serializer());
            Create(new S2CContextMasterThrowRes.Serializer());
            Create(new S2CContextSetContextBaseNtc.Serializer());
            Create(new S2CContextSetContextNtc.Serializer());
            Create(new S2CCraftGetCraftIRCollectionValueListRes.Serializer());
            Create(new S2CCraftGetCraftProgressListRes.Serializer());
            Create(new C2SCraftGetCraftProductInfoRes.Serializer());
            Create(new C2SCraftCancelCraftRes.Serializer());
            Create(new S2CCraftTimeSaveRes.Serializer());
            Create(new C2SCraftGetCraftProductRes.Serializer());
            Create(new S2CCraftGetCraftSettingRes.Serializer());
            Create(new S2CCraftRecipeGetCraftRecipeRes.Serializer());
            Create(new S2CCraftStartCraftRes.Serializer());
            Create(new S2CCraftSkillUpRes.Serializer());
            Create(new S2CCraftResetCraftpointRes.Serializer());
            Create(new S2CCraftSkillAnalyzeRes.Serializer());
            Create(new S2CCraftRecipeGetCraftGradeupRecipeRes.Serializer());
            Create(new S2CCraftStartEquipGradeUpRes.Serializer());
            Create(new S2CCraftStartQualityUpRes.Serializer());
            Create(new S2CCraft_30_21_16_NTC.Serializer());
            Create(new S2CCraftStartAttachElementRes.Serializer());
            Create(new S2CCraftStartDetachElementRes.Serializer());
            Create(new S2CCraftFinishCraftNtc.Serializer());
            Create(new S2CCraftCraftExpUpNtc.Serializer());

            Create(new S2CDispelGetDispelItemSettingsRes.Serializer());
            Create(new S2CDispelGetDispelItemListRes.Serializer());
            Create(new S2CDispelExchangeDispelItemRes.Serializer());

            Create(new S2CEquipChangeCharacterEquipJobItemNtc.Serializer());
            Create(new S2CEquipChangeCharacterEquipJobItemRes.Serializer());
            Create(new S2CEquipChangeCharacterEquipNtc.Serializer());
            Create(new S2CEquipChangeCharacterEquipRes.Serializer());
            Create(new S2CEquipChangeCharacterStorageEquipRes.Serializer());
            Create(new S2CEquipChangePawnEquipJobItemNtc.Serializer());
            Create(new S2CEquipChangePawnEquipJobItemRes.Serializer());
            Create(new S2CEquipChangePawnEquipNtc.Serializer());
            Create(new S2CEquipChangePawnEquipRes.Serializer());
            Create(new S2CEquipChangePawnStorageEquipRes.Serializer());
            Create(new S2CEquipEnhancedGetPacksRes.Serializer());
            Create(new S2CEquipGetCharacterEquipListRes.Serializer());
            Create(new S2CEquipGetCraftLockedElementListRes.Serializer());
            Create(new S2CEquipUpdateEquipHideNtc.Serializer());
            Create(new S2CEquipUpdateHideCharacterHeadArmorRes.Serializer());
            Create(new S2CEquipUpdateHideCharacterLanternRes.Serializer());
            Create(new S2CEquipUpdateHidePawnHeadArmorRes.Serializer());
            Create(new S2CEquipUpdateHidePawnLanternRes.Serializer());
            Create(new S2CExtendEquipSlotNtc.Serializer());
            Create(new S2CGpGetGpRes.Serializer());
            Create(new S2CGpGetGpDetailRes.Serializer());
            Create(new S2CGpGetGpPeriodRes.Serializer());
            Create(new S2CGpGetValidChatComGroupRes.Serializer());
            Create(new S2CGpGpEditGetVoiceListRes.Serializer());
            Create(new S2CGpGpCourseGetAvailableListRes.Serializer());
            Create(new S2CGPCourseStartNtc.Serializer());
            Create(new S2CInnGetPenaltyHealStayPriceRes.Serializer());
            Create(new S2CInnGetStayPriceRes.Serializer());
            Create(new S2CInnStayInnRes.Serializer());
            Create(new S2CInnStayPenaltyHealInnRes.Serializer());
            Create(new S2CInstanceEnemyDieNtc.Serializer());
            Create(new S2CInstanceSetOmInstantKeyValueNtc.Serializer());
            Create(new S2CInstanceExchangeOmInstantKeyValueNtc.Serializer());
            Create(new S2CInstanceAreaResetNtc.Serializer());
            Create(new S2CInstanceEnemyKillRes.Serializer());
            Create(new S2CInstanceEnemyRepopNtc.Serializer());
            Create(new S2CInstanceEnemySubGroupAppearNtc.Serializer());
            Create(new S2CInstanceExchangeOmInstantKeyValueRes.Serializer());
            Create(new S2CInstanceGetDropItemListRes.Serializer());
            Create(new S2CInstanceGetDropItemRes.Serializer());
            Create(new S2CInstanceGetEnemySetListRes.Serializer());
            Create(new S2CInstanceGetGatheringItemListRes.Serializer());
            Create(new S2CInstanceGetGatheringItemRes.Serializer());
            Create(new S2CInstanceGetItemSetListRes.Serializer());
            Create(new S2CInstancePopDropItemNtc.Serializer());
            Create(new S2CInstanceSetOmInstantKeyValueRes.Serializer());
            Create(new S2CInstanceTreasurePointGetCategoryListRes.Serializer());
            Create(new S2CInstanceTreasurePointGetListRes.Serializer());
            Create(new S2CInstanceEnemyGroupDestroyNtc.Serializer());
            Create(new S2CInstanceEnemyGroupResetNtc.Serializer());
            Create(new S2CItemConsumeStorageItemRes.Serializer());
            Create(new S2CItemExtendItemSlotNtc.Serializer());
            Create(new S2CItemGetStorageItemListRes.Serializer());
            Create(new S2CItemUpdateCharacterItemNtc.Serializer());
            Create(new S2CItemSortGetItemSortdataBinRes.Serializer());
            Create(new S2CItemSortGetItemSortdataBinNtc.Serializer());
            Create(new S2CItemSortSetItemSortDataBinRes.Serializer());
            Create(new S2CItemMoveItemRes.Serializer());
            Create(new S2CItemSellItemRes.Serializer());
            Create(new S2CItemUseBagItemRes.Serializer());
            Create(new S2CItemUseJobItemsRes.Serializer());

            Create(new S2CJob_33_3_16_Ntc.Serializer());
            Create(new S2CJobChangeJobNtc.Serializer());
            Create(new S2CJobChangeJobRes.Serializer());
            Create(new S2CJobChangePawnJobNtc.Serializer());
            Create(new S2CJobChangePawnJobRes.Serializer());
            Create(new S2CJobCharacterJobExpUpNtc.Serializer());
            Create(new S2CJobCharacterJobLevelUpMemberNtc.Serializer());
            Create(new S2CJobCharacterJobLevelUpNtc.Serializer());
            Create(new S2CJobCharacterJobLevelUpOtherNtc.Serializer());
            Create(new S2CJobGetJobChangeListRes.Serializer());
            Create(new S2CJobUpdatePlayPointNtc.Serializer());
            Create(new S2CJobGetPlayPointListRes.Serializer());
            Create(new S2CJobJobValueShopGetLineupRes.Serializer());
            Create(new S2CJobJobValueShopBuyItemRes.Serializer());

            Create(new S2COrbDevoteGetReleaseOrbElementListRes.Serializer());
            Create(new S2CJobOrbTreeGetJobOrbTreeStatusListRes.Serializer());
            Create(new C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq.Serializer());
            Create(new S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes.Serializer());
            Create(new C2SOrbDevoteReleaseOrbElementReq.Serializer());
            Create(new S2COrbDevoteReleaseHandlerRes.Serializer());
            Create(new S2COrbDevoteGetOrbGainExtendParamRes.Serializer());
            Create(new S2COrb25_6_16_Ntc.Serializer());
            Create(new C2SOrbDevoteGetPawnReleaseOrbElementListReq.Serializer());
            Create(new S2COrbDevoteGetPawnReleaseOrbElementListRes.Serializer());
            Create(new C2SOrbDevoteReleasePawnOrbElementReq.Serializer());
            Create(new S2COrbDevoteReleasePawnOrbELementRes.Serializer());

            Create(new S2CJobPawnJobExpUpNtc.Serializer());
            Create(new S2CJobPawnJobLevelUpMemberNtc.Serializer());
            Create(new S2CJobPawnJobLevelUpNtc.Serializer());
            Create(new S2CJobUpdateExpModeRes.Serializer());
            Create(new S2CLobbyChatMsgRes.Serializer());
            Create(new S2CLobbyChatMsgNotice.Serializer());
            Create(new S2CLobbyJoinRes.Serializer());
            Create(new S2CLobbyLeaveRes.Serializer());
            Create(new S2CLobbyLobbyDataMsgNotice.Serializer());

            Create(new S2CMailMailGetListHeadRes.Serializer());
            Create(new S2CMailMailGetListFootRes.Serializer());
            Create(new S2CMailMailGetListDataRes.Serializer());
            Create(new S2CMailSystemMailGetListHeadRes.Serializer());
            Create(new S2CMailSystemMailGetListFootRes.Serializer());
            Create(new S2CMailSystemMailGetListDataRes.Serializer());
            Create(new S2CMailSystemMailGetTextRes.Serializer());
            Create(new S2CMailSystemGetAllItemRes.Serializer());
            Create(new S2CMailSystemMailDeleteRes.Serializer());
            Create(new S2CMailSystemMailSendNtc.Serializer());

            Create(new S2CMandragoraGetMyMandragoraRes.Serializer());
            Create(new S2CMyRoomFurnitureListGetRes.Serializer());
            Create(new S2CMyRoomMyRoomBgmUpdateRes.Serializer());
            Create(new S2CMyRoomUpdatePlanetariumRes.Serializer());
            Create(new S2CPartnerPawnPawnLikabilityRewardListGetRes.Serializer());

            Create(new S2CPartyPartyBreakupNtc.Serializer());
            Create(new S2CPartyPartyBreakupRes.Serializer());
            Create(new S2CPartyPartyChangeLeaderNtc.Serializer());
            Create(new S2CPartyPartyChangeLeaderRes.Serializer());
            Create(new S2CPartyPartyCreateRes.Serializer());
            Create(new S2CPartyPartyInviteAcceptNtc.Serializer());
            Create(new S2CPartyPartyInviteCancelNtc.Serializer());
            Create(new S2CPartyPartyInviteCancelRes.Serializer());
            Create(new S2CPartyPartyInviteCharacterRes.Serializer());
            Create(new S2CPartyPartyInviteEntryNtc.Serializer());
            Create(new S2CPartyPartyInviteEntryRes.Serializer());
            Create(new S2CPartyPartyInviteJoinMemberNtc.Serializer());
            Create(new S2CPartyPartyInviteNtc.Serializer());
            Create(new S2CPartyPartyInvitePrepareAcceptNtc.Serializer());
            Create(new S2CPartyPartyInvitePrepareAcceptRes.Serializer());
            Create(new S2CPartyPartyInviteRefuseRes.Serializer());
            Create(new S2CPartyPartyInviteSuccessNtc.Serializer());
            Create(new S2CPartyPartyJoinNtc.Serializer());
            Create(new S2CPartyPartyJoinRes.Serializer());
            Create(new S2CPartyPartyLeaveNtc.Serializer());
            Create(new S2CPartyPartyLeaveRes.Serializer());
            Create(new S2CPartyPartyMemberKickNtc.Serializer());
            Create(new S2CPartyPartyMemberKickRes.Serializer());
            Create(new S2CPartyPartyMemberSessionStatusNtc.Serializer());
            Create(new S2CPartyRecvBinaryMsgAllNtc.Serializer());
            Create(new S2CPartyRecvBinaryMsgNtc.Serializer());

            Create(new S2CPawnExtendMainPawnNtc.Serializer());
            Create(new S2CPawnCreatePawnRes.Serializer());
            Create(new S2CPawnGetPawnProfileNtc.Serializer());
            Create(new S2CPawnHistoryInfoNtc.Serializer());
            Create(new S2CPawnGetPawnTotalScoreInfoNtc.Serializer());
            Create(new S2CPawnGetPawnOrbDevoteInfoNtc.Serializer());
            Create(new S2CPawnJoinPartyPawnNtc.Serializer());
            Create(new S2CPawnGetLostPawnListRes.Serializer());
            Create(new S2CPawnGetMypawnDataRes.Serializer());
            Create(new S2CPawnGetMypawnListRes.Serializer());
            Create(new S2CPawnGetPartyPawnDataRes.Serializer());
            Create(new S2CPawnGetPawnHistoryListRes.Serializer());
            Create(new S2CPawnGetPawnTotalScoreRes.Serializer());
            Create(new S2CPawnGetRegisteredPawnDataRes.Serializer());
            Create(new S2CPawnJoinPartyMypawnRes.Serializer());
            Create(new S2CPawnPawnLostRes.Serializer());
            Create(new S2CPawnSpSkillDeleteStockSkillRes.Serializer());
            Create(new S2CPawnSpSkillGetActiveSkillRes.Serializer());
            Create(new S2CPawnSpSkillGetStockSkillRes.Serializer());
            Create(new S2CPawnSpSkillSetActiveSkillRes.Serializer());
            Create(new S2CPawnTrainingGetPreparetionInfoToAdviceRes.Serializer());
            Create(new S2CPawnTrainingGetTrainingStatusRes.Serializer());
            Create(new S2CPawnTrainingSetTrainingStatusRes.Serializer());
            Create(new S2CProfileGetCharacterProfileRes.Serializer());
            Create(new S2CProfileGetMyCharacterProfileRes.Serializer());

            Create(new S2CQuestQuestLogInfoRes.Serializer());
            Create(new CDataLightQuestClearList.Serializer());
            Create(new S2CQuestCancelPriorityQuestRes.Serializer());
            Create(new S2CQuestGetQuestCompleteListRes.Serializer());
            Create(new S2CQuestGetLightQuestListRes.Serializer());
            Create(new S2CQuestGetLotQuestListRes.Serializer());
            Create(new S2CQuestGetMainQuestListRes.Serializer());
            Create(new S2CQuestGetMainQuestNtc.Serializer());
            Create(new S2CQuestGetPartyQuestProgressInfoRes.Serializer());
            Create(new S2CQuestGetSetQuestListRes.Serializer());
            Create(new S2CQuestGetSetQuestListNtc.Serializer());
            Create(new S2CQuestGetWorldManageQuestListNtc.Serializer());
            Create(new S2CQuestGetWorldManageQuestListRes.Serializer());
            Create(new S2CQuestJoinLobbyQuestInfoNtc.Serializer());
            Create(new S2CQuestLeaderQuestProgressRequestRes.Serializer());
            Create(new S2CQuestLeaderQuestProgressRequestNtc.Serializer());
            Create(new S2CQuestPartyQuestProgressNtc.Serializer());
            Create(new S2CQuestQuestOrderRes.Serializer());
            Create(new S2CQuestQuestProgressNtc.Serializer());
            Create(new S2CQuestQuestProgressRes.Serializer());
            Create(new S2CQuestCompleteNtc.Serializer());
            Create(new S2CQuestQuestProgressWorkSaveNtc.Serializer());
            Create(new S2CQuestQuestCompleteFlagClearRes.Serializer());
            Create(new S2CQuestQuestCancelRes.Serializer());
            Create(new S2CQuestQuestCancelNtc.Serializer());
            Create(new S2CQuestQuestEnableNtc.Serializer());
            Create(new S2CQuestSetPriorityQuestNtc.Serializer());
            Create(new S2CQuestGetPriorityQuestRes.Serializer());
            Create(new S2CQuestGetRewardBoxListRes.Serializer());
            Create(new S2CQuestGetRewardBoxItemRes.Serializer());
            Create(new S2CQuestDeliverItemRes.Serializer());
            Create(new S2CQuestDeliverItemNtc.Serializer());
            Create(new S2CQuestDecideDeliveryItemRes.Serializer());
            Create(new S2CQuestDecideDeliveryItemNtc.Serializer());

            Create(new S2CQuestSendLeaderQuestOrderConditionInfoNtc.Serializer());
            Create(new S2CQuestSendLeaderQuestOrderConditionInfoRes.Serializer());
            Create(new S2CQuestSendLeaderWaitOrderQuestListNtc.Serializer());
            Create(new S2CQuestSendLeaderWaitOrderQuestListRes.Serializer());
            Create(new S2CQuestSetPriorityQuestRes.Serializer());

            Create(new S2CServerGameTimeGetBaseInfoRes.Serializer());
            Create(new S2CServerGetRealTimeRes.Serializer());
            Create(new S2CServerGetServerListRes.Serializer());

            Create(new S2CSkillAbilitySetNtc.Serializer());
            Create(new S2CSkillChangeExSkillRes.Serializer());
            Create(new S2CSkillCustomSkillSetNtc.Serializer());
            Create(new S2CSkillGetAbilityCostRes.Serializer());
            Create(new S2CSkillGetAcquirableAbilityListRes.Serializer());
            Create(new S2CSkillGetAcquirableSkillListRes.Serializer());
            Create(new S2CSkillGetCurrentSetSkillListRes.Serializer());
            Create(new S2CSkillGetLearnedAbilityListRes.Serializer());
            Create(new S2CSkillGetLearnedNormalSkillListRes.Serializer());
            Create(new S2CSkillGetLearnedSkillListRes.Serializer());
            Create(new S2CSkillGetPawnAbilityCostRes.Serializer());
            Create(new S2CSkillGetPawnLearnedAbilityListRes.Serializer());
            Create(new S2CSkillGetPawnLearnedNormalSkillListRes.Serializer());
            Create(new S2CSkillGetPawnLearnedSkillListRes.Serializer());
            Create(new S2CSkillGetPawnSetAbilityListRes.Serializer());
            Create(new S2CSkillGetPawnSetSkillListRes.Serializer());
            Create(new S2CSkillGetPresetAbilityListRes.Serializer());
            Create(new S2CSkillGetSetAbilityListRes.Serializer());
            Create(new S2CSkillGetSetSkillListRes.Serializer());
            Create(new S2CSkillLearnAbilityRes.Serializer());
            Create(new S2CSkillLearnNormalSkillRes.Serializer());
            Create(new S2CSkillLearnPawnAbilityRes.Serializer());
            Create(new S2CSkillLearnPawnNormalSkillRes.Serializer());
            Create(new S2CSkillLearnPawnSkillRes.Serializer());
            Create(new S2CSkillLearnSkillRes.Serializer());
            Create(new S2CSkillNormalSkillLearnNtc.Serializer());
            Create(new S2CSkillPawnAbilitySetNtc.Serializer());
            Create(new S2CSkillPawnCustomSkillSetNtc.Serializer());
            Create(new S2CSkillPawnNormalSkillLearnNtc.Serializer());
            Create(new S2CSkillSetAbilityRes.Serializer());
            Create(new S2CSkillSetOffAbilityRes.Serializer());
            Create(new S2CSkillSetOffPawnAbilityRes.Serializer());
            Create(new S2CSkillSetOffPawnSkillRes.Serializer());
            Create(new S2CSkillSetOffSkillRes.Serializer());
            Create(new S2CSkillSetPawnAbilityRes.Serializer());
            Create(new S2CSkillSetPawnSkillRes.Serializer());
            Create(new S2CSkillSetSkillRes.Serializer());
            Create(new S2CSkillSetPresetAbilityNtc.Serializer());
            Create(new S2CSkillSetPresetPawnAbilityNtc.Serializer());
            Create(new S2CSkillGetCharacterSkillInfoNtc.Serializer());
            Create(new S2CSkillRegisterPresetAbilityRes.Serializer());
            Create(new S2CSkillSetPresetAbilityNameRes.Serializer());
            Create(new S2CSkillSetPresetAbilityListRes.Serializer());

            Create(new S2CSetCommunicationShortcutRes.Serializer());
            Create(new S2CSetShortcutRes.Serializer());
            Create(new S2CShopBuyShopGoodsRes.Serializer());
            Create(new S2CShopGetShopGoodsListRes.Serializer());
            Create(new S2CStageAreaChangeRes.Serializer());
            Create(new S2CStageGetStageListRes.Serializer());
            Create(new S2CTraningRoomGetEnemyListRes.Serializer());
            Create(new S2CTraningRoomSetEnemyRes.Serializer());
            Create(new S2CUserListJoinNtc.Serializer());
            Create(new S2CUserListLeaveNtc.Serializer());
            Create(new S2CWarpAreaWarpRes.Serializer());
            Create(new S2CWarpGetFavoriteWarpPointListRes.Serializer());
            Create(new S2CWarpGetReleaseWarpPointListRes.Serializer());
            Create(new S2CWarpGetReturnLocationRes.Serializer());
            Create(new S2CWarpGetStartPointListRes.Serializer());
            Create(new S2CWarpGetWarpPointListRes.Serializer());
            Create(new S2CWarpLeaderWarpNtc.Serializer());
            Create(new S2CWarpPartyWarpRes.Serializer());
            Create(new S2CWarpRegisterFavoriteWarpRes.Serializer());
            Create(new S2CWarpReleaseWarpPointRes.Serializer());
            Create(new S2CWarpWarpRes.Serializer());

            Create(new S2CFriendGetFriendListRes.Serializer());
            Create(new S2CFriendApplyFriendRes.Serializer());
            Create(new S2CFriendApproveFriendRes.Serializer());
            Create(new S2CFriendRemoveFriendRes.Serializer());
            Create(new S2CFriendRegisterFavoriteFriendRes.Serializer());
            Create(new S2CFriendCancelFriendApplicationRes.Serializer());
            Create(new S2CCharacterCommunityCharacterStatusGetRes.Serializer());

            Create(new S2CFriendApplyFriendNtc.Serializer());
            Create(new S2CFriendApproveFriendNtc.Serializer());
            Create(new S2CFriendRemoveFriendNtc.Serializer());
            Create(new S2CFriendCancelFriendApplicationNtc.Serializer());

            Create(new S2CInstanceGetOmInstantKeyValueAllRes.Serializer());
            Create(new S2CAreaGetLeaderAreaReleaseListRes.Serializer());

            Create(new S2CStampBonusGetListRes.Serializer());
            Create(new S2CStampBonusCheckRes.Serializer());

            Create(new ServerRes.Serializer());
        }

        private static void Create<T>(PacketEntitySerializer<T> serializer) where T : class, IPacketStructure, new()
        {
            Type type = serializer.GetEntityType();
            Serializers.Add(type, serializer);

            PacketId packetId = new T().Id;
            if (packetId != PacketId.UNKNOWN)
            {
                if (packetId.ServerType == ServerType.Login)
                {
                    if (LoginPacketSerializers.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `LoginPacketSerializers` lookup");
                    }
                    else
                    {
                        LoginPacketSerializers.Add(packetId, serializer);
                    }

                    if (LoginStructurePacketFactories.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `LoginStructurePacketFactories` lookup");
                    }
                    else
                    {
                        LoginStructurePacketFactories.Add(packetId, serializer);
                    }
                }
                else if (packetId.ServerType == ServerType.Game)
                {
                    if (GamePacketSerializers.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `GamePacketSerializers` lookup");
                    }
                    else
                    {
                        GamePacketSerializers.Add(packetId, serializer);
                    }

                    if (GameStructurePacketFactories.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `GameStructurePacketFactories` lookup");
                    }
                    else
                    {
                        GameStructurePacketFactories.Add(packetId, serializer);
                    }
                }
            }
        }

        private static void Create<T>(EntitySerializer<T> serializer) where T : class, new()
        {
            if (typeof(IPacketStructure).IsAssignableFrom(typeof(T))
                && typeof(T) != typeof(ServerRes)) // ServerRes is exception to this rule as it is a generic response.
            {
                Logger.Error($"EntitySerializer<{typeof(T)}> should be PacketEntitySerializer<{typeof(T)}> " +
                             $"because {typeof(T)} is assignable from `IPacketStructure`, indicating it is a PacketStructure");
            }

            Type type = serializer.GetEntityType();
            if (Serializers.ContainsKey(type))
            {
                Logger.Error($"Type:{type} has already been added to `Serializers` lookup");
                return;
            }

            Serializers.Add(type, serializer);
        }

        /// <summary>
        /// Provides a Serializer for a specific type of Structure
        /// </summary>
        public static EntitySerializer<T> Get<T>() where T : class, new()
        {
            Type type = typeof(T);
            if (!Serializers.ContainsKey(type))
            {
                return null;
            }

            object obj = Serializers[type];
            EntitySerializer<T> serializer = obj as EntitySerializer<T>;
            return serializer;
        }

        /// <summary>
        /// Provides a Serializer for a PacketId
        /// </summary>
        public static EntitySerializer Get(PacketId packetId)
        {
            if (packetId.ServerType == ServerType.Login && LoginPacketSerializers.ContainsKey(packetId))
            {
                return LoginPacketSerializers[packetId];
            }

            if (packetId.ServerType == ServerType.Game && GamePacketSerializers.ContainsKey(packetId))
            {
                return GamePacketSerializers[packetId];
            }

            return null;
        }

        /// <summary>
        /// Creates a StructuredPacket from a Packet
        /// </summary>
        public static IStructurePacket CreateStructurePacket(Packet packet)
        {
            PacketId packetId = packet.Id;
            if (packetId.ServerType == ServerType.Login && LoginStructurePacketFactories.ContainsKey(packetId))
            {
                return LoginStructurePacketFactories[packetId].Create(packet);
            }

            if (packetId.ServerType == ServerType.Game && GameStructurePacketFactories.ContainsKey(packetId))
            {
                return GameStructurePacketFactories[packetId].Create(packet);
            }

            return null;
        }

        public abstract void WriteObj(IBuffer buffer, object obj);
        public abstract object ReadObj(IBuffer buffer);
        protected abstract Type GetEntityType();
    }

    /// <summary>
    /// PacketStructure Serializer
    /// </summary>
    public abstract class PacketEntitySerializer<T> : EntitySerializer<T>, IStructurePacketFactory
        where T : class, IPacketStructure, new()
    {
        public IStructurePacket Create(Packet packet)
        {
            return new StructurePacket<T>(packet);
        }
    }

    /// <summary>
    /// Generic Object Serializer
    /// </summary>
    public abstract class EntitySerializer<T> : EntitySerializer where T : class, new()
    {
        public override void WriteObj(IBuffer buffer, object obj)
        {
            if (obj is T t)
            {
                Write(buffer, t);
            }
        }

        public override object ReadObj(IBuffer buffer)
        {
            return Read(buffer);
        }

        public abstract void Write(IBuffer buffer, T obj);
        public abstract T Read(IBuffer buffer);

        public List<T> ReadList(IBuffer buffer)
        {
            return ReadEntityList<T>(buffer);
        }

        public void WriteList(IBuffer buffer, List<T> entities)
        {
            WriteEntityList<T>(buffer, entities);
        }

        protected override Type GetEntityType()
        {
            return typeof(T);
        }

        protected void WriteFloat(IBuffer buffer, float value)
        {
            buffer.WriteFloat(value, Endianness.Big);
        }

        protected float ReadFloat(IBuffer buffer)
        {
            return buffer.ReadFloat(Endianness.Big);
        }

        protected void WriteDouble(IBuffer buffer, double value)
        {
            buffer.WriteDouble(value, Endianness.Big);
        }

        protected double ReadDouble(IBuffer buffer)
        {
            return buffer.ReadDouble(Endianness.Big);
        }

        protected void WriteUInt64Array(IBuffer buffer, ulong[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                WriteUInt64(buffer, values[i]);
            }
        }

        protected void WriteUInt64(IBuffer buffer, ulong value)
        {
            buffer.WriteUInt64(value, Endianness.Big);
        }

        protected ulong[] ReadUInt64Array(IBuffer buffer, int length)
        {
            ulong[] values = new ulong[length];
            for (int i = 0; i < length; i++)
            {
                values[i] = ReadUInt64(buffer);
            }

            return values;
        }

        protected ulong ReadUInt64(IBuffer buffer)
        {
            return buffer.ReadUInt64(Endianness.Big);
        }

        protected void WriteUInt32Array(IBuffer buffer, uint[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                WriteUInt32(buffer, values[i]);
            }
        }

        protected void WriteUInt32(IBuffer buffer, uint value)
        {
            buffer.WriteUInt32(value, Endianness.Big);
        }

        protected uint[] ReadUInt32Array(IBuffer buffer, int length)
        {
            uint[] values = new uint[length];
            for (int i = 0; i < length; i++)
            {
                values[i] = ReadUInt32(buffer);
            }

            return values;
        }

        protected uint ReadUInt32(IBuffer buffer)
        {
            return buffer.ReadUInt32(Endianness.Big);
        }

        protected void WriteUInt16(IBuffer buffer, ushort value)
        {
            buffer.WriteUInt16(value, Endianness.Big);
        }

        protected ushort ReadUInt16(IBuffer buffer)
        {
            return buffer.ReadUInt16(Endianness.Big);
        }

        protected void WriteInt64(IBuffer buffer, long value)
        {
            buffer.WriteInt64(value, Endianness.Big);
        }

        protected long ReadInt64(IBuffer buffer)
        {
            return buffer.ReadInt64(Endianness.Big);
        }

        protected void WriteInt32Array(IBuffer buffer, int[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                WriteInt32(buffer, values[i]);
            }
        }

        protected void WriteInt32(IBuffer buffer, int value)
        {
            buffer.WriteInt32(value, Endianness.Big);
        }

        protected int[] ReadInt32Array(IBuffer buffer, int length)
        {
            int[] values = new int[length];
            for (int i = 0; i < length; i++)
            {
                values[i] = ReadInt32(buffer);
            }

            return values;
        }

        protected int ReadInt32(IBuffer buffer)
        {
            return buffer.ReadInt32(Endianness.Big);
        }

        protected void WriteInt16(IBuffer buffer, short value)
        {
            buffer.WriteInt16(value, Endianness.Big);
        }

        protected short ReadInt16(IBuffer buffer)
        {
            return buffer.ReadInt16(Endianness.Big);
        }

        protected void WriteBool(IBuffer buffer, bool value)
        {
            buffer.WriteBool(value);
        }

        protected void WriteByteArray(IBuffer buffer, byte[] value)
        {
            buffer.WriteBytes(value);
        }

        protected void WriteByte(IBuffer buffer, byte value)
        {
            buffer.WriteByte(value);
        }

        protected bool ReadBool(IBuffer buffer)
        {
            return buffer.ReadBool();
        }

        protected byte[] ReadByteArray(IBuffer buffer, int length)
        {
            return buffer.ReadBytes(length);
        }

        protected byte ReadByte(IBuffer buffer)
        {
            return buffer.ReadByte();
        }

        protected void WriteMtString(IBuffer buffer, string str)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(str);
            buffer.WriteUInt16((ushort)utf8.Length, Endianness.Big);
            buffer.WriteBytes(utf8);
        }

        protected string ReadMtString(IBuffer buffer)
        {
            ushort len = buffer.ReadUInt16(Endianness.Big);
            string str = buffer.ReadString(len, Encoding.UTF8);
            return str;
        }

        protected void WriteServerResponse(IBuffer buffer, ServerResponse value)
        {
            buffer.WriteUInt32(value.Error, Endianness.Big);
            buffer.WriteUInt32(value.Result, Endianness.Big);
        }

        protected void ReadServerResponse(IBuffer buffer, ServerResponse value)
        {
            value.Error = buffer.ReadUInt32(Endianness.Big);
            value.Result = value.Error != 0 ? buffer.ReadUInt16(Endianness.Big) : buffer.ReadUInt32(Endianness.Big);
        }

        protected void WriteEntity<TEntity>(IBuffer buffer, TEntity entity) where TEntity : class, new()
        {
            EntitySerializer<TEntity> serializer = Get<TEntity>();
            if (serializer == null)
            {
                // error
                return;
            }

            serializer.Write(buffer, entity);
        }

        public static void WriteMtArray<TEntity>(IBuffer buffer, List<TEntity> entities,
            Action<IBuffer, TEntity> writer)
        {
            buffer.WriteMtArray(entities, writer, Endianness.Big);
        }

        public static List<TEntity> ReadMtArray<TEntity>(IBuffer buffer, Func<IBuffer, TEntity> reader)
        {
            return buffer.ReadMtArray(reader, Endianness.Big);
        }

        protected void WriteEntityList<TEntity>(IBuffer buffer, List<TEntity> entities) where TEntity : class, new()
        {
            WriteUInt32(buffer, (uint)entities.Count);
            for (int i = 0; i < entities.Count; i++)
            {
                WriteEntity(buffer, entities[i]);
            }
        }

        protected List<TEntity> ReadEntityList<TEntity>(IBuffer buffer) where TEntity : class, new()
        {
            List<TEntity> entities = new List<TEntity>();
            uint len = ReadUInt32(buffer);
            for (int i = 0; i < len; i++)
            {
                entities.Add(ReadEntity<TEntity>(buffer));
            }

            return entities;
        }

        protected TEntity ReadEntity<TEntity>(IBuffer buffer) where TEntity : class, new()
        {
            EntitySerializer<TEntity> serializer = Get<TEntity>();
            if (serializer == null)
            {
                // error
                return default;
            }

            return serializer.Read(buffer);
        }

        public byte[] Write(T entity)
        {
            IBuffer buffer = new StreamBuffer();
            Write(buffer, entity);
            return buffer.GetAllBytes();
        }

        public T Read(byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            buffer.SetPositionStart();
            return Read(buffer);
        }
    }
}
