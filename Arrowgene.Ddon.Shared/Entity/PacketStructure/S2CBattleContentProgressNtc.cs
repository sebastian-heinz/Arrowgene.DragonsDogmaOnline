using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentProgressNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_71_19_16_NTC;

        public S2CBattleContentProgressNtc()
        {
            BattleContentStatusList = new List<CDataBattleContentStatus>();
        }

        public List<CDataBattleContentStatus> BattleContentStatusList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentProgressNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentProgressNtc obj)
            {
                WriteEntityList(buffer, obj.BattleContentStatusList);
            }

            public override S2CBattleContentProgressNtc Read(IBuffer buffer)
            {
                S2CBattleContentProgressNtc obj = new S2CBattleContentProgressNtc();
                obj.BattleContentStatusList = ReadEntityList<CDataBattleContentStatus>(buffer);
                return obj;
            }
        }
    }
}


