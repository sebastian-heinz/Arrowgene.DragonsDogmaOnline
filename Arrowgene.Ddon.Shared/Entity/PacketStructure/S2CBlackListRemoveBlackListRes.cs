using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBlackListRemoveBlackListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BLACK_LIST_REMOVE_BLACK_LIST_RES;

        public uint RemoveCharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBlackListRemoveBlackListRes>
        {
            public override void Write(IBuffer buffer, S2CBlackListRemoveBlackListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.RemoveCharacterId);
            }

            public override S2CBlackListRemoveBlackListRes Read(IBuffer buffer)
            {
                S2CBlackListRemoveBlackListRes obj = new S2CBlackListRemoveBlackListRes();
                ReadServerResponse(buffer, obj);
                obj.RemoveCharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

