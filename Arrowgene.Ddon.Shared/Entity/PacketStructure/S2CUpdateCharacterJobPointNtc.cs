using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CUpdateCharacterJobPointNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHARACTER_JOB_POINT_NTC;

        public S2CUpdateCharacterJobPointNtc()
        {
        }

        public JobId Job { get; set; }
        public uint AddJobPoint { get; set; }
        public uint ExtraBonusJobPoint { get; set; }
        public uint TotalJobPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CUpdateCharacterJobPointNtc>
        {
            public override void Write(IBuffer buffer, S2CUpdateCharacterJobPointNtc obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.AddJobPoint);
                WriteUInt32(buffer, obj.ExtraBonusJobPoint);
                WriteUInt32(buffer, obj.TotalJobPoint);
            }

            public override S2CUpdateCharacterJobPointNtc Read(IBuffer buffer)
            {
                S2CUpdateCharacterJobPointNtc obj = new S2CUpdateCharacterJobPointNtc();
                obj.Job = (JobId) ReadByte(buffer);
                obj.AddJobPoint = ReadUInt32(buffer);
                obj.ExtraBonusJobPoint = ReadUInt32(buffer);
                obj.TotalJobPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
