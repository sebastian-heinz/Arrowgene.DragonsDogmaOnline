using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetLevelBonusListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_LEVEL_BONUS_LIST_RES;

        public S2CQuestGetLevelBonusListRes()
        {
            LevelBonusList = new();
        }

        public List<CDataLevelBonus> LevelBonusList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetLevelBonusListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetLevelBonusListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.LevelBonusList);
            }

            public override S2CQuestGetLevelBonusListRes Read(IBuffer buffer)
            {
                S2CQuestGetLevelBonusListRes obj = new S2CQuestGetLevelBonusListRes();
                ReadServerResponse(buffer, obj);
                obj.LevelBonusList = ReadEntityList<CDataLevelBonus>(buffer);
                return obj;
            }
        }
    }
}
