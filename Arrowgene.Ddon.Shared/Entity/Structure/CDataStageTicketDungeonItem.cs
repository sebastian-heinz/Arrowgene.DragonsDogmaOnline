using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageTicketDungeonItem
    {
        public CDataStageTicketDungeonItem()
        {
        }

        public uint ItemId { get; set; }
        public ushort Num { get; set; }

        public class Serializer : EntitySerializer<CDataStageTicketDungeonItem>
        {
            public override void Write(IBuffer buffer, CDataStageTicketDungeonItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }

            public override CDataStageTicketDungeonItem Read(IBuffer buffer)
            {
                CDataStageTicketDungeonItem obj = new CDataStageTicketDungeonItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
