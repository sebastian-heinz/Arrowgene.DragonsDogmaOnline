using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterSearchParam
    {
        public CDataCharacterSearchParam()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterSearchParam>
        {
            public override void Write(IBuffer buffer, CDataCharacterSearchParam obj)
            {
                WriteMtString(buffer, obj.FirstName);
                WriteMtString(buffer, obj.LastName);
            }

            public override CDataCharacterSearchParam Read(IBuffer buffer)
            {
                CDataCharacterSearchParam obj = new CDataCharacterSearchParam();
                obj.FirstName = ReadMtString(buffer);
                obj.LastName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
