using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanSetFurnitureReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_SET_FURNITURE_REQ;

        public List<CDataFurnitureLayoutData> FurnitureLayoutData { get; set; }

        public C2SClanSetFurnitureReq()
        {
            FurnitureLayoutData = new();
        }

        public class Serializer : PacketEntitySerializer<C2SClanSetFurnitureReq>
        {
            public override void Write(IBuffer buffer, C2SClanSetFurnitureReq obj)
            {
                WriteEntityList<CDataFurnitureLayoutData>(buffer, obj.FurnitureLayoutData);
            }

            public override C2SClanSetFurnitureReq Read(IBuffer buffer)
            {
                C2SClanSetFurnitureReq obj = new C2SClanSetFurnitureReq();
                obj.FurnitureLayoutData = ReadEntityList<CDataFurnitureLayoutData>(buffer);
                return obj;
            }
        }
    }
}
