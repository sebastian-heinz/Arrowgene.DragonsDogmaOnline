using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDragonSkill
    {
        public CDataDragonSkill()
        {
        }

        public DragonSkill DragonSkill { get; set; }
        public byte Level { get; set; }

        public class Serializer : EntitySerializer<CDataDragonSkill>
        {
            public override void Write(IBuffer buffer, CDataDragonSkill obj)
            {
                WriteByte(buffer, (byte) obj.DragonSkill);
                WriteByte(buffer, obj.Level);
            }

            public override CDataDragonSkill Read(IBuffer buffer)
            {
                CDataDragonSkill obj = new CDataDragonSkill();
                obj.DragonSkill = (DragonSkill) ReadByte(buffer);
                obj.Level = ReadByte(buffer);
                return obj;
            }
        }
    }
}

