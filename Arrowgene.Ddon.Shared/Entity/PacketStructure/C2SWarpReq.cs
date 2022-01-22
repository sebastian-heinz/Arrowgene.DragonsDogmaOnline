using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpReq {

        public C2SWarpReq() {
            currentPointID = 0;
            destPointID = 0;
            price = 0;
        }

        public uint currentPointID;
        public uint destPointID;
        public uint price;

    }

    public class C2SWarpReqSerializer : EntitySerializer<C2SWarpReq> {
        public override void Write(IBuffer buffer, C2SWarpReq obj)
        {
            WriteUInt32(buffer, obj.currentPointID);
            WriteUInt32(buffer, obj.destPointID);
            WriteUInt32(buffer, obj.price);
        }

        public override C2SWarpReq Read(IBuffer buffer)
        {
            C2SWarpReq obj = new C2SWarpReq();
            obj.currentPointID = ReadUInt32(buffer);
            obj.destPointID = ReadUInt32(buffer);
            obj.price = ReadUInt32(buffer);
            return obj;
        }
    }
}