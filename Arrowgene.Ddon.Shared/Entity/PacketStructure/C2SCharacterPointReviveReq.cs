using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterPointReviveReq
    {
        public C2SCharacterPointReviveReq()
        {
            hpMax = 0;
        }

        public uint hpMax;
    }

    public class C2SCharacterPointReviveReqSerializer : EntitySerializer<C2SCharacterPointReviveReq>
    {
        public override void Write(IBuffer buffer, C2SCharacterPointReviveReq obj)
        {
            WriteUInt32(buffer, obj.hpMax);
        }

        public override C2SCharacterPointReviveReq Read(IBuffer buffer)
        {
            C2SCharacterPointReviveReq obj = new C2SCharacterPointReviveReq();
            obj.hpMax = ReadUInt32(buffer);
            return obj;
        }
    }
}