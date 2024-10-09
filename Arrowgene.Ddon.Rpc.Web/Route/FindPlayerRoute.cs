using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer;
using System.Linq;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    internal class FindPlayerReturn
    {
        public int ServerId { get; set; }

        public FindPlayerReturn() { }
    }

    public class FindPlayerCommand : RpcGetCommand
    {
        public FindPlayerCommand(WebCollection<string, string> queryParams) : base(queryParams)
        {
        }

        public override string Name => "FindPlayerCommand";

        public override RpcCommandResult Execute(DdonGameServer gameServer)
        {
            if (!_queryParams.ContainsKey("accountname"))
            {
                StatusCode = 400;
                ReturnValue = "400 - Invalid request form.";
                return new RpcCommandResult(this, false);
            }
            var accountName = _queryParams["accountname"];

            Account account = gameServer.Database.SelectAccountByName(accountName);
            if (account == null)
            {
                StatusCode = 400;
                ReturnValue = "400 - The specified account does not exist.";
                return new RpcCommandResult(this, false);
            }

            var connection = gameServer.Database.SelectConnectionsByAccountId(account.Id).Where(x => x.Type == ConnectionType.GameServer);
            if (!connection.Any())
            {
                StatusCode = 400;
                ReturnValue = "400 - The specified account is not connected to any GameServers.";
                return new RpcCommandResult(this, false);
            }

            ReturnValue = new FindPlayerReturn()
            {
                ServerId = connection.FirstOrDefault().ServerId,
            };
            return new RpcCommandResult(this, true);
        }
    }

    public class FindPlayerRoute : RpcGetRoute<FindPlayerCommand>
    {
        public FindPlayerRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/findplayer";
    }
}
