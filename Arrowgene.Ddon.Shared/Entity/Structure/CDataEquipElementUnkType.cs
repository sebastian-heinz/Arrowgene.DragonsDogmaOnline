using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipElementUnkType
    {
        byte u0;
        byte u1;
        ushort u2;
        ushort u3;
    }

    public class CDataEquipElementUnkTypeSerializer : EntitySerializer<CDataEquipElementUnkType>
    {
        public override void Write(IBuffer buffer, CDataEquipElementUnkType obj)
        {
            throw new NotImplementedException();
        }

        public override CDataEquipElementUnkType Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
