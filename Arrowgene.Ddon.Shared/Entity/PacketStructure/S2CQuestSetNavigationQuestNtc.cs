using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSetNavigationQuestNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_SET_NAVIGATION_QUEST_NTC;

        public S2CQuestSetNavigationQuestNtc()
        {
            SetQuestOrderList = new List<CDataSetQuestOrderList>();
            MainQuestOrderList = new List<CDataMainQuestOrderList>();
            TutorialQuestOrderList = new List<CDataTutorialQuestOrderList>();
            MobHuntQuestOrderList = new List<CDataMobHuntQuestOrderList>();
            ExpiredQuestList = new List<CDataExpiredQuestList>();
        }

        public uint ErrorCode { get; set; }
        public List<CDataSetQuestOrderList> SetQuestOrderList { get; set; }
        public List<CDataMainQuestOrderList> MainQuestOrderList { get; set; }
        public List<CDataTutorialQuestOrderList> TutorialQuestOrderList { get; set; }
        public List<CDataMobHuntQuestOrderList> MobHuntQuestOrderList { get; set; }
        public List<CDataExpiredQuestList> ExpiredQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestSetNavigationQuestNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestSetNavigationQuestNtc obj)
            {
                WriteUInt32(buffer, obj.ErrorCode);
                WriteEntityList(buffer, obj.SetQuestOrderList);
                WriteEntityList(buffer, obj.MainQuestOrderList);
                WriteEntityList(buffer, obj.TutorialQuestOrderList);
                WriteEntityList(buffer, obj.MobHuntQuestOrderList);
                WriteEntityList(buffer, obj.ExpiredQuestList);
            }

            public override S2CQuestSetNavigationQuestNtc Read(IBuffer buffer)
            {
                S2CQuestSetNavigationQuestNtc obj = new S2CQuestSetNavigationQuestNtc();
                obj.ErrorCode = ReadUInt32(buffer);
                obj.SetQuestOrderList = ReadEntityList<CDataSetQuestOrderList>(buffer);
                obj.MainQuestOrderList = ReadEntityList<CDataMainQuestOrderList>(buffer);
                obj.TutorialQuestOrderList = ReadEntityList<CDataTutorialQuestOrderList>(buffer);
                obj.MobHuntQuestOrderList = ReadEntityList<CDataMobHuntQuestOrderList>(buffer);
                obj.ExpiredQuestList = ReadEntityList<CDataExpiredQuestList>(buffer);
                return obj;
            }
        }
    }
}
