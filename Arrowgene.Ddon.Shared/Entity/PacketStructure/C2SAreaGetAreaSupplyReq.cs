using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaSupplyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_SUPPLY_REQ;

        public C2SAreaGetAreaSupplyReq()
        {
            SelectItemInfoList = new();
        }

        public uint AreaId { get; set; }
        public StorageType StorageType { get; set; }
        public List<CDataSelectItemInfo> SelectItemInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaSupplyReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaSupplyReq obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteByte(buffer, (byte)obj.StorageType);
                WriteEntityList(buffer, obj.SelectItemInfoList);
            }

            public override C2SAreaGetAreaSupplyReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaSupplyReq obj = new C2SAreaGetAreaSupplyReq();
                obj.AreaId = ReadUInt32(buffer);
                obj.StorageType = (StorageType)ReadByte(buffer);
                obj.SelectItemInfoList = ReadEntityList<CDataSelectItemInfo>(buffer);
                return obj;
            }
        }
    }
}
