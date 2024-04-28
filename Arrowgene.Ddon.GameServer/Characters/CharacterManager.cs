using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CharacterManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterManager));

        public static readonly uint BASE_HEALTH = 760U;
        public static readonly uint BASE_STAMINA = 750U;
        public static readonly uint DEFAULT_RING_COUNT = 1;
        public static readonly uint BASE_ABILITY_COST_AMOUNT = 15;

        private readonly IDatabase _Database;

        public CharacterManager(IDatabase database) 
        {
            _Database = database;
        }

        public Character SelectCharacter(uint characterId)
        {
            Character character = _Database.SelectCharacter(characterId);
            if (character == null)
            {
                return null;
            }

            character.ExtendedParams = _Database.SelectOrbGainExtendParam(character.CommonId);
            if (character.ExtendedParams == null)
            {
                // Old DB is in use and new table not populated with required data for character
                Logger.Error($"Character: AccountId={character.AccountId}, CharacterId={character.CharacterId}, CommonId={character.CommonId} is missing table entry in 'ddon_orb_gain_extend_param'.");
                return null;
            }

            UpdateCharacterExtendedParams(character);

            return character;
        }

        public uint GetMaxAugmentAllocation(Character character)
        {
            return CharacterManager.BASE_ABILITY_COST_AMOUNT + character.ExtendedParams.AbilityCost;
        }

        private void UpdateCharacterExtendedParams(Character character)
        {
            var ExtendedParams = character.ExtendedParams;

            // There is always an implicit + 1 ring slot plus the extended params value
            character.JewelrySlotNum = (byte)(CharacterManager.DEFAULT_RING_COUNT + ExtendedParams.JewelrySlot);

            /**
             * There are two physical attack traits and two magic attack traits in
             * the stats menu. This corresponds with the attack of the weapon and
             * the iniate attack of the character on the currently selected job.
             * Similar distinction made with magic. The Gain* stats are extra stats
             * on top of the iniate stats. These come from armors and BO/HO trees.
             */
            character.StatusInfo.GainAttack = ExtendedParams.Attack;
            character.StatusInfo.GainDefense = ExtendedParams.Defence;
            character.StatusInfo.GainMagicAttack = ExtendedParams.MagicAttack;
            character.StatusInfo.GainMagicDefense = ExtendedParams.MagicDefence;
            character.StatusInfo.GainStamina = ExtendedParams.StaminaMax;
            character.StatusInfo.GainHP = ExtendedParams.HpMax;

            // These values reflect in the UI what the health bar displays
            // TODO: Remove this when C2SLobbyLobbyDataMsgReq parsing is supported
            // TODO: Server alone is not able to account for HP added by gear
            // TODO: So we need to "trust" the value sent to the server is correct
            character.StatusInfo.HP = CharacterManager.BASE_HEALTH + ExtendedParams.HpMax;
            character.StatusInfo.WhiteHP = CharacterManager.BASE_HEALTH + ExtendedParams.HpMax;
        }

        public void UpdateCharacterExtendedParamsNtc(GameClient client, Character character)
        {
            UpdateCharacterExtendedParams(character);
            NotifyClientOfCharacterStatus(client, character);
        }

        private void NotifyClientOfCharacterStatus(GameClient client, Character character)
        {
            S2CContextGetLobbyPlayerContextNtc ntc1 = new S2CContextGetLobbyPlayerContextNtc();
            GameStructure.S2CContextGetLobbyPlayerContextNtc(ntc1, character);
            client.Send(ntc1);

            S2CExtendEquipSlotNtc ntc2 = new S2CExtendEquipSlotNtc()
            {
                EquipSlot = EquipCategory.Jewelry,
                AddNum = 0,
                TotalNum = character.JewelrySlotNum
            };

            client.Send(ntc2);
        }
    }
}
