using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SChatSendTellMsgReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHAT_SEND_TELL_MSG_REQ;

        public C2SChatSendTellMsgReq()
        {
        }

        public CDataCommunityCharacterBaseInfo CharacterInfo { get; set; } = new();

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

        public string Message { get; set; } = string.Empty;

        public class Serializer : PacketEntitySerializer<C2SChatSendTellMsgReq>
        {
            public override void Write(IBuffer buffer, C2SChatSendTellMsgReq obj)
            {
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.CharacterInfo);
                WriteByte(buffer, obj.MessageFlavor);
                WriteUInt32(buffer, obj.PhrasesCategory);
                WriteUInt32(buffer, obj.PhrasesIndex);
                WriteMtString(buffer, obj.Message);
            }

            public override C2SChatSendTellMsgReq Read(IBuffer buffer)
            {
                C2SChatSendTellMsgReq obj = new C2SChatSendTellMsgReq();

                obj.CharacterInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.MessageFlavor = ReadByte(buffer);
                obj.PhrasesCategory = ReadUInt32(buffer);
                obj.PhrasesIndex = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);

                return obj;
            }
        }
    }
}
