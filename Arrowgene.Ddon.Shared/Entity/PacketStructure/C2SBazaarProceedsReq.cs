using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarProceedsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_PROCEEDS_REQ;

        public C2SBazaarProceedsReq()
        {
            ItemStorageIndicateNum = new List<CDataItemStorageIndicateNum>();
        }

        public ulong BazaarId { get; set; }
        public uint ItemId { get; set; }
        public ushort Unk0 { get; set; }
        public List<CDataItemStorageIndicateNum> ItemStorageIndicateNum { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarProceedsReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarProceedsReq obj)
            {
                WriteUInt64(buffer, obj.BazaarId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Unk0);
                WriteEntityList<CDataItemStorageIndicateNum>(buffer, obj.ItemStorageIndicateNum);
            }

            public override C2SBazaarProceedsReq Read(IBuffer buffer)
            {
                C2SBazaarProceedsReq obj = new C2SBazaarProceedsReq();
                obj.BazaarId = ReadUInt64(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                obj.ItemStorageIndicateNum = ReadEntityList<CDataItemStorageIndicateNum>(buffer);
                return obj;
            }
        }

    }
}