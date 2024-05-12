using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    public class RpcLoginPacket : RpcPacketBase
    {
        public RpcLoginPacket()
        {
            Unk0 = new byte[6];
        }

        public byte[] Unk0 { get; set; }
        public UInt16 StageNo { get; set; }

        public override void Handle(Character character, RpcPacketHeader Header, IBuffer buffer)
        {
            // TODO: Should we save the stage ID when logging in somewhere?
        }
        private RpcLoginPacket ReadPacketData(IBuffer buffer)
        {
            RpcLoginPacket obj = new RpcLoginPacket();
            obj.Unk0 = ReadBytes(buffer, Unk0.Length);
            obj.StageNo = ReadUInt16(buffer);
            return obj;
        }
    }
}
