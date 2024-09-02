using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEditParam
    {
        public CDataEditParam() {
            EditParamPalette = new CDataEditParamPalette();
        }
    
        public CDataEditParamPalette EditParamPalette { get; set; }
        public bool IsValid { get; set; }
        public bool IsGP { get; set; }
        public bool IsPitchEnable { get; set; }
    
        public class Serializer : EntitySerializer<CDataEditParam>
        {
            public override void Write(IBuffer buffer, CDataEditParam obj)
            {
                WriteEntity<CDataEditParamPalette>(buffer, obj.EditParamPalette);
                WriteBool(buffer, obj.IsValid);
                WriteBool(buffer, obj.IsGP);
                WriteBool(buffer, obj.IsPitchEnable);
            }
        
            public override CDataEditParam Read(IBuffer buffer)
            {
                CDataEditParam obj = new CDataEditParam();
                obj.EditParamPalette = ReadEntity<CDataEditParamPalette>(buffer);
                obj.IsValid = ReadBool(buffer);
                obj.IsGP = ReadBool(buffer);
                obj.IsPitchEnable = ReadBool(buffer);
                return obj;
            }
        }
    }
}