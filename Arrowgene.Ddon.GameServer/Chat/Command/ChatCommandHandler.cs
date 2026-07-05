using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Scripting;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Chat.Command
{
    public class ChatCommandHandler : IChatHandler
    {
        public const char ChatCommandStart = '/';
        public const char ChatCommandSeparator = ' ';

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatCommandHandler));

        private DdonGameServer Server { get; set; }
        private ChatCommandModule ChatCommandModule { get; set; }

        public ChatCommandHandler(DdonGameServer server)
        {
            Server = server;
            ChatCommandModule = server.ScriptManager.ChatCommandModule;
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
            if (!Server.ScriptManager.ChatCommandModule.Commands.ContainsKey(commandKey))
            {
                Logger.Error(client, $"Command '{commandKey}' does not exist");
                responses.Add(ChatResponse.CommandError(client, $"Command does not exist: {commandKey}"));
                return;
            }

            IChatCommand chatCommand = Server.ScriptManager.ChatCommandModule.Commands[commandKey];

            var disableAccountCheckType = Server.GameSettings.ChatCommandsSettings.DisableAccountTypeCheck;
            if (!disableAccountCheckType && (client.Account.State < chatCommand.AccountState))
            {
                Logger.Error(client,
                   $"Not entitled to execute command '{chatCommand.CommandName}' (State < Required: {client.Account.State} < {chatCommand.AccountState})");
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

            chatCommand.Execute(Server, subCommand, client, message, responses);
        }
    }
}
