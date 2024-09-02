using Arrowgene.Buffers;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardBoxRecord
{
    public CDataRewardBoxRecord()
    {
        RewardItemList = new List<CDataRewardBoxItem>();
    }

    public UInt32 ListNo { get; set; }
    public UInt32 QuestId {  get; set; }
    public List<CDataRewardBoxItem> RewardItemList { get; set; }

    public class Serializer : EntitySerializer<CDataRewardBoxRecord>
    {
        public override void Write(IBuffer buffer, CDataRewardBoxRecord obj)
        {
            WriteUInt32(buffer, obj.ListNo);
            WriteUInt32(buffer, obj.QuestId);
            WriteEntityList<CDataRewardBoxItem>(buffer, obj.RewardItemList);
        }

        public override CDataRewardBoxRecord Read(IBuffer buffer)
        {
            CDataRewardBoxRecord obj = new CDataRewardBoxRecord();
            obj.ListNo = ReadUInt32(buffer);
            obj.QuestId = ReadUInt32(buffer);
            obj.RewardItemList = ReadEntityList<CDataRewardBoxItem>(buffer);
            return obj;
        }
    }
}

