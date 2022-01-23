using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterGoldenReviveReq
    {
        public C2SCharacterGoldenReviveReq()
        {
            hpMax = 0;
        }

        public uint hpMax;
    }

    public class C2SCharacterGoldenReviveReqSerializer : EntitySerializer<C2SCharacterGoldenReviveReq>
    {
        public override void Write(IBuffer buffer, C2SCharacterGoldenReviveReq obj)
        {
            WriteUInt32(buffer, obj.hpMax);
        }

        public override C2SCharacterGoldenReviveReq Read(IBuffer buffer)
        {
            C2SCharacterGoldenReviveReq obj = new C2SCharacterGoldenReviveReq();
            obj.hpMax = ReadUInt32(buffer);
            return obj;
        }
    }
}