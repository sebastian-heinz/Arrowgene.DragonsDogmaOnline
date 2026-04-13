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
        private readonly MailSend _mail;
        private readonly WebServerSetting _webServerSetting;
        private class AccountRequest
        {
            public string Action { get; set; }
            public string Account { get; set; }
            public string Email { get; set; }
            public string EmailToken { get; set; }
            public string Password { get; set; }
            public string PasswordToken { get; set; }
        }

        public class AccountResponse
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
            public string Email { get; set; }

            public AccountVerification(string username, string password, string email)
            {
                Username = (username is not null) ? username : "";
                Password = (password is not null) ? password : "";
                Email = (email is not null) ? email : "";

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

                if (Email == null || Email.Trim().Length == 0)
                {
                    Error = true;
                    Message = "E-mail cannot be empty";
                    return;
                }


                if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    Error = true;
                    Message = "Invalid e-mail format";
                    return;
                }
            }
        }

        public AccountRoute(IDatabase database, WebServerSetting webServerSetting)
        {
            _database = database;
            _mail = new MailSend(webServerSetting.MailSetting, webServerSetting);
            _webServerSetting = webServerSetting;
        }

        public override async Task<WebResponse> Post(WebRequest request)
        {
            AccountRequest req = await request.ReadJsonAsync<AccountRequest>();
            if (req == null)
            {
                return await WebResponse.InternalServerError();
            }

            AccountResponse res = new AccountResponse();

            if (_database.CheckBannedIp(request.Host))
            {
                res.Error = "You have been IP banned.";
                WebResponse banresponse = new()
                {
                    StatusCode = 401
                };
                await banresponse.WriteJsonAsync(res);
                return banresponse;
            }

            AccountVerification accountCheck = new(req.Account, req.Password, req.Email); 

            switch (req.Action)
            {
                case "login":

                    string token = CreateLoginToken(req.Account, req.Password);
                    if (token == null)
                    {
                        if ((bool)_webServerSetting.MailSetting.MailRequired)
                            res.Error = "Either your account or password are incorrect, or your email isn't verified yet";
                        else
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

                    Account account = CreateAccount(req.Account, req.Email, req.Password);
                    if (account == null)
                    {
                        res.Error = "Account or e-mail already in use";
                        break;
                    }
                    try
                    {
                        if ((bool)_webServerSetting.MailSetting.MailRequired)
                        {
                            res.Message = "Account created, please check your email for a validation link";
                            await _mail.SendAsync(MailSend.MailModel.NewAccount, account);
                        }
                        else
                        {
                            res.Message = "Account created";
                        }
                        break;
                    }
                    catch
                    {
                        res.Error = $"Verification e-mail could not be sent, try resend it at {((bool)_webServerSetting.MailSetting.IsHttps ? "https" : "http")}://{_webServerSetting.MailSetting.DomainUrl}:{_webServerSetting.PublicWebEndPoint.Port}/web/verify_resend.html";
                        break;
                    }

                case "recover":
                    account = CreatePasswordToken(req.Email);

                    if (account == null)
                    {
                        res.Error = "Account not found";
                        break;
                    }

                    if (!account.MailVerified && (bool)_webServerSetting.MailSetting.MailRequired)
                    {
                        res.Message = "E-mail not verified yet";
                        break;
                    }

                    try
                    {
                        await _mail.SendAsync(MailSend.MailModel.PasswordReset, account);
                        res.Message = "Password token generated";
                        break;
                    }
                    catch
                    {
                        res.Error = "Password reset token could not be sent, check your info try again";
                        break;
                    }

                case "reset":
                    account = ResetPassword(req.Account, req.Password, req.PasswordToken);

                    if (account == null)
                    {
                        res.Error = "Invalid account or token";
                        break;
                    }

                    res.Message = "Password changed";
                    break;

                case "verify":
                    bool verification = VerifyEmail(req.Account, req.EmailToken);
                    if (!verification)
                    {
                        res.Error = "Email not found";
                        break;
                    }

                    res.Message = "Email verified";
                    break;

                case "resend":
                    account = ResendEmailVerification(req.Account, req.Email);
                    if (account == null)
                    {
                        res.Error = "Account and Email combination not found";
                        break;
                    }

                    try
                    {
                        await _mail.SendAsync(MailSend.MailModel.MailVerify, account);
                        res.Message = "Verification token resent";
                        break;

                    }
                    catch
                    {
                        res.Error = "Verification token could not be sent, check your info try again";
                        break;
                    }

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

            Account email = _database.SelectAccountByEmail(mail);
            if (email != null)
            {
                Logger.Error($"{mail} - CreateAccount: email already taken");
                return null;
            }

            string hash = PasswordHash.CreateHash(password);
            string mailToken = GameToken.GenerateToken();
            account = _database.CreateAccount(name, mail, hash, mailToken);
            return account;
        }

        private string CreateLoginToken(string name, string password)
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

            if (!account.MailVerified && (bool)_webServerSetting.MailSetting.MailRequired)
            {
                Logger.Error($"{name} - CreateToken: email not verified yet");
                return null;
            }

            account.LoginToken = GameToken.GenerateToken();
            account.LoginTokenCreated = DateTime.UtcNow;
            _database.UpdateAccount(account);
            return account.LoginToken;
        }

        private Account ResetPassword(string name, string newPassword, string passwordToken)
        {
            Account account = _database.SelectAccountByPasswordTokenAndName(name, passwordToken);
            if (account == null)
            {
                Logger.Error("ResetPassword: account does not exist");
                return null;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                Logger.Error($"ResetPassword: invalid password");
                account.PasswordToken = null;
                _database.UpdateAccount(account);
                return null;
            }

            account.PasswordToken = null;
            account.Hash = PasswordHash.CreateHash(newPassword);
            _database.UpdateAccount(account);
            return account;
        }

        private Account CreatePasswordToken(string mail)
        {
            Account account = _database.SelectAccountByEmail(mail);
            if (account == null)
            {
                Logger.Error($"{mail} - CreatePasswordToken: account does not exist");
                return null;
            }

            account.PasswordToken = GameToken.GenerateToken();
            _database.UpdateAccount(account);
            return account;
        }

        private bool VerifyEmail(string name, string emailToken)
        {

            Account account = _database.SelectAccountByMailTokenAndName(name, emailToken);
            if (account == null)
            {
                Logger.Error("VerifyEmail: account does not exist");
                return false;
            }

            if (account.MailToken != emailToken)
            {
                Logger.Error($"{account.NormalName} - VerifyEmail: invalid email token");
                return false;
            }

            account.MailToken = null;
            account.MailVerified = true;
            account.MailVerifiedAt = DateTime.UtcNow;
            _database.UpdateAccount(account);
            return true;
        }

        private Account ResendEmailVerification(string accountName, string email)
        {
            Account account = _database.SelectAccountByEmailAndName(accountName, email);
            if (account == null)
            {
                Logger.Error("ResendEmailVerification: account does not exist");
                return null;
            }

            account.MailToken = GameToken.GenerateToken();
            _database.UpdateAccount(account);
            return account;
        }
    }
}
