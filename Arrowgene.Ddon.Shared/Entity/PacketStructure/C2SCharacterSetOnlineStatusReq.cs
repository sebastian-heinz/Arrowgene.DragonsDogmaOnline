using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterSetOnlineStatusReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_SET_ONLINE_STATUS_REQ;

        public OnlineStatus OnlineStatus { get; set; }
        public bool IsSaveSetting { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterSetOnlineStatusReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterSetOnlineStatusReq obj)
            {
                WriteByte(buffer, (byte) obj.OnlineStatus);
                WriteBool(buffer, obj.IsSaveSetting);
            }

            public override C2SCharacterSetOnlineStatusReq Read(IBuffer buffer)
            {
                C2SCharacterSetOnlineStatusReq obj = new C2SCharacterSetOnlineStatusReq();
                obj.OnlineStatus = (OnlineStatus) ReadByte(buffer);
                obj.IsSaveSetting = ReadBool(buffer);
                return obj;
            }
        }
    }
}