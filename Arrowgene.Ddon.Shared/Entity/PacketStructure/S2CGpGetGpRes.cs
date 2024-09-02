using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGetGpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GET_GP_RES;

        public S2CGpGetGpRes()
        {
            RealTime = DateTimeOffset.MinValue;
        }

        public uint GP { get; set; }
        public long UseLimit { get; set; }
        public DateTimeOffset RealTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGetGpRes>
        {
            public override void Write(IBuffer buffer, S2CGpGetGpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.GP);
                WriteInt64(buffer, obj.UseLimit);
                WriteUInt64(buffer, (ulong) obj.RealTime.ToUnixTimeSeconds());
                WriteUInt16(buffer, (ushort) obj.RealTime.Millisecond);
            }

            public override S2CGpGetGpRes Read(IBuffer buffer)
            {
                S2CGpGetGpRes obj = new S2CGpGetGpRes();
                ReadServerResponse(buffer, obj);
                obj.GP = ReadUInt32(buffer);
                obj.UseLimit = ReadInt64(buffer);
                ulong unixTimeSeconds = ReadUInt64(buffer);
                ushort unixTimeMilliseconds = ReadUInt16(buffer);
                obj.RealTime = DateTimeOffset.FromUnixTimeSeconds((long) unixTimeSeconds).AddMilliseconds(unixTimeMilliseconds);
                return obj;
            }
        }
    }
}