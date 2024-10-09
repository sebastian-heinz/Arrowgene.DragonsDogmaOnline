using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CharacterManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterManager));

        public static readonly uint BASE_HEALTH = 760U;
        public static readonly uint BASE_STAMINA = 450U;
        public static readonly uint BBM_BASE_HEALTH = 990U;
        public static readonly uint BBM_BASE_STAMINA = 589U;

        public static readonly uint DEFAULT_RING_COUNT = 1;
        public static readonly uint BASE_ABILITY_COST_AMOUNT = 15;

        public static readonly uint MAX_PLAYER_HP = uint.MaxValue;
        public static readonly uint MAX_PLAYER_STAMINA = uint.MaxValue;

        private readonly DdonGameServer _Server;

        public CharacterManager(DdonGameServer server)
        {
            _Server = server;
        }

        public Character SelectCharacter(GameClient client, uint characterId)
        {
            Character character = SelectCharacter(characterId);
            client.Character = character;
            client.UpdateIdentity();

            return character;
        }

        public Character SelectCharacter(uint characterId)
        {
            Character character = _Server.Database.SelectCharacter(characterId);
            if (character == null)
            {
                return null;
            }

            character.Server = _Server.AssetRepository.ServerList.Where(server => server.Id == _Server.Id).Single();
            character.Equipment = character.Storage.GetCharacterEquipment();

            character.ExtendedParams = _Server.Database.SelectOrbGainExtendParam(character.CommonId);
            if (character.ExtendedParams == null)
            {
                // Old DB is in use and new table not populated with required data for character
                Logger.Error($"Character: AccountId={character.AccountId}, CharacterId={character.CharacterId}, CommonId={character.CommonId} is missing table entry in 'ddon_orb_gain_extend_param'.");
                return null;
            }

            UpdateCharacterExtendedParams(character);

            SelectPawns(character);

            return character;
        }

        private void SelectPawns(Character character)
        {
            character.Pawns = _Server.Database.SelectPawnsByCharacterId(character.ContentCharacterId);

            for (int i = 0; i < character.Pawns.Count; i++)
            {
                Pawn pawn = character.Pawns[i];
                pawn.Server = character.Server;
                pawn.Equipment = character.Storage.GetPawnEquipment(i);
                pawn.ExtendedParams = _Server.Database.SelectOrbGainExtendParam(pawn.CommonId);
                if (pawn.ExtendedParams == null)
                {
                    // Old DB is in use and new table not populated with required data for character
                    Logger.Error($"Character: AccountId={character.AccountId}, CharacterId={character.ContentCharacterId}, CommonId={character.CommonId}, PawnCommonId={pawn.CommonId} is missing table entry in 'ddon_orb_gain_extend_param'.");
                }
                UpdateCharacterExtendedParams(pawn);
            }
        }

        public void UpdateOnlineStatus(GameClient client, Character character, OnlineStatus onlineStatus)
        {
            client.Character.OnlineStatus = onlineStatus;
            var charUpdateNtc = new S2CCharacterCommunityCharacterStatusUpdateNtc();
            charUpdateNtc.UpdateCharacterList.Add(ContactListManager.CharacterToListEml(client.Character));
            charUpdateNtc.UpdateMatchingProfileList.Add(new CDataUpdateMatchingProfileInfo()
            {
                CharacterId = client.Character.CharacterId,
                Comment = client.Character.MatchingProfile.Comment,
            });

            // TODO: Is there a reduced set of clients we can send this to?
            foreach (var memberClient in _Server.ClientLookup.GetAll())
            {
                memberClient.Send(charUpdateNtc);
            }
        }

        public uint GetMaxAugmentAllocation(CharacterCommon character)
        {
            return CharacterManager.BASE_ABILITY_COST_AMOUNT + character.ExtendedParams.AbilityCost;
        }

        public void UpdateCharacterExtendedParams(CharacterCommon character, bool newCharacter = false)
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

            /**
             * Seems when the game first loads, the game wants MaxHP to always be 760
             * and MaxStamina to be 450. Then it takes the values from the GainHp and
             * GainStamina and add them to the Max values. Finally it seems to take
             * the stats from the armor/accessories and add them to the running total
             * for each stat, resulting the stats you see in game.
             *
             * Later on when upgrading health at the dragon, if we leave these as
             * the default, the health will get adjusted back down. One thing we
             * can take advantage of is that if we set the character HP > MaxHP,
             * it will only fill up to max HP. This will allow us to refill the
             * health of the player when they upgrade with BO or in other
             * scenarios where this may be required. The same trick also works
             * for stamina.
             */
            if (character.StatusInfo.MaxHP != 0 || newCharacter)
            {
                character.StatusInfo.HP = CharacterManager.MAX_PLAYER_HP;
                character.StatusInfo.WhiteHP = CharacterManager.MAX_PLAYER_HP;
            }
            character.StatusInfo.MaxHP = CharacterManager.BASE_HEALTH;

            if (character.StatusInfo.MaxStamina != 0 || newCharacter)
            {
                character.StatusInfo.Stamina = CharacterManager.MAX_PLAYER_STAMINA;
            }
            character.StatusInfo.MaxStamina = CharacterManager.BASE_STAMINA;
        }

        public void UpdateDatabaseOnExit(Character character)
        {
            // When the character is first logging in, the HP
            // values are set to 0. If the player disconnects
            // before fully logging in, this handler will save
            // a value of 0 HP into the database. The next time
            // the player logs in, they will have no health causing
            // the game to function improperly.
            if (character.GreenHp == 0 || character.WhiteHp == 0)
            {
                return;
            }

            _Server.Database.UpdateStatusInfo(character);

            foreach (var pawn in character.Pawns)
            {
                // Reset pawn HP to base max so next time we log in they are at full health
                pawn.GreenHp = CharacterManager.BASE_HEALTH;
                pawn.WhiteHp = CharacterManager.BASE_HEALTH;

                _Server.Database.UpdateStatusInfo(pawn);
            }
        }

        public void UpdateCharacterExtendedParamsNtc(GameClient client, CharacterCommon character)
        {
            UpdateCharacterExtendedParams(character);
            NotifyClientOfCharacterStatus(client, character);
        }

        private void NotifyClientOfCharacterStatus(GameClient client, CharacterCommon character)
        {

            if (character is Character)
            {
                S2CContextGetLobbyPlayerContextNtc ntc1 = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(ntc1, (Character) character);

                S2CExtendEquipSlotNtc ntc2 = new S2CExtendEquipSlotNtc()
                {
                    EquipSlot = EquipCategory.Jewelry,
                    AddNum = 0,
                    TotalNum = character.JewelrySlotNum
                };

                if (client.Party != null)
                {
                    client.Party.SendToAll(ntc1);
                }
                else
                {
                    client.Send(ntc1);
                }

                client.Send(ntc2);
            }
            else
            {
                PartyMember partyMember = client.Party.GetPartyMemberByCharacter(character);
                if (partyMember == null || partyMember is not PawnPartyMember)
                {
                    Logger.Error($"Failed to find party member in the list");
                    return;
                }

                PawnPartyMember pawnPartyMember = (PawnPartyMember)partyMember;
                if (client.Party != null)
                {
                    client.Party.SendToAll(pawnPartyMember.GetS2CContextGetParty_ContextNtc());
                }
                else
                {
                    // This should never be true but if it is, why?
                    client.Send(pawnPartyMember.GetS2CContextGetParty_ContextNtc());
                }
            }
        }
    }
}
