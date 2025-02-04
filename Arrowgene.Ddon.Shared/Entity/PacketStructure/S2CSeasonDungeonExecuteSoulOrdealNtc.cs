using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonExecuteSoulOrdealNtc : IPacketStructure
    {
        public S2CSeasonDungeonExecuteSoulOrdealNtc()
        {
            TrialName = string.Empty;
            ObjectiveList = new List<CDataSoulOrdealObjective>();
        }

        public PacketId Id => PacketId.S2C_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_NTC;

        public string TrialName { get; set; }
        public uint TrialId { get; set; }
        public List<CDataSoulOrdealObjective> ObjectiveList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonExecuteSoulOrdealNtc>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonExecuteSoulOrdealNtc obj)
            {
                WriteMtString(buffer, obj.TrialName);
                WriteUInt32(buffer, obj.TrialId);
                WriteEntityList(buffer, obj.ObjectiveList);
            }

            public override S2CSeasonDungeonExecuteSoulOrdealNtc Read(IBuffer buffer)
            {
                S2CSeasonDungeonExecuteSoulOrdealNtc obj = new S2CSeasonDungeonExecuteSoulOrdealNtc();
                obj.TrialName = ReadMtString(buffer);
                obj.TrialId = ReadUInt32(buffer);
                obj.ObjectiveList = ReadEntityList<CDataSoulOrdealObjective>(buffer);
                return obj;
            }
        }
    }
}
