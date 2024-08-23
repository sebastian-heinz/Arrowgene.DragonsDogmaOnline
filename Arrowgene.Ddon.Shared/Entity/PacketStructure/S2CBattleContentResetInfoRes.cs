using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentResetInfoRes : ServerResponse
    {
        // バトルコンテンツ：リセット情報の取得 (get reset information)
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_RESET_INFO_RES;

        public S2CBattleContentResetInfoRes()
        {
            ResetInfoList = new List<CDataResetInfo>();
        }

        public List<CDataResetInfo> ResetInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentResetInfoRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentResetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ResetInfoList);
            }

            public override S2CBattleContentResetInfoRes Read(IBuffer buffer)
            {
                S2CBattleContentResetInfoRes obj = new S2CBattleContentResetInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ResetInfoList = ReadEntityList<CDataResetInfo>(buffer);
                return obj;
            }
        }
    }
}
