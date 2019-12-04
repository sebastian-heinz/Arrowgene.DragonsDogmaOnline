using System;
using Ddo.Cli.Argument;
using Ddo.Server;
using Ddo.Server.Setting;

namespace Ddo.Cli.Command.Commands
{
    public class ServerCommand : ConsoleCommand
    {
        private DdoServer _server;
        private readonly LogWriter _logWriter;

        public ServerCommand(LogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        public override void Shutdown()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (_server == null)
            {
                DdoSetting setting = new DdoSetting();
                _server = new DdoServer(setting);
            }

            if (parameter.Arguments.Contains("start"))
            {
                _server.Start();
                return CommandResultType.Completed;
            }

            if (parameter.Arguments.Contains("stop"))
            {
                _server.Stop();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string Key => "server";


        public override string Description =>
            $"Dragons Dogma Online Server. Ex.:{Environment.NewLine}server start{Environment.NewLine}server stop";
    }
}
