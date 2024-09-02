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
            _sinceUnixMillis = long.MinValue;
        }

        public ChatCommand(long sinceUnixMillis)
        {
            _sinceUnixMillis = sinceUnixMillis;
        }

        public IEnumerable<ChatMessageLogEntry> ChatMessageLog { get; set; }

        private readonly long _sinceUnixMillis;

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            ChatMessageLog = gameServer.ChatLogHandler.ChatMessageLog
                .Where(entry => entry.UnixTimeMillis > _sinceUnixMillis);
            return new RpcCommandResult(this, true);
        }
    }
}
