using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClientLookup : ClientLookup<GameClient>
    {
        public GameClientLookup()
        {
        }

        public List<Character> GetAllCharacter()
        {
            List<Character> characters = new List<Character>();
            foreach (GameClient client in GetAll())
            {
                Character character = client.Character;
                if (character == null)
                {
                    continue;
                }

                characters.Add(character);
            }

            return characters;
        }

        public GameClient GetClientByAccountId(int accountId)
        {
            foreach (GameClient client in GetAll())
            {
                Account account = client.Account;
                if (account == null)
                {
                    continue;
                }

                if (account.Id == accountId)
                {
                    return client;
                }
            }

            return null;
        }

        public GameClient GetClientByCharacterId(uint characterId)
        {
            foreach (GameClient client in GetAll())
            {
                Character character = client.Character;
                if (character == null)
                {
                    continue;
                }

                if (character.CharacterId == characterId)
                {
                    return client;
                }
            }

            return null;
        }

        public GameClient GetClientByCharacterName(string FirstName, string LastName)
        {
            foreach (GameClient client in GetAll())
            {
                Character character = client.Character;
                if (character == null)
                {
                    continue;
                }

                if (character.FirstName == FirstName && character.LastName == LastName)
                {
                    return client;
                }
            }

            return null;
        }
    }
}
