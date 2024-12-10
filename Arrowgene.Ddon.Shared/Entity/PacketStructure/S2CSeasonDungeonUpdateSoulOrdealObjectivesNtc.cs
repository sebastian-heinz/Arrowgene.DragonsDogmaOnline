using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc : IPacketStructure
    {
        public S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc()
        {
            Objectives = new List<CDataSoulOrdealObjective>();
        }

        public PacketId Id => PacketId.S2C_SEASON_DUNGEON_UPDATE_SOUL_ORDEAL_OBJECTIVES_NTC;

        public List<CDataSoulOrdealObjective> Objectives { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc obj)
            {
                WriteEntityList(buffer, obj.Objectives);
            }

            public override S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc Read(IBuffer buffer)
            {
                S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc obj = new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc();
                obj.Objectives = ReadEntityList<CDataSoulOrdealObjective>(buffer);
                return obj;
            }
        }
    }
}
