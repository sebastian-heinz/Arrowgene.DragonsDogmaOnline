using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Chat.Command.Commands;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Chat.Command
{
    public class ChatCommandHandler : IChatHandler
    {
        public const char ChatCommandStart = '/';
        public const char ChatCommandSeparator = ' ';

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatCommandHandler));

        private readonly Dictionary<string, ChatCommand> _commands;

        public ChatCommandHandler(DdonGameServer server)
        {
            _commands = new Dictionary<string, ChatCommand>();
            AddCommand(new TestCommand());
            AddCommand(new EnemyCommand());
            AddCommand(new InfoCommand());
            AddCommand(new JobCommand(server));
            AddCommand(new VersionCommand());
            AddCommand(new PartyInviteCommand(server));
        }

        public void AddCommand(ChatCommand command)
        {
            _commands.Add(command.KeyToLowerInvariant, command);
        }

        public void Handle(GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (client == null)
            {
                return;
            }

            if (message.Message == null || message.Message.Length <= 1)
            {
                return;
            }

            if (!message.Message.StartsWith(ChatCommandStart))
            {
                return;
            }
            
            // message starts with `/` treat it as a chat command
            // this should not reach any audience
            message.Deliver = false;
            
            string commandMessage = message.Message.Substring(1);
            string[] command = commandMessage.Split(ChatCommandSeparator);
            if (command.Length <= 0)
            {
                Logger.Error(client, $"Command '{message.Message}' is invalid");
                responses.Add(ChatResponse.CommandError(client, $"Command '{message.Message}' is invalid"));
                return;
            }

            string commandKey = command[0].ToLowerInvariant();
            if (!_commands.ContainsKey(commandKey))
            {
                Logger.Error(client, $"Command '{commandKey}' does not exist");
                responses.Add(ChatResponse.CommandError(client, $"Command does not exist: {commandKey}"));
                return;
            }

            ChatCommand chatCommand = _commands[commandKey];
            if (client.Account.State < chatCommand.AccountState)
            {
                Logger.Error(client,
                    $"Not entitled to execute command '{chatCommand.Key}' (State < Required: {client.Account.State} < {chatCommand.AccountState})");
                responses.Add(ChatResponse.CommandError(client, $"Not authorized to execute this command"));
                return;
            }

            int cmdLength = command.Length - 1;
            string[] subCommand;
            if (cmdLength > 0)
            {
                subCommand = new string[cmdLength];
                Array.Copy(command, 1, subCommand, 0, cmdLength);
            }
            else
            {
                subCommand = new string[0];
            }

            chatCommand.Execute(subCommand, client, message, responses);
        }
    }
}
