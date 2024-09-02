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

        public static void Handle(GameClient client, RpcMsgType msgType, byte[] rpcData)
        {
            IBuffer buffer = new StreamBuffer(rpcData);
            buffer.SetPositionStart();
            
            // It seems like MsgIdFull  is almost like a "message class"
            // where RpcId is a unique action?
            RpcPacketHeader Header = new RpcPacketHeader().Read(buffer);

            // Logger.Debug(Header.AsString());
            if (gRpcPacketHandlers.ContainsKey(Header.MsgDTI) && gRpcPacketHandlers[Header.MsgDTI].ContainsKey(Header.MsgId))
            {
                gRpcPacketHandlers[Header.MsgDTI][Header.MsgId].Handle(client.Character, Header, buffer);
            }
        }

        public static readonly Dictionary<RpcNetMsgDti, Dictionary<ushort, IRpcPacket>> gRpcPacketHandlers = new Dictionary<RpcNetMsgDti, Dictionary<ushort, IRpcPacket>>
        {
            [RpcNetMsgDti.cNetMsgCtrlAction] = new Dictionary<ushort, IRpcPacket>
            {
                {(ushort) RpcMsgIdControl.NET_MSG_ID_PERIODIC_TOP, new RpcCtrlPeriodicTop()},
            }
        };
    }
}
