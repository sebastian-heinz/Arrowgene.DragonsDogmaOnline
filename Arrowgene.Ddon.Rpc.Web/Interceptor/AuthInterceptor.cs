#nullable enable

using System;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;
using Arrowgene.WebServer;

public class AuthInterceptor : IInterceptor
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(AuthInterceptor));

    private readonly IDatabase _database;
    private readonly AccountStateType _minimumState;

    public AuthInterceptor(IDatabase database, AccountStateType minimumState)
    {
        _database = database;
        _minimumState = minimumState;
    }

    public async Task<WebResponse?> InterceptRequest(WebRequest request)
    {
        string authHeader = request.Header.Get("authorization");
        if(authHeader == null)
        {
            Logger.Error("Attempted to access auth protected route with no Authorization header");
            WebResponse response = new WebResponse();
            response.StatusCode = 401;
            await response.WriteAsync("Attempted to access auth protected route with no Authorization header");
            return response;
        }

        if(!authHeader.StartsWith("Basic "))
        {
            Logger.Error("Attempted to access auth protected route with an invalid Authorization method. Only Basic auth is supported.");
            WebResponse response = new WebResponse();
            response.StatusCode = 401;
            await response.WriteAsync("Attempted to access auth protected route with an invalid Authorization method. Only Basic auth is supported.");
            return response;
        }

        string encodedUserAndPassword = authHeader.Substring("Basic ".Length);
        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        string[] usernameAndPassword = encoding.GetString(Convert.FromBase64String(encodedUserAndPassword)).Split(":");
        if(usernameAndPassword.Length != 2)
        {
            Logger.Error("Attempted to access auth protected route with an invalid Basic auth header.");
            WebResponse response = new WebResponse();
            response.StatusCode = 401;
            await response.WriteAsync("Attempted to access auth protected route with an invalid Basic auth header.");
            return response;
        }
        
        string username = usernameAndPassword[0];
        string password = usernameAndPassword[1];

        Account account = _database.SelectAccountByName(username);
        if (account == null)
        {
            Logger.Error($"Attempted to authenticate as a nonexistant user {username}.");
            WebResponse response = new WebResponse();
            response.StatusCode = 401;
            await response.WriteAsync($"Failed to authenticate as {username}.");
            return response;
        }

        if (!PasswordHash.Verify(password, account.Hash))
        {
            Logger.Error($"Attempted to authenticate as {username} with an incorrect password.");
            WebResponse response = new WebResponse();
            response.StatusCode = 401;
            await response.WriteAsync($"Failed to authenticate as {username}.");
            return response;
        }

        if(account.State < _minimumState)
        {
            Logger.Error($"Attempted to access auth protected route as {username} without enough permissions (Account has {account.State}, minimum required {_minimumState}).");
            WebResponse response = new WebResponse();
            response.StatusCode = 403;
            await response.WriteAsync($"Attempted to access auth protected route as {username} without enough permissions.");
            return response;
        }

        return null;
    }
}