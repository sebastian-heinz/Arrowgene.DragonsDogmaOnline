using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CProfileGetMyCharacterProfileRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_GET_MY_CHARACTER_PROFILE_RES;

        public S2CProfileGetMyCharacterProfileRes()
        {
            HistoryElementList = new List<CDataHistoryElement>();
            OnlineId = string.Empty;
            AchieveCategoryStatusList = new List<CDataAchieveCategoryStatus>();
            OrbStatusList = new List<CDataOrbPageStatus>();
            JobOrbTreeStatusList = new List<CDataJobOrbTreeStatus>();
            Unk0 = new List<CDataJobOrbTreeStatus>();
        }

        public List<CDataHistoryElement> HistoryElementList { get; set; }
        public string OnlineId { get; set; }
        public uint AbilityCostMax { get; set; }
        public List<CDataAchieveCategoryStatus> AchieveCategoryStatusList { get; set; }
        public List<CDataOrbPageStatus> OrbStatusList { get; set; } // Dragon Force Augmentation (Jobs/Dragon Force)
        public List<CDataJobOrbTreeStatus> JobOrbTreeStatusList { get; set; } // Skill Augmentation (Jobs/Dragon Force)
        public List<CDataJobOrbTreeStatus> Unk0 { get; set; } // Special Skill Augmentation (Jobs/Dragon Force)


        public class Serializer : PacketEntitySerializer<S2CProfileGetMyCharacterProfileRes>
        {
            public override void Write(IBuffer buffer, S2CProfileGetMyCharacterProfileRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataHistoryElement>(buffer, obj.HistoryElementList);
                WriteMtString(buffer, obj.OnlineId);
                WriteUInt32(buffer, obj.AbilityCostMax);
                WriteEntityList<CDataAchieveCategoryStatus>(buffer, obj.AchieveCategoryStatusList);
                WriteEntityList<CDataOrbPageStatus>(buffer, obj.OrbStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.JobOrbTreeStatusList);
                WriteEntityList<CDataJobOrbTreeStatus>(buffer, obj.Unk0);
            }

            public override S2CProfileGetMyCharacterProfileRes Read(IBuffer buffer)
            {
                S2CProfileGetMyCharacterProfileRes obj = new S2CProfileGetMyCharacterProfileRes();
                ReadServerResponse(buffer, obj);
                obj.HistoryElementList = ReadEntityList<CDataHistoryElement>(buffer);
                obj.OnlineId = ReadMtString(buffer);
                obj.AbilityCostMax = ReadUInt32(buffer);
                obj.AchieveCategoryStatusList = ReadEntityList<CDataAchieveCategoryStatus>(buffer);
                obj.OrbStatusList = ReadEntityList<CDataOrbPageStatus>(buffer);
                obj.JobOrbTreeStatusList = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                obj.Unk0 = ReadEntityList<CDataJobOrbTreeStatus>(buffer);
                return obj;
            }
        }
    }
}
