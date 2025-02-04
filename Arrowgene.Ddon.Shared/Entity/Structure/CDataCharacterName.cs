using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataCharacterName
{
    public CDataCharacterName()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public class Serializer : EntitySerializer<CDataCharacterName>
    {
        public override void Write(IBuffer buffer, CDataCharacterName obj)
        {
            WriteMtString(buffer, obj.FirstName);
            WriteMtString(buffer, obj.LastName);
        }

        public override CDataCharacterName Read(IBuffer buffer)
        {
            CDataCharacterName obj = new CDataCharacterName();
            obj.FirstName = ReadMtString(buffer);
            obj.LastName = ReadMtString(buffer);
            return obj;
        }
    }
}
