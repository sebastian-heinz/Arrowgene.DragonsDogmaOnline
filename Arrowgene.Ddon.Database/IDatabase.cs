using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database
{
    public interface IDatabase
    {
        void Execute(string sql);

        /// <summary>
        /// Return true if database was created, or false if not.
        /// </summary>
        bool CreateDatabase();

        // Account
        Account CreateAccount(string name, string mail, string hash);
        Account SelectAccountById(int accountId);
        Account SelectAccountByName(string accountName);
        Account SelectAccountByLoginToken(string loginToken);
        bool UpdateAccount(Account account);
        bool DeleteAccount(int accountId);

        // CharacterCommon
        public bool UpdateCharacterCommonBaseInfo(CharacterCommon common);
        bool UpdateEditInfo(CharacterCommon character);
        bool UpdateStatusInfo(CharacterCommon character);

        // Character
        bool CreateCharacter(Character character);
        Character SelectCharacter(uint characterId);
        List<Character> SelectCharactersByAccountId(int accountId);
        bool DeleteCharacter(uint characterId);
        bool UpdateCharacterBaseInfo(Character character);
        bool UpdateCharacterMatchingProfile(Character character);
        bool UpdateCharacterArisenProfile(Character character);

        // Pawn
        bool CreatePawn(Pawn pawn);
        Pawn SelectPawn(uint pawnId);
        List<Pawn> SelectPawnsByCharacterId(uint characterId);
        bool DeletePawn(uint pawnId);
        bool UpdatePawnBaseInfo(Pawn pawn);

        // CharacterJobData
        bool ReplaceCharacterJobData(uint commonId, CDataCharacterJobData replacedCharacterJobData);
        bool UpdateCharacterJobData(uint commonId, CDataCharacterJobData updatedCharacterJobData);

        // Wallet Points
        bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint);
        bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint);
        bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint);
        bool DeleteWalletPoint(uint characterId, WalletType type);

        // Released Warp Points
        List<ReleasedWarpPoint> SelectReleasedWarpPoints(uint characterId);
        bool InsertIfNotExistsReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
        bool InsertIfNotExistsReleasedWarpPoints(uint characterId, List<ReleasedWarpPoint> ReleasedWarpPoint);
        bool InsertReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
        bool ReplaceReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint);
        bool UpdateReleasedWarpPoint(uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint);
        bool DeleteReleasedWarpPoint(uint characterId, uint warpPointId);

        // Item
        bool InsertItem(Item item);
        Item SelectItem(string uid);

        //Storage
        bool InsertStorage(uint characterId, StorageType storageType, Storage storage);
        bool UpdateStorage(uint characterId, StorageType storageType, Storage storage);
        bool DeleteStorage(uint characterId, StorageType storageType);

        // Storage Item
        bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum);
        bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum);
        bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo);

        // Equip
        bool InsertEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
        bool ReplaceEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
        bool UpdateEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);
        bool DeleteEquipItem(uint commonId, JobId job, EquipType equipType, byte equipSlot, string itemUId);

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

        // (Learned) Normal Skills / Learned Core Skills
        bool InsertIfNotExistsNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
        bool InsertNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
        bool ReplaceNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam);
        bool UpdateNormalSkillParam(uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam);
        bool DeleteNormalSkillParam(uint commonId, JobId job, uint skillNo);
        List<CDataNormalSkillParam> SelectNormalSkillParam(uint commonId, JobId job);

        // Shortcut
        bool InsertShortcut(uint characterId, CDataShortCut shortcut);
        bool ReplaceShortcut(uint characterId, CDataShortCut shortcut);
        bool UpdateShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataShortCut updatedShortcut);
        bool DeleteShortcut(uint characterId, uint pageNo, uint buttonNo);

        // CommunicationShortcut
        bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut);
        bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut);
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
        int UpsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status,
            ContactListType type, bool requesterFavorite, bool requestedFavorite);
        int DeleteContact(uint requestingCharacterId, uint requestedCharacterId);
        int DeleteContactById(uint id);
        List<ContactListEntity> SelectFriends(uint characterId);
        ContactListEntity SelectRelationship(uint characterId1, uint characterId2);
        ContactListEntity SelectRelationship(uint id);
    }
}
