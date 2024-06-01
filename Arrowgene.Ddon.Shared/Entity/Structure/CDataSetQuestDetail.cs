using System.Collections.Generic;
using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetQuestDetail
    {
        public CDataSetQuestDetail() {
            UndiscoveryWalletPointRatio = new List<CDataWalletPoint>();
            UndiscoveryExpRatio = new List<CDataQuestExp>();
        }
    
        public uint ImageId { get; set; }
        public uint ClearCount { get; set; }
        public uint ClearCharacterNum { get; set; }
        public uint BaseAreaPoint { get; set; }
        public List<CDataWalletPoint> UndiscoveryWalletPointRatio { get; set; } // usUndiscoveryGoldRatio, usUndiscoveryRimRatio?
        public List<CDataQuestExp> UndiscoveryExpRatio { get; set; } // usUndiscoveryExpRatio?
        public ushort LeaderCompleteNum { get; set; }
        public ushort RepeatRewardType { get; set; }
        public byte RepeatRewardValue { get; set; }
        public byte RepeatRewardCompleteNum { get; set; }
        public byte RandomRewardNum { get; set; }
        public byte ChargeRewardNum { get; set; }
        public byte ProgressBonusNum { get; set; }
        public bool IsDiscovery { get; set; }
        
    
        public class Serializer : EntitySerializer<CDataSetQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataSetQuestDetail obj)
            {
                WriteUInt32(buffer, obj.ImageId);
                WriteUInt32(buffer, obj.ClearCount);
                WriteUInt32(buffer, obj.ClearCharacterNum);
                WriteUInt32(buffer, obj.BaseAreaPoint);
                WriteEntityList<CDataWalletPoint>(buffer, obj.UndiscoveryWalletPointRatio);
                WriteEntityList<CDataQuestExp>(buffer, obj.UndiscoveryExpRatio);
                WriteUInt16(buffer, obj.LeaderCompleteNum);
                WriteUInt16(buffer, obj.RepeatRewardType);
                WriteByte(buffer, obj.RepeatRewardValue);
                WriteByte(buffer, obj.RepeatRewardCompleteNum);
                WriteByte(buffer, obj.RandomRewardNum);
                WriteByte(buffer, obj.ChargeRewardNum);
                WriteByte(buffer, obj.ProgressBonusNum);
                WriteBool(buffer, obj.IsDiscovery);
            }
        
            public override CDataSetQuestDetail Read(IBuffer buffer)
            {
                CDataSetQuestDetail obj = new CDataSetQuestDetail();
                obj.ImageId = ReadUInt32(buffer);
                obj.ClearCount = ReadUInt32(buffer);
                obj.ClearCharacterNum = ReadUInt32(buffer);
                obj.BaseAreaPoint = ReadUInt32(buffer);
                obj.UndiscoveryWalletPointRatio = ReadEntityList<CDataWalletPoint>(buffer);
                obj.UndiscoveryExpRatio = ReadEntityList<CDataQuestExp>(buffer);
                obj.LeaderCompleteNum = ReadUInt16(buffer);
                obj.RepeatRewardType = ReadUInt16(buffer);
                obj.RepeatRewardValue = ReadByte(buffer);
                obj.RepeatRewardCompleteNum = ReadByte(buffer);
                obj.RandomRewardNum = ReadByte(buffer);
                obj.ChargeRewardNum = ReadByte(buffer);
                obj.ProgressBonusNum = ReadByte(buffer);
                obj.IsDiscovery = ReadBool(buffer);
                return obj;
            }
        }
    }
}
