using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftResetCraftpointReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_RESET_CRAFTPOINT_REQ;

        public uint PawnID { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftResetCraftpointReq>
        {
            public override void Write(IBuffer buffer, C2SCraftResetCraftpointReq obj)
            {
                WriteUInt32(buffer, obj.PawnID);
            }

            public override C2SCraftResetCraftpointReq Read(IBuffer buffer)
            {
                C2SCraftResetCraftpointReq obj = new C2SCraftResetCraftpointReq();

                obj.PawnID = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
