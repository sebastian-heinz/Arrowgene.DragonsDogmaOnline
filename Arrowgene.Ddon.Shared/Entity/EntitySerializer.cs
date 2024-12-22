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

            Create(new CDataAbilityLevelParam.Serializer());
            Create(new CDataAbilityParam.Serializer());
            Create(new CDataAchieveCategoryStatus.Serializer());
            Create(new CDataAchieveRewardCommon.Serializer());
            Create(new CDataAchievementFurnitureReward.Serializer());
            Create(new CDataAchievementIdentifier.Serializer());
            Create(new CDataAchievementProgress.Serializer());
            Create(new CDataAchievementRewardProgress.Serializer());
            Create(new CDataAddStatusParam.Serializer());
            Create(new CDataAllPlayerContext.Serializer());
            Create(new CDataAreaBaseInfo.Serializer());
            Create(new CDataAreaBonus.Serializer());
            //Create(new CDataAreaInfoList.Serializer());
            //Create(new CDataAreaQuestHint.Serializer());
            Create(new CDataAreaRank.Serializer());
            //Create(new CDataAreaRankSeason3.Serializer());
            //Create(new CDataAreaRankUnk0.Serializer());
            //Create(new CDataAreaRankUpQuestInfo.Serializer());
            Create(new CDataAreaSpotSet.Serializer());
            Create(new CDataArisenProfile.Serializer());

            Create(new CDataBattleContentAvailableRewards.Serializer());
            Create(new CDataBattleContentInfo.Serializer());
            Create(new CDataBattleContentRewardParam.Serializer());
            Create(new CDataBattleContentSituationData.Serializer());
            Create(new CDataBattleContentStage.Serializer());
            Create(new CDataBattleContentStageProgression.Serializer());
            Create(new CDataBattleContentStatus.Serializer());
            Create(new CDataBattleContentUnk4.Serializer());
            Create(new CDataBattleContentUnk5.Serializer());
            Create(new CDataBattleContentUnk6.Serializer());
            Create(new CDataBazaarCharacterInfo.Serializer());
            Create(new CDataBazaarItemBaseInfo.Serializer());
            Create(new CDataBazaarItemHistoryInfo.Serializer());
            Create(new CDataBazaarItemInfo.Serializer());
            Create(new CDataBazaarItemNumOfExhibitionInfo.Serializer());
            //Create(new CDataBorderSupplyItem.Serializer());

            Create(new CDataC2SActionSetPlayerActionHistoryReqElement.Serializer());
            Create(new CDataChangeEquipJobItem.Serializer());
            Create(new CDataCharacterEditPrice.Serializer());
            Create(new CDataCharacterEditPriceInfo.Serializer());
            Create(new CDataCharacterEquipData.Serializer());
            Create(new CDataCharacterEquipInfo.Serializer());
            Create(new CDataCharacterInfo.Serializer());
            Create(new CDataCharacterItemSlotInfo.Serializer());
            Create(new CDataCharacterJobData.Serializer());
            Create(new CDataCharacterLevelParam.Serializer());
            Create(new CDataCharacterListElement.Serializer());
            Create(new CDataCharacterListInfo.Serializer());
            Create(new CDataCharacterMessage.Serializer());
            Create(new CDataCharacterMsgSet.Serializer());
            Create(new CDataCharacterName.Serializer());
            Create(new CDataCharacterReleaseElement.Serializer());
            Create(new CDataCharacterSearchParam.Serializer());
            Create(new CDataCharacterSearchParameter.Serializer());
            Create(new CDataClanConciergeInfo.Serializer());
            Create(new CDataClanConciergeNpc.Serializer());
            Create(new CDataClanHistoryElement.Serializer());
            Create(new CDataClanJoinRequest.Serializer());
            Create(new CDataClanMemberInfo.Serializer());
            Create(new CDataClanNoticePackage.Serializer());
            Create(new CDataClanParam.Serializer());
            Create(new CDataClanPartnerPawnInfo.Serializer());
            Create(new CDataClanScoutEntryInviteInfo.Serializer());
            Create(new CDataClanScoutEntrySearchResult.Serializer());
            Create(new CDataClanSearchParam.Serializer());
            Create(new CDataClanSearchResult.Serializer());
            Create(new CDataClanServerParam.Serializer());
            Create(new CDataClanShopBuffInfo.Serializer());
            Create(new CDataClanShopBuffItem.Serializer());
            Create(new CDataClanShopConciergeItem.Serializer());
            Create(new CDataClanShopFunctionInfo.Serializer());
            Create(new CDataClanShopFunctionItem.Serializer());
            Create(new CDataClanShopInfo.Serializer());
            Create(new CDataClanShopLineupName.Serializer());
            Create(new CDataClanUserParam.Serializer());
            Create(new CDataClanValueInfo.Serializer());
            Create(new CDataClearTimePointBonus.Serializer());
            Create(new CDataCommonU32.Serializer());
            Create(new CDataCommonU64.Serializer());
            Create(new CDataCommonU8.Serializer());
            Create(new CDataCommunicationShortCut.Serializer());
            Create(new CDataCommunityCharacterBaseInfo.Serializer());
            Create(new CDataContentsPlayEnd.Serializer());
            Create(new CDataContentsPlayStartData.Serializer());
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
            Create(new CDataCraftColorant.Serializer());
            Create(new CDataCraftElement.Serializer());
            Create(new CDataCraftMaterial.Serializer());
            Create(new CDataCraftPawnInfo.Serializer());
            Create(new CDataCraftPawnList.Serializer());
            Create(new CDataCraftProduct.Serializer());
            Create(new CDataCraftProductInfo.Serializer());
            Create(new CDataCraftProgress.Serializer());
            Create(new CDataCraftSkillAnalyzeResult.Serializer());
            Create(new CDataCraftSkillRate.Serializer());
            Create(new CDataCraftStartEquipGradeUpUnk0.Serializer());
            Create(new CDataCraftStartEquipGradeUpUnk0Unk0.Serializer());
            Create(new CDataCraftSupportPawnID.Serializer());
            Create(new CDataCraftTimeSaveCost.Serializer());
            Create(new CDataCurrentEquipInfo.Serializer());
            //Create(new CDataCycleContentsNews.Serializer());
            //Create(new CDataCycleContentsNewsDetail.Serializer());
            //Create(new CDataCycleContentsRank.Serializer());
            Create(new CDataCycleContentsStateList.Serializer());
            Create(new CDataCycleContentsNews.Serializer());
            Create(new CDataCycleContentsNewsDetail.Serializer());
            Create(new CDataCycleContentsRank.Serializer());

            Create(new CDataDeliveredItem.Serializer());
            Create(new CDataDeliveredItemRecord.Serializer());
            Create(new CDataDeliveryItem.Serializer());
            Create(new CDataDispelBaseItem.Serializer());
            Create(new CDataDispelBaseItemData.Serializer());
            Create(new CDataDispelItemCategoryInfo.Serializer());
            Create(new CDataDispelLotColor.Serializer());
            Create(new CDataDispelLotCrest.Serializer());
            Create(new CDataDispelLotCrestUnk2.Serializer());
            Create(new CDataDispelLotData.Serializer());
            Create(new CDataDispelLotItem.Serializer());
            Create(new CDataDispelLotPlus.Serializer());
            Create(new CDataDispelResultInfo.Serializer());
            Create(new CDataDragonAbility.Serializer());
            Create(new CDataDropItemSetInfo.Serializer());

            Create(new CDataEditInfo.Serializer());
            Create(new CDataEditParam.Serializer());
            Create(new CDataEditParamPalette.Serializer());
            Create(new CDataEntryBoardItemSearchParameter.Serializer());
            Create(new CDataEntryBoardListParam.Serializer());
            Create(new CDataEntryItem.Serializer());
            Create(new CDataEntryItemParam.Serializer());
            Create(new CDataEntryMemberData.Serializer());
            Create(new CDataEntryRecruitCategoryData.Serializer());
            Create(new CDataEntryRecruitData.Serializer());
            Create(new CDataEntryRecruitJob.Serializer());
            Create(new CDataEquipElementParam.Serializer());
            Create(new CDataEquipItemInfo.Serializer());
            Create(new CDataEquipItemInfoUnk2.Serializer());
            Create(new CDataEquipJobItem.Serializer());
            Create(new CDataEquipSlot.Serializer());
            Create(new CDataErrorMessage.Serializer());
            Create(new CDataExpiredQuestList.Serializer());
            Create(new CDataExpRequirement.Serializer());
            Create(new CDataExpSetting.Serializer());

            Create(new CDataFavoriteWarpPoint.Serializer());
            Create(new CDataFriendInfo.Serializer());
            Create(new CDataFurnitureLayout.Serializer());
            Create(new CDataFurnitureLayoutData.Serializer());

            Create(new CDataGPCourseAvailable.Serializer());
            Create(new CDataGPCourseEffectParam.Serializer());
            Create(new CDataGPCourseInfo.Serializer());
            Create(new CDataGPCourseValid.Serializer());
            Create(new CDataGPDetail.Serializer());
            Create(new CDataGPPeriod.Serializer());
            Create(new CDataGameServerListInfo.Serializer());
            Create(new CDataGameSetting.Serializer());
            Create(new CDataGameTimeBaseInfo.Serializer());
            Create(new CDataGatheringItemElement.Serializer());
            Create(new CDataGatheringItemGetRequest.Serializer());
            Create(new CDataGatheringItemRestriction.Serializer());
            Create(new CDataGetDispelItem.Serializer());
            Create(new CDataGetRewardBoxItem.Serializer());
            Create(new CDataGoodsParam.Serializer());
            Create(new CDataGoodsParamRequirement.Serializer());

            Create(new CDataHasRegionBreakReward.Serializer());
            Create(new CDataHistoryElement.Serializer());

            Create(new CDataInformationParagraph.Serializer());
            Create(new CDataItemEmbodyCostParam.Serializer());
            Create(new CDataItemEmbodyItem.Serializer());
            Create(new CDataItemEquipElement.Serializer());
            Create(new CDataItemEquipElementParam.Serializer());
            Create(new CDataItemList.Serializer());
            Create(new CDataItemSort.Serializer());
            Create(new CDataItemStorageIndicateNum.Serializer());
            Create(new CDataItemUIdList.Serializer());
            Create(new CDataItemUpdateResult.Serializer());

            Create(new CDataJewelryEquipLimit.Serializer());
            Create(new CDataJobBaseInfo.Serializer());
            Create(new CDataJobChangeInfo.Serializer());
            Create(new CDataJobChangeJobResUnk0.Serializer());
            Create(new CDataJobExpMode.Serializer());
            Create(new CDataJobOrbDevoteElement.Serializer());
            Create(new CDataJobOrbTreeStatus.Serializer());
            Create(new CDataJobPlayPoint.Serializer());
            Create(new CDataJobValueShopItem.Serializer());
            Create(new CDataJumpLocation.Serializer());

            Create(new CDataLayoutEnemyData.Serializer());
            Create(new CDataLearnNormalSkillParam.Serializer());
            Create(new CDataLearnedSetAcquirementParam.Serializer());
            Create(new CDataLevelBonus.Serializer());
            Create(new CDataLevelBonusElement.Serializer());
            Create(new CDataLightQuestClearList.Serializer());
            Create(new CDataLightQuestDetail.Serializer());
            Create(new CDataLightQuestList.Serializer());
            Create(new CDataLightQuestOrderList.Serializer());
            Create(new CDataLoadingInfoSchedule.Serializer());
            Create(new CDataLobbyContextPlayer.Serializer());
            Create(new CDataLobbyMemberInfo.Serializer());
            Create(new CDataLoginSetting.Serializer());
            Create(new CDataLostPawnList.Serializer());
            Create(new CDataLotQuestOrderList.Serializer());

            Create(new CDataMDataCraftGradeupRecipe.Serializer());
            Create(new CDataMDataCraftMaterial.Serializer());
            Create(new CDataMDataCraftRecipe.Serializer());
            Create(new CDataMailAttachmentInfo.Serializer());
            Create(new CDataMailAttachmentList.Serializer());
            Create(new CDataMailGPInfo.Serializer());
            Create(new CDataMailInfo.Serializer());
            Create(new CDataMailItemInfo.Serializer());
            Create(new CDataMailLegendPawnInfo.Serializer());
            Create(new CDataMailOptionCourseInfo.Serializer());
            Create(new CDataMailTextInfo.Serializer());
            Create(new CDataMainQuestList.Serializer());
            Create(new CDataMainQuestOrderList.Serializer());
            Create(new CDataMasterInfo.Serializer());
            Create(new CDataMatchingProfile.Serializer());
            Create(new CDataMobHuntQuestDetail.Serializer());
            Create(new CDataMobHuntQuestOrderList.Serializer());
            Create(new CDataMoonSchedule.Serializer());
            Create(new CDataMoveItemUIDFromTo.Serializer());

            Create(new CDataNamedEnemyParamClient.Serializer());
            Create(new CDataNoraPawnInfo.Serializer());
            Create(new CDataNormalSkillParam.Serializer());
            Create(new CDataNpcExtendedFacilityMenuItem.Serializer());

            Create(new CDataOcdActive.Serializer());
            Create(new CDataOmData.Serializer());
            Create(new CDataOrbCategoryStatus.Serializer());
            Create(new CDataOrbGainExtendParam.Serializer());
            Create(new CDataOrbPageStatus.Serializer());
            Create(new CDataOrderConditionInfo.Serializer());

            Create(new CDataPackageQuestDetail.Serializer());
            Create(new CDataPackageQuestList.Serializer());
            Create(new CDataPartnerPawnInfo.Serializer());
            Create(new CDataPartyContextPawn.Serializer());
            Create(new CDataPartyListInfo.Serializer());
            Create(new CDataPartyMember.Serializer());
            Create(new CDataPartyMemberMaxNum.Serializer());
            Create(new CDataPartyMemberMinimum.Serializer());
            Create(new CDataPartyPlayerContext.Serializer());
            Create(new CDataPartyQuestProgressInfo.Serializer());
            Create(new CDataPawnCraftData.Serializer());
            Create(new CDataPawnCraftSkill.Serializer());
            Create(new CDataPawnEquipInfo.Serializer());
            Create(new CDataPawnExpeditionClanSallySpotInfo.Serializer());
            Create(new CDataPawnExpeditionInfo.Serializer());
            Create(new CDataPawnFeedback.Serializer());
            Create(new CDataPawnHistory.Serializer());
            Create(new CDataPawnHp.Serializer());
            Create(new CDataPawnInfo.Serializer());
            Create(new CDataPawnJobChangeInfo.Serializer());
            Create(new CDataPawnList.Serializer());
            Create(new CDataPawnListData.Serializer());
            Create(new CDataPawnReaction.Serializer());
            Create(new CDataPawnSearchParameter.Serializer());
            Create(new CDataPawnTotalScore.Serializer());
            Create(new CDataPawnTrainingPreparationInfoToAdvice.Serializer());
            Create(new CDataPlayPointData.Serializer());
            Create(new CDataPresetAbilityParam.Serializer());
            Create(new CDataPriorityQuest.Serializer());
            Create(new CDataPriorityQuestSetting.Serializer());

            Create(new CDataQuestAdventureGuideList.Serializer());
            Create(new CDataQuestAnnounce.Serializer());
            Create(new CDataQuestCommand.Serializer());
            Create(new CDataQuestContents.Serializer());
            Create(new CDataQuestDefine.Serializer());
            Create(new CDataQuestEnemyInfo.Serializer());
            Create(new CDataQuestExp.Serializer());
            Create(new CDataQuestFlag.Serializer());
            Create(new CDataQuestId.Serializer());
            Create(new CDataQuestIdScheduleId.Serializer());
            Create(new CDataQuestKeyItemPoint.Serializer());
            Create(new CDataQuestKeyItemPointRecord.Serializer());
            Create(new CDataQuestLayoutFlag.Serializer());
            Create(new CDataQuestLayoutFlagSetInfo.Serializer());
            Create(new CDataQuestList.Serializer());
            Create(new CDataQuestLog.Serializer());
            Create(new CDataQuestMobHuntQuestInfo.Serializer());
            Create(new CDataQuestOrderConditionParam.Serializer());
            Create(new CDataQuestOrderList.Serializer());
            Create(new CDataQuestOrderListUnk8.Serializer());
            Create(new CDataQuestPartyBonusInfo.Serializer());
            Create(new CDataQuestProcessState.Serializer());
            Create(new CDataQuestProcessState.MtTypedArrayCDataQuestCommand.Serializer());
            Create(new CDataQuestProgressWork.Serializer());
            Create(new CDataQuestRecruitListItem.Serializer());
            Create(new CDataQuestSetInfo.Serializer());
            Create(new CDataQuestTalkInfo.Serializer());

            Create(new CDataRaidBossEnemyParam.Serializer());
            Create(new CDataRaidBossPlayStartData.Serializer());
            Create(new CDataRefiningMaterialInfo.Serializer());
            Create(new CDataRegisterdPawnList.Serializer());
            Create(new CDataRegisteredLegendPawnInfo.Serializer());
            Create(new CDataReleaseAreaInfoSet.Serializer());
            Create(new CDataReleaseOrbElement.Serializer());
            Create(new CDataRentedPawnList.Serializer());
            Create(new CDataResetInfo.Serializer());
            Create(new CDataResetInfoUnk0.Serializer());
            Create(new CDataRewardBoxItem.Serializer());
            Create(new CDataRewardBoxRecord.Serializer());
            Create(new CDataRewardItem.Serializer());
            Create(new CDataRewardItemDetail.Serializer());
            //Create(new CDataRewardItemInfo.Serializer());

            Create(new CDataS2CCraftGetCraftSettingResUnk0Unk6.Serializer());
            Create(new CDataS2CCraftStartQualityUpResUnk0.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk10.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1.Serializer());
            Create(new CDataRequiredItem.Serializer());
            Create(new CDataS2CEquipEnhancedGetPacksResUnk0Unk9.Serializer());
            Create(new CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1.Serializer());
            Create(new CDataScreenShotCategory.Serializer());
            Create(new CDataSeasonDungeonBlockadeElement.Serializer());
            Create(new CDataSeasonDungeonBuffEffectParam.Serializer());
            Create(new CDataSeasonDungeonBuffEffectReward.Serializer());
            Create(new CDataSeasonDungeonInfo.Serializer());
            Create(new CDataSeasonDungeonRewardItemViewEntry.Serializer());
            Create(new CDataSeasonDungeonSection.Serializer());
            Create(new CDataSeasonDungeonUnk0.Serializer());
            Create(new CDataSeasonDungeonUnk2.Serializer());
            //Create(new CDataSelectItemInfo.Serializer());
            Create(new CDataSetAcquirementParam.Serializer());
            Create(new CDataSetQuestBonusList.Serializer());
            Create(new CDataSetQuestDetail.Serializer());
            Create(new CDataSetQuestInfoList.Serializer());
            Create(new CDataSetQuestList.Serializer());
            Create(new CDataSetQuestOrderList.Serializer());
            Create(new CDataShortCut.Serializer());
            Create(new CDataSituationObjective.Serializer());
            Create(new CDataSkillLevelParam.Serializer());
            Create(new CDataSkillParam.Serializer());
            Create(new CDataSoulOrdealElementParam.Serializer());
            Create(new CDataSoulOrdealItem.Serializer());
            Create(new CDataSoulOrdealItemInfo.Serializer());
            Create(new CDataSoulOrdealObjective.Serializer());
            Create(new CDataSoulOrdealRewardItem.Serializer());
            Create(new CDataSoulOrdealUnk0.Serializer());
            Create(new CDataSoulOrdealUnk1.Serializer());
            Create(new CDataSpSkill.Serializer());
            //Create(new CDataSpotEnemyInfo.Serializer());
            //Create(new CDataSpotInfo.Serializer());
            //Create(new CDataSpotItemInfo.Serializer());
            Create(new CDataStageAreaChangeResUnk0.Serializer());
            Create(new CDataStageAreaChangeResUnk1.Serializer());
            Create(new CDataStageAttribute.Serializer());
            Create(new CDataStageDungeonItem.Serializer());
            Create(new CDataStageInfo.Serializer());
            Create(new CDataStageLayoutEnemyPresetEnemyInfoClient.Serializer());
            Create(new CDataStageLayoutId.Serializer());
            Create(new CDataStageTicketDungeonCategory.Serializer());
            Create(new CDataStageTicketDungeonCategoryInfo.Serializer());
            Create(new CDataStageTicketDungeonItemInfo.Serializer());
            Create(new CDataStampBonus.Serializer());
            Create(new CDataStampBonusAsset.Serializer());
            Create(new CDataStampBonusDaily.Serializer());
            Create(new CDataStampBonusTotal.Serializer());
            Create(new CDataStampCheck.Serializer());
            Create(new CDataStatusInfo.Serializer());
            Create(new CDataStorageEmptySlotNum.Serializer());
            Create(new CDataStorageItemUIDList.Serializer());
            Create(new CDataSubstoryQuestOrderList.Serializer());
            //Create(new CDataSupplyItem.Serializer());

            Create(new CDataTimeGainQuestList.Serializer());
            Create(new CDataTimeGainQuestRestrictions.Serializer());
            Create(new CDataTimeGainQuestUnk1Unk2.Serializer());
            Create(new CDataTimeGainQuestUnk2.Serializer());
            Create(new CDataTimeLimitedQuestOrderList.Serializer());
            Create(new CDataTraningRoomEnemyHeader.Serializer());
            Create(new CDataTutorialQuestList.Serializer());
            Create(new CDataTutorialQuestOrderList.Serializer());

            Create(new CDataURLInfo.Serializer());
            Create(new CDataUpdateMatchingProfileInfo.Serializer());
            Create(new CDataUpdateWalletPoint.Serializer());

            Create(new CDataWalletLimit.Serializer());
            Create(new CDataWalletPoint.Serializer());
            Create(new CDataWarpPoint.Serializer());
            Create(new CDataWeatherForecast.Serializer());
            Create(new CDataWeatherLoop.Serializer());
            Create(new CDataWeatherSchedule.Serializer());
            Create(new CDataWorldManageQuestList.Serializer());
            Create(new CDataWorldManageQuestOrderList.Serializer());

            Create(new CData_772E80.Serializer());

            // Packet structure serializers
            Create(new C2LClientChallengeReq.Serializer());
            Create(new C2LCreateCharacterDataReq.Serializer());
            Create(new C2LDecideCharacterIdReq.Serializer());
            Create(new C2LDeleteCharacterInfoReq.Serializer());
            Create(new C2LGetCharacterListReq.Serializer());
            Create(new C2LGetErrorMessageListReq.Serializer());
            Create(new C2LGetGameSessionKeyReq.Serializer());
            Create(new C2LGetLoginSettingReq.Serializer());
            Create(new C2LGpCourseGetInfoReq.Serializer());
            Create(new C2LLoginReq.Serializer());
            Create(new C2LLogoutReq.Serializer());
            Create(new C2LPingReq.Serializer());

            Create(new C2SAchievementCompleteNtc.Serializer());
            Create(new C2SAchievementGetCategoryProgressListReq.Serializer());
            Create(new C2SAchievementGetFurnitureRewardListReq.Serializer());
            Create(new C2SAchievementGetProgressListReq.Serializer());
            Create(new C2SAchievementGetReceivableRewardListReq.Serializer());
            Create(new C2SAchievementGetRewardListReq.Serializer());
            Create(new C2SAchievementReceivableRewardNtc.Serializer());
            Create(new C2SAchievementRewardReceiveReq.Serializer());

            Create(new C2SActionSetPlayerActionHistoryReq.Serializer());

            //Create(new C2SAreaAreaRankUpReq.Serializer());
            //Create(new C2SAreaBuyAreaQuestHintReq.Serializer());
            Create(new C2SAreaGetAreaBaseInfoListReq.Serializer());
            //Create(new C2SAreaGetAreaMasterInfoReq.Serializer());
            //Create(new C2SAreaGetAreaQuestHintListReq.Serializer());
            //Create(new C2SAreaGetAreaReleaseListReq.Serializer());
            //Create(new C2SAreaGetAreaRewardInfoReq.Serializer());
            //Create(new C2SAreaGetAreaSupplyInfoReq.Serializer());
            //Create(new C2SAreaGetAreaSupplyReq.Serializer());
            //Create(new C2SAreaGetLeaderAreaReleaseListReq.Serializer());
            //Create(new C2SAreaGetSpotInfoListReq.Serializer());

            Create(new C2SBattleContentCharacterInfoReq.Serializer());
            Create(new C2SBattleContentContentEntryReq.Serializer());
            Create(new C2SBattleContentContentFirstPhaseChangeReq.Serializer());
            Create(new C2SBattleContentContentResetReq.Serializer());
            Create(new C2SBattleContentGetContentStatusFromOmReq.Serializer());
            Create(new C2SBattleContentGetRewardReq.Serializer());
            Create(new C2SBattleContentInfoListReq.Serializer());
            Create(new C2SBattleContentInstantClearInfoReq.Serializer());
            Create(new C2SBattleContentPartyMemberInfoReq.Serializer());
            Create(new C2SBattleContentPartyMemberInfoUpdateReq.Serializer());
            Create(new C2SBattleContentResetInfoReq.Serializer());
            Create(new C2SBattleContentRewardListReq.Serializer());

            Create(new C2SBazaarCancelReq.Serializer());
            Create(new C2SBazaarExhibitReq.Serializer());
            Create(new C2SBazaarGetCharacterListReq.Serializer());
            Create(new C2SBazaarGetExhibitPossibleNumReq.Serializer());
            Create(new C2SBazaarGetItemHistoryInfoReq.Serializer());
            Create(new C2SBazaarGetItemInfoReq.Serializer());
            Create(new C2SBazaarGetItemListReq.Serializer());
            Create(new C2SBazaarGetItemPriceLimitReq.Serializer());
            Create(new C2SBazaarProceedsReq.Serializer());
            Create(new C2SBazaarReExhibitReq.Serializer());
            Create(new C2SBazaarReceiveProceedsReq.Serializer());
            Create(new C2SBinarySaveSetCharacterBinSaveDataReq.Serializer());

            Create(new C2SCertClientChallengeReq.Serializer());

            Create(new C2SCharacterCharacterDeadNtc.Serializer());
            Create(new C2SCharacterCharacterDownCancelNtc.Serializer());
            Create(new C2SCharacterCharacterDownNtc.Serializer());
            Create(new C2SCharacterCharacterGoldenReviveReq.Serializer());
            Create(new C2SCharacterCharacterPenaltyReviveReq.Serializer());
            Create(new C2SCharacterCharacterPointReviveReq.Serializer());
            Create(new C2SCharacterCharacterSearchReq.Serializer());
            Create(new C2SCharacterChargeRevivePointReq.Serializer());
            Create(new C2SCharacterCommunityCharacterStatusGetReq.Serializer());
            Create(new C2SCharacterCreateModeCharacterEditParamReq.Serializer());
            Create(new C2SCharacterDecideCharacterIdReq.Serializer());
            Create(new C2SCharacterEditGetShopPriceReq.Serializer());
            Create(new C2SCharacterEditUpdateCharacterEditParamExReq.Serializer());
            Create(new C2SCharacterEditUpdateCharacterEditParamReq.Serializer());
            Create(new C2SCharacterEditUpdatePawnEditParamExReq.Serializer());
            Create(new C2SCharacterEditUpdatePawnEditParamReq.Serializer());
            Create(new C2SCharacterGetReviveChargeableTimeReq.Serializer());
            Create(new C2SCharacterPawnDeadNtc.Serializer());
            Create(new C2SCharacterPawnDownCancelNtc.Serializer());
            Create(new C2SCharacterPawnDownNtc.Serializer());
            Create(new C2SCharacterPawnGoldenReviveReq.Serializer());
            Create(new C2SCharacterPawnPointReviveReq.Serializer());
            Create(new C2SCharacterSetOnlineStatusReq.Serializer());
            Create(new C2SCharacterSwitchGameModeReq.Serializer());

            Create(new C2SChatSendTellMsgReq.Serializer());

            Create(new C2SClanClanBaseGetInfoReq.Serializer());
            Create(new C2SClanClanBaseReleaseReq.Serializer());
            Create(new C2SClanClanConciergeGetListReq.Serializer());
            Create(new C2SClanClanConciergeUpdateReq.Serializer());
            Create(new C2SClanClanCreateReq.Serializer());
            Create(new C2SClanClanExpelMemberReq.Serializer());
            Create(new C2SClanClanGetHistoryReq.Serializer());
            Create(new C2SClanClanGetInfoReq.Serializer());
            Create(new C2SClanClanGetJoinRequestedListReq.Serializer());
            Create(new C2SClanClanGetMemberListReq.Serializer());
            Create(new C2SClanClanGetMyInfoReq.Serializer());
            Create(new C2SClanClanGetMyJoinRequestListReq.Serializer());
            Create(new C2SClanClanGetMyMemberListReq.Serializer());
            Create(new C2SClanClanInviteAcceptReq.Serializer());
            Create(new C2SClanClanInviteReq.Serializer());
            Create(new C2SClanClanLeaveMemberReq.Serializer());
            Create(new C2SClanClanNegotiateMasterReq.Serializer());
            Create(new C2SClanClanPartnerPawnDataGetReq.Serializer());
            Create(new C2SClanClanScoutEntryGetInviteListReq.Serializer());
            Create(new C2SClanClanScoutEntryGetInvitedListReq.Serializer());
            Create(new C2SClanClanScoutEntryGetMyReq.Serializer());
            Create(new C2SClanClanScoutEntrySearchReq.Serializer());
            Create(new C2SClanClanSearchReq.Serializer());
            Create(new C2SClanClanSetMemberRankReq.Serializer());
            Create(new C2SClanClanSettingUpdateReq.Serializer());
            Create(new C2SClanClanShopBuyBuffItemReq.Serializer());
            Create(new C2SClanClanShopBuyFunctionItemReq.Serializer());
            Create(new C2SClanClanShopGetBuffItemListReq.Serializer());
            Create(new C2SClanClanShopGetFunctionItemListReq.Serializer());
            Create(new C2SClanClanUpdateReq.Serializer());
            Create(new C2SClanGetFurnitureReq.Serializer());
            Create(new C2SClanSetFurnitureReq.Serializer());

            Create(new C2SConnectionGetLoginAnnouncementReq.Serializer());
            Create(new C2SConnectionLoginReq.Serializer());
            Create(new C2SConnectionLogoutReq.Serializer());
            Create(new C2SConnectionMoveInServerReq.Serializer());
            Create(new C2SConnectionMoveOutServerReq.Serializer());
            Create(new C2SConnectionPingReq.Serializer());
            Create(new C2SConnectionReserveServerReq.Serializer());

            Create(new C2SContextGetSetContextReq.Serializer());
            Create(new C2SContextMasterThrowReq.Serializer());
            Create(new C2SContextSetContextNtc.Serializer());
            Create(new C2SContext_35_5_16_Ntc.Serializer());

            Create(new C2SCraftCancelCraftReq.Serializer());
            Create(new C2SCraftCancelCraftRes.Serializer());
            Create(new C2SCraftGetCraftIRCollectionValueListReq.Serializer());
            Create(new C2SCraftGetCraftProductInfoReq.Serializer());
            Create(new C2SCraftGetCraftProductInfoRes.Serializer());
            Create(new C2SCraftGetCraftProductReq.Serializer());
            Create(new C2SCraftGetCraftProductRes.Serializer());
            Create(new C2SCraftGetCraftProgressListReq.Serializer());
            Create(new C2SCraftGetCraftSettingReq.Serializer());
            Create(new C2SCraftRecipeGetCraftGradeupRecipeReq.Serializer());
            Create(new C2SCraftRecipeGetCraftRecipeReq.Serializer());
            Create(new C2SCraftResetCraftpointReq.Serializer());
            Create(new C2SCraftSkillAnalyzeReq.Serializer());
            Create(new C2SCraftSkillUpReq.Serializer());
            Create(new C2SCraftStartAttachElementReq.Serializer());
            Create(new C2SCraftStartCraftReq.Serializer());
            Create(new C2SCraftStartDetachElementReq.Serializer());
            Create(new C2SCraftStartEquipColorChangeReq.Serializer());
            Create(new C2SCraftStartEquipGradeUpReq.Serializer());
            Create(new C2SCraftStartQualityUpReq.Serializer());
            Create(new C2SCraftTimeSaveReq.Serializer());

            Create(new C2SDispelExchangeDispelItemReq.Serializer());
            Create(new C2SDispelGetDispelItemListReq.Serializer());
            Create(new C2SDispelGetDispelItemSettingsReq.Serializer());

            Create(new C2SEntryBoardEntryBoardItemCreateReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemEntryReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemExtendTimeoutReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemForceStartReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemInfoChangeReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemInfoMyselfReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemInviteReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemLeaveReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemListReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemReadyReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemRecreateReq.Serializer());
            Create(new C2SEntryBoardEntryBoardItemReq.Serializer());
            Create(new C2SEntryBoardEntryBoardListReq.Serializer());
            Create(new C2SEntryBoardItemKickReq.Serializer());
            Create(new C2SEntryBoardPartyRecruitCategoryListReq.Serializer());
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
            Create(new C2SEventEndNtc.Serializer());
            Create(new C2SEventStartNtc.Serializer());

            Create(new C2SFriendApplyFriendReq.Serializer());
            Create(new C2SFriendApproveFriendReq.Serializer());
            Create(new C2SFriendCancelFriendApplicationReq.Serializer());
            Create(new C2SFriendGetFriendListReq.Serializer());
            Create(new C2SFriendRegisterFavoriteFriendReq.Serializer());
            Create(new C2SFriendRemoveFriendReq.Serializer());

            Create(new C2SGpGetGpDetailReq.Serializer());
            Create(new C2SGpGetGpPeriodReq.Serializer());
            Create(new C2SGpGetGpReq.Serializer());
            Create(new C2SGpGetValidChatComGroupReq.Serializer());
            Create(new C2SGpGpEditGetVoiceListReq.Serializer());

            Create(new C2SInnGetPenaltyHealStayPriceReq.Serializer());
            Create(new C2SInnGetStayPriceReq.Serializer());
            Create(new C2SInnStayInnReq.Serializer());
            Create(new C2SInnStayPenaltyHealInnReq.Serializer());

            Create(new C2SInstanceCharacterEndBadStatusNtc.Serializer());
            Create(new C2SInstanceCharacterStartBadStatusNtc.Serializer());
            Create(new C2SInstanceEnemyBadStatusEndNtc.Serializer());
            Create(new C2SInstanceEnemyBadStatusStartNtc.Serializer());
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
            Create(new C2SInstancePlTouchOmNtc.Serializer());
            Create(new C2SInstanceSetOmInstantKeyValueReq.Serializer());
            Create(new C2SInstanceTraningRoomGetEnemyListReq.Serializer());
            Create(new C2SInstanceTraningRoomSetEnemyReq.Serializer());
            Create(new C2SInstanceTreasurePointGetCategoryListReq.Serializer());
            Create(new C2SInstanceTreasurePointGetListReq.Serializer());

            Create(new C2SItemChangeAttrDiscardReq.Serializer());
            Create(new C2SItemConsumeStorageItemReq.Serializer());
            Create(new C2SItemEmbodyItemsReq.Serializer());
            Create(new C2SItemGetDefaultStorageEmptySlotNumReq.Serializer());
            Create(new C2SItemGetEmbodyPayCostReq.Serializer());
            Create(new C2SItemGetPostItemListReq.Serializer());
            Create(new C2SItemGetSpecifiedHavingItemListReq.Serializer());
            Create(new C2SItemGetStorageItemListReq.Serializer());
            Create(new C2SItemGetValuableItemListReq.Serializer());
            Create(new C2SItemMoveItemReq.Serializer());
            Create(new C2SItemSellItemReq.Serializer());
            Create(new C2SItemSortGetItemSortDataBinReq.Serializer());
            Create(new C2SItemSortSetItemSortDataBinReq.Serializer());
            Create(new C2SItemUseBagItemReq.Serializer());
            Create(new C2SItemUseJobItemsReq.Serializer());

            Create(new C2SJobChangeJobReq.Serializer());
            Create(new C2SJobChangePawnJobReq.Serializer());
            Create(new C2SJobGetJobChangeListReq.Serializer());
            Create(new C2SJobGetPlayPointListReq.Serializer());
            Create(new C2SJobJobValueShopBuyItemReq.Serializer());
            Create(new C2SJobJobValueShopGetLineupReq.Serializer());
            Create(new C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq.Serializer());
            Create(new C2SJobUpdateExpModeReq.Serializer());

            Create(new C2SLoadingInfoLoadingGetInfoReq.Serializer());

            Create(new C2SLobbyChatMsgReq.Serializer());
            Create(new C2SLobbyJoinReq.Serializer());
            Create(new C2SLobbyLeaveReq.Serializer());
            Create(new C2SLobbyLobbyDataMsgReq.Serializer());

            Create(new C2SMailMailGetListDataReq.Serializer());
            Create(new C2SMailMailGetListFootReq.Serializer());
            Create(new C2SMailMailGetListHeadReq.Serializer());
            Create(new C2SMailSystemMailDeleteReq.Serializer());

            Create(new C2SMailSystemMailGetAllItemReq.Serializer());
            Create(new C2SMailSystemMailGetListDataReq.Serializer());
            Create(new C2SMailSystemMailGetListFootReq.Serializer());
            Create(new C2SMailSystemMailGetListHeadReq.Serializer());
            Create(new C2SMailSystemMailGetTextReq.Serializer());

            Create(new C2SMandragoraGetMyMandragoraReq.Serializer());

            Create(new C2SMyRoomFurnitureListGetReq.Serializer());
            Create(new C2SMyRoomMyRoomBgmUpdateReq.Serializer());
            Create(new C2SMyRoomUpdatePlanetariumReq.Serializer());

            Create(new C2SNpcGetNpcExtendedFacilityReq.Serializer());

            Create(new C2SOrbDevoteGetPawnReleaseOrbElementListReq.Serializer());
            Create(new C2SOrbDevoteReleaseOrbElementReq.Serializer());
            Create(new C2SOrbDevoteReleasePawnOrbElementReq.Serializer());

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
            Create(new C2SPartyPartyMemberSetValueReq.Serializer());
            Create(new C2SPartySendBinaryMsgAllNtc.Serializer());
            Create(new C2SPartySendBinaryMsgNtc.Serializer());

            Create(new C2SPawnCreatePawnReq.Serializer());
            Create(new C2SPawnDeleteMyPawnReq.Serializer());
            Create(new C2SPawnExpeditionGetSallyInfoReq.Serializer());
            Create(new C2SPawnGetLostPawnListReq.Serializer());
            Create(new C2SPawnGetMyPawnListReq.Serializer());
            Create(new C2SPawnGetMypawnDataReq.Serializer());
            Create(new C2SPawnGetNoraPawnListReq.Serializer());
            Create(new C2SPawnGetOfficialPawnListReq.Serializer());
            Create(new C2SPawnGetPartyPawnDataReq.Serializer());
            Create(new C2SPawnGetPawnHistoryListReq.Serializer());
            Create(new C2SPawnGetPawnTotalScoreReq.Serializer());
            Create(new C2SPawnGetRegisteredPawnDataReq.Serializer());
            Create(new C2SPawnGetRegisteredPawnListReq.Serializer());
            Create(new C2SPawnGetRentedPawnDataReq.Serializer());
            Create(new C2SPawnJoinPartyMypawnReq.Serializer());
            Create(new C2SPawnJoinPartyRentedPawnReq.Serializer());
            Create(new C2SPawnLostPawnGoldenReviveReq.Serializer());
            Create(new C2SPawnLostPawnPointReviveReq.Serializer());
            Create(new C2SPawnLostPawnReviveReq.Serializer());
            Create(new C2SPawnLostPawnWalletReviveReq.Serializer());
            Create(new C2SPawnPawnLostReq.Serializer());
            Create(new C2SPawnRentRegisteredPawnReq.Serializer());
            Create(new C2SPawnRentalPawnLostReq.Serializer());
            Create(new C2SPawnReturnRentedPawnReq.Serializer());
            Create(new C2SPawnSpSkillDeleteStockSkillReq.Serializer());
            Create(new C2SPawnSpSkillGetActiveSkillReq.Serializer());
            Create(new C2SPawnSpSkillGetStockSkillReq.Serializer());
            Create(new C2SPawnSpSkillSetActiveSkillReq.Serializer());
            Create(new C2SPawnTrainingGetPreparetionInfoToAdviceReq.Serializer());
            Create(new C2SPawnTrainingGetTrainingStatusReq.Serializer());
            Create(new C2SPawnTrainingSetTrainingStatusReq.Serializer());
            Create(new C2SPawnUpdatePawnReactionListReq.Serializer());
            Create(new C2SPhotoPhotoTakeNtc.Serializer());
            Create(new C2SProfileGetCharacterProfileReq.Serializer());
            Create(new C2SProfileGetMyCharacterProfileReq.Serializer());

            Create(new C2SQuestCancelPriorityQuestReq.Serializer());
            Create(new C2SQuestDecideDeliveryItemReq.Serializer());
            Create(new C2SQuestDeliverItemReq.Serializer());
            Create(new C2SQuestEndDistributionQuestCancelReq.Serializer());
            Create(new C2SQuestGetAdventureGuideQuestListReq.Serializer());
            Create(new C2SQuestGetAdventureGuideQuestNtcReq.Serializer());
            Create(new C2SQuestGetAreaBonusListReq.Serializer());
            //Create(new C2SQuestGetAreaInfoListReq.Serializer());
            //Create(new C2SQuestGetCycleContentsNewsListReq.Serializer());
            Create(new C2SQuestGetCycleContentsStateListReq.Serializer());
            Create(new C2SQuestGetEndContentsGroupReq.Serializer());
            Create(new C2SQuestGetEndContentsRecruitListReq.Serializer());
            Create(new C2SQuestGetLevelBonusListReq.Serializer());
            Create(new C2SQuestGetLightQuestListReq.Serializer());
            Create(new C2SQuestGetLotQuestListReq.Serializer());
            Create(new C2SQuestGetMainQuestListReq.Serializer());
            Create(new C2SQuestGetMobHuntQuestListReq.Serializer());
            Create(new C2SQuestGetPackageQuestListReq.Serializer());
            Create(new C2SQuestGetPartyBonusListReq.Serializer());
            //Create(new C2SQuestGetPartyQuestProgressInfoReq.Serializer());
            Create(new C2SQuestGetPriorityQuestReq.Serializer());
            Create(new C2SQuestGetQuestPartyBonusListReq.Serializer());
            Create(new C2SQuestGetQuestCompleteListReq.Serializer());
            Create(new C2SQuestGetQuestScheduleInfoReq.Serializer());
            Create(new C2SQuestGetRewardBoxItemReq.Serializer());
            Create(new C2SQuestGetRewardBoxListReq.Serializer());
            Create(new C2SQuestGetSetQuestInfoListReq.Serializer());
            Create(new C2SQuestGetSetQuestListReq.Serializer());
            Create(new C2SQuestGetTutorialQuestListReq.Serializer());
            Create(new C2SQuestLeaderQuestProgressRequestReq.Serializer());
            Create(new C2SQuestPlayEndReq.Serializer());
            Create(new C2SQuestPlayEntryCancelReq.Serializer());
            Create(new C2SQuestPlayEntryReq.Serializer());
            Create(new C2SQuestPlayInterruptAnswerReq.Serializer());
            Create(new C2SQuestPlayInterruptReq.Serializer());
            Create(new C2SQuestPlayStartTimerReq.Serializer());
            Create(new C2SQuestPlayerStartReq.Serializer());
            Create(new C2SQuestQuestCancelReq.Serializer());
            Create(new C2SQuestQuestCompleteFlagClearReq.Serializer());
            Create(new C2SQuestQuestLogInfoReq.Serializer());
            Create(new C2SQuestQuestOrderReq.Serializer());
            Create(new C2SQuestQuestProgressReq.Serializer());
            Create(new C2SQuestSendLeaderQuestOrderConditionInfoReq.Serializer());
            Create(new C2SQuestSendLeaderWaitOrderQuestListReq.Serializer());
            Create(new C2SQuestSetPriorityQuestReq.Serializer());

            Create(new C2SSeasonDungeonDeliverItemForExReq.Serializer());
            Create(new C2SSeasonDungeonExecuteSoulOrdealReq.Serializer());
            Create(new C2SSeasonDungeonGetBlockadeIdFromNpcIdReq.Serializer());
            Create(new C2SSeasonDungeonGetBlockadeIdFromOmReq.Serializer());
            Create(new C2SSeasonDungeonGetExRequiredItemReq.Serializer());
            Create(new C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq.Serializer());
            Create(new C2SSeasonDungeonGetIdFromNpcIdReq.Serializer());
            Create(new C2SSeasonDungeonGetInfoReq.Serializer());
            Create(new C2SSeasonDungeonGetSoulOrdealListfromOmReq.Serializer());
            Create(new C2SSeasonDungeonGetSoulOrdealRewardListForViewReq.Serializer());
            Create(new C2SSeasonDungeonGetSoulOrdealRewardListReq.Serializer());
            Create(new C2SSeasonDungeonGetStatueStateNtc.Serializer());
            Create(new C2SSeasonDungeonInterruptSoulOrdealReq.Serializer());
            Create(new C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq.Serializer());
            Create(new C2SSeasonDungeonReceiveSoulOrdealRewardReq.Serializer());
            Create(new C2SSeasonDungeonSoulOrdealCancelReadyReq.Serializer());
            Create(new C2SSeasonDungeonSoulOrdealReadyReq.Serializer());
            Create(new C2SSeasonDungeonUpdateKeyPointDoorStatusReq.Serializer());

            Create(new C2SServerGameTimeGetBaseInfoReq.Serializer());
            Create(new C2SServerGetGameSettingReq.Serializer());
            Create(new C2SServerGetRealTimeReq.Serializer());
            Create(new C2SServerGetScreenShotCategoryReq.Serializer());
            Create(new C2SServerGetServerListReq.Serializer());
            Create(new C2SServerWeatherForecastGetReq.Serializer());

            Create(new C2SSetCommunicationShortcutReq.Serializer());
            Create(new C2SSetShortcutReq.Serializer());

            Create(new C2SShopBuyShopGoodsReq.Serializer());
            Create(new C2SShopGetShopGoodsListReq.Serializer());

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
            Create(new C2SSkillRegisterPresetAbilityReq.Serializer());
            Create(new C2SSkillSetAbilityReq.Serializer());
            Create(new C2SSkillSetOffAbilityReq.Serializer());
            Create(new C2SSkillSetOffPawnAbilityReq.Serializer());
            Create(new C2SSkillSetOffPawnSkillReq.Serializer());
            Create(new C2SSkillSetOffSkillReq.Serializer());
            Create(new C2SSkillSetPawnAbilityReq.Serializer());
            Create(new C2SSkillSetPawnSkillReq.Serializer());
            Create(new C2SSkillSetPresetAbilityListReq.Serializer());
            Create(new C2SSkillSetPresetAbilityNameReq.Serializer());
            Create(new C2SSkillSetSkillReq.Serializer());

            Create(new C2SStageAreaChangeReq.Serializer());
            Create(new C2SStageGetSpAreaChangeIdFromNpcIdReq.Serializer());
            Create(new C2SStageGetSpAreaChangeInfoReq.Serializer());
            Create(new C2SStageGetStageListReq.Serializer());
            Create(new C2SStageGetTicketDungeonCategoryListReq.Serializer());
            Create(new C2SStageGetTicketDungeonInfoListReq.Serializer());
            Create(new C2SStageUnisonAreaChangeBeginRecruitmentReq.Serializer());
            Create(new C2SStageUnisonAreaChangeGetRecruitmentStateReq.Serializer());
            Create(new C2SStageUnisonAreaChangeReadyCancelReq.Serializer());
            Create(new C2SStageUnisonAreaChangeReadyReq.Serializer());
            Create(new C2SStampBonusCheckReq.Serializer());
            Create(new C2SStampBonusGetListReq.Serializer());
            Create(new C2SStampBonusRecieveDailyReq.Serializer());
            Create(new C2SStampBonusRecieveTotalReq.Serializer());

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

            Create(new C2S_SEASON_62_40_16_NTC.Serializer());
            Create(new C2S_SEASON_DUNGEON_62_12_16_NTC.Serializer());

            Create(new S2CAchievementGetCategoryProgressListRes.Serializer());
            Create(new S2CAchievementGetFurnitureRewardListRes.Serializer());
            Create(new S2CAchievementGetProgressListRes.Serializer());
            Create(new S2CAchievementGetReceivableRewardListRes.Serializer());
            Create(new S2CAchievementGetRewardListRes.Serializer());
            Create(new S2CAchievementRewardReceiveRes.Serializer());
            Create(new S2CActionSetPlayerActionHistoryRes.Serializer());
            //Create(new S2CAreaAreaRankUpRes.Serializer());
            //Create(new S2CAreaBuyAreaQuestHintRes.Serializer());
            Create(new S2CAreaGetAreaBaseInfoListRes.Serializer());
            //Create(new S2CAreaGetAreaMasterInfoRes.Serializer());
            //Create(new S2CAreaGetAreaQuestHintListRes.Serializer());
            //Create(new S2CAreaGetAreaReleaseListRes.Serializer());
            //Create(new S2CAreaGetAreaRewardInfoRes.Serializer());
            //Create(new S2CAreaGetAreaSupplyInfoRes.Serializer());
            //Create(new S2CAreaGetAreaSupplyRes.Serializer());
            Create(new S2CAreaGetLeaderAreaReleaseListRes.Serializer());
            //Create(new S2CAreaGetSpotInfoListRes.Serializer());
            //Create(new S2CAreaPointUpNtc.Serializer());
            //Create(new S2CAreaRankUpReadyNtc.Serializer());

            Create(new S2CBattleContentAreaChangeNtc.Serializer());
            Create(new S2CBattleContentCharacterInfoRes.Serializer());
            Create(new S2CBattleContentClearContentNtc.Serializer());
            Create(new S2CBattleContentClearTierNtc.Serializer());
            Create(new S2CBattleContentContentEntryNtc.Serializer());
            Create(new S2CBattleContentContentEntryRes.Serializer());
            Create(new S2CBattleContentContentFirstPhaseChangeRes.Serializer());
            Create(new S2CBattleContentContentResetNtc.Serializer());
            Create(new S2CBattleContentContentResetRes.Serializer());
            Create(new S2CBattleContentGetContentStatusFromOmRes.Serializer());
            Create(new S2CBattleContentGetRewardRes.Serializer());
            Create(new S2CBattleContentGetRewardsNtc.Serializer());
            Create(new S2CBattleContentInfoListRes.Serializer());
            Create(new S2CBattleContentInstantClearInfoRes.Serializer());
            Create(new S2CBattleContentPartyMemberInfoRes.Serializer());
            Create(new S2CBattleContentPartyMemberInfoUpdateNtc.Serializer());
            Create(new S2CBattleContentPartyMemberInfoUpdateRes.Serializer());
            Create(new S2CBattleContentProgressNtc.Serializer());
            Create(new S2CBattleContentResetInfoRes.Serializer());
            Create(new S2CBattleContentRewardListRes.Serializer());
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
            Create(new S2CBazaarReExhibitRes.Serializer());
            Create(new S2CBazaarReceiveProceedsRes.Serializer());

            Create(new S2CBinarySaveSetCharacterBinSaveDataRes.Serializer());

            Create(new S2CCertClientChallengeRes.Serializer());

            Create(new S2CCharacterCharacterGoldenReviveRes.Serializer());
            Create(new S2CCharacterCharacterPenaltyReviveRes.Serializer());
            Create(new S2CCharacterCharacterPointReviveRes.Serializer());
            Create(new S2CCharacterCharacterSearchRes.Serializer());
            Create(new S2CCharacterChargeRevivePointRes.Serializer());
            Create(new S2CCharacterCommunityCharacterStatusGetRes.Serializer());
            Create(new S2CCharacterCommunityCharacterStatusUpdateNtc.Serializer());
            Create(new S2CCharacterContentsReleaseElementNtc.Serializer());
            Create(new S2CCharacterCreateModeCharacterEditParamRes.Serializer());
            Create(new S2CCharacterDecideCharacterIdRes.Serializer());
            Create(new S2CCharacterEditGetShopPriceRes.Serializer());
            Create(new S2CCharacterEditUpdateCharacterEditParamExRes.Serializer());
            Create(new S2CCharacterEditUpdateCharacterEditParamRes.Serializer());
            Create(new S2CCharacterEditUpdateEditParamExNtc.Serializer());
            Create(new S2CCharacterEditUpdateEditParamNtc.Serializer());
            Create(new S2CCharacterEditUpdatePawnEditParamExRes.Serializer());
            Create(new S2CCharacterEditUpdatePawnEditParamRes.Serializer());
            Create(new S2CCharacterFinishDeathPenaltyNtc.Serializer());
            Create(new S2CCharacterFinishLanternNtc.Serializer());
            Create(new S2CCharacterFinishLanternOtherNtc.Serializer());
            Create(new S2CCharacterGetCharacterStatusNtc.Serializer());
            Create(new S2CCharacterGetReviveChargeableTimeRes.Serializer());
            Create(new S2CCharacterPawnGoldenReviveRes.Serializer());
            Create(new S2CCharacterPawnPointReviveRes.Serializer());
            Create(new S2CCharacterSetOnlineStatusRes.Serializer());
            Create(new S2CCharacterStartDeathPenaltyNtc.Serializer());
            Create(new S2CCharacterStartLanternNtc.Serializer());
            Create(new S2CCharacterStartLanternOtherNtc.Serializer());
            Create(new S2CCharacterSwitchGameModeNtc.Serializer());
            Create(new S2CCharacterSwitchGameModeRes.Serializer());
            Create(new S2CCharacterUpdateRevivePointNtc.Serializer());

            Create(new S2CChatSendTellMsgRes.Serializer());

            Create(new S2CClanClanBaseGetInfoRes.Serializer());
            Create(new S2CClanClanBaseReleaseRes.Serializer());
            Create(new S2CClanClanBaseReleaseStateUpdateNtc.Serializer());
            Create(new S2CClanClanConciergeGetListRes.Serializer());
            Create(new S2CClanClanConciergeUpdateRes.Serializer());
            Create(new S2CClanClanCreateRes.Serializer());
            Create(new S2CClanClanExpelMemberRes.Serializer());
            Create(new S2CClanClanGetHistoryRes.Serializer());
            Create(new S2CClanClanGetInfoRes.Serializer());
            Create(new S2CClanClanGetJoinRequestedListRes.Serializer());
            Create(new S2CClanClanGetMemberListRes.Serializer());
            Create(new S2CClanClanGetMyInfoRes.Serializer());
            Create(new S2CClanClanGetMyJoinRequestListRes.Serializer());
            Create(new S2CClanClanGetMyMemberListRes.Serializer());
            Create(new S2CClanClanInviteAcceptRes.Serializer());
            Create(new S2CClanClanInviteNtc.Serializer());
            Create(new S2CClanClanInviteRes.Serializer());
            Create(new S2CClanClanJoinDisapproveNtc.Serializer());
            Create(new S2CClanClanJoinMemberNtc.Serializer());
            Create(new S2CClanClanJoinNtc.Serializer());
            Create(new S2CClanClanJoinSelfNtc.Serializer());
            Create(new S2CClanClanLeaveMemberNtc.Serializer());
            Create(new S2CClanClanLeaveMemberRes.Serializer());
            Create(new S2CClanClanLevelUpNtc.Serializer());
            Create(new S2CClanClanNegotiateMasterNtc.Serializer());
            Create(new S2CClanClanNegotiateMasterRes.Serializer());
            Create(new S2CClanClanPartnerPawnDataGetRes.Serializer());
            Create(new S2CClanClanPointAddNtc.Serializer());
            Create(new S2CClanClanQuestClearNtc.Serializer());
            Create(new S2CClanClanScoutEntryDisapproveInviteNtc.Serializer());
            Create(new S2CClanClanScoutEntryGetInviteListRes.Serializer());
            Create(new S2CClanClanScoutEntryGetInvitedListRes.Serializer());
            Create(new S2CClanClanScoutEntryGetMyRes.Serializer());
            Create(new S2CClanClanScoutEntrySearchRes.Serializer());
            Create(new S2CClanClanSearchRes.Serializer());
            Create(new S2CClanClanSetMemberRankNtc.Serializer());
            Create(new S2CClanClanSetMemberRankRes.Serializer());
            Create(new S2CClanClanSettingUpdateRes.Serializer());
            Create(new S2CClanClanShopBuyBuffItemRes.Serializer());
            Create(new S2CClanClanShopBuyFunctionItemRes.Serializer());
            Create(new S2CClanClanShopBuyItemNtc.Serializer());
            Create(new S2CClanClanShopGetBuffItemListRes.Serializer());
            Create(new S2CClanClanShopGetFunctionItemListRes.Serializer());
            Create(new S2CClanClanUpdateCommonNtc.Serializer());
            Create(new S2CClanClanUpdateNtc.Serializer());
            Create(new S2CClanClanUpdateRes.Serializer());
            Create(new S2CClanGetFurnitureRes.Serializer());
            Create(new S2CClanSetFurnitureRes.Serializer());

            Create(new S2CConnection_10_Ntc.Serializer());
            Create(new S2CConnectionCriticalErrorNtc.Serializer());
            Create(new S2CConnectionErrorNtc.Serializer());
            Create(new S2CConnectionGetLoginAnnouncementRes.Serializer());
            Create(new S2CConnectionInformationNtc.Serializer());
            Create(new S2CConnectionKickNtc.Serializer());
            Create(new S2CConnectionLoginRes.Serializer());
            Create(new S2CConnectionLogoutRes.Serializer());
            Create(new S2CConnectionMoveInServerRes.Serializer());
            Create(new S2CConnectionMoveOutServerRes.Serializer());
            Create(new S2CConnectionPingRes.Serializer());
            Create(new S2CConnectionReserveServerRes.Serializer());

            Create(new S2CContextGetAllPlayerContextNtc.Serializer());
            Create(new S2CContextGetLobbyPlayerContextNtc.Serializer());
            Create(new S2CContextGetPartyMypawnContextNtc.Serializer());
            Create(new S2CContextGetPartyPlayerContextNtc.Serializer());
            Create(new S2CContextGetPartyRentedPawnContextNtc.Serializer());
            Create(new S2CContextGetSetContextRes.Serializer());
            Create(new S2CContextMasterChangeNtc.Serializer());
            Create(new S2CContextMasterInfoNtc.Serializer());
            Create(new S2CContextMasterThrowNtc.Serializer());
            Create(new S2CContextMasterThrowRes.Serializer());
            Create(new S2CContextSetContextBaseNtc.Serializer());
            Create(new S2CContextSetContextNtc.Serializer());

            Create(new S2CCraftCraftExpUpNtc.Serializer());
            Create(new S2CCraftCraftRankUpNtc.Serializer());
            Create(new S2CCraftFinishCraftNtc.Serializer());
            Create(new S2CCraftGetCraftIRCollectionValueListRes.Serializer());
            Create(new S2CCraftGetCraftProgressListRes.Serializer());
            Create(new S2CCraftGetCraftSettingRes.Serializer());
            Create(new S2CCraftRecipeGetCraftGradeupRecipeRes.Serializer());
            Create(new S2CCraftRecipeGetCraftRecipeRes.Serializer());
            Create(new S2CCraftResetCraftpointRes.Serializer());
            Create(new S2CCraftSkillAnalyzeRes.Serializer());
            Create(new S2CCraftSkillUpRes.Serializer());
            Create(new S2CCraftStartAttachElementRes.Serializer());
            Create(new S2CCraftStartCraftRes.Serializer());
            Create(new S2CCraftStartDetachElementRes.Serializer());
            Create(new S2CCraftStartEquipColorChangeRes.Serializer());
            Create(new S2CCraftStartEquipGradeUpRes.Serializer());
            Create(new S2CCraftStartQualityUpRes.Serializer());
            Create(new S2CCraftTimeSaveRes.Serializer());
            Create(new S2CCraft_30_21_16_NTC.Serializer());

            Create(new S2CDispelExchangeDispelItemRes.Serializer());
            Create(new S2CDispelGetDispelItemListRes.Serializer());
            Create(new S2CDispelGetDispelItemSettingsRes.Serializer());

            Create(new S2CEntryBoardEntryBoardItemChangeMemberNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardItemCreateRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemEntryRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemExtendTimeoutRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemForceStartRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemInfoChangeNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardItemInfoChangeRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemInfoMyselfRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemInviteNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardItemInviteRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemLeaveNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardItemLeaveRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemListRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemReadyNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardItemReadyRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemRecreateNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardItemRecreateRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemRes.Serializer());
            Create(new S2CEntryBoardEntryBoardItemReserveNtc.Serializer());
            Create(new S2CEntryBoardEntryBoardListRes.Serializer());
            Create(new S2CEntryBoardItemKickRes.Serializer());
            Create(new S2CEntryBoardItemPartyNtc.Serializer());
            Create(new S2CEntryBoardItemTimeoutTimerNtc.Serializer());
            Create(new S2CEntryBoardItemUnreadyNtc.Serializer());
            Create(new S2CEntryBoardPartyRecruitCategoryListRes.Serializer());

            Create(new S2CEquipChangeCharacterEquipJobItemNtc.Serializer());
            Create(new S2CEquipChangeCharacterEquipJobItemRes.Serializer());
            Create(new S2CEquipChangeCharacterEquipLobbyNtc.Serializer());
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

            Create(new S2CFriendApplyFriendNtc.Serializer());
            Create(new S2CFriendApplyFriendRes.Serializer());
            Create(new S2CFriendApproveFriendNtc.Serializer());
            Create(new S2CFriendApproveFriendRes.Serializer());
            Create(new S2CFriendCancelFriendApplicationNtc.Serializer());
            Create(new S2CFriendCancelFriendApplicationRes.Serializer());
            Create(new S2CFriendGetFriendListRes.Serializer());
            Create(new S2CFriendRegisterFavoriteFriendRes.Serializer());
            Create(new S2CFriendRemoveFriendNtc.Serializer());
            Create(new S2CFriendRemoveFriendRes.Serializer());

            Create(new S2CGPCourseExtendNtc.Serializer());
            Create(new S2CGPCourseStartNtc.Serializer());
            Create(new S2CGpCourseEndNtc.Serializer());
            Create(new S2CGpGetGpDetailRes.Serializer());
            Create(new S2CGpGetGpPeriodRes.Serializer());
            Create(new S2CGpGetGpRes.Serializer());
            Create(new S2CGpGetValidChatComGroupRes.Serializer());
            Create(new S2CGpGpCourseGetAvailableListRes.Serializer());
            Create(new S2CGpGpEditGetVoiceListRes.Serializer());

            Create(new S2CInnGetPenaltyHealStayPriceRes.Serializer());
            Create(new S2CInnGetStayPriceRes.Serializer());
            Create(new S2CInnStayInnRes.Serializer());
            Create(new S2CInnStayPenaltyHealInnRes.Serializer());

            Create(new S2CInstanceAreaResetNtc.Serializer());
            Create(new S2CInstanceEnemyDieNtc.Serializer());
            Create(new S2CInstanceEnemyGroupDestroyNtc.Serializer());
            Create(new S2CInstanceEnemyGroupResetNtc.Serializer());
            Create(new S2CInstanceEnemyKillRes.Serializer());
            Create(new S2CInstanceEnemyRepopNtc.Serializer());
            Create(new S2CInstanceEnemySubGroupAppearNtc.Serializer());
            Create(new S2CInstanceExchangeOmInstantKeyValueNtc.Serializer());
            Create(new S2CInstanceExchangeOmInstantKeyValueRes.Serializer());
            Create(new S2CInstanceGatheringEnemyAppearNtc.Serializer());
            Create(new S2CInstanceGetDropItemListRes.Serializer());
            Create(new S2CInstanceGetDropItemRes.Serializer());
            Create(new S2CInstanceGetEnemySetListRes.Serializer());
            Create(new S2CInstanceGetGatheringItemListRes.Serializer());
            Create(new S2CInstanceGetGatheringItemRes.Serializer());
            Create(new S2CInstanceGetItemSetListRes.Serializer());
            Create(new S2CInstanceGetOmInstantKeyValueAllRes.Serializer());
            Create(new S2CInstancePopDropItemNtc.Serializer());
            Create(new S2CInstanceSetOmInstantKeyValueNtc.Serializer());
            Create(new S2CInstanceSetOmInstantKeyValueRes.Serializer());
            Create(new S2CInstanceTraningRoomGetEnemyListRes.Serializer());
            Create(new S2CInstanceTraningRoomSetEnemyRes.Serializer());
            Create(new S2CInstanceTreasurePointGetCategoryListRes.Serializer());
            Create(new S2CInstanceTreasurePointGetListRes.Serializer());

            Create(new S2CItemAchievementRewardReceiveNtc.Serializer());
            Create(new S2CItemChangeAttrDiscardRes.Serializer());
            Create(new S2CItemConsumeStorageItemRes.Serializer());
            Create(new S2CItemEmbodyItemsRes.Serializer());
            Create(new S2CItemExtendItemSlotNtc.Serializer());
            Create(new S2CItemGetDefaultStorageEmptySlotNumRes.Serializer());
            Create(new S2CItemGetEmbodyPayCostRes.Serializer());
            Create(new S2CItemGetPostItemListRes.Serializer());
            Create(new S2CItemGetSpecifiedHavingItemListRes.Serializer());
            Create(new S2CItemGetStorageItemListRes.Serializer());
            Create(new S2CItemGetValuableItemListRes.Serializer());
            Create(new S2CItemMoveItemRes.Serializer());
            Create(new S2CItemSellItemRes.Serializer());
            Create(new S2CItemSortGetItemSortdataBinNtc.Serializer());
            Create(new S2CItemSortGetItemSortdataBinRes.Serializer());
            Create(new S2CItemSortSetItemSortDataBinRes.Serializer());
            Create(new S2CItemSwitchStorageNtc.Serializer());
            Create(new S2CItemUpdateCharacterItemNtc.Serializer());
            Create(new S2CItemUseBagItemRes.Serializer());
            Create(new S2CItemUseJobItemsRes.Serializer());

            Create(new S2CJobChangeJobNtc.Serializer());
            Create(new S2CJobChangeJobRes.Serializer());
            Create(new S2CJobChangePawnJobNtc.Serializer());
            Create(new S2CJobChangePawnJobRes.Serializer());
            Create(new S2CJobCharacterJobExpUpNtc.Serializer());
            Create(new S2CJobCharacterJobLevelUpMemberNtc.Serializer());
            Create(new S2CJobCharacterJobLevelUpNtc.Serializer());
            Create(new S2CJobCharacterJobLevelUpOtherNtc.Serializer());
            Create(new S2CJobGetJobChangeListRes.Serializer());
            Create(new S2CJobGetPlayPointListRes.Serializer());
            Create(new S2CJobJobValueShopBuyItemRes.Serializer());
            Create(new S2CJobJobValueShopGetLineupRes.Serializer());
            Create(new S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes.Serializer());
            Create(new S2CJobOrbTreeGetJobOrbTreeStatusListRes.Serializer());
            Create(new S2CJobPawnJobExpUpNtc.Serializer());
            Create(new S2CJobPawnJobLevelUpMemberNtc.Serializer());
            Create(new S2CJobPawnJobLevelUpNtc.Serializer());
            Create(new S2CJobPawnJobPointNtc.Serializer());
            Create(new S2CJobUpdateExpModeRes.Serializer());
            Create(new S2CJobUpdatePlayPointNtc.Serializer());
            Create(new S2CJob_33_3_16_Ntc.Serializer());

            Create(new S2CLoadingInfoLoadingGetInfoRes.Serializer());

            Create(new S2CLobbyChatMsgNotice.Serializer());
            Create(new S2CLobbyChatMsgRes.Serializer());
            Create(new S2CLobbyJoinRes.Serializer());
            Create(new S2CLobbyLeaveRes.Serializer());
            Create(new S2CLobbyLobbyDataMsgNotice.Serializer());

            Create(new S2CMailMailGetListDataRes.Serializer());
            Create(new S2CMailMailGetListFootRes.Serializer());
            Create(new S2CMailMailGetListHeadRes.Serializer());

            Create(new S2CMailSystemGetAllItemRes.Serializer());
            Create(new S2CMailSystemMailDeleteRes.Serializer());
            Create(new S2CMailSystemMailGetListDataRes.Serializer());
            Create(new S2CMailSystemMailGetListFootRes.Serializer());
            Create(new S2CMailSystemMailGetListHeadRes.Serializer());
            Create(new S2CMailSystemMailGetTextRes.Serializer());
            Create(new S2CMailSystemMailSendNtc.Serializer());

            Create(new S2CMandragoraGetMyMandragoraRes.Serializer());

            Create(new S2CMyRoomFurnitureListGetRes.Serializer());
            Create(new S2CMyRoomMyRoomBgmUpdateRes.Serializer());
            Create(new S2CMyRoomUpdatePlanetariumRes.Serializer());

            Create(new S2CNpcGetNpcExtendedFacilityRes.Serializer());

            Create(new S2COrb25_6_16_Ntc.Serializer());
            Create(new S2COrbDevoteGetOrbGainExtendParamRes.Serializer());
            Create(new S2COrbDevoteGetPawnReleaseOrbElementListRes.Serializer());
            Create(new S2COrbDevoteGetReleaseOrbElementListRes.Serializer());
            Create(new S2COrbDevoteReleaseHandlerRes.Serializer());
            Create(new S2COrbDevoteReleasePawnOrbELementRes.Serializer());

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
            Create(new S2CPartyPartyMemberSetValueNtc.Serializer());
            Create(new S2CPartyPartyMemberSetValueRes.Serializer());
            Create(new S2CPartyPartyQuestProgressNtc.Serializer());
            Create(new S2CPartyRecvBinaryMsgAllNtc.Serializer());
            Create(new S2CPartyRecvBinaryMsgNtc.Serializer());

            Create(new S2CPawnCreatePawnRes.Serializer());
            Create(new S2CPawnDeleteMyPawnRes.Serializer());
            Create(new S2CPawnExpeditionGetSallyInfoRes.Serializer());
            Create(new S2CPawnExtendMainPawnSlotNtc.Serializer());
            Create(new S2CPawnExtendSupportPawnSlotNtc.Serializer());
            Create(new S2CPawnGetLostPawnListRes.Serializer());
            Create(new S2CPawnGetMypawnDataRes.Serializer());
            Create(new S2CPawnGetMypawnListRes.Serializer());
            Create(new S2CPawnGetNoraPawnListRes.Serializer());
            Create(new S2CPawnGetOfficialPawnListRes.Serializer());
            Create(new S2CPawnGetPartyPawnDataRes.Serializer());
            Create(new S2CPawnGetPawnHistoryInfoNtc.Serializer());
            Create(new S2CPawnGetPawnHistoryListRes.Serializer());
            Create(new S2CPawnGetPawnOrbDevoteInfoNtc.Serializer());
            Create(new S2CPawnGetPawnProfileNtc.Serializer());
            Create(new S2CPawnGetPawnTotalScoreInfoNtc.Serializer());
            Create(new S2CPawnGetPawnTotalScoreRes.Serializer());
            Create(new S2CPawnGetRegisteredPawnDataRes.Serializer());
            Create(new S2CPawnGetRegisteredPawnListRes.Serializer());
            Create(new S2CPawnGetRentedPawnDataRes.Serializer());
            Create(new S2CPawnGetRentedPawnListRes.Serializer());
            Create(new S2CPawnJoinPartyMypawnRes.Serializer());
            Create(new S2CPawnJoinPartyPawnNtc.Serializer());
            Create(new S2CPawnJoinPartyRentedPawnRes.Serializer());
            Create(new S2CPawnLostPawnGoldenReviveRes.Serializer());
            Create(new S2CPawnLostPawnPointReviveRes.Serializer());
            Create(new S2CPawnLostPawnReviveRes.Serializer());
            Create(new S2CPawnLostPawnWalletReviveRes.Serializer());
            Create(new S2CPawnPawnLostNtc.Serializer());
            Create(new S2CPawnPawnLostRes.Serializer());
            Create(new S2CPawnRentRegisteredPawnRes.Serializer());
            Create(new S2CPawnRentalPawnLostNtc.Serializer());
            Create(new S2CPawnRentalPawnLostRes.Serializer());
            Create(new S2CPawnReturnRentedPawnRes.Serializer());
            Create(new S2CPawnSpSkillDeleteStockSkillRes.Serializer());
            Create(new S2CPawnSpSkillGetActiveSkillRes.Serializer());
            Create(new S2CPawnSpSkillGetStockSkillRes.Serializer());
            Create(new S2CPawnSpSkillSetActiveSkillRes.Serializer());
            Create(new S2CPawnTrainingGetPreparetionInfoToAdviceRes.Serializer());
            Create(new S2CPawnTrainingGetTrainingStatusRes.Serializer());
            Create(new S2CPawnTrainingSetTrainingStatusRes.Serializer());
            Create(new S2CPawnUpdatePawnReactionListNtc.Serializer());
            Create(new S2CPawnUpdatePawnReactionListRes.Serializer());

            Create(new S2CProfileGetCharacterProfileRes.Serializer());
            Create(new S2CProfileGetMyCharacterProfileRes.Serializer());

            Create(new S2CQuestGetAdventureGuideQuestListRes.Serializer());
            Create(new S2CQuestCancelPriorityQuestRes.Serializer());
            Create(new S2CQuestCompleteNtc.Serializer());
            Create(new S2CQuestDecideDeliveryItemNtc.Serializer());
            Create(new S2CQuestDecideDeliveryItemRes.Serializer());
            Create(new S2CQuestDeliverItemNtc.Serializer());
            Create(new S2CQuestDeliverItemRes.Serializer());
            Create(new S2CQuestEndDistributionQuestCancelRes.Serializer());
            Create(new S2CQuestGetAdventureGuideQuestNtcRes.Serializer());
            Create(new S2CQuestGetAreaBonusListRes.Serializer());
            //Create(new S2CQuestGetAreaInfoListRes.Serializer());
            //Create(new S2CQuestGetCycleContentsNewsListRes.Serializer());
            Create(new S2CQuestGetCycleContentsStateListRes.Serializer());
            Create(new S2CQuestGetEndContentsGroupRes.Serializer());
            Create(new S2CQuestGetEndContentsRecruitListRes.Serializer());
            Create(new S2CQuestGetLevelBonusListRes.Serializer());
            Create(new S2CQuestGetLightQuestListRes.Serializer());
            Create(new S2CQuestGetLotQuestListRes.Serializer());
            Create(new S2CQuestGetMainQuestListRes.Serializer());
            Create(new S2CQuestGetMainQuestNtc.Serializer());
            Create(new S2CQuestGetMobHuntQuestListRes.Serializer());
            Create(new S2CQuestGetPackageQuestListRes.Serializer());
            Create(new S2CQuestGetPartyBonusListRes.Serializer());
            Create(new S2CQuestGetPartyQuestProgressInfoRes.Serializer());
            Create(new S2CQuestGetPriorityQuestRes.Serializer());
            Create(new S2CQuestGetQuestPartyBonusListRes.Serializer());
            Create(new S2CQuestGetQuestCompleteListRes.Serializer());
            Create(new S2CQuestGetQuestScheduleInfoRes.Serializer());
            Create(new S2CQuestGetRewardBoxItemRes.Serializer());
            Create(new S2CQuestGetRewardBoxListRes.Serializer());
            Create(new S2CQuestGetSetQuestInfoListRes.Serializer());
            Create(new S2CQuestGetSetQuestListNtc.Serializer());
            Create(new S2CQuestGetSetQuestListRes.Serializer());
            Create(new S2CQuestGetTutorialQuestListRes.Serializer());
            Create(new S2CQuestGetWorldManageQuestListNtc.Serializer());
            Create(new S2CQuestGetWorldManageQuestListRes.Serializer());
            Create(new S2CQuestJoinLobbyQuestInfoNtc.Serializer());
            Create(new S2CQuestLeaderQuestProgressRequestNtc.Serializer());
            Create(new S2CQuestLeaderQuestProgressRequestRes.Serializer());
            Create(new S2CQuestPartyQuestProgressNtc.Serializer());
            Create(new S2CQuestPlayAddTimerNtc.Serializer());
            Create(new S2CQuestPlayEndNtc.Serializer());
            Create(new S2CQuestPlayEndRes.Serializer());
            Create(new S2CQuestPlayEntryCancelNtc.Serializer());
            Create(new S2CQuestPlayEntryCancelRes.Serializer());
            Create(new S2CQuestPlayEntryNtc.Serializer());
            Create(new S2CQuestPlayEntryRes.Serializer());
            Create(new S2CQuestPlayInterruptAnswerNtc.Serializer());
            Create(new S2CQuestPlayInterruptAnswerRes.Serializer());
            Create(new S2CQuestPlayInterruptNtc.Serializer());
            Create(new S2CQuestPlayInterruptRes.Serializer());
            Create(new S2CQuestPlayStartTimerNtc.Serializer());
            Create(new S2CQuestPlayStartTimerRes.Serializer());
            Create(new S2CQuestPlayTimeupNtc.Serializer());
            Create(new S2CQuestPlayerStartRes.Serializer());
            Create(new S2CQuestQuestCancelNtc.Serializer());
            Create(new S2CQuestQuestCancelRes.Serializer());
            Create(new S2CQuestQuestCompleteFlagClearRes.Serializer());
            Create(new S2CQuestQuestEnableNtc.Serializer());
            Create(new S2CQuestQuestLogInfoRes.Serializer());
            Create(new S2CQuestQuestOrderNtc.Serializer());
            Create(new S2CQuestQuestOrderRes.Serializer());
            Create(new S2CQuestQuestProgressNtc.Serializer());
            Create(new S2CQuestQuestProgressRes.Serializer());
            Create(new S2CQuestQuestProgressWorkSaveNtc.Serializer());
            Create(new S2CQuestRaidBossPlayStartNtc.Serializer());
            Create(new S2CQuestSendLeaderQuestOrderConditionInfoNtc.Serializer());
            Create(new S2CQuestSendLeaderQuestOrderConditionInfoRes.Serializer());
            Create(new S2CQuestSendLeaderWaitOrderQuestListNtc.Serializer());
            Create(new S2CQuestSendLeaderWaitOrderQuestListRes.Serializer());
            Create(new S2CQuestSetPriorityQuestNtc.Serializer());
            Create(new S2CQuestSetPriorityQuestRes.Serializer());
            Create(new S2CQuestTimeGainQuestPlayStartNtc.Serializer());

            Create(new S2CSeasonDungeonAreaBuffEffectNtc.Serializer());
            Create(new S2CSeasonDungeonDeliverItemForExRes.Serializer());
            Create(new S2CSeasonDungeonEndSoulOrdealNtc.Serializer());
            Create(new S2CSeasonDungeonExecuteSoulOrdealNtc.Serializer());
            Create(new S2CSeasonDungeonExecuteSoulOrdealRes.Serializer());
            Create(new S2CSeasonDungeonGetBlockadeIdFromNpcIdRes.Serializer());
            Create(new S2CSeasonDungeonGetBlockadeIdFromOmRes.Serializer());
            Create(new S2CSeasonDungeonGetExRequiredItemRes.Serializer());
            Create(new S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes.Serializer());
            Create(new S2CSeasonDungeonGetIdFromNpcIdRes.Serializer());
            Create(new S2CSeasonDungeonGetInfoRes.Serializer());
            Create(new S2CSeasonDungeonGetSoulOrdealListfromOmRes.Serializer());
            Create(new S2CSeasonDungeonGetSoulOrdealRewardListForViewRes.Serializer());
            Create(new S2CSeasonDungeonGetSoulOrdealRewardListRes.Serializer());
            Create(new S2CSeasonDungeonGroupReadyNtc.Serializer());
            Create(new S2CSeasonDungeonInterruptSoulOrdealRes.Serializer());
            Create(new S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes.Serializer());
            Create(new S2CSeasonDungeonReceiveSoulOrdealRewardRes.Serializer());
            Create(new S2CSeasonDungeonSetOmStateNtc.Serializer());
            Create(new S2CSeasonDungeonSoulOrdealCancelReadyRes.Serializer());
            Create(new S2CSeasonDungeonSoulOrdealReadyRes.Serializer());
            Create(new S2CSeasonDungeonUpdateKeyPointDoorStatusRes.Serializer());
            Create(new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc.Serializer());

            Create(new S2CServerGameTimeGetBaseInfoRes.Serializer());
            Create(new S2CServerGetGameSettingRes.Serializer());
            Create(new S2CServerGetRealTimeRes.Serializer());
            Create(new S2CServerGetScreenShotCategoryRes.Serializer());
            Create(new S2CServerGetServerListRes.Serializer());
            Create(new S2CServerTimeUpdateNtc.Serializer());
            Create(new S2CServerWeatherForecastGetRes.Serializer());

            Create(new S2CSetCommunicationShortcutRes.Serializer());
            Create(new S2CSetShortcutRes.Serializer());

            Create(new S2CShopBuyShopGoodsRes.Serializer());
            Create(new S2CShopGetShopGoodsListRes.Serializer());

            Create(new S2CSituationDataEndNtc.Serializer());
            Create(new S2CSituationDataUpdateObjectivesNtc.Serializer());

            Create(new S2CSkillAbilitySetNtc.Serializer());
            Create(new S2CSkillChangeExSkillRes.Serializer());
            Create(new S2CSkillCustomSkillSetNtc.Serializer());
            Create(new S2CSkillGetAbilityCostRes.Serializer());
            Create(new S2CSkillGetAcquirableAbilityListRes.Serializer());
            Create(new S2CSkillGetAcquirableSkillListRes.Serializer());
            Create(new S2CSkillGetCharacterSkillInfoNtc.Serializer());
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
            Create(new S2CSkillRegisterPresetAbilityRes.Serializer());
            Create(new S2CSkillSetAbilityRes.Serializer());
            Create(new S2CSkillSetOffAbilityRes.Serializer());
            Create(new S2CSkillSetOffPawnAbilityRes.Serializer());
            Create(new S2CSkillSetOffPawnSkillRes.Serializer());
            Create(new S2CSkillSetOffSkillRes.Serializer());
            Create(new S2CSkillSetPawnAbilityRes.Serializer());
            Create(new S2CSkillSetPawnSkillRes.Serializer());
            Create(new S2CSkillSetPresetAbilityListRes.Serializer());
            Create(new S2CSkillSetPresetAbilityNameRes.Serializer());
            Create(new S2CSkillSetPresetAbilityNtc.Serializer());
            Create(new S2CSkillSetPresetPawnAbilityNtc.Serializer());
            Create(new S2CSkillSetSkillRes.Serializer());

            Create(new S2CStageAreaChangeRes.Serializer());
            Create(new S2CStageDungeonStartNtc.Serializer());
            Create(new S2CStageGetSpAreaChangeIdFromNpcIdRes.Serializer());
            Create(new S2CStageGetSpAreaChangeInfoRes.Serializer());
            Create(new S2CStageGetStageListRes.Serializer());
            Create(new S2CStageGetTicketDungeonCategoryListRes.Serializer());
            Create(new S2CStageGetTicketDungeonInfoListRes.Serializer());
            Create(new S2CStageUnisonAreaChangeBeginRecruitmentRes.Serializer());
            Create(new S2CStageUnisonAreaChangeGetRecruitmentStateRes.Serializer());
            Create(new S2CStageUnisonAreaChangeReadyCancelRes.Serializer());
            Create(new S2CStageUnisonAreaChangeReadyRes.Serializer());
            Create(new S2CStageUnisonAreaReadyCancelNtc.Serializer());

            Create(new S2CStampBonusCheckRes.Serializer());
            Create(new S2CStampBonusGetListRes.Serializer());
            Create(new S2CStampBonusRecieveDailyRes.Serializer());
            Create(new S2CStampBonusRecieveTotalRes.Serializer());

            Create(new S2CUpdateCharacterJobPointNtc.Serializer());
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

            Create(new S2C_63_0_16_NTC.Serializer());
            Create(new S2C_63_10_16_NTC.Serializer());
            Create(new S2C_63_11_16_NTC.Serializer());
            Create(new S2C_63_7_16_NTC.Serializer());
            Create(new S2C_BATTLE_71_13_16_NTC.Serializer());
            Create(new S2C_SEASON_62_22_16_NTC.Serializer());
            Create(new S2C_SEASON_62_28_16_NTC.Serializer());
            Create(new S2C_SEASON_62_39_16_NTC.Serializer());

            Create(new L2CClientChallengeRes.Serializer());
            Create(new L2CCreateCharacterDataNtc.Serializer());
            Create(new L2CCreateCharacterDataRes.Serializer());
            Create(new L2CDecideCharacterIdRes.Serializer());
            Create(new L2CDeleteCharacterInfoRes.Serializer());
            Create(new L2CGetCharacterListRes.Serializer());
            Create(new L2CGetErrorMessageListNtc.Serializer());
            Create(new L2CGetErrorMessageListRes.Serializer());
            Create(new L2CGetGameSessionKeyRes.Serializer());
            Create(new L2CGetLoginSettingRes.Serializer());
            Create(new L2CGpCourseGetInfoRes.Serializer());
            Create(new L2CLoginRes.Serializer());
            Create(new L2CLoginWaitNumNtc.Serializer());
            Create(new L2CLogoutRes.Serializer());
            Create(new L2CNextConnectionServerNtc.Serializer());
            Create(new L2CPingRes.Serializer());

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
