using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class MsgSet : ResourceFile
    {

        public class MsgGroup
        {
            public uint GroupSerial { get; set; }
            public uint GroupType { get; set; }
            public byte NpcId { get; set; }
            public uint GroupNameSerial { get; set; }
            public uint NameDispOff { get; set; }
            public MsgData MsgData { get; set; }
        }
        
        public class MsgData
        {
            public uint MsgSerial { get; set; }
            public uint GmdIndex { get; set; }
            public uint MsgType { get; set; }
            public uint JumpGroupSerial { get; set; }
            public uint DispType { get; set; }
            public uint DispTime { get; set; }
            public uint SetMotion { get; set; }
            public uint VoiceReqNo { get; set; }
            public byte TalkFaceType { get; set; }
        }

        public List<MsgGroup> MsgGroups { get; }

        public MsgSet()
        {
            MsgGroups = new List<MsgGroup>();
        }

        protected override void ReadResource(IBuffer buffer)
        {
            MsgGroups.Clear();
            List<MsgGroup> msgGroups = ReadMtArray(buffer, ReadEntry);
            MsgGroups.AddRange(msgGroups);
        }

        private MsgGroup ReadEntry(IBuffer buffer)
        {
            MsgGroup entry = new MsgGroup();
            
            return entry;
        }
    }
}
