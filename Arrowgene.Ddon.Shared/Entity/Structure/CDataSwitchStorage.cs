using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSwitchStorage
    {
        public CDataSwitchStorage()
        {
        }

        public StorageType TargetStorageType { get; set; }
        public StorageType ChangeStorageType { get; set; }

        public class Serializer : EntitySerializer<CDataSwitchStorage>
        {
            public override void Write(IBuffer buffer, CDataSwitchStorage obj)
            {
                WriteByte(buffer, (byte) obj.TargetStorageType);
                WriteByte(buffer, (byte) obj.ChangeStorageType);
            }

            public override CDataSwitchStorage Read(IBuffer buffer)
            {
                CDataSwitchStorage obj = new CDataSwitchStorage();
                obj.TargetStorageType = (StorageType)ReadByte(buffer);
                obj.ChangeStorageType = (StorageType)ReadByte(buffer);
                return obj;
            }
        }
    }
}
