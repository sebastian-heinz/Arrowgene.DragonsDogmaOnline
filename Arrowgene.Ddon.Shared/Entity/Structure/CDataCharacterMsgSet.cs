using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterMsgSet
    {
        public CDataCharacterMsgSet()
        {
            SetNo = 0;
            MsgSetName = "";
            CharacterMessageList = new List<CDataCharacterMessage>();
        }

        public uint SetNo;
        public string MsgSetName;
        public List<CDataCharacterMessage> CharacterMessageList;

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
