using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTraningRoomEnemyHeader
    {
        public CDataTraningRoomEnemyHeader()
        {
            ID = 0;
            StrName = string.Empty;
        }

        public uint ID { get; set; }
        public string StrName { get; set; }

        public class Serializer : EntitySerializer<CDataTraningRoomEnemyHeader>
        {
            public override void Write(IBuffer buffer, CDataTraningRoomEnemyHeader obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteMtString(buffer, obj.StrName);
            }

            public override CDataTraningRoomEnemyHeader Read(IBuffer buffer)
            {
                CDataTraningRoomEnemyHeader obj = new CDataTraningRoomEnemyHeader();
                obj.ID = ReadUInt32(buffer);
                obj.StrName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}