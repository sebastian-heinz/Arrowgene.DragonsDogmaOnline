using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanGetFurnitureRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_GET_FURNITURE_RES;

        public S2CClanGetFurnitureRes()
        {
            FurnitureLayouts = new();
        }

        public List<CDataFurnitureLayout> FurnitureLayouts { get; set; }
        
        public class Serializer : PacketEntitySerializer<S2CClanGetFurnitureRes>
        {
            public override void Write(IBuffer buffer, S2CClanGetFurnitureRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataFurnitureLayout>(buffer, obj.FurnitureLayouts);
            }

            public override S2CClanGetFurnitureRes Read(IBuffer buffer)
            {
                S2CClanGetFurnitureRes obj = new S2CClanGetFurnitureRes();
                ReadServerResponse(buffer, obj);
                obj.FurnitureLayouts = ReadEntityList<CDataFurnitureLayout>(buffer);
                return obj;
            }
        }

    }
}
