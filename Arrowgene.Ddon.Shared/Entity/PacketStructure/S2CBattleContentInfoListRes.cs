using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentInfoListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_INFO_LIST_RES;

        public S2CBattleContentInfoListRes()
        {
            BattleContentInfoList = new List<CDataBattleContentListEntry>();
        }

        public List<CDataBattleContentListEntry> BattleContentInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentInfoListRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentInfoListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataBattleContentListEntry>(buffer, obj.BattleContentInfoList);
            }

            public override S2CBattleContentInfoListRes Read(IBuffer buffer)
            {
                S2CBattleContentInfoListRes obj = new S2CBattleContentInfoListRes();
                ReadServerResponse(buffer, obj);
                obj.BattleContentInfoList = ReadEntityList<CDataBattleContentListEntry>(buffer);
                return obj;
            }
        }
    }
}
