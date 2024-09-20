using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataExpSetting
    {
        public GameMode Type { get; set; }
        public List<CDataExpRequirement> ExpList { get; set; }

        public CDataExpSetting()
        {
            ExpList = new List<CDataExpRequirement>();
        }

        public class Serializer : EntitySerializer<CDataExpSetting>
        {
            public override void Write(IBuffer buffer, CDataExpSetting obj)
            {
                WriteByte(buffer, (byte)obj.Type);
                WriteEntityList<CDataExpRequirement>(buffer, obj.ExpList);
            }

            public override CDataExpSetting Read(IBuffer buffer)
            {
                CDataExpSetting obj = new CDataExpSetting();
                obj.Type = (GameMode)ReadByte(buffer);
                obj.ExpList = ReadEntityList<CDataExpRequirement>(buffer);
                return obj;
            }
        }
    }
}
