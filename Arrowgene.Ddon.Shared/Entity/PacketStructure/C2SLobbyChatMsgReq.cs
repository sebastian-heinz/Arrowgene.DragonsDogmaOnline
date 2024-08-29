using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLobbyChatMsgReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_LOBBY_LOBBY_CHAT_MSG_REQ;

        public LobbyChatMsgType Type { get; set; }

        /// <summary>
        /// Seemingly always 0?
        /// </summary>
        public uint Unk1 { get; set; } // Target ID?

        /// <summary>
        /// Note that My Phrases are interpreted as regular message & emotes do not use this packet
        /// 0 == regular message
        /// 1 == phrases
        /// </summary>
        public byte MessageFlavor { get; set; }

        /// <summary>
        /// Communication -> Phrases -> Greetings, Questions, Replies, ...
        /// Min is 1 (Greetings), Max is 8 (Tactics II)
        /// </summary>
        public uint PhrasesCategory { get; set; }

        /// <summary>
        /// Index within the Phrases communication UI
        /// 0 == not a phrase, but rather a direct message
        /// Min 1 == Category Greetings - "How do you do"
        /// Max 317 == Category Tactics II - "Let's dedicate this victory to our clan!"
        /// </summary>
        public uint PhrasesIndex { get; set; }

        public string Message { get; set; }

        public C2SLobbyChatMsgReq()
        {
            Type = 0;
            Unk1 = 0;
            MessageFlavor = 0;
            PhrasesCategory = 0;
            PhrasesIndex = 0;
            Message = string.Empty;
        }

        public class Serializer : PacketEntitySerializer<C2SLobbyChatMsgReq>
        {
            public override void Write(IBuffer buffer, C2SLobbyChatMsgReq obj)
            {
                WriteByte(buffer, (byte)obj.Type);
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.MessageFlavor);
                WriteUInt32(buffer, obj.PhrasesCategory);
                WriteUInt32(buffer, obj.PhrasesIndex);
                WriteMtString(buffer, obj.Message);
            }

            public override C2SLobbyChatMsgReq Read(IBuffer buffer)
            {
                C2SLobbyChatMsgReq obj = new C2SLobbyChatMsgReq();
                obj.Type = (LobbyChatMsgType)ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.MessageFlavor = ReadByte(buffer);
                obj.PhrasesCategory = ReadUInt32(buffer);
                obj.PhrasesIndex = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
