using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobValueShopItem
    {
        public uint LineupId;
        public uint ItemId;
        public uint Price;
        public bool IsCountLimit;
        public bool CanSelectStorage;
        public byte UnableReason;

        public class Serializer : EntitySerializer<CDataJobValueShopItem>
        {
            public override void Write(IBuffer buffer, CDataJobValueShopItem obj)
            {
                WriteUInt32(buffer, obj.LineupId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.Price);
                WriteBool(buffer, obj.IsCountLimit);
                WriteBool(buffer, obj.CanSelectStorage);
                WriteByte(buffer, obj.UnableReason);
            }

            public override CDataJobValueShopItem Read(IBuffer buffer)
            {
                CDataJobValueShopItem obj = new CDataJobValueShopItem();
                obj.LineupId = buffer.ReadUInt32();
                obj.ItemId = buffer.ReadUInt32();
                obj.Price = buffer.ReadUInt32();
                obj.IsCountLimit = buffer.ReadBool();
                obj.CanSelectStorage = buffer.ReadBool();
                obj.UnableReason = buffer.ReadByte();
                return obj;
            }
        }
    }
}
