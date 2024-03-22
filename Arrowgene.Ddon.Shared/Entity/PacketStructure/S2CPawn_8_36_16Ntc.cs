using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawn_8_36_16Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_8_36_16_NTC;

        public S2CPawn_8_36_16Ntc()
        {
            OrbPageStatusList = new List<CDataOrbPageStatus>();
            JobOrbTreeStatusList = new List<CDataJobOrbTreeStatus>();
            Unk0 = new List<CDataJobOrbTreeStatus>();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public List<CDataOrbPageStatus> OrbPageStatusList { get; set; }
        public List<CDataJobOrbTreeStatus> JobOrbTreeStatusList { get; set; }
        public List<CDataJobOrbTreeStatus> Unk0 { get; set; } // Probably High Orbs

        public class Serializer : PacketEntitySerializer<S2CPawn_8_36_16Ntc>
        {
            public override void Write(IBuffer buffer, S2CPawn_8_36_16Ntc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataOrbPageStatus>(buffer, obj.OrbPageStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.JobOrbTreeStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.Unk0);
            }

            public override S2CPawn_8_36_16Ntc Read(IBuffer buffer)
            {
                S2CPawn_8_36_16Ntc obj = new S2CPawn_8_36_16Ntc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.OrbPageStatusList = ReadEntityList<CDataOrbPageStatus>(buffer);
                obj.JobOrbTreeStatusList = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                obj.Unk0 = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                return obj;
            }
        }

    }
}