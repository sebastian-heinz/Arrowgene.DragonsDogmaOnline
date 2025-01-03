using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionKickNtc : ServerResponse
    {
        public S2CConnectionKickNtc()
        {
            Text = string.Empty;
        }

        public override PacketId Id => PacketId.S2C_CONNECTION_KICK_NTC;

        public string Text { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionKickNtc>
        {
            public override void Write(IBuffer buffer, S2CConnectionKickNtc obj)
            {
                WriteMtString(buffer, obj.Text);
            }

            public override S2CConnectionKickNtc Read(IBuffer buffer)
            {
                S2CConnectionKickNtc obj = new S2CConnectionKickNtc();
                obj.Text = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
