using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageTicketDungeonCategory
    {
        public CDataStageTicketDungeonCategory()
        {
            CategoryName = string.Empty;
        }

        public uint CategoryId { get; set; }
        public string CategoryName { get; set; }

        public class Serializer : EntitySerializer<CDataStageTicketDungeonCategory>
        {
            public override void Write(IBuffer buffer, CDataStageTicketDungeonCategory obj)
            {
                WriteUInt32(buffer, obj.CategoryId);
                WriteMtString(buffer, obj.CategoryName);
            }

            public override CDataStageTicketDungeonCategory Read(IBuffer buffer)
            {
                CDataStageTicketDungeonCategory obj = new CDataStageTicketDungeonCategory();
                obj.CategoryId = ReadUInt32(buffer);
                obj.CategoryName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
