using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPhotoPhotoTakeNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PHOTO_PHOTO_TAKE_NTC;

        public C2SPhotoPhotoTakeNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SPhotoPhotoTakeNtc>
        {
            public override void Write(IBuffer buffer, C2SPhotoPhotoTakeNtc obj)
            {
            }

            public override C2SPhotoPhotoTakeNtc Read(IBuffer buffer)
            {
                C2SPhotoPhotoTakeNtc obj = new C2SPhotoPhotoTakeNtc();
                return obj;
            }
        }
    }
}
