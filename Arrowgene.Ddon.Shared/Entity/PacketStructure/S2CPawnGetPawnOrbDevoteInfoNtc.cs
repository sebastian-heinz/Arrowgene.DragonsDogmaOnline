using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPawnOrbDevoteInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_GET_PAWN_ORB_DEVOTE_INFO_NTC;

        public S2CPawnGetPawnOrbDevoteInfoNtc()
        {
            OrbPageStatusList = new List<CDataOrbPageStatus>();
            JobOrbTreeStatusList = new List<CDataJobOrbTreeStatus>();
            Unk0 = new List<CDataJobOrbTreeStatus>();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public List<CDataOrbPageStatus> OrbPageStatusList { get; set; } // Dragon Force Augmentation
        public List<CDataJobOrbTreeStatus> JobOrbTreeStatusList { get; set; } // Skill Augmentation
        public List<CDataJobOrbTreeStatus> Unk0 { get; set; } // Special Skill Augmentation tree?

        public class Serializer : PacketEntitySerializer<S2CPawnGetPawnOrbDevoteInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPawnOrbDevoteInfoNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataOrbPageStatus>(buffer, obj.OrbPageStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.JobOrbTreeStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.Unk0);
            }

            public override S2CPawnGetPawnOrbDevoteInfoNtc Read(IBuffer buffer)
            {
                S2CPawnGetPawnOrbDevoteInfoNtc obj = new S2CPawnGetPawnOrbDevoteInfoNtc();
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
