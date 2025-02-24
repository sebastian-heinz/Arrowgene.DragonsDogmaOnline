using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionLoginReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_LOGIN_REQ;

        public string SessionKey { get; set; } = string.Empty;
        
        public PlatformType PlatformType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SConnectionLoginReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionLoginReq obj)
            {
                WriteMtString(buffer, obj.SessionKey);
                WriteByte(buffer, (byte)obj.PlatformType);
            }

            public override C2SConnectionLoginReq Read(IBuffer buffer)
            {
                C2SConnectionLoginReq obj = new C2SConnectionLoginReq();
                obj.SessionKey = ReadMtString(buffer);
                obj.PlatformType = (PlatformType)ReadByte(buffer);
                return obj;
            }
        }
    }
}
