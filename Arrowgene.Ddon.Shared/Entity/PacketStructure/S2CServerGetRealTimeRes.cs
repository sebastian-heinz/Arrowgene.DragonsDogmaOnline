using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CServerGetRealTimeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SERVER_GET_REAL_TIME_RES;

        public S2CServerGetRealTimeRes()
        {
            MTime=0;
            Msec=0;
        }

        public S2CServerGetRealTimeRes(DateTimeOffset dateTimeOffset)
        {
            MTime = (ulong) dateTimeOffset.ToUnixTimeSeconds();
            Msec = (ushort) dateTimeOffset.Millisecond;
        }

        public ulong MTime { get; set; }
        public ushort Msec { get; set; }

        public class Serializer : PacketEntitySerializer<S2CServerGetRealTimeRes>
        {
            public override void Write(IBuffer buffer, S2CServerGetRealTimeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.MTime);
                WriteUInt16(buffer, obj.Msec);
            }

            public override S2CServerGetRealTimeRes Read(IBuffer buffer)
            {
                S2CServerGetRealTimeRes obj = new S2CServerGetRealTimeRes();
                ReadServerResponse(buffer, obj);
                obj.MTime = ReadUInt64(buffer);
                obj.Msec = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}