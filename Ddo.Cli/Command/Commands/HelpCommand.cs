using System;
using System.Collections.Generic;
using System.Text;
using Ddo.Cli.Argument;

namespace Ddo.Cli.Command.Commands
{
    public class HelpCommand : ConsoleCommand
    {
        private readonly Dictionary<string, IConsoleCommand> _commands;

        public HelpCommand(Dictionary<string, IConsoleCommand> commands)
        {
            _commands = commands;
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
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

                IConsoleCommand consoleCommandHelp = _commands[subKey];
                Logger.Info(ShowHelp(consoleCommandHelp));
                return CommandResultType.Continue;
            }

            ShowHelp();
            return CommandResultType.Continue;
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
                IConsoleCommand command = _commands[key];
                sb.Append(ShowHelp(command));
            }

            Logger.Info(sb.ToString());
        }

        private string ShowHelp(IConsoleCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(command.Key);
            sb.Append(Environment.NewLine);
            sb.Append($"- {command.Description}");
            return sb.ToString();
        }

        public override string Key => "help";
        public override string Description => "Displays this text";
    }
}
