using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.Chat.Log;

namespace Arrowgene.Ddon.Rpc.Command
{
    public class ChatCommand : IRpcCommand
    {
        public string Name => "ChatCommand";

        public ChatCommand()
        {
            _since = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        }

        public ChatCommand(DateTime since)
        {
            _since = since;
        }

        public IEnumerable<ChatMessageLogEntry> ChatMessageLog { get; set; }

        private readonly DateTime _since;

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            ChatMessageLog = gameServer.ChatLogHandler.ChatMessageLog
                .Where(entry => entry.DateTime >= _since);
            return new RpcCommandResult(this, true);
        }
    }
}
