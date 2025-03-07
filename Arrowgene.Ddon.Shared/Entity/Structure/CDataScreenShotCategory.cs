using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataScreenShotCategory
    {
        public uint Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public class Serializer : EntitySerializer<CDataScreenShotCategory>
        {
            public override void Write(IBuffer buffer, CDataScreenShotCategory obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteMtString(buffer, obj.Name);
            }

            public override CDataScreenShotCategory Read(IBuffer buffer)
            {
                CDataScreenShotCategory obj = new CDataScreenShotCategory();
                obj.Id = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
