using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetExRequiredItemReq : IPacketStructure
    {
        public C2SSeasonDungeonGetExRequiredItemReq()
        {
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_REQ;

        public uint EpitaphId { get; set; } // Value returned by S2CSeasonDungeonGetBlockadeIdFromOmRes

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetExRequiredItemReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetExRequiredItemReq obj)
            {
                WriteUInt32(buffer, obj.EpitaphId);
            }

            public override C2SSeasonDungeonGetExRequiredItemReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetExRequiredItemReq obj = new C2SSeasonDungeonGetExRequiredItemReq();
                obj.EpitaphId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
