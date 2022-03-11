using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemSort
    {
        public CDataItemSort()
        {
            StorageType=0;
            Bin=new byte[1024];
        }

        public byte StorageType;
        public byte[] Bin;

        public class Serializer : EntitySerializer<CDataItemSort>
        {
            public override void Write(IBuffer buffer, CDataItemSort obj)
            {
                WriteByte(buffer, obj.StorageType);
                WriteByteArray(buffer, obj.Bin);
            }

            public override CDataItemSort Read(IBuffer buffer)
            {
                CDataItemSort obj = new CDataItemSort();
                obj.StorageType = ReadByte(buffer);
                obj.Bin = ReadByteArray(buffer, 1024);
                return obj;
            }
        }
    }
}