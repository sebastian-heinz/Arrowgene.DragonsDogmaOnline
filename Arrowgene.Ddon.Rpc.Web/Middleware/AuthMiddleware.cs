#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Middleware;

public class AuthMiddleware : IWebMiddleware
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(AuthMiddleware));

    private readonly IDatabase _database;
    private readonly Dictionary<string, AccountStateType> _routeAndRequiredMinimumState;
    private readonly Dictionary<string, Account> _credentialCache;

    public AuthMiddleware(IDatabase database)
    {
        _database = database;
        _routeAndRequiredMinimumState = new Dictionary<string, AccountStateType>();
        _credentialCache = new();
    }

    public void Require(AccountStateType minimumState, string route)
    {
        _routeAndRequiredMinimumState.Add(route, minimumState);
    }

    public async Task<WebResponse> Handle(WebRequest request, WebMiddlewareDelegate next)
    {
        if(!_routeAndRequiredMinimumState.ContainsKey(request.Path))
        {
            // Don't intercept request if the request path isn't registered in the middleware
            return await next(request);
        }

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

        // Short-circuit the DB handling if we've already cached the account.
        Account account = _credentialCache.GetValueOrDefault(username) ?? _database.SelectAccountByName(username);
        _credentialCache[username] = account;
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

        AccountStateType minimumRequiredAccountStateType = _routeAndRequiredMinimumState[request.Path];
        if (account.State < minimumRequiredAccountStateType)
        {
            Logger.Error($"Attempted to access auth protected route as {username} without enough permissions (Account has {account.State}, minimum required {minimumRequiredAccountStateType}).");
            WebResponse response = new WebResponse();
            response.StatusCode = 403;
            await response.WriteAsync($"Attempted to access auth protected route as {username} without enough permissions.");
            return response;
        }

        return await next(request);
    }
}
