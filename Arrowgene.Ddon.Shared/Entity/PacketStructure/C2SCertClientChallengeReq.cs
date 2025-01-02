using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCertClientChallengeReq : IPacketStructure
    {
        public C2SCertClientChallengeReq()
        {
            CommonKeyEnc = new byte[256];
            PasswordEnc = new byte[62];
        }

        public PacketId Id => PacketId.C2S_CERT_CLIENT_CHALLENGE_REQ;

        public byte CommonKeySrcSize { get; set; }
        public byte[] CommonKeyEnc { get; set; }
        public byte PasswordSrcSize { get; set; }
        public byte PasswordEncSize { get; set; }
        public byte[] PasswordEnc { get; set; }


        public class Serializer : PacketEntitySerializer<C2SCertClientChallengeReq>
        {
            public override void Write(IBuffer buffer, C2SCertClientChallengeReq obj)
            {
                WriteByte(buffer, obj.CommonKeySrcSize);
                WriteByteArray(buffer, obj.CommonKeyEnc);
                WriteByteArray(buffer, new byte[3]); // Padding
                WriteByte(buffer, obj.PasswordSrcSize);
                WriteByte(buffer, obj.PasswordEncSize);
                WriteByteArray(buffer, obj.PasswordEnc);
            }

            public override C2SCertClientChallengeReq Read(IBuffer buffer)
            {
                C2SCertClientChallengeReq obj = new C2SCertClientChallengeReq();
                obj.CommonKeySrcSize = ReadByte(buffer);
                obj.CommonKeyEnc = ReadByteArray(buffer, 256);
                buffer.ReadBytes(3); // Padding?
                obj.PasswordSrcSize = ReadByte(buffer);
                obj.PasswordEncSize = ReadByte(buffer);
                obj.PasswordEnc = buffer.ReadBytesTerminated(0);
                return obj;
            }
        }
    }
}
