using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    // Suspect this might be some sort of progress
    public class CDataBattleContentSituationData
    {
        public CDataBattleContentSituationData()
        {
        }

        public GameMode GameMode { get; set; } // Impacts BBM announce/situation; seems like if not 2, it is blank
        public ulong StartTime { get; set; } // Controls if game thinks content is in progress or not
        public bool RewardReceived { get; set; } // Controls Reward message (true = Rewards not available)
        public bool Unk3 { get; set; }
        public BattleContentRewardBonus RewardBonus { get; set; }
        public uint ReportReset { get; set; } // Related to reset progress (Lyka gets ! in menu if > 0 and RewardReceived = true)
        public uint ReportSearchResults { get; set; } // 0x18 (Lyka gets ! in menu)
        public uint Unk7 { get; set; }
        public byte Unk8 { get; set; } // Impacts "Status:" some how
        public ulong Unktime { get; set; } // Reset time?
        public uint ContentId { get; set; } // Corresponds with the "ID" in the BattleContentStageList
        public uint Unk11 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentSituationData>
        {
            public override void Write(IBuffer buffer, CDataBattleContentSituationData obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
                WriteUInt64(buffer, obj.StartTime);
                WriteBool(buffer, obj.RewardReceived);
                WriteBool(buffer, obj.Unk3);
                WriteByte(buffer, (byte) obj.RewardBonus);
                WriteUInt32(buffer, obj.ReportReset);
                WriteUInt32(buffer, obj.ReportSearchResults);
                WriteUInt32(buffer, obj.Unk7);
                WriteByte(buffer, obj.Unk8);
                WriteUInt64(buffer, obj.Unktime);
                WriteUInt32(buffer, obj.ContentId);
                WriteUInt32(buffer, obj.Unk11);
            }

            public override CDataBattleContentSituationData Read(IBuffer buffer)
            {
                CDataBattleContentSituationData obj = new CDataBattleContentSituationData();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                obj.StartTime = ReadUInt64(buffer);
                obj.RewardReceived = ReadBool(buffer);
                obj.Unk3 = ReadBool(buffer);
                obj.RewardBonus = (BattleContentRewardBonus) ReadByte(buffer);
                obj.ReportReset = ReadUInt32(buffer);
                obj.ReportSearchResults = ReadUInt32(buffer);
                obj.Unk7 = ReadUInt32(buffer);
                obj.Unk8 = ReadByte(buffer);
                obj.Unktime = ReadUInt64(buffer);
                obj.ContentId = ReadUInt32(buffer);
                obj.Unk11 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

