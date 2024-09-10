using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDragonAbility
    {
        public CDataDragonAbility()
        {
        }

        public DragonAbility DragonAbility { get; set; }
        public uint Level { get; set; }

        public class Serializer : EntitySerializer<CDataDragonAbility>
        {
            public override void Write(IBuffer buffer, CDataDragonAbility obj)
            {
                WriteByte(buffer, (byte) obj.DragonAbility);
                WriteUInt32(buffer, obj.Level);
            }

            public override CDataDragonAbility Read(IBuffer buffer)
            {
                CDataDragonAbility obj = new CDataDragonAbility();
                obj.DragonAbility = (DragonAbility) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

