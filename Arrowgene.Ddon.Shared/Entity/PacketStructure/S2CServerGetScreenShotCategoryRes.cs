using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CServerGetScreenShotCategoryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SERVER_GET_SCREEN_SHOT_CATEGORY_RES;

        public List<CDataScreenShotCategory> CategoryList;

        public S2CServerGetScreenShotCategoryRes()
        {
            CategoryList = new List<CDataScreenShotCategory>();
        }

        public class Serializer : PacketEntitySerializer<S2CServerGetScreenShotCategoryRes>
        {
            public override void Write(IBuffer buffer, S2CServerGetScreenShotCategoryRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataScreenShotCategory>(buffer, obj.CategoryList);
            }

            public override S2CServerGetScreenShotCategoryRes Read(IBuffer buffer)
            {
                S2CServerGetScreenShotCategoryRes obj = new S2CServerGetScreenShotCategoryRes();
                ReadServerResponse(buffer, obj);
                obj.CategoryList = ReadEntityList<CDataScreenShotCategory>(buffer);
                return obj;
            }
        }
    }
}
