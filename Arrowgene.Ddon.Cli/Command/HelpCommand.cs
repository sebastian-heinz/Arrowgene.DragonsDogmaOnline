using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class HelpCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(HelpCommand));
        private readonly Dictionary<string, ICommand> _commands;

        public string Key => "help";
        public string Description => "Displays this text";

        public HelpCommand(Dictionary<string, ICommand> commands)
        {
            _commands = commands;
        }

        public CommandResultType Run(CommandParameter parameter)
        {
            if (parameter.Arguments.Count >= 1)
            {
                string subKey = parameter.Arguments[0];
                if (!_commands.ContainsKey(subKey))
                {
                    Logger.Error(
                        $"Command: 'help {subKey}' not available. Type 'help' for a list of available commands.");
                    return CommandResultType.Continue;
                }

                ICommand consoleCommandHelp = _commands[subKey];
                Logger.Info(ShowHelp(consoleCommandHelp));
                return CommandResultType.Continue;
            }

            ShowHelp();
            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
        
        private void ShowHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("Available Commands:");
            foreach (string key in _commands.Keys)
            {
                sb.Append(Environment.NewLine);
                sb.Append("----------");
                ICommand command = _commands[key];
                sb.Append(ShowHelp(command));
            }

            Logger.Info(sb.ToString());
        }

        private string ShowHelp(ICommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(command.Key);
            sb.Append(Environment.NewLine);
            sb.Append($"- {command.Description}");
            return sb.ToString();
        }
    }
}
