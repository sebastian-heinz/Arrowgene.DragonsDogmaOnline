using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSwitchStorage // CDataSwitchStorage?
    {
        public StorageType StorageType { get; set; }
        public ushort Num { get; set; }

        public class Serializer : EntitySerializer<CDataSwitchStorage>
        {
            public override void Write(IBuffer buffer, CDataSwitchStorage obj)
            {
                WriteByte(buffer, (byte) obj.StorageType);
                WriteUInt16(buffer, obj.Num);
            }

            public override CDataSwitchStorage Read(IBuffer buffer)
            {
                CDataSwitchStorage obj = new CDataSwitchStorage();
                obj.StorageType = (StorageType)ReadByte(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
