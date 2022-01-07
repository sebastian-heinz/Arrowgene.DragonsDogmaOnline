using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataGPCourseValid
    {
        uint ID;
        uint CourseID;
        // length prefix
        string NameA;
        // length prefix
        string NameB;
        ulong StartTime;
        ulong EndTime;
    }

    public class CDataGPCourseValidSerializer : EntitySerializer<CDataGPCourseValid>
    {
        public override void Write(IBuffer buffer, CDataGPCourseValid obj)
        {
            throw new NotImplementedException();
        }

        public override CDataGPCourseValid Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
