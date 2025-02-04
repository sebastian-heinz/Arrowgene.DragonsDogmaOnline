using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonBlockadeElement
    {
        public CDataSeasonDungeonBlockadeElement()
        {
            Name = string.Empty;
        }

        public uint EpitaphId { get; set; }
        public string Name { get; set; }

        public class Serializer : EntitySerializer<CDataSeasonDungeonBlockadeElement>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonBlockadeElement obj)
            {
                WriteUInt32(buffer, obj.EpitaphId);
                WriteMtString(buffer, obj.Name);
            }

            public override CDataSeasonDungeonBlockadeElement Read(IBuffer buffer)
            {
                CDataSeasonDungeonBlockadeElement obj = new CDataSeasonDungeonBlockadeElement();
                obj.EpitaphId = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

