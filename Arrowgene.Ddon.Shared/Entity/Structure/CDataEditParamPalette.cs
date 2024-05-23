using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEditParamPalette
    {    
        public uint ParamTypeId { get; set; }
        public uint UID { get; set; }
    
        public class Serializer : EntitySerializer<CDataEditParamPalette>
        {
            public override void Write(IBuffer buffer, CDataEditParamPalette obj)
            {
                WriteUInt32(buffer, obj.ParamTypeId);
                WriteUInt32(buffer, obj.UID);
            }
        
            public override CDataEditParamPalette Read(IBuffer buffer)
            {
                CDataEditParamPalette obj = new CDataEditParamPalette();
                obj.ParamTypeId = ReadUInt32(buffer);
                obj.UID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}