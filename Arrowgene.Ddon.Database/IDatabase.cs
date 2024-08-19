#nullable enable
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql.Core.Migration;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Arrowgene.Ddon.Database
{
    public interface IDatabase
    {
        void Execute(string sql);
        void Execute(DbConnection conn, string sql);
        bool ExecuteInTransaction(Action<DbConnection> action);
        int ExecuteNonQuery(DbConnection conn, string query, Action<DbCommand> action);
        void ExecuteReader(DbConnection conn, string sql, Action<DbCommand> commandAction, Action<DbDataReader> readAction);

        // Generic functions for getting/setting
        void AddParameter(DbCommand command, string name, object? value, DbType type);
        void AddParameter(DbCommand command, string name, string value);
        void AddParameter(DbCommand command, string name, Int32 value);
        void AddParameter(DbCommand command, string name, float value);
        void AddParameter(DbCommand command, string name, byte value);
        void AddParameter(DbCommand command, string name, UInt16 value);
        void AddParameter(DbCommand command, string name, UInt32 value);
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
        /// Return true if database was created, or false if not.
        /// </summary>
        bool CreateDatabase();
        bool MigrateDatabase(DatabaseMigrator migrator, uint toVersion);

        // Meta
        bool CreateMeta(DatabaseMeta meta);
        bool SetMeta(DatabaseMeta meta);
        DatabaseMeta GetMeta();

        // Account
        Account CreateAccount(string name, string mail, string hash);
        Account SelectAccountById(int accountId);
        Account SelectAccountByName(string accountName);
        Account SelectAccountByLoginToken(string loginToken);
        bool UpdateAccount(Account account);
        bool DeleteAccount(int accountId);

        // CharacterCommon
        bool UpdateCharacterCommonBaseInfo(CharacterCommon common, DbConnection? connectionIn = null);
        bool UpdateEditInfo(CharacterCommon character);
        bool UpdateStatusInfo(CharacterCommon character);

        // Character
        bool CreateCharacter(Character character);
        Character SelectCharacter(uint characterId);
        List<Character> SelectCharactersByAccountId(int accountId);
        List<Character> SelectAllCharacters();
        List<Character> SelectAllCharacters(DbConnection conn);
        bool DeleteCharacter(uint characterId);
        bool UpdateCharacterBaseInfo(Character character);
        bool UpdateCharacterMatchingProfile(Character character);
        bool UpdateCharacterArisenProfile(Character character);
        bool UpdateMyPawnSlot(uint characterId, uint num);
        bool UpdateCharacterBinaryData(uint characterId, byte[] data);

        // Pawn
        bool CreatePawn(Pawn pawn);
        Pawn SelectPawn(uint pawnId);
        List<Pawn> SelectPawnsByCharacterId(uint characterId);
        List<Pawn> SelectPawnsByCharacterId(DbConnection conn, uint characterId);
        bool DeletePawn(uint pawnId);
        bool UpdatePawnBaseInfo(Pawn pawn);

        // Pawn Training Status
        bool ReplacePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
        bool InsertPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
        bool InsertIfNotExistsPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
        bool UpdatePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
        
        #region Pawn craft progress
        bool ReplacePawnCraftProgress(CraftProgress craftProgress);
        bool InsertPawnCraftProgress(CraftProgress craftProgress);
        bool InsertIfNotExistsPawnCraftProgress(CraftProgress craftProgress);
        bool UpdatePawnCraftProgress(CraftProgress craftProgress);
        bool DeletePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId);
        CraftProgress SelectPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId);
        #endregion
        
        // Pawn Sp Skills
        bool InsertSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill);
        bool DeleteSpSkill(uint pawnId, JobId job, byte spSkillId);

        // CharacterJobData
        bool ReplaceCharacterJobData(uint commonId, CDataCharacterJobData replacedCharacterJobData);
        bool UpdateCharacterJobData(uint commonId, CDataCharacterJobData updatedCharacterJobData);

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
        bool InsertStorage(uint characterId, StorageType storageType, Storage storage);
        bool UpdateStorage(uint characterId, StorageType storageType, Storage storage);
        bool DeleteStorage(uint characterId, StorageType storageType);

        // Storage Item
        bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);
        bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);
        bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo, DbConnection? connectionIn = null);
        bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);
        bool UpdateItemEquipPoints(string itemUID, uint EquipPoints);

        // Equip
        bool InsertEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
        bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId, DbConnection? connectionIn = null);
        bool UpdateEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
        bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, DbConnection? connectionIn = null);
        List<EquipItem> SelectEquipItemByCharacter(uint characterCommonId);

        // Job Items
        bool InsertEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo);
        bool ReplaceEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo);
        bool DeleteEquipJobItem(uint commonId, JobId job, ushort slotNo);

        // CustomSkills
        bool InsertLearnedCustomSkill(uint commonId, CustomSkill skill);
        bool UpdateLearnedCustomSkill(uint commonId, CustomSkill updatedSkill);
        bool InsertEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill);
        bool ReplaceEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill);
        bool UpdateEquippedCustomSkill(uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill skill);
        bool DeleteEquippedCustomSkill(uint commonId, JobId job, byte slotNo);

        // Abilities
        bool InsertLearnedAbility(uint commonId, Ability ability);
        bool UpdateLearnedAbility(uint commonId, Ability ability);
        bool InsertEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability);
        bool ReplaceEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability);
        bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities);
        bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equipptedToJob, byte slotNo, Ability ability);
        bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo);
        bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob);
        bool ReplaceAbilityPreset(uint characterId, CDataPresetAbilityParam preset);
        bool UpdateAbilityPreset(uint characterId, CDataPresetAbilityParam preset);

        bool InsertSecretAbilityUnlock(uint commonId, SecretAbility secretAbility);
        List<SecretAbility> SelectAllUnlockedSecretAbilities(uint commonId);

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
        bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut);
        bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null);
        bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataCommunicationShortCut updatedCommunicationShortcut);
        bool DeleteCommunicationShortcut(uint characterId, uint pageNo, uint buttonNo);

        // GameToken
        bool SetToken(GameToken token);
        GameToken SelectTokenByAccountId(int accountId);
        GameToken SelectToken(string tokenStr);
        bool DeleteTokenByAccountId(int accountId);
        bool DeleteToken(string token);

        // Connections
        bool InsertConnection(Connection connection);
        List<Connection> SelectConnectionsByAccountId(int accountId);
        bool DeleteConnection(int serverId, int accountId);
        bool DeleteConnectionsByAccountId(int accountId);
        bool DeleteConnectionsByServerId(int serverId);
        
        // ContactList
        int InsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status,
            ContactListType type, bool requesterFavorite, bool requestedFavorite);
        int UpdateContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status,
            ContactListType type, bool requesterFavorite, bool requestedFavorite);
        int DeleteContact(uint requestingCharacterId, uint requestedCharacterId);
        int DeleteContactById(uint id);
        List<ContactListEntity> SelectContactsByCharacterId(uint characterId);
        ContactListEntity SelectContactsByCharacterId(uint characterId1, uint characterId2);
        ContactListEntity SelectContactListById(uint id);

        // Dragon Force Augmentation
        bool InsertIfNotExistsDragonForceAugmentation(uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo);
        List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(uint commonId);
        bool InsertGainExtendParam(uint commonId, CDataOrbGainExtendParam Param);
        bool UpdateOrbGainExtendParam(uint commonId, CDataOrbGainExtendParam Param);
        CDataOrbGainExtendParam SelectOrbGainExtendParam(uint commonId);

        // Bazaar
        ulong InsertBazaarExhibition(BazaarExhibition exhibition);
        int UpdateBazaarExhibiton(BazaarExhibition exhibition);
        int DeleteBazaarExhibition(ulong bazaarId);
        BazaarExhibition SelectBazaarExhibitionByBazaarId(ulong bazaarId);
        List<BazaarExhibition> FetchCharacterBazaarExhibitions(uint characterId);
        List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(uint itemId, uint excludedCharacterId);
        List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(List<uint> itemIds, uint excludedCharacterId);

        // Rewards
        bool InsertBoxRewardItems(uint commonId, QuestBoxRewards rewards);
        bool DeleteBoxRewardItem(uint commonId, uint uniqId);
        List<QuestBoxRewards> SelectBoxRewardItems(uint commonId);

        // Completed Quests
        List<CompletedQuest> GetCompletedQuestsByType(uint characterCommonId, QuestType questType);
        CompletedQuest GetCompletedQuestsById(uint characterCommonId, QuestId questId);
        bool InsertIfNotExistCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType);

        // Quest Progress
        bool InsertQuestProgress(uint characterCommonId, QuestId questId, QuestType questType, uint step);
        bool UpdateQuestProgress(uint characterCommonId, QuestId questId, QuestType questType, uint step);
        bool RemoveQuestProgress(uint characterCommonId, QuestId questId, QuestType questType);
        List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType);
        QuestProgress GetQuestProgressById(uint characterCommonId, QuestId questId);

        // Quest Priority
        bool InsertPriorityQuest(uint characterCommonId, QuestId questId);
        List<QuestId> GetPriorityQuests(uint characterCommonId);
        bool DeletePriorityQuest(uint characterCommonId, QuestId questId);

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
        bool UpdateSystemMailAttachmentReceivedStatus(ulong messageId, ulong attachmentId, bool isReceived);
        bool DeleteSystemMailAttachment(ulong messageId);

        // Additional Status
        bool InsertIfNotExistsAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2);
        bool InsertAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2);
        List<CDataAddStatusParam> GetAddStatusByUID(string itemUid);
        bool UpdateAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2);

        // Play points
        bool ReplaceCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData);
        bool UpdateCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData);

        // Stamps
        public bool InsertCharacterStampData(uint id, CharacterStampBonus stampData);
        public bool UpdateCharacterStampData(uint id, CharacterStampBonus stampData);
        // Crests
        bool InsertCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? connectionIn = null);
        bool UpdateCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount);
        bool RemoveCrest(uint characterCommonId, string itemUId, uint slot);
        List<Crest> GetCrests(uint characterCommonId, string itemUId);
    }
}
