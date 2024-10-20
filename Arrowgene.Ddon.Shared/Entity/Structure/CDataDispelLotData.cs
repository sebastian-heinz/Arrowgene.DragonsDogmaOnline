using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLotData
    {
        public CDataDispelLotData()
        {
            ItemLot = new CDataDispelLotItem();
            CrestLot = new List<CDataDispelLotCrest>();
            ColorLot = new List<CDataDispelLotColor>();
            PlusLot = new List<CDataDispelLotPlus>();
        }

        public uint ItemId { get; set; }
        public CDataDispelLotItem ItemLot {  get; set; }
        public List<CDataDispelLotCrest> CrestLot {  get; set; }
        public List<CDataDispelLotColor> ColorLot {  get; set; }
        public List<CDataDispelLotPlus> PlusLot { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLotData>
        {
            public override void Write(IBuffer buffer, CDataDispelLotData obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteEntity(buffer, obj.ItemLot);
                WriteEntityList(buffer, obj.CrestLot);
                WriteEntityList(buffer, obj.ColorLot);
                WriteEntityList(buffer, obj.PlusLot);
            }

            public override CDataDispelLotData Read(IBuffer buffer)
            {
                CDataDispelLotData obj = new CDataDispelLotData();
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemLot = ReadEntity<CDataDispelLotItem>(buffer);
                obj.CrestLot = ReadEntityList<CDataDispelLotCrest>(buffer);
                obj.ColorLot = ReadEntityList<CDataDispelLotColor>(buffer);
                obj.PlusLot = ReadEntityList<CDataDispelLotPlus>(buffer);
                return obj;
            }
        }
    }
}

