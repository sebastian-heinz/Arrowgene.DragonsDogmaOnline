using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrb25_6_16_Ntc : IPacketStructure
    {
        // TODO: Unsure if this is correct but the client is ok when sending this packet to it
        public PacketId Id => PacketId.S2C_ORB_25_6_16_NTC;

        public S2COrb25_6_16_Ntc()
        {
            OrbStatusList = new List<CDataOrbPageStatus>();
            JobOrbTreeStatusList = new List<CDataJobOrbTreeStatus>();
            Unk0 = new List<CDataJobOrbTreeStatus>();
        }

        public List<CDataOrbPageStatus> OrbStatusList {  get; set; } // Dragon Force Augmentation (Jobs/Dragon Force)
        public List<CDataJobOrbTreeStatus> JobOrbTreeStatusList {  get; set; } // Skill Augmentation (Jobs/Dragon Force)
        public List<CDataJobOrbTreeStatus> Unk0 { get; set; } // Special Skill Augmentation (Jobs/Dragon Force)

        public class Serializer : PacketEntitySerializer<S2COrb25_6_16_Ntc>
        {
            public override void Write(IBuffer buffer, S2COrb25_6_16_Ntc obj)
            {
                WriteEntityList<CDataOrbPageStatus>(buffer, obj.OrbStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.JobOrbTreeStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.Unk0);
            }

            public override S2COrb25_6_16_Ntc Read(IBuffer buffer)
            {
                S2COrb25_6_16_Ntc obj = new S2COrb25_6_16_Ntc();
                obj.OrbStatusList = ReadEntityList<CDataOrbPageStatus>(buffer);
                obj.JobOrbTreeStatusList = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                obj.Unk0 = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                return obj;
            }
        }
    }
}
