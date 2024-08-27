using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentInstantClearInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_INSTANT_CLEAR_INFO_RES;

        public S2CBattleContentInstantClearInfoRes()
        {
            Unk0 = new List<CDataBattleContentUnk4>();
        }

        public List<CDataBattleContentUnk4> Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentInstantClearInfoRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentInstantClearInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.Unk0);
            }

            public override S2CBattleContentInstantClearInfoRes Read(IBuffer buffer)
            {
                S2CBattleContentInstantClearInfoRes obj = new S2CBattleContentInstantClearInfoRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadEntityList<CDataBattleContentUnk4>(buffer);
                return obj;
            }
        }
    }
}

