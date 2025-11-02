using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterMsgSet
    {
        public uint SetNo { get; set; }
        public string MsgSetName { get; set; } = "";
        public List<CDataCharacterMessage> CharacterMessageList { get; set; } = [];

        public class Serializer : EntitySerializer<CDataCharacterMsgSet>
        {
            public override void Write(IBuffer buffer, CDataCharacterMsgSet obj)
            {
                WriteUInt32(buffer, obj.SetNo);
                WriteMtString(buffer, obj.MsgSetName);
                WriteEntityList(buffer, obj.CharacterMessageList);
            }

            public override CDataCharacterMsgSet Read(IBuffer buffer)
            {
                CDataCharacterMsgSet obj = new CDataCharacterMsgSet();
                obj.SetNo = ReadUInt32(buffer);
                obj.MsgSetName = ReadMtString(buffer);
                obj.CharacterMessageList = ReadEntityList<CDataCharacterMessage>(buffer);
                return obj;
            }
        }
    }
}
