using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanScoutEntrySearchResult
    {
        public CDataClanScoutEntrySearchResult()
        {
            CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo();
            Comment = "";
        }

        CDataCommunityCharacterBaseInfo CommunityCharacterBaseInfo { get; set; }
        string Comment { get; set; }

        public class Serializer : EntitySerializer<CDataClanScoutEntrySearchResult>
        {
            public override void Write(IBuffer buffer, CDataClanScoutEntrySearchResult obj)
            {
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.CommunityCharacterBaseInfo);
                WriteMtString(buffer, obj.Comment);
            }

            public override CDataClanScoutEntrySearchResult Read(IBuffer buffer)
            {
                CDataClanScoutEntrySearchResult obj = new CDataClanScoutEntrySearchResult();
                obj.CommunityCharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.Comment = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
