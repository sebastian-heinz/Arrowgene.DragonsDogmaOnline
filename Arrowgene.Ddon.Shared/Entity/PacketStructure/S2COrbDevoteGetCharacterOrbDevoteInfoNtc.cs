using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteGetCharacterOrbDevoteInfoNtc : IPacketStructure
    {
        // TODO: Unsure if this is correct but the client is ok when sending this packet to it
        public PacketId Id => PacketId.S2C_ORB_DEVOTE_GET_CHARACTER_ORB_DEVOTE_INFO_NTC;

        public S2COrbDevoteGetCharacterOrbDevoteInfoNtc()
        {
            OrbStatusList = new List<CDataOrbPageStatus>();
            JobOrbTreeStatusList = new List<CDataJobOrbTreeStatus>();
            Unk0 = new List<CDataJobOrbTreeStatus>();
        }

        public List<CDataOrbPageStatus> OrbStatusList {  get; set; } // Dragon Force Augmentation (Jobs/Dragon Force)
        public List<CDataJobOrbTreeStatus> JobOrbTreeStatusList {  get; set; } // Skill Augmentation (Jobs/Dragon Force)
        public List<CDataJobOrbTreeStatus> Unk0 { get; set; } // Special Skill Augmentation (Jobs/Dragon Force)

        public class Serializer : PacketEntitySerializer<S2COrbDevoteGetCharacterOrbDevoteInfoNtc>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteGetCharacterOrbDevoteInfoNtc obj)
            {
                WriteEntityList<CDataOrbPageStatus>(buffer, obj.OrbStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.JobOrbTreeStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.Unk0);
            }

            public override S2COrbDevoteGetCharacterOrbDevoteInfoNtc Read(IBuffer buffer)
            {
                S2COrbDevoteGetCharacterOrbDevoteInfoNtc obj = new S2COrbDevoteGetCharacterOrbDevoteInfoNtc();
                obj.OrbStatusList = ReadEntityList<CDataOrbPageStatus>(buffer);
                obj.JobOrbTreeStatusList = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                obj.Unk0 = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                return obj;
            }
        }
    }
}
