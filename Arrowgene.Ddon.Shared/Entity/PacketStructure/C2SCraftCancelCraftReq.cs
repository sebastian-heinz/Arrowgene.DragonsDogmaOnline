using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftCancelCraftReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_CANCEL_CRAFT_REQ;

        public uint CraftMainPawnID { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftCancelCraftReq>
        {
            public override void Write(IBuffer buffer, C2SCraftCancelCraftReq obj)
            {
                WriteUInt32(buffer, obj.CraftMainPawnID);
            }

            public override C2SCraftCancelCraftReq Read(IBuffer buffer)
            {
                C2SCraftCancelCraftReq obj = new C2SCraftCancelCraftReq();

                obj.CraftMainPawnID = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
