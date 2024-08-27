using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentGetRewardsNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_GET_REWARD_NTC;

        public S2CBattleContentGetRewardsNtc()
        {
            StatusList = new List<CDataBattleContentStatus>();
        }

        public uint CharacterId { get; set; }
        public List<CDataBattleContentStatus> StatusList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentGetRewardsNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentGetRewardsNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList(buffer, obj.StatusList);
            }

            public override S2CBattleContentGetRewardsNtc Read(IBuffer buffer)
            {
                S2CBattleContentGetRewardsNtc obj = new S2CBattleContentGetRewardsNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.StatusList = ReadEntityList<CDataBattleContentStatus>(buffer);
                return obj;
            }
        }
    }
}

