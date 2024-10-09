using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    public class KickRequest
    {
        public KickRequest()
        {
            AccountName = string.Empty;
        }

        public string AccountName { get; set; }

    }

    public class KickCommand : RpcPostCommand<KickRequest>
    {
        public KickCommand(KickRequest entry) : base(entry)
        {
        }

        public override string Name => "KickCommand";

        public override RpcCommandResult Execute(DdonGameServer gameServer)
        {
            var accountName = _entry.AccountName.ToLowerInvariant();
            bool action = false;
            foreach (var client in gameServer.ClientLookup.GetAll())
            {
                if (client.Account.NormalName == accountName)
                {
                    client.Close();
                    action = true;
                    break;
                }
            }
            Account account = gameServer.Database.SelectAccountByName(accountName);
            action = account != null && gameServer.Database.DeleteConnection(gameServer.Id, account.Id) || action;

            return new RpcCommandResult(this, action);
        }
    }

    public class KickRoute : RpcPostRoute<KickRequest, KickCommand>
    {
        public KickRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/kick";
    }
}
