using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyChatMsgNotice : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_NTC;

        public LobbyChatMsgType Type { get; set; }

        /// <summary>
        /// When Type is Tell, then this supports the client in identifying source/target.
        /// </summary>
        public uint HandleId { get; set; }

        public CDataCommunityCharacterBaseInfo CharacterBaseInfo { get; set; }

        /// <summary>
        /// 0 for regular message
        /// 1 for built-in phrases-type message
        /// </summary>
        public byte MessageFlavor { get; set; }

        public uint PhrasesCategory { get; set; }
        public uint PhrasesIndex { get; set; }
        public string Message { get; set; }

        public S2CLobbyChatMsgNotice()
        {
            Type = 0;
            HandleId = 0;
            CharacterBaseInfo = new CDataCommunityCharacterBaseInfo();
            MessageFlavor = 0;
            PhrasesCategory = 0;
            PhrasesIndex = 0;
            Message = string.Empty;
        }

        public class Serializer : PacketEntitySerializer<S2CLobbyChatMsgNotice>
        {
            public override void Write(IBuffer buffer, S2CLobbyChatMsgNotice obj)
            {
                WriteByte(buffer, (byte)obj.Type);
                WriteUInt32(buffer, obj.HandleId);
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.CharacterBaseInfo);
                WriteByte(buffer, obj.MessageFlavor);
                WriteUInt32(buffer, obj.PhrasesCategory);
                WriteUInt32(buffer, obj.PhrasesIndex);
                WriteMtString(buffer, obj.Message);
            }

            public override S2CLobbyChatMsgNotice Read(IBuffer buffer)
            {
                S2CLobbyChatMsgNotice obj = new S2CLobbyChatMsgNotice();
                obj.Type = (LobbyChatMsgType)ReadByte(buffer);
                obj.HandleId = ReadUInt32(buffer);
                obj.CharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.MessageFlavor = ReadByte(buffer);
                obj.PhrasesCategory = ReadUInt32(buffer);
                obj.PhrasesIndex = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
