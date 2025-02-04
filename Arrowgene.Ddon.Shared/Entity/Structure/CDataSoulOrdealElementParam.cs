using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealElementParam
    {
        public CDataSoulOrdealElementParam()
        {
            TrialName = string.Empty;
            TrialCost = new List<CDataSoulOrdealItem>();
            TrialRewards = new List<CDataSoulOrdealUnk0>();
        }

        public uint TrialId { get; set; }
        public string TrialName { get; set; }
        public List<CDataSoulOrdealItem> TrialCost { get; set; }
        public List<CDataSoulOrdealUnk0> TrialRewards { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealElementParam>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealElementParam obj)
            {
                WriteUInt32(buffer, obj.TrialId);
                WriteMtString(buffer, obj.TrialName);
                WriteEntityList(buffer, obj.TrialCost);
                WriteEntityList(buffer, obj.TrialRewards);
            }

            public override CDataSoulOrdealElementParam Read(IBuffer buffer)
            {
                CDataSoulOrdealElementParam obj = new CDataSoulOrdealElementParam();
                obj.TrialId = ReadUInt32(buffer);
                obj.TrialName = ReadMtString(buffer);
                obj.TrialCost = ReadEntityList<CDataSoulOrdealItem>(buffer);
                obj.TrialRewards = ReadEntityList<CDataSoulOrdealUnk0>(buffer);
                return obj;
            }
        }
    }
}
