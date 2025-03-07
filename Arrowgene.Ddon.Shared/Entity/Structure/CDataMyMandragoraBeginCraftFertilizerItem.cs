using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraBeginCraftFertilizerItem
    {
        /// <summary>
        /// ItemUID, taken from the lefthand side of the menu where you choose fertilizer items
        /// </summary>
        public string ItemUID { get; set; } = string.Empty;
        /// <summary>
        /// Always 1
        /// </summary>
        public byte ItemNum { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraBeginCraftFertilizerItem>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraBeginCraftFertilizerItem obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteByte(buffer, obj.ItemNum);
            }

            public override CDataMyMandragoraBeginCraftFertilizerItem Read(IBuffer buffer)
            {
                CDataMyMandragoraBeginCraftFertilizerItem obj = new CDataMyMandragoraBeginCraftFertilizerItem();
                obj.ItemUID = ReadMtString(buffer);
                obj.ItemNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
