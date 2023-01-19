using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnTotalScore
    {
        public CDataPawnTotalScore()
        {
            AveragePawnFeedbackList = new List<CDataPawnFeedback>();
        }

        public uint RentalCount { get; set; }
        public uint BattleCount { get; set; }
        public uint CraftCount { get; set; }
        public List<CDataPawnFeedback> AveragePawnFeedbackList { get; set; }

        public class Serializer : EntitySerializer<CDataPawnTotalScore>
        {
            public override void Write(IBuffer buffer, CDataPawnTotalScore obj)
            {
                WriteUInt32(buffer, obj.RentalCount);
                WriteUInt32(buffer, obj.BattleCount);
                WriteUInt32(buffer, obj.CraftCount);
                WriteEntityList<CDataPawnFeedback>(buffer, obj.AveragePawnFeedbackList);
            }
        
            public override CDataPawnTotalScore Read(IBuffer buffer)
            {
                CDataPawnTotalScore obj = new CDataPawnTotalScore();
                obj.RentalCount = ReadUInt32(buffer);
                obj.BattleCount = ReadUInt32(buffer);
                obj.CraftCount = ReadUInt32(buffer);
                obj.AveragePawnFeedbackList = ReadEntityList<CDataPawnFeedback>(buffer);
                return obj;
            }
        }
    }
}