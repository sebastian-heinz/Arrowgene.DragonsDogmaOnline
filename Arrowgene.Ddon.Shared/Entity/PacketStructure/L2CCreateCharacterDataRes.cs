using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CCreateCharacterDataRes : ServerResponse
    {
        /// <summary>
        /// How many people you are waiting for until your character registers
        /// </summary>
        public uint WaitNum { get; set; }

        public override PacketId Id => PacketId.L2C_CREATE_CHARACTER_DATA_RES;

        public class Serializer : PacketEntitySerializer<L2CCreateCharacterDataRes>
        {
            public override void Write(IBuffer buffer, L2CCreateCharacterDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.WaitNum);
            }

            public override L2CCreateCharacterDataRes Read(IBuffer buffer)
            {
                L2CCreateCharacterDataRes obj = new L2CCreateCharacterDataRes();
                ReadServerResponse(buffer, obj);
                obj.WaitNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
