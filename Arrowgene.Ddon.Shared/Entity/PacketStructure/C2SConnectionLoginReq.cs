using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionLoginReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_LOGIN_REQ;

        public C2SConnectionLoginReq()
        {
            SessionKey = string.Empty;
        }

        public string SessionKey { get; set; }
        
        public PlatformType PlatformType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SConnectionLoginReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionLoginReq obj)
            {
                WriteMtString(buffer, obj.SessionKey);
                buffer.WriteEnumByte(obj.PlatformType);
            }

            public override C2SConnectionLoginReq Read(IBuffer buffer)
            {
                C2SConnectionLoginReq obj = new C2SConnectionLoginReq();
                obj.SessionKey = ReadMtString(buffer);
                if (!buffer.ReadEnumByte(out PlatformType platformType))
                {
                    platformType = PlatformType.None;
                }

                obj.PlatformType = platformType;
                return obj;
            }
        }
    }
}
