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
            public bool NameDispOff { get; set; }
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
            public int VoiceReqNo { get; set; }
            public byte TalkFaceType { get; set; }
        }

        public List<MsgGroup> MsgGroups { get; }

        public MsgSet()
        {
            MsgGroups = new List<MsgGroup>();
        }
        
        protected override void ReadResource(IBuffer buffer)
        {
            ushort version = ReadUInt16(buffer);
            MsgGroups.Clear();
            uint countA = ReadUInt32(buffer);
            uint countB = ReadUInt32(buffer);

            for (int i = 0; i < countA; i++)
            {
                byte a = ReadByte(buffer);
                uint b = ReadUInt32(buffer);
                uint c = ReadUInt32(buffer);
                uint d = ReadUInt32(buffer);
                uint e = ReadUInt32(buffer);
                byte f = ReadByte(buffer);
                uint g = ReadUInt32(buffer);
                byte h = ReadByte(buffer);
                uint j = ReadUInt32(buffer);
                uint k = ReadUInt32(buffer);
                uint l = ReadUInt32(buffer);
                uint m = ReadUInt32(buffer);
                uint n = ReadUInt32(buffer);
                uint o = ReadUInt32(buffer);
                uint p = ReadUInt32(buffer);
                uint q = ReadUInt32(buffer);
                uint r = ReadByte(buffer);
            }
            
            List<MsgGroup> msgGroups = ReadMtArray(buffer, ReadEntry);
            MsgGroups.AddRange(msgGroups);
        }

        private MsgGroup ReadEntry(IBuffer buffer)
        {
        
            
         //   MsgGroup entry = new MsgGroup();
         //   entry.GroupSerial = ReadUInt32(buffer);
         //   entry.GroupType = ReadUInt32(buffer);
         //   entry.NpcId = ReadByte(buffer);
         //   entry.GroupNameSerial = ReadUInt32(buffer);
         //   entry.NameDispOff = ReadBool(buffer);
//
         //   MsgData msgData = new MsgData();
         //   msgData.MsgSerial = ReadUInt32(buffer);
         //   msgData.GmdIndex = ReadUInt32(buffer);
         //   msgData.MsgType = ReadUInt32(buffer);
         //   msgData.JumpGroupSerial = ReadUInt32(buffer);
         //   msgData.DispType = ReadUInt32(buffer);
         //   msgData.DispTime = ReadUInt32(buffer);
         //   msgData.SetMotion = ReadUInt32(buffer);
         //   msgData.VoiceReqNo = ReadInt32(buffer);
         //   msgData.TalkFaceType = ReadByte(buffer);
//
         //   entry.MsgData = msgData;
            
            return new MsgGroup();
        }
    }
}
