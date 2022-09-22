using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.LoginServer
{
    public class LoginClientLookup : ClientLookup<LoginClient>
    {
        public LoginClientLookup()
        {
        }

        public LoginClient GetClientByAccountId(int accountId)
        {
            foreach (LoginClient client in GetAll())
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
