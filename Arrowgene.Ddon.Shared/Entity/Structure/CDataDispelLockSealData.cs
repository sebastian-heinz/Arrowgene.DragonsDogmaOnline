using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLockSealData
    {
        public uint SealIndex { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public bool LockStatus { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLockSealData>
        {
            public override void Write(IBuffer buffer, CDataDispelLockSealData obj)
            {
                WriteUInt32(buffer, obj.SealIndex);
                WriteMtString(buffer, obj.DisplayText);
                WriteBool(buffer, obj.LockStatus);
            }

            public override CDataDispelLockSealData Read(IBuffer buffer)
            {
                CDataDispelLockSealData obj = new CDataDispelLockSealData();
                obj.SealIndex = ReadUInt32(buffer);
                obj.DisplayText = ReadMtString(buffer);
                obj.LockStatus = ReadBool(buffer);
                return obj;
            }
        }
    }
}
