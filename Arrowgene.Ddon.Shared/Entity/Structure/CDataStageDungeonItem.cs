using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageDungeonItem
    {
        public CDataStageDungeonItem()
        {
        }

        public uint ItemId { get; set; }
        public ushort Num { get; set; }

        public class Serializer : EntitySerializer<CDataStageDungeonItem>
        {
            public override void Write(IBuffer buffer, CDataStageDungeonItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }

            public override CDataStageDungeonItem Read(IBuffer buffer)
            {
                CDataStageDungeonItem obj = new CDataStageDungeonItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
