using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataExpSetting
    {
        /// <summary>
        /// 1 = Job Level
        /// 2 = Clan Level
        /// </summary>
        public byte Type { get; set; }
        public List<CDataExpRequirement> ExpList { get; set; }

        public CDataExpSetting()
        {
            ExpList = new List<CDataExpRequirement>();
        }

        public class Serializer : EntitySerializer<CDataExpSetting>
        {
            public override void Write(IBuffer buffer, CDataExpSetting obj)
            {
                WriteByte(buffer, obj.Type);
                WriteEntityList<CDataExpRequirement>(buffer, obj.ExpList);
            }

            public override CDataExpSetting Read(IBuffer buffer)
            {
                CDataExpSetting obj = new CDataExpSetting();
                obj.Type = ReadByte(buffer);
                obj.ExpList = ReadEntityList<CDataExpRequirement>(buffer);
                return obj;
            }
        }
    }
}
