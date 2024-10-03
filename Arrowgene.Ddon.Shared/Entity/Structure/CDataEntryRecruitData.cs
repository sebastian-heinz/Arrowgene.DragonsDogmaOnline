using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryRecruitData
    {
        public CDataEntryRecruitData()
        {
            EnableJobList = new List<CDataEntryRecruitJob>();
        }

        public ushort Id { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataEntryRecruitJob> EnableJobList {  get; set; }

        public class Serializer : EntitySerializer<CDataEntryRecruitData>
        {
            public override void Write(IBuffer buffer, CDataEntryRecruitData obj)
            {
                WriteUInt16(buffer, obj.Id);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.EnableJobList);
            }

            public override CDataEntryRecruitData Read(IBuffer buffer)
            {
                CDataEntryRecruitData obj = new CDataEntryRecruitData();
                obj.Id = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.EnableJobList = ReadEntityList<CDataEntryRecruitJob>(buffer);
                return obj;
            }
        }
    }
}
