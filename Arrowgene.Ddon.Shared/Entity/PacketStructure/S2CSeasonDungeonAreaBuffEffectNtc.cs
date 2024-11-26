using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonAreaBuffEffectNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SEASON_DUNGEON_AREA_BUFFS_NTC;

        public S2CSeasonDungeonAreaBuffEffectNtc()
        {
            BuffEffectParamList = new List<CDataSeasonDungeonBuffEffectParam>();
        }

        public List<CDataSeasonDungeonBuffEffectParam> BuffEffectParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonAreaBuffEffectNtc>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonAreaBuffEffectNtc obj)
            {
                WriteEntityList(buffer, obj.BuffEffectParamList);
            }

            public override S2CSeasonDungeonAreaBuffEffectNtc Read(IBuffer buffer)
            {
                S2CSeasonDungeonAreaBuffEffectNtc obj = new S2CSeasonDungeonAreaBuffEffectNtc();
                obj.BuffEffectParamList = ReadEntityList<CDataSeasonDungeonBuffEffectParam>(buffer);
                return obj;
            }
        }
    }
}
