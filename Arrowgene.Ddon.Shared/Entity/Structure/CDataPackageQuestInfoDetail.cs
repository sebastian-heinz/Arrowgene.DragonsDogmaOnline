using Arrowgene.Buffers;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPackageQuestInfoDetail
    {
        public CDataPackageQuestInfoDetail()
        {
        }

        public uint MissionId { get; set; } // Controls the text and image used
        public int CurrentProgress { get; set; }
        public int MaxProgress { get; set; }
        public bool Unk3 { get; set; } // Makes Progress Bar show up when Unk3 and Unk4 are set
        public bool Unk4 { get; set; } // Makes Progress Bar show up when Unk3 and Unk4 are set
        public bool IsComplete { get; set; }
        public DateTimeOffset Unk6 { get; set; }
        public List<CDataPackageQuestInfoDetailList> PackageQuestInfoDetailList = new();

        public class Serializer : EntitySerializer<CDataPackageQuestInfoDetail>
        {
            public override void Write(IBuffer buffer, CDataPackageQuestInfoDetail obj)
            {
                WriteUInt32(buffer, obj.MissionId);
                WriteInt32(buffer, obj.CurrentProgress);
                WriteInt32(buffer, obj.MaxProgress);
                WriteBool(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteBool(buffer, obj.IsComplete);
                WriteInt64(buffer, obj.Unk6.ToUnixTimeSeconds());
                WriteEntityList(buffer, obj.PackageQuestInfoDetailList);
            }

            public override CDataPackageQuestInfoDetail Read(IBuffer buffer)
            {
                CDataPackageQuestInfoDetail obj = new CDataPackageQuestInfoDetail();
                obj.MissionId = ReadUInt32(buffer);
                obj.CurrentProgress = ReadInt32(buffer);
                obj.MaxProgress = ReadInt32(buffer);
                obj.Unk3 = ReadBool(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.IsComplete = ReadBool(buffer);
                obj.Unk6 = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.PackageQuestInfoDetailList = ReadEntityList<CDataPackageQuestInfoDetailList>(buffer);
                return obj;
            }
        }
    }
}

