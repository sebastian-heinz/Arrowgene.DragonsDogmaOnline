using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLockSettingUpdate
    {
        public uint SealIndex { get; set; }
        public bool LockStatus { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLockSettingUpdate>
        {
            public override void Write(IBuffer buffer, CDataDispelLockSettingUpdate obj)
            {
                WriteUInt32(buffer, obj.SealIndex);
                WriteBool(buffer, obj.LockStatus);
            }

            public override CDataDispelLockSettingUpdate Read(IBuffer buffer)
            {
                CDataDispelLockSettingUpdate obj = new CDataDispelLockSettingUpdate();
                obj.SealIndex = ReadUInt32(buffer);
                obj.LockStatus = ReadBool(buffer);
                return obj;
            }
        }
    }
}
