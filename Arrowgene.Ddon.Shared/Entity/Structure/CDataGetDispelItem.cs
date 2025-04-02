using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataGetDispelItem
{
    public CDataGetDispelItem()
    {
        UIDList = new List<CDataItemUIDList>();
    }

    public byte StorageType { get; set; } // Storage where the item exists
    public uint Id { get; set; } // Corresponds with the ID field in CDataDispelBaseItem
    public uint Unk1 { get; set; }
    public List<CDataItemUIDList> UIDList { get; set; } // Items consumed in the exchange

    public class Serializer : EntitySerializer<CDataGetDispelItem>
    {
        public override void Write(IBuffer buffer, CDataGetDispelItem obj)
        {
            WriteByte(buffer, obj.StorageType);
            WriteUInt32(buffer, obj.Id);
            WriteUInt32(buffer, obj.Unk1);
            WriteEntityList(buffer, obj.UIDList);
        }

        public override CDataGetDispelItem Read(IBuffer buffer)
        {
            CDataGetDispelItem obj = new CDataGetDispelItem();
            obj.StorageType = ReadByte(buffer);
            obj.Id = ReadUInt32(buffer);
            obj.Unk1 = ReadUInt32(buffer);
            obj.UIDList = ReadEntityList<CDataItemUIDList>(buffer);
            return obj;
        }
    }
}

