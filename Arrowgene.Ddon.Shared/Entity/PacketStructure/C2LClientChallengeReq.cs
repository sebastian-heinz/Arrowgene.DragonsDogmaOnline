using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LClientChallengeReq : IPacketStructure
    {
        public C2LClientChallengeReq()
        {
            CommonKeyEnc = new byte[256];
            PasswordEnc = new byte[62];
        }

        public PacketId Id => PacketId.C2L_CLIENT_CHALLENGE_REQ;

        public byte CommonKeySrcSize { get; set; }
        public byte[] CommonKeyEnc { get; set; }
        public byte PasswordSrcSize { get; set; }
        public byte PasswordEncSize { get; set; }
        public byte[] PasswordEnc { get; set; }


        public class Serializer : PacketEntitySerializer<C2LClientChallengeReq>
        {
            public override void Write(IBuffer buffer, C2LClientChallengeReq obj)
            {
                WriteByte(buffer, obj.CommonKeySrcSize);
                WriteByteArray(buffer, obj.CommonKeyEnc);
                WriteByteArray(buffer, new byte[3]); // Padding
                WriteByte(buffer, obj.PasswordSrcSize);
                WriteByte(buffer, obj.PasswordEncSize);
                WriteByteArray(buffer, obj.PasswordEnc);
            }

            public override C2LClientChallengeReq Read(IBuffer buffer)
            {
                C2LClientChallengeReq obj = new C2LClientChallengeReq();
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
