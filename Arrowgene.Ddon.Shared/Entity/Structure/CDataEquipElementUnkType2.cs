using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipElementUnkType2
    {
        byte u0;
        ushort u2;
    }

    public class CDataEquipElementUnkType2Serializer : EntitySerializer<CDataEquipElementUnkType2>
    {
        public override void Write(IBuffer buffer, CDataEquipElementUnkType2 obj)
        {
            throw new NotImplementedException();
        }

        public override CDataEquipElementUnkType2 Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
