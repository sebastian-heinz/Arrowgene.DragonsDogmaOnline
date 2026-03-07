using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetGameSessionKeyRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_GET_GAME_SESSION_KEY_RES;

        public string SessionKey { get; set; } = string.Empty; // MtString
        public ushort GameServerUniqueID { get; set; } // 0 means some error during the login occurred

        public class Serializer : PacketEntitySerializer<L2CGetGameSessionKeyRes>
        {

            public override void Write(IBuffer buffer, L2CGetGameSessionKeyRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteMtString(buffer, obj.SessionKey);
                WriteUInt16(buffer, obj.GameServerUniqueID);
            }

            public override L2CGetGameSessionKeyRes Read(IBuffer buffer)
            {
                L2CGetGameSessionKeyRes obj = new L2CGetGameSessionKeyRes();
                ReadServerResponse(buffer, obj);
                obj.SessionKey = ReadMtString(buffer);
                obj.GameServerUniqueID = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
