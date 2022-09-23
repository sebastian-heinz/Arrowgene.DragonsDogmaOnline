using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClientLookup : ClientLookup<GameClient>
    {
        public GameClientLookup()
        {
        }

        public GameClient GetClientByAccountId(int accountId)
        {
            foreach (GameClient client in GetAll())
            {
                if (client.Account == null)
                {
                    continue;
                }

                if (client.Account.Id == accountId)
                {
                    return client;
                }
            }

            return null;
        }
    }
}
