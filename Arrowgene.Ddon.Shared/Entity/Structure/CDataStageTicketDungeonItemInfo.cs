using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStageTicketDungeonItemInfo
    {
        public CDataStageTicketDungeonItemInfo()
        {
        }

        public string ItemUid { get; set; } = string.Empty;
        public uint Num { get; set; }

        public class Serializer : EntitySerializer<CDataStageTicketDungeonItemInfo>
        {
            public override void Write(IBuffer buffer, CDataStageTicketDungeonItemInfo obj)
            {
                WriteMtString(buffer, obj.ItemUid);
                WriteUInt32(buffer, obj.Num);
            }

            public override CDataStageTicketDungeonItemInfo Read(IBuffer buffer)
            {
                CDataStageTicketDungeonItemInfo obj = new CDataStageTicketDungeonItemInfo();
                obj.ItemUid = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
