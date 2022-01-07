using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataMatchingProfile
    {
        byte EntryJob;
        uint EntryJobLevel;
        byte CurrentJob;
        uint CurrentJobLevel;
        uint ObjectiveType1;
        uint ObjectiveType2;
        uint PlayStyle;
        // length prefix
        string Comment;
        byte IsJoinParty;
    }

    public class CDataMatchingProfileSerializer : EntitySerializer<CDataMatchingProfile>
    {
        public override void Write(IBuffer buffer, CDataMatchingProfile obj)
        {
            throw new NotImplementedException();
        }

        public override CDataMatchingProfile Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
