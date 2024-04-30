using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.RpcPacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RpcHandler
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RpcHandler));

        public static void Handle(GameClient client, byte packetType, byte[] rpcData)
        {
            IBuffer buffer = new StreamBuffer(rpcData);
            buffer.SetPositionStart();

            RpcPacketHeader Header = new RpcPacketHeader().Read(buffer);
            if (gRpcPacketHandlers.ContainsKey(Header.MsgIdFull))
            {
                gRpcPacketHandlers[Header.MsgIdFull].Handle(client.Character, buffer);
            }
        }

        public static readonly Dictionary<RpcMessageId, IRpcPacket> gRpcPacketHandlers = new Dictionary<RpcMessageId, IRpcPacket>()
        {
            {RpcMessageId.HeartBeat1, new RpcHeartbeatPacket()},
        };
    }
}
