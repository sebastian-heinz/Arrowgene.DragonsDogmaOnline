using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentCharacterInfoRes : ServerResponse
    {
        // バトルコンテンツ：進行情報の取得 (Obtaining Progress Information)
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_CHARACTER_INFO_RES;

        public S2CBattleContentCharacterInfoRes()
        {
            SituationData = new CDataBattleContentSituationData();
            Unk2List = new List<CDataBattleContentAvailableRewards>();
        }

        public CDataBattleContentSituationData SituationData { get; set; }
        public List<CDataBattleContentAvailableRewards> Unk2List { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentCharacterInfoRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentCharacterInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.SituationData);
                WriteEntityList(buffer, obj.Unk2List);
            }

            public override S2CBattleContentCharacterInfoRes Read(IBuffer buffer)
            {
                S2CBattleContentCharacterInfoRes obj = new S2CBattleContentCharacterInfoRes();
                ReadServerResponse(buffer, obj);
                obj.SituationData = ReadEntity<CDataBattleContentSituationData>(buffer);
                obj.Unk2List = ReadEntityList<CDataBattleContentAvailableRewards>(buffer);
                return obj;
            }
        }
    }
}
