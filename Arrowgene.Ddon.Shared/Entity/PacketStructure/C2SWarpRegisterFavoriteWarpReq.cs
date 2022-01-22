using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SWarpRegisterFavoriteWarpReq
    {
        public C2SWarpRegisterFavoriteWarpReq() {
            slotNo = 0;
            warpPointID = 0;
        }

        public uint slotNo;
        public uint warpPointID;
    }

    public class C2SWarpRegisterFavoriteWarpReqSerializer : EntitySerializer<C2SWarpRegisterFavoriteWarpReq>
    {
        public override void Write(IBuffer buffer, C2SWarpRegisterFavoriteWarpReq obj)
        {
            WriteUInt32(buffer, obj.slotNo);
            WriteUInt32(buffer, obj.warpPointID);
        }

        public override C2SWarpRegisterFavoriteWarpReq Read(IBuffer buffer)
        {
            C2SWarpRegisterFavoriteWarpReq obj = new C2SWarpRegisterFavoriteWarpReq();
            obj.slotNo = ReadUInt32(buffer);
            obj.warpPointID = ReadUInt32(buffer);
            return obj;
        }
    }
}