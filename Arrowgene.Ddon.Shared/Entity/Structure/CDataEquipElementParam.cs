using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipElementParam
    {
        byte u0;
        uint u1;
        ushort u2;
    }

    public class CDataEquipElementParamSerializer : EntitySerializer<CDataEquipElementParam>
    {
        public override void Write(IBuffer buffer, CDataEquipElementParam obj)
        {
            throw new NotImplementedException();
        }

        public override CDataEquipElementParam Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
