using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CServerGetServerListRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_SERVER_GET_SERVER_LIST_RES;

    public S2CServerGetServerListRes()
    {
        GameServerListInfo = new List<CDataGameServerListInfo>();
    }

    public List<CDataGameServerListInfo> GameServerListInfo { get; set; }


    public class Serializer : PacketEntitySerializer<S2CServerGetServerListRes>
    {
        public override void Write(IBuffer buffer, S2CServerGetServerListRes obj)
        {
            WriteServerResponse(buffer, obj);
            WriteEntityList<CDataGameServerListInfo>(buffer, obj.GameServerListInfo);
        }

        public override S2CServerGetServerListRes Read(IBuffer buffer)
        {
            S2CServerGetServerListRes obj = new S2CServerGetServerListRes();
            ReadServerResponse(buffer, obj);
            obj.GameServerListInfo = ReadEntityList<CDataGameServerListInfo>(buffer);
            return obj;
        }
    }
}
