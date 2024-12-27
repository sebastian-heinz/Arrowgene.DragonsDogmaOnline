using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGameItemStorage
    {
        public StorageType StorageType { get; set; }

        public CDataGameItemStorage()
        {
        }

        public class Serializer : EntitySerializer<CDataGameItemStorage>
        {
            public override void Write(IBuffer buffer, CDataGameItemStorage obj)
            {
                WriteByte(buffer, (byte)obj.StorageType);
            }

            public override CDataGameItemStorage Read(IBuffer buffer)
            {
                CDataGameItemStorage obj = new CDataGameItemStorage
                {
                    StorageType = (StorageType)ReadByte(buffer),
                };

                return obj;
            }
        }
    }
}
