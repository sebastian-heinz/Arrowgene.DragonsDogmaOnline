using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Route;

namespace Arrowgene.Ddon.WebServer
{
    public class AccountRoute : WebRoute
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(AccountRoute));


        public override string Route => "/api/account";

        private readonly IDatabase _database;

        private class AccountRequest
        {
            public string Action { get; set; }
            public string Account { get; set; }
            public string Password { get; set; }
        }

        private class AccountResponse
        {
            public string Error { get; set; }
            public string Message { get; set; }
            public string Token { get; set; }
        }

        private class AccountVerification
        {
            public bool Error { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }

            public AccountVerification(string username, string password)
            {
                Username = username;
                Password = password;

                // Very simple data checks on the parameters.

                if (Username.Trim().Length == 0)
                {
                    Error = true;
                    Message = "Account ID cannot be empty";
                    return;
                }

                // Disallow any whitespace.

                if (Regex.IsMatch(Username, @"\s"))
                {
                    Error = true;
                    Message = "Account ID cannot contain spaces";
                    return;
                }

                if (Password.Trim().Length == 0)
                {
                    Error = true;
                    Message = "Password cannot be empty";
                    return;
                }

                if (Regex.IsMatch(Password, @"\s"))
                {
                    Error = true;
                    Message = "Password cannot contain spaces";
                    return;
                }
            }
        }

        public AccountRoute(IDatabase database)
        {
            _database = database;
        }

        public override async Task<WebResponse> Post(WebRequest request)
        {
            AccountRequest req = await request.ReadJsonAsync<AccountRequest>();
            if (req == null)
            {
                return await WebResponse.InternalServerError();
            }

            AccountResponse res = new AccountResponse();
            AccountVerification accountCheck = new(req.Account, req.Password);

            switch (req.Action)
            {
                case "login":

                    string token = CreateToken(req.Account, req.Password);
                    if (token == null)
                    {
                        res.Error = "Account or password wrong";
                        break;
                    }

                    res.Message = "Login Success";
                    res.Token = token;
                    break;
                case "create":

                    if (accountCheck.Error)
                    {
                        res.Error = accountCheck.Message;
                        break;
                    }

                    Account account = CreateAccount(req.Account, $"{req.Account}@dd.on", req.Password);
                    if (account == null)
                    {
                        res.Error = "Account already exists";
                        break;
                    }

                    res.Message = "Account created";
                    break;
            }

            WebResponse response = new WebResponse();
            response.StatusCode = 200;
            await response.WriteJsonAsync(res);
            return response;
        }

        private Account CreateAccount(string name, string mail, string password)
        {
            Account account = _database.SelectAccountByName(name);
            if (account != null)
            {
                Logger.Error($"{name} - CreateAccount: account already taken");
                return null;
            }

            string hash = PasswordHash.CreateHash(password);
            account = _database.CreateAccount(name, mail, hash);
            return account;
        }

        private string CreateToken(string name, string password)
        {
            Account account = _database.SelectAccountByName(name);
            if (account == null)
            {
                Logger.Error($"{name} - CreateToken: account does not exist");
                return null;
            }

            if (!PasswordHash.Verify(password, account.Hash))
            {
                Logger.Error($"{name} - CreateToken: wrong password provided");
                return null;
            }

            account.LoginToken = GameToken.GenerateLoginToken();
            account.LoginTokenCreated = DateTime.UtcNow;
            _database.UpdateAccount(account);
            return account.LoginToken;
        }
    }
}
