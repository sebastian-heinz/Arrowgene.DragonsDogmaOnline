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

namespace Arrowgene.Ddon.Database.Sql;

/// <summary>
///     Operations for SQL type databases.
/// </summary>
public abstract class SqlDb : IDatabase
{
    protected const int NoRowsAffected = 0;
    protected const long NoAutoIncrement = 0;

    protected virtual DbConnection ReusableConnection { get; set; }

    public virtual DbConnection OpenNewConnection()
    {
        throw new NotImplementedException("This is driver-dependent and must be implemented.");
    }

    /// <summary>
    ///     Reusing connections can usually only be done in special cases, depending on the database engine:
    ///     - One operation at a time.
    ///     - Not thread-safe.
    ///     If unsure, check DB engine connector documentation or prefer to use <see cref="OpenNewConnection" />.
    /// </summary>
    /// <returns>An opened, prior-existing connection</returns>
    public DbConnection OpenExistingConnection()
    {
        switch (ReusableConnection.State)
        {
            case ConnectionState.Closed:
                ReusableConnection.Open();
                break;
            case ConnectionState.Broken:
                ReusableConnection.Close();
                ReusableConnection.Open();
                break;
        }

        return ReusableConnection;
    }

    public bool ExecuteInTransaction(Action<DbConnection> action)
    {
        using DbConnection connection = OpenNewConnection();
        using DbTransaction transaction = connection.BeginTransaction();
        try
        {
            action(connection);
            transaction.Commit();
            return true;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            connection.Close();
        }
    }

    public int ExecuteNonQuery(DbConnection conn, string query, Action<DbCommand> nonQueryAction, bool rethrowException = false)
    {
        try
        {
            using DbCommand command = Command(query, conn);
            nonQueryAction(command);
            return command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            if (rethrowException)
            {
                throw;
            }
            Exception(ex, query);
            return NoRowsAffected;
        }
    }

    public void ExecuteReader(DbConnection conn, string query, Action<DbCommand> nonQueryAction, Action<DbDataReader> readAction, bool rethrowException = false)
    {
        try
        {
            using DbCommand command = Command(query, conn);
            nonQueryAction(command);
            using DbDataReader reader = command.ExecuteReader();
            readAction(reader);
        }
        catch (Exception ex)
        {
            if (rethrowException)
            {
                throw;
            }
            Exception(ex, query);
        }
    }

    public void Execute(string query, bool rethrowException = false)
    {
        using DbConnection connection = OpenNewConnection();
        try
        {
            Execute(connection, query, rethrowException);
        }
        finally
        {
            connection.Close();
        }
    }

    public abstract void ExecuteQuerySafe(DbConnection? connectionIn, Action<DbConnection> work);
    public abstract T ExecuteQuerySafe<T>(DbConnection? connectionIn, Func<DbConnection, T> work);

    public virtual void Stop()
    {
        throw new NotImplementedException("This is driver-dependent and must be implemented.");
    }


    public virtual bool CreateDatabase()
    {
        throw new NotImplementedException("This is driver-dependent and must be implemented.");
    }


    public void Execute(DbConnection? conn, string query, bool rethrowException = false)
    {
        try
        {
            using DbCommand command = Command(query, conn);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            if (rethrowException)
            {
                throw;
            }
            Exception(ex, query);
        }
    }

    public virtual void AddParameter(DbCommand command, string name, object? value, DbType type)
    {
        DbParameter parameter = Parameter(command, name, value, type);
        command.Parameters.Add(parameter);
    }

    public virtual void AddParameter(DbCommand command, string name, string value)
    {
        AddParameter(command, name, value, DbType.String);
    }

    public virtual void AddParameter(DbCommand command, string name, int value)
    {
        AddParameter(command, name, value, DbType.Int32);
    }

    public virtual void AddParameter(DbCommand command, string name, float value)
    {
        AddParameter(command, name, value, DbType.Double);
    }

    public virtual void AddParameter(DbCommand command, string name, byte value)
    {
        AddParameter(command, name, value, DbType.Byte);
    }

    public virtual void AddParameter(DbCommand command, string name, ushort value)
    {
        AddParameter(command, name, (short)value, DbType.Int16);
    }

    public virtual void AddParameter(DbCommand command, string name, uint value)
    {
        AddParameter(command, name, (int)value, DbType.Int32);
    }

    public virtual void AddParameter(DbCommand command, string name, byte[] value)
    {
        AddParameter(command, name, value, DbType.Binary);
    }

    public virtual void AddParameter(DbCommand command, string name, bool value)
    {
        AddParameter(command, name, value, DbType.Boolean);
    }

    public virtual string? GetStringNullable(DbDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;

        return reader.GetString(ordinal);
    }

    public virtual byte[]? GetBytesNullable(DbDataReader reader, int ordinal, int size)
    {
        if (reader.IsDBNull(ordinal)) return null;

        byte[] buffer = new byte[size];
        reader.GetBytes(ordinal, 0, buffer, 0, size);
        return buffer;
    }

    public virtual int GetInt32(DbDataReader reader, string column)
    {
        return reader.GetInt32(reader.GetOrdinal(column));
    }

    public virtual uint GetUInt32(DbDataReader reader, string column)
    {
        return (uint)reader.GetInt32(reader.GetOrdinal(column));
    }

    public virtual byte GetByte(DbDataReader reader, string column)
    {
        return reader.GetByte(reader.GetOrdinal(column));
    }

    public virtual short GetInt16(DbDataReader reader, string column)
    {
        return reader.GetInt16(reader.GetOrdinal(column));
    }

    public virtual ushort GetUInt16(DbDataReader reader, string column)
    {
        return (ushort)reader.GetInt16(reader.GetOrdinal(column));
    }

    public virtual long GetInt64(DbDataReader reader, string column)
    {
        return reader.GetInt64(reader.GetOrdinal(column));
    }

    public virtual ulong GetUInt64(DbDataReader reader, string column)
    {
        return (ulong)reader.GetInt64(reader.GetOrdinal(column));
    }

    public virtual float GetFloat(DbDataReader reader, string column)
    {
        return reader.GetFloat(reader.GetOrdinal(column));
    }

    public virtual string GetString(DbDataReader reader, string column)
    {
        return reader.GetString(reader.GetOrdinal(column));
    }

    public virtual bool GetBoolean(DbDataReader reader, string column)
    {
        return reader.GetBoolean(reader.GetOrdinal(column));
    }

    public virtual byte[] GetBytes(DbDataReader reader, string column, int size)
    {
        byte[] buffer = new byte[size];
        reader.GetBytes(reader.GetOrdinal(column), 0, buffer, 0, size);
        return buffer;
    }

    public bool MigrateDatabase(DatabaseMigrator migrator, uint toVersion)
    {
        uint currentVersion = GetMeta().DatabaseVersion;
            bool result = migrator.MigrateDatabase(this, currentVersion, toVersion);
            if (result)
            {
                SetMeta(new DatabaseMeta { DatabaseVersion = DdonDatabaseBuilder.Version });
                ExecuteNonQuery(OpenNewConnection(), "VACUUM;", _ => { }, out _, true);
                ExecuteNonQuery(OpenNewConnection(), "ANALYZE;", _ => { }, out _, true);
            }
            Stop();
            return result;
    }

    public abstract bool CreateMeta(DatabaseMeta meta);

    public abstract DatabaseMeta GetMeta();
    public abstract Account? CreateAccount(string name, string mail, string hash);
    public abstract Account SelectAccountById(int accountId);
    public abstract Account? SelectAccountByName(string accountName);
    public abstract Account? SelectAccountByLoginToken(string loginToken);
    public abstract bool UpdateAccount(Account account);
    public abstract bool DeleteAccount(int accountId);
    public abstract Storages SelectAllStoragesByCharacterId(uint characterId);
    public abstract bool UpdateCharacterCommonBaseInfo(CharacterCommon common, DbConnection? connectionIn = null);
    public abstract bool UpdateEditInfo(CharacterCommon character);
    public abstract bool UpdateStatusInfo(CharacterCommon character);
    public abstract bool CreateCharacter(Character character);
    public abstract Character SelectCharacter(uint characterId, DbConnection? connectionIn = null);
    public abstract List<Character> SelectCharactersByAccountId(int accountId, GameMode gameMode);
    public abstract List<Character> SelectAllCharacters();
    public abstract List<Character> SelectAllCharacters(DbConnection conn);
    public abstract bool DeleteCharacter(uint characterId);
    public abstract bool UpdateCharacterBaseInfo(Character character);
    public abstract bool UpdateCharacterMatchingProfile(Character character);
    public abstract bool UpdateCharacterArisenProfile(Character character);
    public abstract bool UpdateMyPawnSlot(uint characterId, uint num, DbConnection? connectionIn = null);
    public abstract bool UpdateRentalPawnSlot(uint characterId, uint num, DbConnection? connectionIn = null);
    public abstract bool UpdateCharacterBinaryData(uint characterId, byte[] data);
    public abstract void CreateItems(DbConnection conn, Character character);
    public abstract void CreateListItems(DbConnection conn, Character character, StorageType storageType, List<(uint ItemId, uint Amount)> itemList);
    public abstract CDataCharacterSearchParam SelectCharacterNameById(uint characterId);
    public abstract CDataCharacterSearchParam SelectCharacterNameById(DbConnection connection, uint characterId);
    public abstract bool CreatePawn(Pawn pawn);
    public abstract Pawn SelectPawn(uint pawnId);
    public abstract Pawn SelectPawn(DbConnection connection, uint pawnId);
    public abstract List<Pawn> SelectPawnsByCharacterId(uint characterId, DbConnection? connectionIn = null);
    public abstract List<uint> SelectOfficialPawns();
    public abstract List<uint> SelectAllPlayerPawns(uint limit = 100);
    public abstract List<uint> SelectAllPlayerPawns(DbConnection connection, uint limit = 100);
    public abstract List<uint> SelectClanPawns(uint clanId, uint characterId = 0, uint limit = 100, DbConnection? connectionIn = null);
    public abstract List<CDataRegisterdPawnList> SelectRegisteredPawns(Character searchingCharacter, CDataPawnSearchParameter searchParams);
    public abstract List<CDataRegisterdPawnList> SelectRegisteredPawns(DbConnection conn, Character searchingCharacter, CDataPawnSearchParameter searchParams);
    public abstract bool DeletePawn(uint pawnId, DbConnection? connectionIn = null);
    public abstract bool UpdatePawnBaseInfo(Pawn pawn, DbConnection? connectionIn = null);
    public abstract uint GetPawnOwnerCharacterId(uint pawnId, DbConnection? connectionIn = null);
    public abstract bool ReplacePawnReaction(uint pawnId, CDataPawnReaction pawnReaction, DbConnection? connectionIn = null);
    public abstract bool ReplacePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus, DbConnection? connectionIn = null);
    public abstract bool InsertPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
    public abstract bool InsertIfNotExistsPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
    public abstract bool UpdatePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus);
    public abstract bool ReplacePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null);
    public abstract bool InsertPawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null);
    public abstract bool UpdatePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null);
    public abstract bool DeletePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null);
    public abstract CraftProgress? SelectPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null);
    public abstract bool InsertSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill);
    public abstract bool DeleteSpSkill(uint pawnId, JobId job, byte spSkillId);
    public abstract bool ReplaceCharacterJobData(uint commonId, CDataCharacterJobData replacedCharacterJobData, DbConnection? connectionIn = null);
    public abstract bool UpdateCharacterJobData(uint commonId, CDataCharacterJobData updatedCharacterJobData, DbConnection? connectionIn = null);
    public abstract bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint);
    public abstract bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint);
    public abstract bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint, DbConnection? connectionIn = null);
    public abstract bool DeleteWalletPoint(uint characterId, WalletType type);
    public abstract List<ReleasedWarpPoint> SelectReleasedWarpPoints(uint characterId);
    public abstract bool InsertIfNotExistsReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
    public abstract bool InsertIfNotExistsReleasedWarpPoints(uint characterId, List<ReleasedWarpPoint> ReleasedWarpPoint);
    public abstract bool InsertReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
    public abstract bool ReplaceReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
    public abstract bool UpdateReleasedWarpPoint(uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint);
    public abstract bool DeleteReleasedWarpPoint(uint characterId, uint warpPointId);
    public abstract Item SelectStorageItemByUId(string uId, DbConnection? connectionIn = null);
    public abstract bool InsertStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn);
    public abstract bool UpdateStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn);
    public abstract bool DeleteStorage(uint characterId, StorageType storageType, DbConnection? connectionIn = null);
    public abstract bool ReplaceStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null);
    public abstract bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);
    public abstract bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);
    public abstract bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo, DbConnection? connectionIn = null);
    public abstract bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null);
    public abstract void DeleteAllStorageItems(DbConnection connection, uint characterId);
    public abstract bool UpdateItemEquipPoints(string itemUID, uint equipPoints, DbConnection? connectionIn = null);
    public abstract bool InsertEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
    public abstract bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId, DbConnection? connectionIn = null);
    public abstract bool UpdateEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
    public abstract bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, DbConnection? connectionIn = null);
    public abstract void DeleteAllEquipItems(uint commonId, DbConnection? connectionIn = null);
    public abstract List<EquipItem> SelectEquipItemByCharacter(uint characterCommonId);
    public abstract bool InsertEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo);
    public abstract bool ReplaceEquipJobItem(string itemUId, uint commonId, JobId job, ushort slotNo);
    public abstract bool DeleteEquipJobItem(uint commonId, JobId job, ushort slotNo);
    public abstract bool InsertLearnedCustomSkill(uint commonId, CustomSkill skill, DbConnection? connectionIn = null);
    public abstract bool UpdateLearnedCustomSkill(uint commonId, CustomSkill updatedSkill, DbConnection? connectionIn = null);
    public abstract bool InsertEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill);
    public abstract bool ReplaceEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill);
    public abstract bool UpdateEquippedCustomSkill(uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill skill);
    public abstract bool DeleteEquippedCustomSkill(uint commonId, JobId job, byte slotNo);
    public abstract bool InsertLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null);
    public abstract bool UpdateLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null);
    public abstract bool InsertEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability);
    public abstract bool ReplaceEquippedAbility(uint commonId, JobId equipptedToJob, byte slotNo, Ability ability);
    public abstract bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities);
    public abstract bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equipptedToJob, byte slotNo, Ability ability);
    public abstract bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo);
    public abstract bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob);
    public abstract bool ReplaceAbilityPreset(uint characterId, CDataPresetAbilityParam preset);
    public abstract bool UpdateAbilityPreset(uint characterId, CDataPresetAbilityParam preset);
    public abstract bool InsertSecretAbilityUnlock(uint commonId, AbilityId secretAbility, DbConnection? connectionIn = null);
    public abstract List<AbilityId> SelectAllUnlockedSecretAbilities(uint commonId);
    public abstract bool InsertIfNotExistsNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
    public abstract bool InsertNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
    public abstract bool ReplaceNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
    public abstract bool UpdateNormalSkillParam(uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam);
    public abstract bool DeleteNormalSkillParam(uint commonId, JobId job, uint skillNo);
    public abstract List<CDataNormalSkillParam> SelectNormalSkillParam(uint commonId, JobId job);
    public abstract bool InsertShortcut(uint characterId, CDataShortCut shortcut);
    public abstract bool ReplaceShortcut(uint characterId, CDataShortCut shortcut, DbConnection? connectionIn = null);
    public abstract bool UpdateShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataShortCut updatedShortcut);
    public abstract bool DeleteShortcut(uint characterId, uint pageNo, uint buttonNo);
    public abstract bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null);
    public abstract bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null);
    public abstract bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataCommunicationShortCut updatedCommunicationShortcut, DbConnection? connectionIn = null);
    public abstract bool DeleteCommunicationShortcut(uint characterId, uint pageNo, uint buttonNo);
    public abstract bool SetToken(GameToken token);
    public abstract GameToken SelectTokenByAccountId(int accountId);
    public abstract GameToken SelectToken(string tokenStr);
    public abstract bool DeleteTokenByAccountId(int accountId);
    public abstract bool DeleteToken(string token);
    public abstract bool InsertConnection(Connection connection);
    public abstract List<Connection> SelectConnections();
    public abstract List<Connection> SelectConnectionsByAccountId(int accountId);
    public abstract bool DeleteConnection(int serverId, int accountId);
    public abstract bool DeleteConnectionsByAccountId(int accountId);
    public abstract bool DeleteConnectionsByServerId(int serverId);

    public abstract int InsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite,
        bool requestedFavorite);

    public abstract int UpdateContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite,
        bool requestedFavorite);

    public abstract int DeleteContact(uint requestingCharacterId, uint requestedCharacterId);
    public abstract int DeleteContactById(uint id);
    public abstract List<ContactListEntity> SelectContactsByCharacterId(uint characterId);
    public abstract ContactListEntity SelectContactsByCharacterId(uint characterId1, uint characterId2);
    public abstract ContactListEntity SelectContactListById(uint id);
    public abstract List<(ContactListEntity, CDataCharacterListElement)> SelectFullContactListByCharacterId(uint characterId, DbConnection? connectionIn = null);
    public abstract bool InsertIfNotExistsDragonForceAugmentation(uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo, DbConnection? connectionIn = null);
    public abstract List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(uint commonId, DbConnection? connectionIn = null);
    public abstract bool InsertGainExtendParam(uint commonId, CDataOrbGainExtendParam Param);
    public abstract bool UpdateOrbGainExtendParam(uint commonId, CDataOrbGainExtendParam param, DbConnection? connectionIn = null);
    public abstract CDataOrbGainExtendParam SelectOrbGainExtendParam(uint commonId, DbConnection? connectionIn = null);
    public abstract ulong InsertBazaarExhibition(BazaarExhibition exhibition);
    public abstract int UpdateBazaarExhibiton(BazaarExhibition exhibition);
    public abstract int DeleteBazaarExhibition(ulong bazaarId);
    public abstract BazaarExhibition SelectBazaarExhibitionByBazaarId(ulong bazaarId);
    public abstract List<BazaarExhibition> FetchCharacterBazaarExhibitions(uint characterId);
    public abstract List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdExcludingOwn(uint itemId, uint excludedCharacterId, DbConnection? connectionIn = null);
    public abstract List<BazaarExhibition> SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(List<uint> itemIds, uint excludedCharacterId, DbConnection? connectionIn = null);
    public abstract bool InsertBoxRewardItems(uint commonId, QuestBoxRewards rewards, DbConnection? connectionIn = null);
    public abstract bool DeleteBoxRewardItem(uint commonId, uint uniqId, DbConnection? connectionIn = null);
    public abstract List<QuestBoxRewards> SelectBoxRewardItems(uint commonId, DbConnection? connectionIn = null);
    public abstract List<CompletedQuest> GetCompletedQuestsByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null);
    public abstract CompletedQuest GetCompletedQuestsById(uint characterCommonId, QuestId questId, DbConnection? connectionIn = null);
    public abstract bool InsertCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, DbConnection? connectionIn = null);
    public abstract bool ReplaceCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null);
    public abstract bool InsertQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null);
    public abstract bool UpdateQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null);
    public abstract bool RemoveQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, DbConnection? connectionIn = null);
    public abstract List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null);
    public abstract QuestProgress GetQuestProgressByScheduleId(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null);
    public abstract bool InsertPriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null);
    public abstract List<uint> GetPriorityQuestScheduleIds(uint characterCommonId, DbConnection? connectionIn = null);
    public abstract bool DeletePriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null);
    public abstract long InsertSystemMailAttachment(SystemMailAttachment attachment);
    public abstract long InsertSystemMailAttachment(DbConnection connection, SystemMailAttachment attachment);
    public abstract long InsertSystemMailMessage(SystemMailMessage message);
    public abstract long InsertSystemMailMessage(DbConnection connection, SystemMailMessage message);
    public abstract List<SystemMailMessage> SelectSystemMailMessages(uint characterId);
    public abstract SystemMailMessage SelectSystemMailMessage(ulong messageId);
    public abstract bool UpdateSystemMailMessageState(ulong messageId, MailState messageState);
    public abstract bool DeleteSystemMailMessage(ulong messageId);
    public abstract List<SystemMailAttachment> SelectAttachmentsForSystemMail(ulong messageId);
    public abstract bool UpdateSystemMailAttachmentReceivedStatus(ulong messageId, ulong attachmentId, bool isReceived);
    public abstract bool DeleteSystemMailAttachment(ulong messageId);
    public abstract bool InsertIfNotExistsAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2);
    public abstract bool InsertAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2);
    public abstract List<CDataAddStatusParam> GetAddStatusByUID(string itemUid);
    public abstract bool UpdateAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2);
    public abstract bool ReplaceCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData, DbConnection? connectionIn = null);
    public abstract bool UpdateCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData, DbConnection? connectionIn = null);
    public abstract bool InsertCharacterStampData(uint id, CharacterStampBonus stampData);
    public abstract bool UpdateCharacterStampData(uint id, CharacterStampBonus stampData);
    public abstract bool InsertCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? connectionIn = null);
    public abstract bool UpdateCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? ConnectionIn = null);
    public abstract bool RemoveCrest(uint characterCommonId, string itemUId, uint slot, DbConnection? connectionIn = null);
    public abstract List<Crest> GetCrests(uint characterCommonId, string itemUId);
    public abstract bool InsertBBMCharacterId(uint characterId, uint bbmCharacterId);
    public abstract uint SelectBBMCharacterId(uint characterId, DbConnection? connectionIn = null);
    public abstract uint SelectBBMNormalCharacterId(uint bbmCharacterId);
    public abstract bool InsertBBMProgress(uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime);
    public abstract bool UpdateBBMProgress(uint characterId, BitterblackMazeProgress progress, DbConnection? connectionIn = null);
    public abstract BitterblackMazeProgress SelectBBMProgress(uint characterId);
    public abstract bool RemoveBBMProgress(uint characterId);
    public abstract bool InsertBBMRewards(uint characterId, uint goldMarks, uint silverMarks, uint redMarks);
    public abstract bool UpdateBBMRewards(uint characterId, BitterblackMazeRewards rewards, DbConnection? connectionIn = null);
    public abstract bool RemoveBBMRewards(uint characterId);
    public abstract BitterblackMazeRewards SelectBBMRewards(uint characterId, DbConnection? connectionIn = null);
    public abstract bool InsertBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure, DbConnection? connectionIn = null);
    public abstract bool InsertBBMContentTreasure(uint characterId, uint contentId, uint amount, DbConnection? connectionIn = null);
    public abstract bool UpdateBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure);
    public abstract bool UpdateBBMContentTreasure(uint characterId, uint contentId, uint amount);
    public abstract bool RemoveBBMContentTreasure(uint characterId);
    public abstract List<BitterblackMazeTreasure> SelectBBMContentTreasure(uint characterId, DbConnection? connectionIn = null);
    public abstract bool CreateClan(CDataClanParam clanParam);
    public abstract bool DeleteClan(CDataClanParam clan, DbConnection? connectionIn = null);
    public abstract uint SelectClanMembershipByCharacterId(uint characterId, DbConnection? connectionIn = null);
    public abstract ClanName GetClanNameByClanId(uint clanId, DbConnection? connectionIn = null);
    public abstract CDataClanParam SelectClan(uint clanId, DbConnection? connectionIn = null);
    public abstract List<CDataClanSearchResult> SearchClans(CDataClanSearchParam search, DbConnection? connectionIn = null);
    public abstract bool UpdateClan(CDataClanParam clan, DbConnection? connectionIn = null);
    public abstract bool InsertClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null);
    public abstract bool DeleteClanMember(uint characterId, uint clanId, DbConnection? connectionIn = null);
    public abstract List<CDataClanMemberInfo> GetClanMemberList(uint clanId, DbConnection? connectionIn = null);
    public abstract CDataClanMemberInfo GetClanMember(uint characterId, DbConnection? connectionIn = null);
    public abstract bool UpdateClanMember(CDataClanMemberInfo memberInfo, uint clanId, DbConnection? connectionIn = null);
    public abstract List<uint> SelectClanShopPurchases(uint clanId, DbConnection? connectionIn = null);
    public abstract bool InsertClanShopPurchase(uint clanId, uint lineupId, DbConnection? connectionIn = null);
    public abstract List<(byte Type, uint Id)> SelectClanBaseCustomizations(uint clanId, DbConnection? connectionIn = null);
    public abstract bool InsertOrUpdateClanBaseCustomization(uint clanId, byte type, uint furnitureId, DbConnection? connectionIn = null);
    public abstract bool DeleteClanBaseCustomization(uint clanId, byte type, DbConnection? connectionIn = null);
    public abstract bool InsertEpitaphRoadUnlock(uint characterId, uint epitaphId, DbConnection? connectionIn = null);
    public abstract HashSet<uint> GetEpitaphRoadUnlocks(uint characterId, DbConnection? connectionIn = null);
    public abstract bool InsertEpitaphWeeklyReward(uint characterId, uint epitaphId, DbConnection? connectionIn = null);
    public abstract HashSet<uint> GetEpitaphClaimedWeeklyRewards(uint characterId, DbConnection? connectionIn = null);
    public abstract void DeleteWeeklyEpitaphClaimedRewards(DbConnection? connectionIn = null);
    public abstract Dictionary<TaskType, SchedulerTaskEntry> SelectAllTaskEntries();
    public abstract bool UpdateScheduleInfo(TaskType type, long timestamp);
    public abstract bool InsertAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null);
    public abstract bool UpdateAreaRank(uint characterId, AreaRank areaRank, DbConnection? connectionIn = null);
    public abstract Dictionary<QuestAreaId, AreaRank> SelectAreaRank(uint characterId, DbConnection? connectionIn = null);
    public abstract List<(uint CharacterId, AreaRank Rank)> SelectAllAreaRank(DbConnection? connectionIn = null);
    public abstract bool ResetAreaRankPoint(DbConnection? connectionIn = null);
    public abstract bool InsertAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null);
    public abstract bool UpdateAreaRankSupply(uint characterId, QuestAreaId areaId, uint index, uint itemId, uint num, DbConnection? connectionIn = null);
    public abstract Dictionary<QuestAreaId, List<CDataRewardItemInfo>> SelectAreaRankSupply(uint characterId, DbConnection? connectionIn = null);
    public abstract List<CDataRewardItemInfo> SelectAreaRankSupply(uint characterId, QuestAreaId areaId, DbConnection? connectionIn = null);
    public abstract bool DeleteAreaRankSupply(DbConnection? connectionIn = null);
    public abstract bool InsertRankRecord(uint characterId, uint questId, long score, DbConnection? connectionIn = null);
    public abstract List<uint> SelectUsedRankingBoardQuests(DbConnection? connectionIn = null);
    public abstract List<CDataRankingData> SelectRankingDataByCharacterId(uint characterId, uint questId, uint limit = 1000, DbConnection? connectionIn = null);
    public abstract List<CDataRankingData> SelectRankingData(uint questId, uint limit = 1000, DbConnection? connectionIn = null);
    public abstract bool DeleteAllRankRecords(DbConnection? connectionIn = null);
    public abstract bool InsertPartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null);
    public abstract bool UpdatePartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null);
    public abstract PartnerPawnData GetPartnerPawnRecord(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    public abstract bool SetPartnerPawn(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    public abstract bool InsertPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null);
    public abstract bool HasPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null);
    public abstract void DeleteAllPartnerPawnLastAffectionIncreaseRecords(DbConnection? connectionIn = null);
    public abstract HashSet<uint> GetPartnerPawnPendingRewards(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    public abstract bool InsertPartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null);
    public abstract void DeletePartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null);
    public abstract bool InsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null);
    public abstract bool UpdateRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null);
    public abstract bool HasRecycleEquipmentRecord(uint characterId, DbConnection? connectionIn = null);
    public abstract byte GetRecycleEquipmentAttempts(uint characterId, DbConnection? connectionIn = null);
    public abstract void ResetRecyleEquipmentRecords(DbConnection? connectionIn = null);
    public abstract bool UpsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null);
    public abstract bool InsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null);
    public abstract bool UpdateEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null);
    public abstract bool HasEquipmentLimitBreakRecord(uint characterId, string itemUID, DbConnection? connectionIn = null);
    public abstract bool UpsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null);
    public abstract List<CDataAddStatusParam> GetEquipmentLimitBreakRecord(string itemUID, DbConnection? connectionIn = null);
    public abstract Dictionary<(AchievementType, uint), uint> SelectAchievementProgress(uint characterId, DbConnection? connectionIn = null);
    public abstract bool UpsertAchievementProgress(uint characterId, AchievementType achievementType, uint achievementParam, uint progress, DbConnection? connectionIn = null);
    public abstract Dictionary<uint, DateTimeOffset> SelectAchievementStatus(uint characterId, DbConnection? connectionIn = null);
    public abstract bool InsertAchievementStatus(uint characterId, AchievementAsset achievement, bool reward = false, DbConnection? connectionIn = null);
    public abstract Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> SelectAchievementUniqueCrafts(uint characterId, DbConnection? connectionIn = null);
    public abstract bool InsertAchievementUniqueCraft(uint characterId, AchievementCraftTypeParam craftType, ItemId itemId, DbConnection? connectionIn = null);
    public abstract HashSet<(UnlockableItemCategory Category, uint Id)> SelectUnlockedItems(uint characterId, DbConnection? connectionIn = null);
    public abstract bool InsertUnlockedItem(uint characterId, UnlockableItemCategory type, uint itemId, DbConnection? connectionIn = null);
    public abstract Dictionary<ItemId, byte> SelectMyRoomCustomization(uint characterId, DbConnection? connectionIn = null);
    public abstract bool UpsertMyRoomCustomization(uint characterId, byte layoutId, uint itemId, DbConnection? connectionIn = null);
    public abstract bool DeleteMyRoomCustomization(uint characterId, uint itemId, DbConnection? connectionIn = null);
    public abstract bool InsertJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null);
    public abstract bool HasJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null);
    public abstract CDataReleaseElement GetJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null);
    public abstract List<CDataReleaseElement> GetJobMasterReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null);
    public abstract bool InsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    public abstract bool UpdateJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    public abstract bool HasJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    public abstract bool UpsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    public abstract CDataActiveJobOrder GetJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);
    public abstract List<CDataActiveJobOrder> GetJobMasterActiveOrders(uint characterId, JobId jobId, DbConnection? connectionIn = null);
    public abstract bool DeleteJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null);

    public abstract bool InsertJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId,
        CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    public abstract bool UpdateJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId,
        CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    public abstract bool HasJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    public abstract bool UpsertJobMasterActiveOrdersProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId,
        CDataJobOrderProgress jobOrderProgress,
        DbConnection? connectionIn = null);

    public abstract List<CDataJobOrderProgress> GetJobMasterActiveOrderProgress(uint characterId, JobId jobId, ReleaseType releaseType, uint releaseId,
        DbConnection? connectionIn = null);

    public abstract bool SetMeta(DatabaseMeta meta);

    public abstract bool UpdateCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null);

    public abstract bool InsertSkillAugmentationReleasedElement(uint characterId, JobId jobId, uint releaseId, DbConnection? connectionIn = null);
    public abstract HashSet<uint> GetSkillAugmentationReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null);

    public abstract bool UpsertJobEmblemData(uint characterId, JobEmblem jobEmblem, DbConnection? connectionIn = null);
    public abstract JobEmblem GetJobEmblemData(uint characterId, JobId jobId, DbConnection? connectionIn = null);
    public abstract List<JobEmblem> GetAllJobEmblemData(uint characterId, DbConnection? connectionIn = null);

    public abstract bool InsertPawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    public abstract bool DeletePawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null);
    public abstract HashSet<uint> GetPawnFavorites(uint characterId, DbConnection? connectionIn = null);

    protected virtual DbCommand Command(string query, DbConnection connection)
    {
        throw new NotImplementedException("This is driver-dependent and must be implemented.");
    }

    protected virtual long AutoIncrement(DbConnection connection, DbCommand command)
    {
        throw new NotImplementedException("This is driver-dependent and must be implemented.");
    }

    public int ExecuteNonQuery(string query, Action<DbCommand> nonQueryAction, bool rethrowException = false)
    {
        using DbConnection connection = OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, query, nonQueryAction, rethrowException);
        }
        finally
        {
            connection.Close();
        }
    }

    public int ExecuteNonQuery(string query, Action<DbCommand> nonQueryAction, out long autoIncrement, bool rethrowException = false)
    {
        using DbConnection connection = OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, query, nonQueryAction, out autoIncrement, rethrowException);
        }
        finally
        {
            connection.Close();
        }
    }

    public int ExecuteNonQuery(DbConnection conn, string query, Action<DbCommand> nonQueryAction, out long autoIncrement, bool rethrowException = false)
    {
        try
        {
            using DbCommand command = Command(query, conn);
            nonQueryAction(command);
            int rowsAffected = command.ExecuteNonQuery();
            autoIncrement = AutoIncrement(conn, command);
            return rowsAffected;
        }
        catch (Exception ex)
        {
            if (rethrowException)
            {
                throw;
            }
            Exception(ex, query);
            autoIncrement = NoAutoIncrement;
            return NoRowsAffected;
        }
    }

    public void ExecuteReader(string query, Action<DbCommand> nonQueryAction, Action<DbDataReader> readAction, bool rethrowException = false)
    {
        using DbConnection connection = OpenNewConnection();
        try
        {
            ExecuteReader(connection, query, nonQueryAction, readAction, rethrowException);
        }
        finally
        {
            connection.Close();
        }
    }

    public void ExecuteReader(string query, Action<DbDataReader> readAction, bool rethrowException = false)
    {
        using DbConnection connection = OpenNewConnection();
        try
        {
            ExecuteReader(connection, query, readAction, rethrowException);
        }
        finally
        {
            connection.Close();
        }
    }

    public void ExecuteReader(DbConnection conn, string query, Action<DbDataReader> readAction, bool rethrowException = false)
    {
        try
        {
            using DbCommand command = Command(query, conn);
            using DbDataReader reader = command.ExecuteReader();
            readAction(reader);
        }
        catch (Exception ex)
        {
            if (rethrowException)
            {
                throw;
            }
            Exception(ex, query);
        }
    }

    public string ServerVersion()
    {
        using DbConnection connection = OpenNewConnection();
        try
        {
            return ServerVersion(connection);
        }
        finally
        {
            connection.Close();
        }
    }

    public string ServerVersion(DbConnection conn)
    {
        try
        {
            return conn.ServerVersion;
        }
        catch (Exception ex)
        {
            Exception(ex);
            return string.Empty;
        }
    }

    protected abstract void Exception(Exception ex, string? query = null);

    protected virtual DbParameter Parameter(DbCommand command, string name, object? value, DbType type)
    {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        parameter.DbType = type;
        return parameter;
    }

    public virtual void AddParameter(DbCommand command, string name, long value)
    {
        AddParameter(command, name, value, DbType.Int64);
    }

    public virtual void AddParameter(DbCommand command, string name, ulong value)
    {
        AddParameter(command, name, (long)value, DbType.Int64);
    }

    public virtual void AddParameterEnumInt32<T>(DbCommand command, string name, T value) where T : Enum
    {
        AddParameter(command, name, (int)(object)value, DbType.Int32);
    }

    public virtual void AddParameter(DbCommand command, string name, DateTime? value)
    {
        AddParameter(command, name, value, DbType.DateTime);
    }

    public virtual void AddParameter(DbCommand command, string name, DateTime value)
    {
        AddParameter(command, name, value, DbType.DateTime);
    }

    public virtual DateTime? GetDateTimeNullable(DbDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;

        return reader.GetDateTime(ordinal);
    }

    public virtual T GetEnumInt32<T>(DbDataReader reader, string column) where T : Enum
    {
        return (T)(object)reader.GetInt32(reader.GetOrdinal(column));
    }

    public virtual DateTime GetDateTime(DbDataReader reader, string column)
    {
        return reader.GetDateTime(reader.GetOrdinal(column));
    }

    public virtual DateTime? GetDateTimeNullable(DbDataReader reader, string column)
    {
        int ordinal = reader.GetOrdinal(column);
        return GetDateTimeNullable(reader, ordinal);
    }

    public virtual string? GetStringNullable(DbDataReader reader, string column)
    {
        int ordinal = reader.GetOrdinal(column);
        return GetStringNullable(reader, ordinal);
    }

    public virtual byte[]? GetBytesNullable(DbDataReader reader, string column, int size)
    {
        int ordinal = reader.GetOrdinal(column);
        return GetBytesNullable(reader, ordinal, size);
    }
}
