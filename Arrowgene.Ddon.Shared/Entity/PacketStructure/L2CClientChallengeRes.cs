using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CClientChallengeRes : ServerResponse
    {
        public L2CClientChallengeRes()
        {
            PasswordEnc = new byte[16];
            Padding = new byte[48];
        }

        public override PacketId Id => PacketId.L2C_CLIENT_CHALLENGE_RES;

        public byte PasswordSrcSize { get; set; }
        public byte PasswordEncSize { get; set; }
        public byte[] PasswordEnc {  get; set; }
        private byte[] Padding { get; set; }

        public class Serializer : PacketEntitySerializer<L2CClientChallengeRes>
        {

            public override void Write(IBuffer buffer, L2CClientChallengeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.PasswordSrcSize);
                WriteByte(buffer, obj.PasswordEncSize);
                WriteByteArray(buffer, obj.PasswordEnc);
                WriteByteArray(buffer, obj.Padding);
            }

            public override L2CClientChallengeRes Read(IBuffer buffer)
            {
                L2CClientChallengeRes obj = new L2CClientChallengeRes();
                ReadServerResponse(buffer, obj);
                obj.PasswordSrcSize = ReadByte(buffer);
                obj.PasswordEncSize = ReadByte(buffer);
                obj.PasswordEnc = ReadByteArray(buffer, 16);
                return obj;
            }
        }
    }
}
