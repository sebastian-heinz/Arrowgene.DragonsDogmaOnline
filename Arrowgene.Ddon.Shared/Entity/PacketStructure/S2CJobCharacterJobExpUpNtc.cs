using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobCharacterJobExpUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHARACTER_JOB_EXP_UP_NTC;

        public S2CJobCharacterJobExpUpNtc()
        {
            JobId=0;
            AddExp=0;
            ExtraBonusExp=0;
            TotalExp=0;
            Type=0;
        }

        public JobId JobId;
        public uint AddExp;
        public uint ExtraBonusExp;
        public uint TotalExp;
        public byte Type;

        public class Serializer : PacketEntitySerializer<S2CJobCharacterJobExpUpNtc>
        {
            public override void Write(IBuffer buffer, S2CJobCharacterJobExpUpNtc obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteUInt32(buffer, obj.AddExp);
                WriteUInt32(buffer, obj.ExtraBonusExp);
                WriteUInt32(buffer, obj.TotalExp);
                WriteByte(buffer, obj.Type);
            }

            public override S2CJobCharacterJobExpUpNtc Read(IBuffer buffer)
            {
                S2CJobCharacterJobExpUpNtc obj = new S2CJobCharacterJobExpUpNtc();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.AddExp = ReadUInt32(buffer);
                obj.ExtraBonusExp = ReadUInt32(buffer);
                obj.TotalExp = ReadUInt32(buffer);
                obj.Type = ReadByte(buffer);
                return obj;
            }
        }
    }
}