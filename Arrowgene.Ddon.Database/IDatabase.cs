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

        // Character
        bool CreateCharacter(Character character);
        bool UpdateCharacter(Character character);
        Character SelectCharacter(uint characterId);
        List<Character> SelectCharactersByAccountId(int accountId);
        bool DeleteCharacter(uint characterId);

        bool UpdateCharacterBaseInfo(Character character);

        // Equip
        bool InsertEquipItem(uint characterId, JobId job, EquipItem equipItem);
        bool UpdateEquipItem(uint characterId, JobId job, EquipItem equipItem);
        bool DeleteEquipItem(uint characterId, JobId job, EquipItem equipItem);

        // CustomSkills
        bool InsertEquippedCustomSkill(uint characterId, CustomSkill skill);
        bool ReplaceEquippedCustomSkill(uint characterId, CustomSkill skill);
        bool UpdateEquippedCustomSkill(uint characterId, JobId oldJob, byte oldSlotNo, CustomSkill skill);
        bool DeleteEquippedCustomSkill(uint characterId, JobId job, byte slotNo);

        // Abilities
        bool InsertEquippedAbility(uint characterId, Ability skill);
        bool ReplaceEquippedAbility(uint characterId, Ability skill);
        bool ReplaceEquippedAbilities(uint characterId, JobId equippedToJob, List<Ability> abilities);
        bool UpdateEquippedAbility(uint characterId, JobId oldEquippedToJob, byte oldSlotNo, Ability skill);
        bool DeleteEquippedAbility(uint characterId, JobId equippedToJob, byte slotNo);
        bool DeleteEquippedAbilities(uint characterId, JobId equippedToJob);

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
    }
}
