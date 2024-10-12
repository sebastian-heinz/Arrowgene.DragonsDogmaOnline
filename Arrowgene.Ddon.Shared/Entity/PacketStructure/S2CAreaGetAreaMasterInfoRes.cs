using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaMasterInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_MASTER_INFO_RES;

        public S2CAreaGetAreaMasterInfoRes()
        {
            ReleaseList = new List<CDataCommonU32>();
            SupplyItemInfoList = new List<CDataBorderSupplyItem>();
            AreaRankUpQuestInfoList = new List<CDataAreaRankUpQuestInfo>();
        }

        public uint AreaId { get; set; }
        public uint Rank { get; set; }
        public uint Point { get; set; }
        public uint WeekPoint { get; set; }
        public uint LastWeekPoint { get; set; }
        public uint ToNextPoint { get; set; }
        public List<CDataCommonU32> ReleaseList { get; set; }
        public bool CanReceiveSupply { get; set; }
        public bool CanRankUp { get; set; }
        public List<CDataBorderSupplyItem> SupplyItemInfoList { get; set; }
        public List<CDataAreaRankUpQuestInfo> AreaRankUpQuestInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaMasterInfoRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaMasterInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Point);
                WriteUInt32(buffer, obj.WeekPoint);
                WriteUInt32(buffer, obj.LastWeekPoint);
                WriteUInt32(buffer, obj.ToNextPoint);
                WriteEntityList(buffer, obj.ReleaseList);
                WriteBool(buffer, obj.CanReceiveSupply);
                WriteBool(buffer, obj.CanRankUp);
                WriteEntityList(buffer, obj.SupplyItemInfoList);
                WriteEntityList(buffer, obj.AreaRankUpQuestInfoList);
            }

            public override S2CAreaGetAreaMasterInfoRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaMasterInfoRes obj = new S2CAreaGetAreaMasterInfoRes();
                ReadServerResponse(buffer, obj);
                obj.AreaId = ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                obj.Point = ReadUInt32(buffer);
                obj.WeekPoint = ReadUInt32(buffer);
                obj.LastWeekPoint = ReadUInt32(buffer);
                obj.ToNextPoint = ReadUInt32(buffer);
                obj.ReleaseList = ReadEntityList<CDataCommonU32>(buffer);
                obj.CanReceiveSupply = ReadBool(buffer);
                obj.CanRankUp = ReadBool(buffer);
                obj.SupplyItemInfoList = ReadEntityList<CDataBorderSupplyItem>(buffer);
                obj.AreaRankUpQuestInfoList = ReadEntityList<CDataAreaRankUpQuestInfo>(buffer);
                return obj;
            }
        }
    }
}
