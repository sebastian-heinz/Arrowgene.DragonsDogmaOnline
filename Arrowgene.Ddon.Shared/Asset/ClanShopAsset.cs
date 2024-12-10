using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class ClanShopAsset
    {
        public ClanShopAsset() { 
            Name = string.Empty;
        }

        public ClanShopLineupType Type { get; set; }
        public uint LineupId { get; set; }
        public uint RequireClanPoint { get; set; }
        public byte RequireLevel { get; set; }
        public uint IconID { get; set; }
        public string Name { get; set; }
        public uint SubId { get; set; }
        public byte SubType { get; set; }
        public uint Require { get; set; }

        public CDataClanShopBuffItem ToCDataClanShopBuffItem()
        {
            var ret = new CDataClanShopBuffItem()
            {
                LineupId = LineupId,
                RequireClanPoint = RequireClanPoint,
                RequireLevel = RequireLevel,
                IconID = IconID,
                Name = Name,
                BuffInfo = new()
                {
                    new CDataClanShopBuffInfo()
                    {
                        BuffId = SubId,
                        BuffType = SubType
                    }
                }
            };
            if (Require > 0)
            {
                ret.RequireLineupId.Add(new CDataCommonU32(Require));
            }

            return ret;
        }

        public CDataClanShopFunctionItem ToCDataClanShopFunctionItem()
        {
            var ret = new CDataClanShopFunctionItem()
            {
                LineupId = LineupId,
                RequireClanPoint = RequireClanPoint,
                RequireLevel = RequireLevel,
                IconID = IconID,
                Name = Name,
                FunctionInfo = new()
                {
                    new CDataClanShopFunctionInfo()
                    {
                        FunctionId = SubId,
                        FunctionType = SubType
                    }
                }
            };
            if (Require > 0)
            {
                ret.RequireLineupId.Add(new CDataCommonU32(Require));
            }

            return ret;
        }

        public CDataClanShopLineupName ToCDataClanShopLineupName()
        {
            return new()
            {
                Name = Name,
                LineupID = LineupId
            };
        }
    }
}
