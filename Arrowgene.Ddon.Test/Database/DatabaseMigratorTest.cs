#nullable enable
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql.Core.Migration;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Xunit;
using Xunit.Abstractions;

namespace Arrowgene.Ddon.Test.Database
{
    public class DatabaseMigratorTest : TestBase
    {
        private IDatabase db = new MockDatabase();

        public DatabaseMigratorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TestStraightforwardMigration()
        {
            MockMigrationStrategy.CALL_COUNT = 0;
            var from0to1 = new MockMigrationStrategy() { From = 0, To = 1 };
            var from1to2 = new MockMigrationStrategy() { From = 1, To = 2 };
            var from2to3 = new MockMigrationStrategy() { From = 2, To = 3 };
            var strategies = new List<IMigrationStrategy>() {
                from0to1,
                from1to2,
                from2to3
            };
            var migrator = new DatabaseMigrator(strategies);
            bool result = migrator.MigrateDatabase(db, 0, 3);

            Assert.True(result);

            Assert.True(from0to1.Called);
            Assert.True(from0to1.CallOrder == 0);
            Assert.True(from1to2.Called);
            Assert.True(from1to2.CallOrder == 1);
            Assert.True(from2to3.Called);
            Assert.True(from2to3.CallOrder == 2);
        }

        [Fact]
        public void TestShortestMigrationPath()
        {
            MockMigrationStrategy.CALL_COUNT = 0;
            var from0to1 = new MockMigrationStrategy() { From = 0, To = 1 };
            var from1to2 = new MockMigrationStrategy() { From = 1, To = 2 };
            var from2to3 = new MockMigrationStrategy() { From = 2, To = 3 };
            var from0to3 = new MockMigrationStrategy() { From = 0, To = 3 };
            var strategies = new List<IMigrationStrategy>() {
                from0to1,
                from1to2,
                from2to3,
                from0to3,
            };
            var migrator = new DatabaseMigrator(strategies);
            bool result = migrator.MigrateDatabase(db, 0, 3);

            Assert.True(result);

            Assert.False(from0to1.Called);
            Assert.False(from1to2.Called);
            Assert.False(from2to3.Called);
            Assert.True(from0to3.Called);
            Assert.True(from2to3.CallOrder == 0);
        }

        [Fact]
        public void TestConvolutedMigrationPath()
        {
            MockMigrationStrategy.CALL_COUNT = 0;
            var from0to2 = new MockMigrationStrategy() { From = 0, To = 2 };
            var from2to1 = new MockMigrationStrategy() { From = 2, To = 1 };
            var from1to3 = new MockMigrationStrategy() { From = 1, To = 3 };
            var strategies = new List<IMigrationStrategy>() {
                from1to3,
                from2to1,
                from0to2,
            };
            var migrator = new DatabaseMigrator(strategies);
            bool result = migrator.MigrateDatabase(db, 0, 3);

            Assert.True(result);

            Assert.True(from0to2.Called);
            Assert.True(from0to2.CallOrder == 0);
            Assert.True(from2to1.Called);
            Assert.True(from2to1.CallOrder == 1);
            Assert.True(from1to3.Called);
            Assert.True(from1to3.CallOrder == 2);
        }

        [Fact]
        public void TestShortestConvolutedMigrationPath()
        {
            MockMigrationStrategy.CALL_COUNT = 0;
            var from0to2 = new MockMigrationStrategy() { From = 0, To = 2 };
            var from2to3 = new MockMigrationStrategy() { From = 2, To = 3 };
            var from3to1 = new MockMigrationStrategy() { From = 3, To = 1 };
            var from1to4 = new MockMigrationStrategy() { From = 1, To = 4 };
            var from0to1 = new MockMigrationStrategy() { From  = 0, To  = 1 };
            var strategies = new List<IMigrationStrategy>() {
                from0to2,
                from2to3,
                from3to1,
                from1to4,
                from0to1
            };
            var migrator = new DatabaseMigrator(strategies);
            bool result = migrator.MigrateDatabase(db, 0, 4);

            Assert.True(result);

            Assert.True(from0to1.Called);
            Assert.True(from0to1.CallOrder == 0);
            Assert.True(from1to4.Called);
            Assert.True(from1to4.CallOrder == 1);
            Assert.False(from0to2.Called);
            Assert.False(from2to3.Called);
            Assert.False(from3to1.Called);
        }

        [Fact]
        public void TestNoMigrationPath()
        {
            MockMigrationStrategy.CALL_COUNT = 0;
            var from0to1 = new MockMigrationStrategy() { From = 0, To = 1 };
            var from1to2 = new MockMigrationStrategy() { From = 1, To = 2 };
            var from2to3 = new MockMigrationStrategy() { From = 2, To = 3 };
            var strategies = new List<IMigrationStrategy>() {
                from0to1,
                from1to2,
                from2to3
            };
            var migrator = new DatabaseMigrator(strategies);

            Assert.Throws<Exception>(() => migrator.MigrateDatabase(db, 0, 4));

            Assert.False(from0to1.Called);
            Assert.False(from1to2.Called);
            Assert.False(from2to3.Called);
        }

        [Fact]
        public void TestNoMigrationNeeded()
        {
            MockMigrationStrategy.CALL_COUNT = 0;
            var from0to1 = new MockMigrationStrategy() { From = 0, To = 1 };
            var from1to2 = new MockMigrationStrategy() { From = 1, To = 2 };
            var from2to3 = new MockMigrationStrategy() { From = 2, To = 3 };
            var strategies = new List<IMigrationStrategy>() {
                from0to1,
                from1to2,
                from2to3
            };
            var migrator = new DatabaseMigrator(strategies);
            bool result = migrator.MigrateDatabase(db, 3, 3);

            Assert.True(result);

            Assert.False(from0to1.Called);
            Assert.False(from1to2.Called);
            Assert.False(from2to3.Called);
        }
    }

    class MockDatabase : IDatabase
    {
        public bool ExecuteInTransaction(Action<DbConnection> action) { 
            action.Invoke(null); return true; 
        }

        public int ExecuteNonQuery(DbConnection conn, string command, Action<DbCommand> action, bool rethrowException = false) { return 1; }
        public void ExecuteReader(string command, Action<DbDataReader> action, bool rethrowException = false) {}
        public void ExecuteReader(DbConnection conn, string sql, Action<DbCommand> commandAction, Action<DbDataReader> readAction, bool rethrowException = false) {}
        public void ExecuteQuerySafe(DbConnection? connectionIn, Action<DbConnection> work) {}
        T IDatabase.ExecuteQuerySafe<T>(DbConnection? connectionIn, Func<DbConnection, T> work) { throw new NotImplementedException(); }

        public Account CreateAccount(string name, string mail, string hash) { return new Account(); }
        public bool CreateCharacter(Character character) { return true; }
        public bool CreateDatabase() { return true; }
        public bool CreatePawn(Pawn pawn) { return true; }
        public void CreateItems(DbConnection connection, Character character) { }
        public void CreateListItems(DbConnection conn, Character character, StorageType storageType, List<(uint ItemId, uint Amount)> itemList) {}

        public bool DeleteAccount(int accountId) { return true; }
        public int DeleteBazaarExhibition(ulong bazaarId) { return 1; }
        public bool DeleteBoxRewardItem(uint commonId, uint uniqId, DbConnection? connectionIn = null) { return true; }
        public bool DeleteCharacter(uint characterId) { return true; }
        public bool DeleteCommunicationShortcut(uint characterId, uint pageNo, uint buttonNo) { return true; }
        public bool DeleteConnection(int serverId, int accountId) { return true; }
        public bool DeleteConnectionsByAccountId(int accountId) { return true; }
        public bool DeleteConnectionsByServerId(int serverId) { return true; }
        public int DeleteContact(uint requestingCharacterId, uint requestedCharacterId) { return 1; }
        public int DeleteContactById(uint id) { return 1; }
        public bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, DbConnection? connectionIn = null) { return true; }
        public bool DeleteEquipJobItem(uint commonId, JobId job, ushort slotNo) { return true; }
        public bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob) { return true; }
        public bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo) { return true; }
        public bool DeleteEquippedCustomSkill(uint commonId, JobId job, byte slotNo) { return true; }
        public bool DeleteNormalSkillParam(uint commonId, JobId job, uint skillNo) { return true; }
        public bool DeletePawn(uint pawnId, DbConnection? connectionIn = null) { return true; }
        public bool DeletePriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null) { return true; }
        public bool DeleteReleasedWarpPoint(uint characterId, uint warpPointId) { return true; }
        public bool DeleteShortcut(uint characterId, uint pageNo, uint buttonNo) { return true; }
        public bool DeleteSpSkill(uint pawnId, JobId job, byte spSkillId) { return true; }
        public bool DeleteStorage(uint characterId, StorageType storageType) { return true; }
        public bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo, DbConnection? connectionIn = null) { return true; }
        public void DeleteAllStorageItems(DbConnection connection, uint characterId) { }
        public void DeleteAllEquipItems(uint commonId, DbConnection? connectionIn = null) { }
        public bool DeleteToken(string token) { return true; }
        public bool DeleteTokenByAccountId(int accountId) { return true; }
        public bool DeleteWalletPoint(uint characterId, WalletType type) { return true; }
        public DbConnection OpenNewConnection() { return null; }
        public DbConnection OpenExistingConnection() { return null; }
        public void Execute(string sql, bool rethrowException = false) {}
        public void Execute(DbConnection conn, string sql, bool rethrowException = false) {}
        public List<BazaarExhibition> FetchCharacterBazaarExhibitions(uint characterId) { return new List<BazaarExhibition>(); }
        public CompletedQuest GetCompletedQuestsById(uint characterCommonId, QuestId questId, DbConnection? connectionIn = null) { return new CompletedQuest(); }
        public List<CompletedQuest> GetCompletedQuestsByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null) { return new List<CompletedQuest>(); }
        public bool CreateMeta(DatabaseMeta meta) { return true; }
        public DatabaseMeta GetMeta() { return new DatabaseMeta(); }
        public List<uint> GetPriorityQuestScheduleIds(uint characterCommonId, DbConnection? connectionIn = null) { return new List<uint>(); }
        public QuestProgress GetQuestProgressByScheduleId(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null) { return new QuestProgress(); }
        public List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null) { return new List<QuestProgress>(); }
        public ulong InsertBazaarExhibition(BazaarExhibition exhibition) { return 1; }
        public bool InsertBoxRewardItems(uint commonId, QuestBoxRewards rewards, DbConnection? connectionIn = null) { return true; }
        public bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null) { return true; }
        public bool InsertConnection(Connection connection) { return true; }
        public int InsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite, bool requestedFavorite) { return 1; }
        public bool InsertEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId) { return true; }
        public bool InsertEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo) { return true; }
        public bool InsertEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability) { return true; }
        public bool InsertEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill) { return true; }
        public bool InsertGainExtendParam(uint commonId, CDataOrbGainExtendParam Param) { return true; }
        public bool InsertCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, DbConnection? connectionIn = null) { return true; }
        public bool InsertIfNotExistsDragonForceAugmentation(uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo, DbConnection? connectionIn = null) { return true; }
        public bool InsertIfNotExistsNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam) { return true; }
        public bool InsertIfNotExistsPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus) { return true; }
        public bool InsertIfNotExistsReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint) { return true; }
        public bool InsertIfNotExistsReleasedWarpPoints(uint characterId, List<ReleasedWarpPoint> ReleasedWarpPoint) { return true; }
        public bool InsertLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null) { return true; }
        public bool InsertLearnedCustomSkill(uint commonId, CustomSkill skill, DbConnection? connectionIn = null) { return true; }
        public bool InsertNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam) { return true; }
        public bool InsertPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus) { return true; }
        public bool InsertPriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null) { return true; }

        public bool UpdateCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null) { return true;}

        public bool InsertQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null) { return true; }
        public bool InsertReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint) { return true; }
        public bool InsertSecretAbilityUnlock(uint commonId, AbilityId secretAbility, DbConnection? connectionIn = null) { return true; }
        public bool InsertShortcut(uint characterId, CDataShortCut shortcut) { return true; }
        public CraftProgress SelectPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null) { return new CraftProgress(); }
        public bool InsertSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill) { return true; }
        public bool InsertStorage(uint characterId, StorageType storageType, Storage storage) { return true; }
        public bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null) { return true; }
        public bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint) { return true; }
        public bool RemoveQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceCharacterJobData(uint commonId, CDataCharacterJobData replacedCharacterJobData, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo) { return true; }
        public bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities) { return true; }
        public bool ReplaceEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability) { return true; }
        public bool ReplaceEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill) { return true; }
        public bool ReplaceNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam) { return true; }
        public bool ReplacePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint) { return true; }
        public bool ReplaceShortcut(uint characterId, CDataShortCut shortcut, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint) { return true; }
        public bool ReplaceCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count, DbConnection? connectionIn = null) { return true; }
        public Account SelectAccountById(int accountId) { return new Account(); }
        public Account SelectAccountByLoginToken(string loginToken) { return new Account(); }
        public Account SelectAccountByName(string accountName) { return new Account(); }
        public List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(uint itemId, uint excludedCharacterId, DbConnection? connectionIn = null) { return new List<BazaarExhibition>(); }
        public List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(List<uint> itemIds, uint excludedCharacterId, DbConnection? connectionIn = null) { return new List<BazaarExhibition>(); }
        public List<AbilityId> SelectAllUnlockedSecretAbilities(uint commonId) { return new List<AbilityId>(); }
        public BazaarExhibition SelectBazaarExhibitionByBazaarId(ulong bazaarId) { return new BazaarExhibition(); }
        public List<QuestBoxRewards> SelectBoxRewardItems(uint commonId, DbConnection? connectionIn = null) { return new List<QuestBoxRewards>(); }
        public Character SelectCharacter(uint characterId, DbConnection? connectionIn = null) { return new Character(); }
        public List<Character> SelectCharactersByAccountId(int accountId, GameMode gameMode) { return new List<Character>(); }
        public List<Character> SelectAllCharacters() { return new List<Character>(); }
        public List<Character> SelectAllCharacters(DbConnection conn) { return new List<Character>(); }
        public List<Connection> SelectConnections() { return new(); }
        public List<Connection> SelectConnectionsByAccountId(int accountId) { return new List<Connection>(); }
        public ContactListEntity SelectContactListById(uint id) { return new ContactListEntity(); }
        public List<ContactListEntity> SelectContactsByCharacterId(uint characterId) { return new List<ContactListEntity>(); }
        public ContactListEntity SelectContactsByCharacterId(uint characterId1, uint characterId2) { return new ContactListEntity(); }
        public List<(ContactListEntity, CDataCharacterListElement)> SelectFullContactListByCharacterId(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public Item SelectStorageItemByUId(string uId, DbConnection? connectionIn = null) { return new Item(); }

        public bool InsertStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null) { return true; }
        public bool UpdateStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null) { return true; }
        public bool DeleteStorage(uint characterId, StorageType storageType, DbConnection? connectionIn = null) { return true; }
        public bool ReplaceStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null) { return true; }

        public List<CDataNormalSkillParam> SelectNormalSkillParam(uint commonId, JobId job) { return new List<CDataNormalSkillParam>(); }
        public CDataOrbGainExtendParam SelectOrbGainExtendParam(uint commonId, DbConnection? connectionIn = null) { return new CDataOrbGainExtendParam(); }
        public List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(uint commonId, DbConnection? connectionIn = null) { return new List<CDataReleaseOrbElement>(); }
        public Pawn SelectPawn(uint pawnId) { return new Pawn(); }
        public Pawn SelectPawn(DbConnection connection, uint pawnId) { return new Pawn(); }
        public List<Pawn> SelectPawnsByCharacterId(uint characterId, DbConnection? connectionIn = null) { return new List<Pawn>(); }
        public List<ReleasedWarpPoint> SelectReleasedWarpPoints(uint characterId) { return new List<ReleasedWarpPoint>(); }
        public GameToken SelectToken(string tokenStr) { return new GameToken(); }
        public GameToken SelectTokenByAccountId(int accountId) { return new GameToken(); }
        public List<EquipItem> SelectEquipItemByCharacter(uint characterCommonId) { return new List<EquipItem>(); }
        public Storages SelectAllStoragesByCharacterId(uint characterId) { return new Storages(new Dictionary<StorageType, ushort>()); }
        public bool SetMeta(DatabaseMeta meta) { return true; }
        public bool SetToken(GameToken token) { return true; }
        public bool UpdateAccount(Account account) { return true; }
        public int UpdateBazaarExhibiton(BazaarExhibition exhibition) { return 1; }
        public bool UpdateCharacterArisenProfile(Character character) { return true; }
        public bool UpdateCharacterBaseInfo(Character character) { return true; }
        public bool UpdateCharacterCommonBaseInfo(CharacterCommon common, DbConnection? connectionIn = null) { return true; }
        public bool UpdateCharacterJobData(uint commonId, CDataCharacterJobData updatedCharacterJobData, DbConnection? connectionIn = null) { return true; }
        public bool UpdateCharacterMatchingProfile(Character character) { return true; }
        public bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataCommunicationShortCut updatedCommunicationShortcut, DbConnection? connectionIn = null) { return true; }
        public int UpdateContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite, bool requestedFavorite) { return 1; }
        public bool UpdateEditInfo(CharacterCommon character) { return true; }
        public bool UpdateEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId) { return true; }
        public bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equipptedToJob, byte slotNo, Ability ability) { return true; }
        public bool UpdateEquippedCustomSkill(uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill skill) { return true; }
        public bool UpdateLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null) { return true; }
        public bool UpdateLearnedCustomSkill(uint commonId, CustomSkill updatedSkill, DbConnection? connectionIn = null) { return true; }
        public bool UpdateNormalSkillParam(uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam) { return true; }
        public bool UpdateOrbGainExtendParam(uint commonId, CDataOrbGainExtendParam param, DbConnection? connectionIn = null) { return true; }
        public bool UpdatePawnBaseInfo(Pawn pawn, DbConnection? connectionIn = null) { return true; }
        public bool UpdatePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus) { return true; }
        public bool ReplacePawnReaction(uint pawnId, CDataPawnReaction pawnReaction, DbConnection? connectionIn = null) { return true; }
        public bool ReplacePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null) { return true; }
        public bool InsertPawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null) { return true; }
        public bool UpdatePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null) { return true; }
        public bool DeletePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null) { return true; }
        public bool UpdateQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null) { return true; }
        public bool UpdateReleasedWarpPoint(uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint) { return true; }
        public bool UpdateShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataShortCut updatedShortcut) { return true; }
        public bool UpdateStatusInfo(CharacterCommon character) { return true; }
        public bool UpdateStorage(uint characterId, StorageType storageType, Storage storage) { return true; }
        public bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null) { return true; }
        public bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint, DbConnection? connectionIn = null) { return true; }
        public bool UpdateMyPawnSlot(uint characterId, uint num, DbConnection? connectionIn = null) { return true; }
        public bool UpdateRentalPawnSlot(uint characterId, uint num, DbConnection? connectionIn = null) { return true; }

        public bool MigrateDatabase(DatabaseMigrator migrator, uint toVersion) { return true; }
        public long InsertSystemMailMessage(SystemMailMessage message) { return 0; }
        public long InsertSystemMailMessage(DbConnection connection, SystemMailMessage message) { return 0; }
        public List<SystemMailMessage> SelectSystemMailMessages(uint characterId) { return new List<SystemMailMessage>(); }
        public SystemMailMessage SelectSystemMailMessage(ulong messageId) { return new SystemMailMessage(); }
        public bool UpdateSystemMailMessageState(ulong messageId, MailState messageState) {  return true; }
        public bool DeleteSystemMailMessage(ulong messageId) { return true; }
        public long InsertSystemMailAttachment(SystemMailAttachment attachment) { return 0; }
        public long InsertSystemMailAttachment(DbConnection connection, SystemMailAttachment attachment) { return 0;  }
        public List<SystemMailAttachment> SelectAttachmentsForSystemMail(ulong messageId) { return new List<SystemMailAttachment>(); }
        public bool UpdateSystemMailAttachmentReceivedStatus(ulong messageId, ulong attachmentId, bool isReceived) {  return true; }
        public bool DeleteSystemMailAttachment(ulong messageId) { return true; }
        public bool UpdateItemEquipPoints(string itemUID, uint equipPoints, DbConnection? connectionIn = null) {return true; }
        public bool InsertIfNotExistsAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2) { return true; }
        public bool InsertAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2) { return true; }
        public List<CDataAddStatusParam> GetAddStatusByUID(string itemUid) { return new List<CDataAddStatusParam>(); }
        public bool UpdateAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2) { return true; }
        public bool ReplaceCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData, DbConnection? connectionIn = null) { return true; }
        public bool UpdateCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData, DbConnection? connectionIn = null) { return true; }
        public bool InsertCharacterStampData(uint id, CharacterStampBonus stampData) { return true; }
        public bool UpdateCharacterStampData(uint id, CharacterStampBonus stampData) { return true; }
        public bool InsertCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? connectionIn = null) { return true; }
        public bool UpdateCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? ConnectionIn = null) { return true; }
        public bool RemoveCrest(uint characterCommonId, string itemUId, uint slot, DbConnection? connectionIn = null) { return true; }
        public List<Crest> GetCrests(uint characterCommonId, string itemUId) { return new List<Crest>(); }
        public bool ReplaceAbilityPreset(uint characterId, CDataPresetAbilityParam preset) {return true; }
        public bool UpdateAbilityPreset(uint characterId, CDataPresetAbilityParam preset) { return true; }
        public bool UpdateCharacterBinaryData(uint characterId, byte[] data) { return true; }
        public bool InsertBBMCharacterId(uint characterId, uint bbmCharacterId) { return false; }
        public uint SelectBBMCharacterId(uint characterId, DbConnection? connectionIn = null) { return 0; }
        public uint SelectBBMNormalCharacterId(uint bbmCharacterId) { return 0; }
        public bool InsertBBMProgress(uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime) { return true;  }
        public bool UpdateBBMProgress(uint characterId, BitterblackMazeProgress progress, DbConnection? connectionIn = null) { return true; }
        public bool RemoveBBMProgress(uint characterId) { return true; }
        public BitterblackMazeProgress SelectBBMProgress(uint characterId) { return new BitterblackMazeProgress(); }
        public bool InsertBBMRewards(uint characterId, uint goldMarks, uint silverMarks, uint redMarks) { return true; }
        public bool UpdateBBMRewards(uint characterId, BitterblackMazeRewards rewards, DbConnection? connectionIn = null) { return true; }
        public bool RemoveBBMRewards(uint characterId) { return true; }
        public BitterblackMazeRewards SelectBBMRewards(uint characterId, DbConnection? connectionIn = null) { return new BitterblackMazeRewards(); }
        public bool InsertBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure, DbConnection? connectionIn = null) { return true; }
        public bool InsertBBMContentTreasure(uint characterId, uint contentId, uint amount, DbConnection? connectionIn = null) { return true; }
        public bool UpdateBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure) { return true; }
        public bool UpdateBBMContentTreasure(uint characterId, uint contentId, uint amount) { return true; }
        public bool RemoveBBMContentTreasure(uint characterId) { return true; }
        public List<BitterblackMazeTreasure> SelectBBMContentTreasure(uint characterId, DbConnection? connectionIn = null) { return new List<BitterblackMazeTreasure>(); }
        public List<uint> SelectOfficialPawns() { return new List<uint>(); }
        public List<uint> SelectAllPlayerPawns(uint limit = 100) { return new List<uint>(); }
        public List<uint> SelectAllPlayerPawns(DbConnection connection, uint limit = 100) { return new List<uint>(); }
        public List<uint> SelectClanPawns(uint clanId, uint characterId = 0, uint limit = 100, DbConnection? connectionIn = null) { return new(); }
        public List<uint> SelectRandomPlayerPawns(uint limit = 100) { return new List<uint>(); }
        public List<uint> SelectRandomPlayerPawns(DbConnection connection, uint limit = 100) { return new List<uint>(); }
        public uint GetPawnOwnerCharacterId(uint pawnId, DbConnection? connectionIn = null) { return 0; }
        public CDataCharacterSearchParam SelectCharacterNameById(uint characterId) { return new CDataCharacterSearchParam(); }
        public CDataCharacterSearchParam SelectCharacterNameById(DbConnection connection, uint characterId) { return new CDataCharacterSearchParam(); }

        public bool CreateClan(CDataClanParam clanParam) { return true; }
        public bool DeleteClan(CDataClanParam clan, DbConnection? connectionIn = null) { return true; }
        public uint SelectClanMembershipByCharacterId(uint characterId, DbConnection? connectionIn = null) { return 0; }
        public CDataClanParam SelectClan(uint clanId, DbConnection? connectionIn = null) { return new CDataClanParam(); }
        public ClanName GetClanNameByClanId(uint clanId, DbConnection? connectionIn = null) { return new ClanName(); }
        public bool UpdateClan(CDataClanParam clan, DbConnection? connectionIn = null) { return true; }
        public bool InsertClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null) { return true; }
        public bool DeleteClanMember(uint characterId, uint clanId, DbConnection? connectionIn = null) { return true; }
        public List<CDataClanMemberInfo> GetClanMemberList(uint clanId, DbConnection? connectionIn = null) { return new(); }
        public CDataClanMemberInfo GetClanMember(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool UpdateClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null) { return true; }
        public List<uint> SelectClanShopPurchases(uint clanId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertClanShopPurchase(uint clanId, uint lineupId, DbConnection? connectionIn = null) { return true; }
        public List<(byte Type, uint Id)> SelectClanBaseCustomizations(uint clanId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertOrUpdateClanBaseCustomization(uint clanId, byte type, uint furnitureId, DbConnection? connectionIn = null) { return true; }
        public bool DeleteClanBaseCustomization(uint clanId, byte type, DbConnection? connectionIn = null) { return true; }
        public List<CDataClanSearchResult> SearchClans(CDataClanSearchParam param, DbConnection? connectionIn = null) { return new(); }

        public bool InsertEpitaphRoadUnlock(uint characterId, uint epitaphId, DbConnection? connectionIn = null) { return true; }
        public HashSet<uint> GetEpitaphRoadUnlocks(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertEpitaphWeeklyReward(uint characterId, uint epitaphId, DbConnection? connectionIn = null) { return true; }
        public HashSet<uint> GetEpitaphClaimedWeeklyRewards(uint characterId, DbConnection? connectionIn = null) { return new(); }

        public Dictionary<TaskType, SchedulerTaskEntry> SelectAllTaskEntries() { return new(); }
        public bool UpdateScheduleInfo(TaskType type, long timestamp) { return true; }
        public List<CDataRegisterdPawnList> SelectRegisteredPawns(Character searchingCharacter, CDataPawnSearchParameter searchParams) { return new List<CDataRegisterdPawnList>(); }
        public List<CDataRegisterdPawnList> SelectRegisteredPawns(DbConnection conn, Character searchingCharacter, CDataPawnSearchParameter searchParams) { return new List<CDataRegisterdPawnList>(); }
        public void DeleteWeeklyEpitaphClaimedRewards(DbConnection? connectionIn = null) { }
        public bool InsertAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null) { return true; }
        public bool UpdateAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null) { return true; }
        public Dictionary<QuestAreaId, AreaRank> SelectAreaRank(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public List<(uint CharacterId, AreaRank Rank)> SelectAllAreaRank(DbConnection? connectionIn = null) { return new(); }
        public bool ResetAreaRankPoint(DbConnection? connectionIn = null) { return true; }
        public bool InsertAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null) { return true; }
        public bool UpdateAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null) { return true; }
        public Dictionary<QuestAreaId, List<CDataRewardItemInfo>> SelectAreaRankSupply(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public List<CDataRewardItemInfo> SelectAreaRankSupply(uint characterId, QuestAreaId areaId, DbConnection? connectionIn = null) { return new(); }
        public bool DeleteAreaRankSupply(DbConnection? connectionIn = null) { return true; }


        public bool InsertRankRecord(uint characterId, uint questId, long score, DbConnection? connectionIn = null) { return true; }
        public List<uint> SelectUsedRankingBoardQuests(DbConnection? connectionIn = null) { return new(); }
        public List<CDataRankingData> SelectRankingDataByCharacterId(uint characterId, uint questId, uint limit = 1000, DbConnection? connectionIn = null) { return new(); }
        public List<CDataRankingData> SelectRankingData(uint questId, uint limit = 1000, DbConnection? connectionIn = null) { return new(); }
        public bool DeleteAllRankRecords(DbConnection? connectionIn = null) { return true; }

        public bool InsertPartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null) { return true; }
        public bool UpdatePartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null) { return true; }
        public PartnerPawnData GetPartnerPawnRecord(uint characterId, uint pawnId, DbConnection? connectionIn = null) { return new(); }
        public bool SetPartnerPawn(uint characterId, uint pawnId, DbConnection? connectionIn = null) { return true; }

        public bool InsertPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null) { return true; }
        public bool HasPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null) { return true; }
        public void DeleteAllPartnerPawnLastAffectionIncreaseRecords(DbConnection? connectionIn = null) { }

        public HashSet<uint> GetPartnerPawnPendingRewards(uint characterId, uint pawnId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertPartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null) { return true; }
        public void DeletePartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null) { }

        public bool InsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null) { return true; }
        public bool UpdateRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null) { return true;  }
        public bool HasRecycleEquipmentRecord(uint characterId, DbConnection? connectionIn = null) { return true; }
        public byte GetRecycleEquipmentAttempts(uint characterId, DbConnection? connectionIn = null) { return 0; }
        public void ResetRecyleEquipmentRecords(DbConnection? connectionIn = null) { }
        public bool UpsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null) { return true; }

        public bool InsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null) { return true; }
        public bool UpdateEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null) { return true; }
        public bool HasEquipmentLimitBreakRecord(uint characterId, string itemUID, DbConnection? connectionIn = null) { return true; }
        public bool UpsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null) { return true; }
        public List<CDataAddStatusParam> GetEquipmentLimitBreakRecord(string itemUID, DbConnection? connectionIn = null) { return new(); }

        public Dictionary<(AchievementType, uint), uint> SelectAchievementProgress(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool UpsertAchievementProgress(uint characterId, AchievementType achievementType, uint achievementParam, uint progress, DbConnection? connectionIn = null) { return true; }
        public Dictionary<uint, DateTimeOffset> SelectAchievementStatus(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertAchievementStatus(uint characterId, AchievementAsset achievement, bool reward = false, DbConnection? connectionIn = null) { return true; }
        public Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> SelectAchievementUniqueCrafts(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertAchievementUniqueCraft(uint characterId, AchievementCraftTypeParam craftType, ItemId itemId, DbConnection? connectionIn = null) { return true; }

        public HashSet<(UnlockableItemCategory Category, uint Id)> SelectUnlockedItems(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool InsertUnlockedItem(uint characterId, UnlockableItemCategory type, uint itemId, DbConnection? connectionIn = null) { return true; }
        public Dictionary<ItemId, byte> SelectMyRoomCustomization(uint characterId, DbConnection? connectionIn = null) { return new(); }
        public bool UpsertMyRoomCustomization(uint characterId, byte layoutId, uint itemId, DbConnection? connectionIn = null) { return true; }
        public bool DeleteMyRoomCustomization(uint characterId, uint itemId, DbConnection? connectionIn = null) { return true; }
        public void Stop() { }

        public bool InsertJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null) { return true; }
        public bool HasJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null) { return true; }
        public CDataReleaseElement GetJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null) { return new(); }
        public List<CDataReleaseElement> GetJobMasterReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null) { return new(); }

        public bool InsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null) { return true; }
        public bool UpdateJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null) { return true; }
        public bool HasJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null) { return true; }
        public bool UpsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null) { return true; }
        public CDataActiveJobOrder GetJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null) { return new(); }
        public List<CDataActiveJobOrder> GetJobMasterActiveOrders(uint characterId, JobId jobId, DbConnection? connectionIn = null) { return new(); }
        public bool DeleteJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null) { return true; }

        public bool InsertJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null) { return true; }
        public bool UpdateJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null) { return true; }
        public bool HasJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null) { return true; }
        public bool UpsertJobMasterActiveOrdersProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null) { return true; }
        public List<CDataJobOrderProgress> GetJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, DbConnection? connectionIn = null) { return new(); }

        public bool InsertSkillAugmentationReleasedElement(uint characterId, OrbTreeType orbTreeType, JobId jobId, uint releaseId, DbConnection? connectionIn = null) { return true; }
        public HashSet<uint> GetSkillAugmentationReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null) { return new(); }

        public bool UpsertJobEmblemData(uint characterId, JobEmblem jobEmblem, DbConnection? connectionIn = null) { return true; }
        public JobEmblem GetJobEmblemData(uint characterId, JobId jobId, DbConnection? connectionIn = null) { return new(); }
        public List<JobEmblem> GetAllJobEmblemData(uint characterId, DbConnection? connectionIn = null) { return new(); }

        public bool InsertLightQuestRecord(LightQuestRecord lightQuestRecord, DbConnection? connectionIn = null) { return true; }
        public List<LightQuestRecord> SelectLightQuestRecords(DbConnection? connectionIn = null) { return new(); }
        public bool DeleteLightQuestRecord(uint scheduleId, DbConnection? connectionIn = null) { return true; }

        public bool InsertPawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null) { return true; }
        public bool DeletePawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null) { return true; }
        public HashSet<uint> GetPawnFavorites(uint characterId, DbConnection? connectionIn = null) { return new(); }

        public void AddParameter(DbCommand command, string name, object? value, DbType type) { }
        public void AddParameter(DbCommand command, string name, string value) { }
        public void AddParameter(DbCommand command, string name, Int32 value) { }
        public void AddParameter(DbCommand command, string name, float value) { }
        public void AddParameter(DbCommand command, string name, byte value) { }
        public void AddParameter(DbCommand command, string name, UInt16 value) { }
        public void AddParameter(DbCommand command, string name, UInt32 value) { }
        public void AddParameter(DbCommand command, string name, byte[] value) { }
        public void AddParameter(DbCommand command, string name, bool value) { }
        public string? GetStringNullable(DbDataReader reader, int ordinal) { return ""; }
        public byte[]? GetBytesNullable(DbDataReader reader, int ordinal, int size) { return null; }
        public int GetInt32(DbDataReader reader, string column) { return 0; }
        public uint GetUInt32(DbDataReader reader, string column) { return 0; }
        public byte GetByte(DbDataReader reader, string column) { return 0; }
        public short GetInt16(DbDataReader reader, string column) { return 0; }
        public ushort GetUInt16(DbDataReader reader, string column) { return 0; }
        public long GetInt64(DbDataReader reader, string column) { return 0; }
        public ulong GetUInt64(DbDataReader reader, string column) { return 0; }
        public float GetFloat(DbDataReader reader, string column) { return 0; }
        public string GetString(DbDataReader reader, string column) { return ""; }
        public bool GetBoolean(DbDataReader reader, string column) { return false; }
        public byte[] GetBytes(DbDataReader reader, string column, int size) { return null; }
    }

    class MockMigrationStrategy : IMigrationStrategy
    {
        public static uint CALL_COUNT = 0;

        public uint From { get; set; }

        public uint To { get; set; }
        public bool DisableTransaction { get; }

        public bool Called { get; private set; } = false;
        public uint CallOrder { get; private set; }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            Called = true;
            CallOrder = CALL_COUNT++;
            return true;
        }
    }
}
