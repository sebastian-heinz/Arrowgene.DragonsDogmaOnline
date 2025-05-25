using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblem
    {
        public CDataJobEmblem()
        {
            EmblemStatList = new();
            EquipElementParamList = new();
        }

        public JobId Job { get; set; }
        public List<CDataJobEmblemStatParam> EmblemStatList { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblem>
        {
            public override void Write(IBuffer buffer, CDataJobEmblem obj)
            {
                WriteByte(buffer, (byte)obj.Job);
                WriteEntityList(buffer, obj.EmblemStatList);
                WriteEntityList(buffer, obj.EquipElementParamList);
            }

            public override CDataJobEmblem Read(IBuffer buffer)
            {
                CDataJobEmblem obj = new CDataJobEmblem();
                obj.Job = (JobId)ReadByte(buffer);
                obj.EmblemStatList = ReadEntityList<CDataJobEmblemStatParam>(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                return obj;
            }
        }
    }
}
