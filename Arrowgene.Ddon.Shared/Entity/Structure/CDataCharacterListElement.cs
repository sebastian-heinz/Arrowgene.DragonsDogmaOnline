using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataCharacterListElement
    {
        uint CharacterID;
        // length prefix
        string FirstName;
        // length prefix
        string LastName;
        // length prefix
        string ClanName;
        ushort ServerID;
        byte OnlineStatus;

        DoubleByteThing jobInfo0;
        DoubleByteThing jobInfo1;
        /*
        Represents:
        CurrentJobInfo_ucJob;
        CurrentJobInfo_ucLv;
        EntryJobInfo_ucJob;
        EntryJobInfo_ucLv;
        */

        // length prefix
        string m_wstrMatchingPlofile;
        byte unk2;
    }

    public class CDataCharacterListElementSerializer : EntitySerializer<CDataCharacterListElement>
    {
        public override void Write(IBuffer buffer, CDataCharacterListElement obj)
        {
            throw new NotImplementedException();
        }

        public override CDataCharacterListElement Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
