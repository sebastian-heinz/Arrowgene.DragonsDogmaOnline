using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CDecideCharacterIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_DECIDE_CHARACTER_ID_RES;

        public uint CharacterId { get; set; }
        public uint WaitNum { get; set; }
        
        public class Serializer : PacketEntitySerializer<L2CDecideCharacterIdRes>
        {

            public override void Write(IBuffer buffer, L2CDecideCharacterIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.WaitNum);
            }

            public override L2CDecideCharacterIdRes Read(IBuffer buffer)
            {
                L2CDecideCharacterIdRes obj = new L2CDecideCharacterIdRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.WaitNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
