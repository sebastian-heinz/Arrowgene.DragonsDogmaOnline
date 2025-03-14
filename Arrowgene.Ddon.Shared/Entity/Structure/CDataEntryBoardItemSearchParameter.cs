using Arrowgene.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryBoardItemSearchParameter
    {
        public CDataEntryBoardItemSearchParameter()
        {
            SearchGroups = new List<CDataCommonU8>();
        }

        public List<CDataCommonU8> SearchGroups {  get; set; } // 1 = Friends, 2 = Clan Members, 3 = Party Members, 4 = Group Chat
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool RankSetting {  get; set; }
        public uint RankMin {  get; set; } // Min Level
        public uint RankMax {  get; set; } // Max Level
        public uint Job {  get; set; } // Job Mask
        public bool IsNoPassword {  get; set; }
        public uint RequiredItemRankMin {  get; set; } // IR MIN
        public uint RequiredItemRankMax {  get; set; } // IR MAX
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }


        public class Serializer : EntitySerializer<CDataEntryBoardItemSearchParameter>
        {
            public override void Write(IBuffer buffer, CDataEntryBoardItemSearchParameter obj)
            {
                WriteEntityList(buffer, obj.SearchGroups);
                WriteMtString(buffer, obj.FirstName);
                WriteMtString(buffer, obj.LastName);
                WriteBool(buffer, obj.RankSetting);
                WriteUInt32(buffer, obj.RankMin);
                WriteUInt32(buffer, obj.RankMax);
                WriteUInt32(buffer, obj.Job);
                WriteBool(buffer, obj.IsNoPassword);
                WriteUInt32(buffer, obj.RequiredItemRankMin);
                WriteUInt32(buffer, obj.RequiredItemRankMax);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataEntryBoardItemSearchParameter Read(IBuffer buffer)
            {
                CDataEntryBoardItemSearchParameter obj = new CDataEntryBoardItemSearchParameter();
                obj.SearchGroups = ReadEntityList<CDataCommonU8>(buffer);
                obj.FirstName = ReadMtString(buffer);
                obj.LastName = ReadMtString(buffer);
                obj.RankSetting = ReadBool(buffer);
                obj.RankMin = ReadUInt32(buffer);
                obj.RankMax = ReadUInt32(buffer);
                obj.Job = ReadUInt32(buffer);
                obj.IsNoPassword = ReadBool(buffer);
                obj.RequiredItemRankMin = ReadUInt32(buffer);
                obj.RequiredItemRankMax = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
