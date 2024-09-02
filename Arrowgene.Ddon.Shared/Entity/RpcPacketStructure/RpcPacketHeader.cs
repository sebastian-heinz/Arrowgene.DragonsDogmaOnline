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

        // public UInt32 SessionId { get; set; }

        public UInt32 Unk0 {  get; set; }
        public UInt32 Unk1 {  get; set; }
        public UInt32 RpcId { get; set; }
        public RpcNetMsgDti MsgDTI { get; set; }
        public UInt16 MsgId { get; set; }
        public UInt32 SearchId { get; set; }

        public override void Handle(Character character, RpcPacketHeader Header, IBuffer buffer)
        {
            /* special case nothing to do; should not be called */
            throw new NotImplementedException();
        }

        public RpcPacketHeader Read(IBuffer buffer)
        {
            RpcPacketHeader obj = new RpcPacketHeader();
            // obj.SessionId = ReadUInt32(buffer);    // NetMsgData.Head.SessionId
            obj.Unk0 = ReadUInt32(buffer);
            obj.Unk1 = ReadUInt32(buffer);
            obj.RpcId = ReadUInt32(buffer); // NetMsgData.Head.RpcId
            obj.MsgDTI = (RpcNetMsgDti) ReadUInt16(buffer);
            obj.MsgId = ReadUInt16(buffer);
            obj.SearchId = ReadUInt32(buffer);     // NetMsgData.Head.SearchId, seems to either a PawnId or 0
            return obj;
        }

        private string GetMsgIdForDTI(RpcNetMsgDti type, ushort msgId)
        {
            string result;
            switch (type)
            {
                case RpcNetMsgDti.cNetMsgCtrlAction:
                    result = $"{(RpcMsgIdControl) msgId}";
                    break;
                case RpcNetMsgDti.cNetMsgSetNormal:
                    result = $"{(RpcMsgIdSetNormal)msgId}";
                    break;
                case RpcNetMsgDti.cNetMsgGameNormal:
                    result = $"{(RpcMsgIdGameNormal)msgId}";
                    break;
                case RpcNetMsgDti.cNetMsgGameEasy:
                    result = $"{(RpcMsgIdGameEasy)msgId}";
                    break;
                case RpcNetMsgDti.cNetMsgToolNormal:
                    result = $"{(RpcMsgIdToolNormal)msgId}";
                    break;
                case RpcNetMsgDti.cNetMsgToolEasy:
                    result = $"{(RpcMsgIdToolEasy)msgId}";
                    break;
                default:
                    result = $"Unknown (0x{msgId}:x)";
                    break;
            }

            return result;
        }

        public string AsString()
        {
            string MessageIdName = GetMsgIdForDTI(MsgDTI, MsgId);
            return $"Unk0=0x{Unk0:x}, Unk1=0x{Unk1:x} RpcId={RpcId}, MsgDTI={MsgDTI}, MessageId={MessageIdName}, SearchId={SearchId}";
        }
    }
}
