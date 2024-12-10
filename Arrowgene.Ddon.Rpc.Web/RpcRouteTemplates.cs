using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.WebServer;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Rpc.Web
{
    public abstract class RpcCommandReturnable : IRpcCommand
    {
        public abstract string Name { get; }

        public abstract RpcCommandResult Execute(DdonGameServer gameServer);

        public object ReturnValue;
        public int StatusCode;
    }

    public abstract class RpcBodyCommand<T> : RpcCommandReturnable
    {
        protected readonly T _entry;

        protected RpcBodyCommand(T entry)
        {
            _entry = entry;
        }
    }

    public abstract class RpcQueryCommand : RpcCommandReturnable
    {
        protected readonly WebCollection<string, string> _queryParams;

        protected RpcQueryCommand(WebCollection<string, string> queryParams)
        {
            _queryParams = queryParams;
        }
    }

    public abstract class RpcMixedCommand<T> : RpcCommandReturnable 
    {
        protected readonly T _entry;
        protected readonly WebCollection<string, string> _queryParams;

        protected RpcMixedCommand(T entry, WebCollection<string, string> queryParams)
        {
            _entry = entry; 
            _queryParams = queryParams;
        }
    }

    public abstract class RpcRouteTemplate : RpcWebRoute
    {
        public RpcRouteTemplate(IRpcExecuter executer) : base(executer)
        {
        }

        protected async Task<WebResponse> HandleBody<RequestT, CommandT>(WebRequest request)
            where RequestT : class, new()
            where CommandT : RpcBodyCommand<RequestT>
        {
            try
            {
                RequestT requestObject = JsonSerializer.Deserialize<RequestT>(request.Body);
                CommandT commandObject = (CommandT)Activator.CreateInstance(typeof(CommandT), requestObject);
                RpcCommandResult result = Executer.Execute(commandObject);

                if (!result.Success)
                {
                    WebResponse response = new WebResponse();
                    response.StatusCode = commandObject.StatusCode > 0 ? commandObject.StatusCode : 500;
                    if (commandObject.ReturnValue != null)
                    {
                        await response.WriteJsonAsync(commandObject.ReturnValue);
                    }
                    else
                    {
                        await response.WriteAsync("Error");
                    }
                    return response;
                }
                else
                {
                    WebResponse response = new WebResponse();
                    response.StatusCode = commandObject.StatusCode > 0 ? commandObject.StatusCode : 200;
                    await response.WriteJsonAsync(commandObject.ReturnValue);
                    return response;
                }
            }
            catch (JsonException)
            {
                WebResponse response = new WebResponse();
                response.StatusCode = 400;
                await response.WriteAsync("Invalid request body.");
                return response;
            }
        }

        protected async Task<WebResponse> HandleQuery<CommandT>(WebRequest request)
            where CommandT : RpcQueryCommand
        {
            WebResponse response = new WebResponse();
            CommandT commandObject = (CommandT)Activator.CreateInstance(typeof(CommandT), request.QueryParameter);
            RpcCommandResult result = Executer.Execute(commandObject);
            if (!result.Success)
            {
                response.StatusCode = commandObject.StatusCode > 0 ? commandObject.StatusCode : 500;
                if (commandObject.ReturnValue != null)
                {
                    await response.WriteJsonAsync(commandObject.ReturnValue);
                }
                else
                {
                    await response.WriteAsync("Error");
                }
                return response;
            }
            response.StatusCode = commandObject.StatusCode > 0 ? commandObject.StatusCode : 200;
            await response.WriteJsonAsync(commandObject.ReturnValue);
            return response;
        }

        protected async Task<WebResponse> HandleMixed<RequestT, CommandT>(WebRequest request)
            where RequestT : class, new()
            where CommandT : RpcMixedCommand<RequestT>
        {
            try
            {
                RequestT requestObject = JsonSerializer.Deserialize<RequestT>(request.Body);
                CommandT commandObject = (CommandT)Activator.CreateInstance(typeof(CommandT), requestObject, request.QueryParameter);
                RpcCommandResult result = Executer.Execute(commandObject);

                if (!result.Success)
                {
                    WebResponse response = new WebResponse();
                    response.StatusCode = commandObject.StatusCode > 0 ? commandObject.StatusCode : 500;
                    if (commandObject.ReturnValue != null)
                    {
                        await response.WriteJsonAsync(commandObject.ReturnValue);
                    }
                    else
                    {
                        await response.WriteAsync("Error");
                    }
                    return response;
                }
                else
                {
                    WebResponse response = new WebResponse();
                    response.StatusCode = commandObject.StatusCode > 0 ? commandObject.StatusCode : 200;
                    await response.WriteJsonAsync(commandObject.ReturnValue);
                    return response;
                }
            }
            catch (JsonException)
            {
                WebResponse response = new WebResponse();
                response.StatusCode = 400;
                await response.WriteAsync("Invalid request body.");
                return response;
            }
        }
    }
}
