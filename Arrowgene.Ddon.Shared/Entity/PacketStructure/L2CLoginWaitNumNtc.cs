using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CLoginWaitNumNtc : IPacketStructure
    {
        public PacketId Id => PacketId.L2C_LOGIN_WAIT_NUM_NTC;
        
        public uint Unknown { get; set; }
        
        public class Serializer : PacketEntitySerializer<L2CLoginWaitNumNtc>
        {

            public override void Write(IBuffer buffer, L2CLoginWaitNumNtc obj)
            {
                WriteUInt32(buffer, obj.Unknown);
            }

            public override L2CLoginWaitNumNtc Read(IBuffer buffer)
            {
                L2CLoginWaitNumNtc obj = new L2CLoginWaitNumNtc();
                obj.Unknown = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
