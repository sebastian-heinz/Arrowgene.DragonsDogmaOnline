using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterListElement
    {
        public CDataCharacterListElement()
        {
            CharacterID = 0;
            FirstName = "";
            LastName = "";
            ClanName = "";
            ServerID = 0;
            OnlineStatus = 0;
            jobInfo0 = new CDataJobInfo();
            jobInfo1 = new CDataJobInfo();
            m_wstrMatchingPlofile = "";
            unk2 = 0;
        }

        public uint CharacterID;
        public string FirstName;
        public string LastName;
        public string ClanName;
        public ushort ServerID;
        public byte OnlineStatus;
        public CDataJobInfo jobInfo0;
        public CDataJobInfo jobInfo1;

        /*
        Represents:
        CurrentJobInfo_ucJob;
        CurrentJobInfo_ucLv;
        EntryJobInfo_ucJob;
        EntryJobInfo_ucLv;
        */
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
            obj.jobInfo0 = ReadEntity<CDataJobInfo>(buffer);
            obj.jobInfo1 = ReadEntity<CDataJobInfo>(buffer);
            obj.m_wstrMatchingPlofile = ReadMtString(buffer);
            obj.unk2 = ReadByte(buffer);
            return obj;
        }
    }
}
