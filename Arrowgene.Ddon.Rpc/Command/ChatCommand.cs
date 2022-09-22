using System;
using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer;
using static Arrowgene.Ddon.GameServer.Chat.ChatManager;

namespace Arrowgene.Ddon.Rpc.Command
{
    public class ChatCommand : IRpcCommand
    {
        public string Name => "ChatCommand";

        public ChatCommand()
        {
            _since = DateTime.MinValue;
        }

        public ChatCommand(DateTime since)
        {
            _since = since;
        }

        public IEnumerable<ChatMessageLogEntry> ChatMessageLog { get; set; }

        private DateTime _since;

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            ChatMessageLog = gameServer.ChatManager.ChatMessageLog
                .Where(entry => entry.DateTime >= _since);
            return new RpcCommandResult(this, true);
        }
    }
}