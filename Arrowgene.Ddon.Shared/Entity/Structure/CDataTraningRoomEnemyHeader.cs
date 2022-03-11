using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTraningRoomEnemyHeader
    {
        public CDataTraningRoomEnemyHeader()
        {
            Id = 0;
            Name = string.Empty;
        }

        public uint Id { get; set; }
        public string Name { get; set; }

        public class Serializer : EntitySerializer<CDataTraningRoomEnemyHeader>
        {
            public override void Write(IBuffer buffer, CDataTraningRoomEnemyHeader obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteMtString(buffer, obj.Name);
            }

            public override CDataTraningRoomEnemyHeader Read(IBuffer buffer)
            {
                CDataTraningRoomEnemyHeader obj = new CDataTraningRoomEnemyHeader();
                obj.Id = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
