using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftTimeSaveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_CRAFT_TIME_SAVE_REQ;

        public C2SCraftTimeSaveReq()
        {
        }

        public uint PawnID { get; set; }
        /// <summary>
        /// TODO: Related to CraftSettingHandler => ID of TimeSave data?
        /// </summary>
        public byte ID { get; set; }
        /// TODO: Always 1?
        public byte Num { get; set; }
        /// <summary>
        /// true while triggered directly from craft item, false when triggered from production status
        /// </summary>
        public bool IsInit { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftTimeSaveReq>
        {
            public override void Write(IBuffer buffer, C2SCraftTimeSaveReq obj)
            {
                WriteUInt32(buffer, obj.PawnID);
                WriteByte(buffer, obj.ID);
                WriteByte(buffer, obj.Num);
                WriteBool(buffer, obj.IsInit);
            }

            public override C2SCraftTimeSaveReq Read(IBuffer buffer)
            {
                C2SCraftTimeSaveReq obj = new C2SCraftTimeSaveReq();

                obj.PawnID = ReadUInt32(buffer);
                obj.ID = ReadByte(buffer);
                obj.Num = ReadByte(buffer);
                obj.IsInit = ReadBool(buffer);

                return obj;
            }
        }
    }
}
