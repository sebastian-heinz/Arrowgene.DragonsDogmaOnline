using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterSetOnlineStatusRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_SET_ONLINE_STATUS_RES;

        public OnlineStatus OnlineStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterSetOnlineStatusRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterSetOnlineStatusRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.OnlineStatus);
            }

            public override S2CCharacterSetOnlineStatusRes Read(IBuffer buffer)
            {
                S2CCharacterSetOnlineStatusRes obj = new S2CCharacterSetOnlineStatusRes();
                ReadServerResponse(buffer, obj);
                obj.OnlineStatus = (OnlineStatus) ReadByte(buffer);
                return obj;
            }
        }
    }
}