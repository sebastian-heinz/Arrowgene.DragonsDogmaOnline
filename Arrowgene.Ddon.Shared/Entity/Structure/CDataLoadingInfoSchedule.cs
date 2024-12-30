using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLoadingInfoSchedule
    {
        public CDataLoadingInfoSchedule()
        {
            Text = string.Empty;
            BeginDateTime = DateTimeOffset.MinValue;
            EndDateTime = DateTimeOffset.MaxValue;
        }

        public string Text { get; set; }
        public uint ImageId { get; set; }
        public uint Priority { get; set; }
        public DateTimeOffset BeginDateTime { get; set; }
        public DateTimeOffset EndDateTime { get; set; }

        public class Serializer : EntitySerializer<CDataLoadingInfoSchedule>
        {
            public override void Write(IBuffer buffer, CDataLoadingInfoSchedule obj)
            {
                WriteMtString(buffer, obj.Text);
                WriteUInt32(buffer, obj.ImageId);
                WriteUInt32(buffer, obj.Priority);
                WriteInt64(buffer, obj.BeginDateTime.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.EndDateTime.ToUnixTimeSeconds());
            }

            public override CDataLoadingInfoSchedule Read(IBuffer buffer)
            {
                CDataLoadingInfoSchedule obj = new CDataLoadingInfoSchedule();
                obj.Text = ReadMtString(buffer);
                obj.ImageId = ReadUInt32(buffer);
                obj.Priority = ReadUInt32(buffer);
                obj.BeginDateTime = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.EndDateTime = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                return obj;
            }
        }
    }
}
