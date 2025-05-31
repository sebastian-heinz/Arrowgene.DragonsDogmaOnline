#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql.Core.Migration;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model.Scheduler;

namespace Arrowgene.Ddon.Database;

public interface IDatabase
{
    DbConnection OpenNewConnection();
    DbConnection OpenExistingConnection();
    void Execute(string sql, bool rethrowException = false);
    void Execute(DbConnection conn, string sql, bool rethrowException = false);
    bool ExecuteInTransaction(Action<DbConnection> action);
    int ExecuteNonQuery(DbConnection conn, string query, Action<DbCommand> action, bool rethrowException = false);
    void ExecuteReader(DbConnection conn, string sql, Action<DbCommand> commandAction, Action<DbDataReader> readAction, bool rethrowException = false);
    void ExecuteQuerySafe(DbConnection? connectionIn, Action<DbConnection> work);
    T ExecuteQuerySafe<T>(DbConnection? connectionIn, Func<DbConnection, T> work);
    void Stop();

    // Generic functions for getting/setting
    void AddParameter(DbCommand command, string name, object? value, DbType type);
    void AddParameter(DbCommand command, string name, string value);
    void AddParameter(DbCommand command, string name, int value);
    void AddParameter(DbCommand command, string name, float value);
    void AddParameter(DbCommand command, string name, byte value);
    void AddParameter(DbCommand command, string name, ushort value);
    void AddParameter(DbCommand command, string name, uint value);
    void AddParameter(DbCommand command, string name, byte[] value);
    void AddParameter(DbCommand command, string name, bool value);
    string? GetStringNullable(DbDataReader reader, int ordinal);
    byte[]? GetBytesNullable(DbDataReader reader, int ordinal, int size);
    int GetInt32(DbDataReader reader, string column);
    uint GetUInt32(DbDataReader reader, string column);
    byte GetByte(DbDataReader reader, string column);
    short GetInt16(DbDataReader reader, string column);
    ushort GetUInt16(DbDataReader reader, string column);
    long GetInt64(DbDataReader reader, string column);
    ulong GetUInt64(DbDataReader reader, string column);
    float GetFloat(DbDataReader reader, string column);
    string GetString(DbDataReader reader, string column);
    bool GetBoolean(DbDataReader reader, string column);
    byte[] GetBytes(DbDataReader reader, string column, int size);

    /// <summary>
    ///     Return true if database was created, or false if not.
    /// </summary>
    bool CreateDatabase();

    bool MigrateDatabase(DatabaseMigrator migrator, uint toVersion);

    // Meta
    bool CreateMeta(DatabaseMeta meta);
    bool SetMeta(DatabaseMeta meta);
    DatabaseMeta GetMeta();

    // Account
    Account? CreateAccount(string name, string mail, string hash);
    Account SelectAccountById(int accountId);
    Account? SelectAccountByName(string accountName);
    Account? SelectAccountByLoginToken(string loginToken);
    bool UpdateAccount(Account account);
    bool DeleteAccount(int accountId);
    Storages SelectAllStoragesByCharacterId(uint characterId);

    // CharacterCommon
    bool UpdateCharacterCommonBaseInfo(CharacterCommon common, DbConnection? connectionIn = null);

    bool UpdateEditInfo(CharacterCommon character);
    bool UpdateStatusInfo(CharacterCommon character);

    // Character
    bool CreateCharacter(Character character);
    Character SelectCharacter(uint characterId, DbConnection? connectionIn = null);
    List<Character> SelectCharactersByAccountId(int accountId, GameMode gameMode);
    List<Character> SelectAllCharacters();
    List<Character> SelectAllCharacters(DbConnection conn);
    bool DeleteCharacter(uint characterId);
    bool UpdateCharacterBaseInfo(Character character);
    bool UpdateCharacterMatchingProfile(Character character);
    bool UpdateCharacterArisenProfile(Character character);
    bool UpdateMyPawnSlot(uint characterId, uint num, DbConnection? connectionIn = null);
    bool UpdateRentalPawnSlot(uint characterId, uint num, DbConnection? connectionIn = null);
    bool UpdateCharacterBinaryData(uint characterId, byte[] data);
    void CreateItems(DbConnection conn, Character character);
    void CreateListItems(DbConnection conn, Character character, StorageType storageType, List<(uint ItemId, uint Amount)> itemList);

    CDataCharacterSearchParam SelectCharacterNameById(uint characterId);

    CDataCharacterSearchParam SelectCharacterNameById(DbConnection connection, uint characterId);

    // Pawn
    bool CreatePawn(Pawn pawn);
    Pawn SelectPawn(uint pawnId);
    Pawn SelectPawn(DbConnection connection, uint pawnId);
    List<Pawn> SelectPawnsByCharacterId(uint characterId, DbConnection? connectionIn = null);
    List<uint> SelectOfficialPawns();
    List<uint> SelectAllPlayerPawns(uint limit = 100);
    List<uint> SelectAllPlayerPawns(DbConnection connection, uint limit = 100);
    List<uint> SelectClanPawns(uint clanId, uint characterId = 0, uint limit = 100, DbConnection? connectionIn = null);

    List<CDataRegisterdPawnList> SelectRegisteredPawns(Character searchingCharacter, CDataPawnSearchParameter searchParams);

    List<CDataRegisterdPawnList> SelectRegisteredPawns(DbConnection conn, Character searchingCharacter, CDataPawnSearchParameter searchParams);

    bool DeletePawn(uint pawnId, DbConnection? connectionIn = null);
    bool UpdatePawnBaseInfo(Pawn pawn, DbConnection? connectionIn = null);
    uint GetPawnOwnerCharacterId(uint pawnId, DbConnection? connectionIn = null);
    bool ReplacePawnReaction(uint pawnId, CDataPawnReaction pawnReaction, DbConnection? connectionIn = null);

    // Pawn Training Status
    bool ReplacePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus, DbConnection? connectionIn = null);
    bool InsertPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
    bool InsertIfNotExistsPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
    bool UpdatePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);

    // Pawn Sp Skills
    bool InsertSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill);
    bool DeleteSpSkill(uint pawnId, JobId job, byte spSkillId);

    // CharacterJobData
    bool ReplaceCharacterJobData(uint commonId, CDataCharacterJobData replacedCharacterJobData, DbConnection? connectionIn = null);
    bool UpdateCharacterJobData(uint commonId, CDataCharacterJobData updatedCharacterJobData, DbConnection? connectionIn = null);

    // Wallet Points
    bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint);
    bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint);

    bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint, DbConnection? connectionIn = null);

    bool DeleteWalletPoint(uint characterId, WalletType type);

    // Released Warp Points
    List<ReleasedWarpPoint> SelectReleasedWarpPoints(uint characterId);

    bool InsertIfNotExistsReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);

    bool InsertIfNotExistsReleasedWarpPoints(uint characterId, List<ReleasedWarpPoint> ReleasedWarpPoint);

    bool InsertReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
    bool ReplaceReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
    bool UpdateReleasedWarpPoint(uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint);
    bool DeleteReleasedWarpPoint(uint characterId, uint warpPointId);

    //Storage
    Item SelectStorageItemByUId(string uId, DbConnection? connectionIn = null);
    bool InsertStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null);
    bool UpdateStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null);
    bool DeleteStorage(uint characterId, StorageType storageType, DbConnection? connectionIn = null);
    bool ReplaceStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null);

    // Storage Item
    bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);

    bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);

    bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo, DbConnection? connectionIn = null);

    bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);

    public void DeleteAllStorageItems(DbConnection connection, uint characterId);

    bool UpdateItemEquipPoints(string itemUID, uint equipPoints, DbConnection? connectionIn = null);

    // Equip
    bool InsertEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);

    bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId, DbConnection? connectionIn = null);

    bool UpdateEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);

    bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, DbConnection? connectionIn = null);

    void DeleteAllEquipItems(uint commonId, DbConnection? connectionIn = null);
    List<EquipItem> SelectEquipItemByCharacter(uint characterCommonId);

    // Job Items
    bool InsertEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo);
    bool ReplaceEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo);
    bool DeleteEquipJobItem(uint commonId, JobId job, ushort slotNo);

    // CustomSkills
    bool InsertLearnedCustomSkill(uint commonId, CustomSkill skill, DbConnection? connectionIn = null);
    bool UpdateLearnedCustomSkill(uint commonId, CustomSkill updatedSkill, DbConnection? connectionIn = null);
    bool InsertEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill);
    bool ReplaceEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill);

    bool UpdateEquippedCustomSkill(uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill skill);

    bool DeleteEquippedCustomSkill(uint commonId, JobId job, byte slotNo);

    // Abilities
    bool InsertLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null);
    bool UpdateLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null);

    bool InsertEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability);

    bool ReplaceEquippedAbility(
        uint commonId,
        JobId equipptedToJob,
        byte slotNo,
        Ability ability
    );

    bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities);

    bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equipptedToJob, byte slotNo, Ability ability);

    bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo);
    bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob);
    bool ReplaceAbilityPreset(uint characterId, CDataPresetAbilityParam preset);
    bool UpdateAbilityPreset(uint characterId, CDataPresetAbilityParam preset);

    bool InsertSecretAbilityUnlock(uint commonId, AbilityId secretAbility, DbConnection? connectionIn = null);
    List<AbilityId> SelectAllUnlockedSecretAbilities(uint commonId);

    // (Learned) Normal Skills / Learned Core Skills
    bool InsertIfNotExistsNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);

    bool InsertNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
    bool ReplaceNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);

    bool UpdateNormalSkillParam(uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam);

    bool DeleteNormalSkillParam(uint commonId, JobId job, uint skillNo);
    List<CDataNormalSkillParam> SelectNormalSkillParam(uint commonId, JobId job);

    // Shortcut
    bool InsertShortcut(uint characterId, CDataShortCut shortcut);

    bool ReplaceShortcut(uint characterId, CDataShortCut shortcut, DbConnection? connectionIn = null);

    bool UpdateShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataShortCut updatedShortcut);

    bool DeleteShortcut(uint characterId, uint pageNo, uint buttonNo);

    // CommunicationShortcut
    bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null);

    bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null);

    bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataCommunicationShortCut updatedCommunicationShortcut, DbConnection? connectionIn = null);

    bool DeleteCommunicationShortcut(uint characterId, uint pageNo, uint buttonNo);

    // GameToken
    bool SetToken(GameToken token);
    GameToken SelectTokenByAccountId(int accountId);
    GameToken SelectToken(string tokenStr);
    bool DeleteTokenByAccountId(int accountId);
    bool DeleteToken(string token);

    // Connections
    bool InsertConnection(Connection connection);
    List<Connection> SelectConnections();
    List<Connection> SelectConnectionsByAccountId(int accountId);
    bool DeleteConnection(int serverId, int accountId);
    bool DeleteConnectionsByAccountId(int accountId);
    bool DeleteConnectionsByServerId(int serverId);

    // ContactList
    int InsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite, bool requestedFavorite);

    int UpdateContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite, bool requestedFavorite);

    int DeleteContact(uint requestingCharacterId, uint requestedCharacterId);
    int DeleteContactById(uint id);
    List<ContactListEntity> SelectContactsByCharacterId(uint characterId);
    ContactListEntity SelectContactsByCharacterId(uint characterId1, uint characterId2);
    ContactListEntity SelectContactListById(uint id);
    List<(ContactListEntity, CDataCharacterListElement)> SelectFullContactListByCharacterId(uint characterId, DbConnection? connectionIn = null);

    // Dragon Force Augmentation
    bool InsertIfNotExistsDragonForceAugmentation(uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo, DbConnection? connectionIn = null);

    List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(
        uint commonId, DbConnection? connectionIn = null
    );

    bool InsertGainExtendParam(uint commonId, CDataOrbGainExtendParam Param);
    bool UpdateOrbGainExtendParam(uint commonId, CDataOrbGainExtendParam param, DbConnection? connectionIn = null);
    CDataOrbGainExtendParam SelectOrbGainExtendParam(uint commonId, DbConnection? connectionIn = null);

    // Bazaar
    ulong InsertBazaarExhibition(BazaarExhibition exhibition);
    int UpdateBazaarExhibiton(BazaarExhibition exhibition);
    int DeleteBazaarExhibition(ulong bazaarId);
    BazaarExhibition SelectBazaarExhibitionByBazaarId(ulong bazaarId);
    List<BazaarExhibition> FetchCharacterBazaarExhibitions(uint characterId);

    List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(uint itemId, uint excludedCharacterId, DbConnection? connectionIn = null);

    List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(List<uint> itemIds, uint excludedCharacterId, DbConnection? connectionIn = null);

    // Rewards
    bool InsertBoxRewardItems(uint commonId, QuestBoxRewards rewards, DbConnection? connectionIn = null);
    bool DeleteBoxRewardItem(uint commonId, uint uniqId, DbConnection? connectionIn = null);
    List<QuestBoxRewards> SelectBoxRewardItems(uint commonId, DbConnection? connectionIn = null);

    // Completed Quests
    List<CompletedQuest> GetCompletedQuestsByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null);
    CompletedQuest GetCompletedQuestsById(uint characterCommonId, QuestId questId, DbConnection? connectionIn = null);

    bool InsertCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, DbConnection? connectionIn = null);

    bool ReplaceCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null);
    bool UpdateCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null);

    // Quest Progress
    bool InsertQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null);
    bool UpdateQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null);
    bool RemoveQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, DbConnection? connectionIn = null);
    List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null);
    QuestProgress GetQuestProgressByScheduleId(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null);

    // Quest Priority
    bool InsertPriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null);
    List<uint> GetPriorityQuestScheduleIds(uint characterCommonId, DbConnection? connectionIn = null);
    bool DeletePriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null);

    // System mail
    long InsertSystemMailAttachment(SystemMailAttachment attachment);
    long InsertSystemMailAttachment(DbConnection connection, SystemMailAttachment attachment);
    long InsertSystemMailMessage(SystemMailMessage message);
    long InsertSystemMailMessage(DbConnection connection, SystemMailMessage message);
    List<SystemMailMessage> SelectSystemMailMessages(uint characterId);
    SystemMailMessage SelectSystemMailMessage(ulong messageId);
    bool UpdateSystemMailMessageState(ulong messageId, MailState messageState);
    bool DeleteSystemMailMessage(ulong messageId);

    // System mail attachments
    List<SystemMailAttachment> SelectAttachmentsForSystemMail(ulong messageId);

    bool UpdateSystemMailAttachmentReceivedStatus(
        ulong messageId,
        ulong attachmentId,
        bool isReceived
    );

    bool DeleteSystemMailAttachment(ulong messageId);

    // Additional Status
    bool InsertIfNotExistsAddStatus(
        string itemUid,
        uint characterId,
        byte isAddStat1,
        byte isAddStat2,
        ushort addStat1,
        ushort addStat2
    );

    bool InsertAddStatus(
        string itemUid,
        uint characterId,
        byte isAddStat1,
        byte isAddStat2,
        ushort addStat1,
        ushort addStat2
    );

    List<CDataAddStatusParam> GetAddStatusByUID(string itemUid);

    bool UpdateAddStatus(
        string itemUid,
        uint characterId,
        byte isAddStat1,
        byte isAddStat2,
        ushort addStat1,
        ushort addStat2
    );

    // Play points
    bool ReplaceCharacterPlayPointData(
        uint id,
        CDataJobPlayPoint updatedCharacterPlayPointData,
        DbConnection? connectionIn = null
    );

    bool UpdateCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData, DbConnection? connectionIn = null);

    // Stamps
    public bool InsertCharacterStampData(uint id, CharacterStampBonus stampData);
    public bool UpdateCharacterStampData(uint id, CharacterStampBonus stampData);

    // Crests
    bool InsertCrest(
        uint characterCommonId,
        string itemUId,
        uint slot,
        uint crestId,
        uint crestAmount,
        DbConnection? connectionIn = null
    );

    bool UpdateCrest(
        uint characterCommonId,
        string itemUId,
        uint slot,
        uint crestId,
        uint crestAmount,
        DbConnection? ConnectionIn = null
    );

    bool RemoveCrest(uint characterCommonId, string itemUId, uint slot, DbConnection? connectionIn = null);
    List<Crest> GetCrests(uint characterCommonId, string itemUId);

    // Bitterblack Maze Progress
    bool InsertBBMCharacterId(uint characterId, uint bbmCharacterId);
    uint SelectBBMCharacterId(uint characterId, DbConnection? connectionIn = null);
    uint SelectBBMNormalCharacterId(uint bbmCharacterId);

    bool InsertBBMProgress(
        uint characterId,
        ulong startTime,
        uint contentId,
        BattleContentMode contentMode,
        uint tier,
        bool killedDeath,
        ulong lastTicketTime
    );

    bool UpdateBBMProgress(uint characterId, BitterblackMazeProgress progress, DbConnection? connectionIn = null);
    BitterblackMazeProgress SelectBBMProgress(uint characterId);
    bool RemoveBBMProgress(uint characterId);

    // Bitterblack Maze Rewards
    bool InsertBBMRewards(uint characterId, uint goldMarks, uint silverMarks, uint redMarks);
    bool UpdateBBMRewards(uint characterId, BitterblackMazeRewards rewards, DbConnection? connectionIn = null);
    bool RemoveBBMRewards(uint characterId);
    BitterblackMazeRewards SelectBBMRewards(uint characterId, DbConnection? connectionIn = null);

    // Bitterblack Maze Treasure
    bool InsertBBMContentTreasure(
        uint characterId,
        BitterblackMazeTreasure treasure,
        DbConnection? connectionIn = null
    );

    bool InsertBBMContentTreasure(
        uint characterId,
        uint contentId,
        uint amount,
        DbConnection? connectionIn = null
    );

    bool UpdateBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure);
    bool UpdateBBMContentTreasure(uint characterId, uint contentId, uint amount);
    bool RemoveBBMContentTreasure(uint characterId);
    List<BitterblackMazeTreasure> SelectBBMContentTreasure(uint characterId, DbConnection? connectionIn = null);

    // Clan
    bool CreateClan(CDataClanParam clanParam);
    bool DeleteClan(CDataClanParam clan, DbConnection? connectionIn = null);
    uint SelectClanMembershipByCharacterId(uint characterId, DbConnection? connectionIn = null);
    ClanName GetClanNameByClanId(uint clanId, DbConnection? connectionIn = null);
    CDataClanParam SelectClan(uint clanId, DbConnection? connectionIn = null);
    bool UpdateClan(CDataClanParam clan, DbConnection? connectionIn = null);
    bool InsertClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null);
    bool DeleteClanMember(uint characterId, uint clanId, DbConnection? connectionIn = null);
    List<CDataClanMemberInfo> GetClanMemberList(uint clanId, DbConnection? connectionIn = null);
    CDataClanMemberInfo GetClanMember(uint characterId, DbConnection? connectionIn = null);
    bool UpdateClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null);
    List<uint> SelectClanShopPurchases(uint clanId, DbConnection? connectionIn = null);
    bool InsertClanShopPurchase(uint clanId, uint lineupId, DbConnection? connectionIn = null);
    List<(byte Type, uint Id)> SelectClanBaseCustomizations(uint clanId, DbConnection? connectionIn = null);
    bool InsertOrUpdateClanBaseCustomization(uint clanId, byte type, uint furnitureId, DbConnection? connectionIn = null);
    bool DeleteClanBaseCustomization(uint clanId, byte type, DbConnection? connectionIn = null);
    List<CDataClanSearchResult> SearchClans(CDataClanSearchParam param, DbConnection? connectionIn = null);

    // Epitaph Road
    bool InsertEpitaphRoadUnlock(uint characterId, uint epitaphId, DbConnection? connectionIn = null);
    HashSet<uint> GetEpitaphRoadUnlocks(uint characterId, DbConnection? connectionIn = null);

    bool InsertEpitaphWeeklyReward(uint characterId, uint epitaphId, DbConnection? connectionIn = null);
    HashSet<uint> GetEpitaphClaimedWeeklyRewards(uint characterId, DbConnection? connectionIn = null);
    void DeleteWeeklyEpitaphClaimedRewards(DbConnection? connectionIn = null);

    // Scheduler
    Dictionary<TaskType, SchedulerTaskEntry> SelectAllTaskEntries();
    bool UpdateScheduleInfo(TaskType type, long timestamp);

    // Area Rank
    bool InsertAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null);
    bool UpdateAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null);
    Dictionary<QuestAreaId, AreaRank> SelectAreaRank(uint characterId, DbConnection? connectionIn = null);
    List<(uint CharacterId, AreaRank Rank)> SelectAllAreaRank(DbConnection? connectionIn = null);
    bool ResetAreaRankPoint(DbConnection? connectionIn = null);
    bool InsertAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null);
    bool UpdateAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null);
    Dictionary<QuestAreaId, List<CDataRewardItemInfo>> SelectAreaRankSupply(uint characterId, DbConnection? connectionIn = null);
    List<CDataRewardItemInfo> SelectAreaRankSupply(uint characterId, QuestAreaId areaId, DbConnection? connectionIn = null);
    bool DeleteAreaRankSupply(DbConnection? connectionIn = null);

    // Rank Boards
    bool InsertRankRecord(uint characterId, uint questId, long score, DbConnection? connectionIn = null);
    List<uint> SelectUsedRankingBoardQuests(DbConnection? connectionIn = null);
    List<CDataRankingData> SelectRankingDataByCharacterId(uint characterId, uint questId, uint limit = 1000, DbConnection? connectionIn = null);
    List<CDataRankingData> SelectRankingData(uint questId, uint limit = 1000, DbConnection? connectionIn = null);
    bool DeleteAllRankRecords(DbConnection? connectionIn = null);

    // Partner Pawn
    bool InsertPartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null);
    bool UpdatePartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null);
    PartnerPawnData GetPartnerPawnRecord(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    bool SetPartnerPawn(uint characterId, uint pawnId, DbConnection? connectionIn = null);

    bool InsertPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null);
    bool HasPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null);
    void DeleteAllPartnerPawnLastAffectionIncreaseRecords(DbConnection? connectionIn = null);

    HashSet<uint> GetPartnerPawnPendingRewards(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    bool InsertPartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null);
    void DeletePartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null);

    // Equipment Recycle
    bool InsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null);
    bool UpdateRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null);
    bool HasRecycleEquipmentRecord(uint characterId, DbConnection? connectionIn = null);
    byte GetRecycleEquipmentAttempts(uint characterId, DbConnection? connectionIn = null);
    void ResetRecyleEquipmentRecords(DbConnection? connectionIn = null);
    bool UpsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null);

    // Equipment Limit Break
    bool InsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null);
    bool UpdateEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null);
    bool HasEquipmentLimitBreakRecord(uint characterId, string itemUID, DbConnection? connectionIn = null);
    bool UpsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null);
    List<CDataAddStatusParam> GetEquipmentLimitBreakRecord(string itemUID, DbConnection? connectionIn = null);

    // Achievements
    Dictionary<(AchievementType, uint), uint> SelectAchievementProgress(uint characterId, DbConnection? connectionIn = null);
    bool UpsertAchievementProgress(uint characterId, AchievementType achievementType, uint achievementParam, uint progress, DbConnection? connectionIn = null);
    Dictionary<uint, DateTimeOffset> SelectAchievementStatus(uint characterId, DbConnection? connectionIn = null);
    bool InsertAchievementStatus(uint characterId, AchievementAsset achievement, bool reward = false, DbConnection? connectionIn = null);
    Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> SelectAchievementUniqueCrafts(uint characterId, DbConnection? connectionIn = null);
    bool InsertAchievementUniqueCraft(uint characterId, AchievementCraftTypeParam craftType, ItemId itemId, DbConnection? connectionIn = null);

    // Unlockable Items (Recipes, Furniture, Backgrounds)
    HashSet<(UnlockableItemCategory Category, uint Id)> SelectUnlockedItems(uint characterId, DbConnection? connectionIn = null);
    bool InsertUnlockedItem(uint characterId, UnlockableItemCategory type, uint itemId, DbConnection? connectionIn = null);
    Dictionary<ItemId, byte> SelectMyRoomCustomization(uint characterId, DbConnection? connectionIn = null);
    bool UpsertMyRoomCustomization(uint characterId, byte layoutId, uint itemId, DbConnection? connectionIn = null);
    bool DeleteMyRoomCustomization(uint characterId, uint itemId, DbConnection? connectionIn = null);

    // Job Master
    bool InsertJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null);
    bool HasJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null);
    CDataReleaseElement GetJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null);
    List<CDataReleaseElement> GetJobMasterReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null);

    bool InsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    bool UpdateJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    bool HasJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    bool UpsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    CDataActiveJobOrder GetJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    List<CDataActiveJobOrder> GetJobMasterActiveOrders(uint characterId, JobId jobId, DbConnection? connectionIn = null);
    bool DeleteJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);

    bool InsertJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    bool UpdateJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    bool HasJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    bool UpsertJobMasterActiveOrdersProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    List<CDataJobOrderProgress> GetJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId,
        DbConnection? connectionIn = null);

    bool InsertSkillAugmentationReleasedElement(uint characterId, JobId jobId, uint releaseId, DbConnection? connectionIn = null);
    HashSet<uint> GetSkillAugmentationReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null);

    public bool UpsertJobEmblemData(uint characterId, JobEmblem jobEmblem, DbConnection? connectionIn = null);
    public JobEmblem GetJobEmblemData(uint characterId, JobId jobId, DbConnection? connectionIn = null);
    public List<JobEmblem> GetAllJobEmblemData(uint characterId, DbConnection? connectionIn = null);

    bool InsertPawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    bool DeletePawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    HashSet<uint> GetPawnFavorites(uint characterId, DbConnection? connectionIn = null);

    #region Pawn craft progress

    bool ReplacePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null);
    bool InsertPawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null);
    bool UpdatePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null);
    bool DeletePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null);
    CraftProgress? SelectPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null);

    #endregion
}
