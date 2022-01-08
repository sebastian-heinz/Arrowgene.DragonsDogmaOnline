using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataCharacterListElement
    {
        public uint CharacterID;

        // length prefix
        public string FirstName;

        // length prefix
        public string LastName;

        // length prefix
        public string ClanName;
        public ushort ServerID;
        public byte OnlineStatus;

        public DoubleByteThing jobInfo0;

        public DoubleByteThing jobInfo1;
        /*
        Represents:
        CurrentJobInfo_ucJob;
        CurrentJobInfo_ucLv;
        EntryJobInfo_ucJob;
        EntryJobInfo_ucLv;
        */

        // length prefix
        public string m_wstrMatchingPlofile;
        public byte unk2;
    }

    public class CDataCharacterListElementSerializer : EntitySerializer<CDataCharacterListElement>
    {
        public override void Write(IBuffer buffer, CDataCharacterListElement obj)
        {
            WriteUInt32(buffer, obj.CharacterID);
            WriteMtString(buffer, obj.FirstName);
            WriteMtString(buffer, obj.LastName);
            WriteMtString(buffer, obj.ClanName);
            WriteUInt16(buffer, obj.ServerID);
            WriteByte(buffer, obj.OnlineStatus);
            WriteEntity(buffer, obj.jobInfo0);
            WriteEntity(buffer, obj.jobInfo1);
            WriteMtString(buffer, obj.m_wstrMatchingPlofile);
            WriteByte(buffer, obj.unk2);
        }

        public override CDataCharacterListElement Read(IBuffer buffer)
        {
            CDataCharacterListElement obj = new CDataCharacterListElement();
            obj.CharacterID = ReadUInt32(buffer);
            obj.FirstName = ReadMtString(buffer);
            obj.LastName = ReadMtString(buffer);
            obj.ClanName = ReadMtString(buffer);
            obj.ServerID = ReadUInt16(buffer);
            obj.OnlineStatus = ReadByte(buffer);
            obj.jobInfo0 = ReadEntity<DoubleByteThing>(buffer);
            obj.jobInfo1 = ReadEntity<DoubleByteThing>(buffer);
            obj.m_wstrMatchingPlofile = ReadMtString(buffer);
            obj.unk2 = ReadByte(buffer);
            return obj;
        }
    }
}
