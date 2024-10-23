using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    public class InfoGetCommand : RpcQueryCommand
    {
        public class Info
        {
            public uint StageId { get; set; }
            public byte LayerNo { get; set; }
            public uint GroupId { get; set; }
            public uint StageNo { get; set; }
            public double X { get; set; }
            public float Y { get; set; }
            public double Z { get; set; }
            public uint CharacterId { get; set; }
            public int AccountId { get; set; }
            public string AccountName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Info()
            {
                FirstName = string.Empty;
                LastName = string.Empty;
                AccountName = string.Empty;
            }
        }

        public InfoGetCommand(WebCollection<string, string> queryParams) : base(queryParams)
        {
        }

        public override string Name => "InfoGetCommand";

        public override RpcCommandResult Execute(DdonGameServer gameServer)
        {
            var connectionParam = _queryParams.Get("connection");
            var accountIdParam = _queryParams.Get("id");
            var charNameParam = _queryParams.Get("charactername");
            var accNameParam = _queryParams.Get("accountname");

            if (connectionParam is not null)
            {
                List<Connection> connections = new List<Connection>();
                bool accountIdParseSuccess = int.TryParse(accountIdParam, out int accountIdParse);
                int accountId = accountIdParseSuccess ? accountIdParse : 0;
                if (!accountIdParseSuccess) // Fall back to account name.
                {
                    if (accNameParam is not null)
                    {
                        accountId = gameServer.Database.SelectAccountByName(accNameParam.ToLowerInvariant())?.Id ?? accountId;
                    }
                }
                if (accountId > 0)
                {
                    connections.AddRange(gameServer.Database.SelectConnectionsByAccountId(accountId));
                }
                else
                {
                    connections.AddRange(gameServer.Database.SelectConnections());
                }

                ReturnValue = connections;
            }
            else
            {
                List<Info> infos = new List<Info>();
                foreach (GameClient client in gameServer.ClientLookup.GetAll())
                {
                    if (client == null || client.Character == null)
                    {
                        continue;
                    }

                    Info info = new Info();
                    info.X = client.Character.X;
                    info.Y = client.Character.Y;
                    info.Z = client.Character.Z;
                    info.StageNo = client.Character.StageNo;
                    info.StageId = client.Character.Stage.Id;
                    info.GroupId = client.Character.Stage.GroupId;
                    info.LayerNo = client.Character.Stage.LayerNo;

                    Account account = client.Account;
                    if (account != null)
                    {
                        info.AccountId = account.Id;
                        info.AccountName = account.NormalName;
                    }

                    Character character = client.Character;
                    if (character != null)
                    {
                        info.CharacterId = character.CharacterId;
                        info.FirstName = character.FirstName;
                        info.LastName = character.LastName;
                    }

                    infos.Add(info);
                }
                if (accountIdParam is not null && uint.TryParse(accountIdParam, out uint accountId))
                {
                    infos = infos.Where(x => x.AccountId == accountId).ToList();
                }
                if (accNameParam is not null)
                {
                    infos = infos.Where(x => x.AccountName == accNameParam).ToList();
                }
                if (charNameParam is not null)
                {
                    string[] nameSplit = charNameParam.Split(" ");
                    infos = infos.Where(x => x.FirstName == nameSplit[0] && x.LastName == nameSplit[1]).ToList();
                }

                ReturnValue = infos;
            }

            return new RpcCommandResult(this, true);
        }
    }

    public class InfoDeleteCommand : RpcQueryCommand
    {
        public InfoDeleteCommand(WebCollection<string, string> queryParams) : base(queryParams)
        {
        }

        public override string Name => "InfoDeleteCommand";

        public override RpcCommandResult Execute(DdonGameServer gameServer)
        {
            if (!_queryParams.ContainsKey("accountname"))
            {
                StatusCode = 400;
                return new RpcCommandResult(this, false);
            }

            var accountName = _queryParams["accountname"].ToLowerInvariant();
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

    public class InfoRoute : RpcRouteTemplate
    {
        public InfoRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/info";

        public override async Task<WebResponse> Get(WebRequest request)
        {
            return await HandleQuery<InfoGetCommand>(request);
        }

        public override async Task<WebResponse> Delete(WebRequest request)
        {
            return await HandleQuery<InfoDeleteCommand>(request);
        }
    }
}
