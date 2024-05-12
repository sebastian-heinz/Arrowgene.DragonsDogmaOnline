using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    public class RpcPacketHeader : RpcPacketBase
    {
        public RpcPacketHeader()
        {
        }

        public UInt32 SessionId { get; set; }
        public UInt64 RpcId { get; set; }
        public RpcMessageId MsgIdFull { get; set; }
        public UInt32 SearchId { get; set; }

        public override void Handle(Character character, RpcPacketHeader Header, IBuffer buffer)
        {
            /* special case nothing to do; should not be called */
            throw new NotImplementedException();
        }

        public RpcPacketHeader Read(IBuffer buffer)
        {
            RpcPacketHeader obj = new RpcPacketHeader();
            obj.SessionId = ReadUInt32(buffer);    // NetMsgData.Head.SessionId
            obj.RpcId = ReadUInt64(buffer); // NetMsgData.Head.RpcId
            obj.MsgIdFull = (RpcMessageId) ReadUInt32(buffer);    // NetMsgData.Head.MsgIdFull
            obj.SearchId = ReadUInt32(buffer);     // NetMsgData.Head.SearchId, seems to either a PawnId or 0
            return obj;
        }
    }
}
