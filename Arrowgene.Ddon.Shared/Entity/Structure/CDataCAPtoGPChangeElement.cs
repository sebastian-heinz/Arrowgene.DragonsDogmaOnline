using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCAPtoGPChangeElement
    {
        public uint ID { get; set; }
        public uint CAP { get; set; }
        public uint GP { get; set; }
        public string Comment { get; set; }
        public uint BackIconID { get; set; }
        public uint FrameIconID { get; set; }
    
        public class Serializer : EntitySerializer<CDataCAPtoGPChangeElement>
        {
            public override void Write(IBuffer buffer, CDataCAPtoGPChangeElement obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteUInt32(buffer, obj.CAP);
                WriteUInt32(buffer, obj.GP);
                WriteMtString(buffer, obj.Comment);
                WriteUInt32(buffer, obj.BackIconID);
                WriteUInt32(buffer, obj.FrameIconID);
            }
        
            public override CDataCAPtoGPChangeElement Read(IBuffer buffer)
            {
                CDataCAPtoGPChangeElement obj = new CDataCAPtoGPChangeElement();
                obj.ID = ReadUInt32(buffer);
                obj.CAP = ReadUInt32(buffer);
                obj.GP = ReadUInt32(buffer);
                obj.Comment = ReadMtString(buffer);
                obj.BackIconID = ReadUInt32(buffer);
                obj.FrameIconID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
