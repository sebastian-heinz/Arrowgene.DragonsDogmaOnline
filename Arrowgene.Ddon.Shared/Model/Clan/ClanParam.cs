using Arrowgene.Ddon.Shared.Entity.Structure;
using System;

namespace Arrowgene.Ddon.Shared.Model.Clan
{
    public class ClanParam
    {
        public ClanParam()
        {
            MasterInfo = new CDataClanMemberInfo();
            Name = string.Empty;
            ShortName = string.Empty;
            Comment = string.Empty;
            BoardMessage = string.Empty;
        }

        public ClanParam(CDataClanParam clanParam)
            : this(clanParam.ClanUserParam, clanParam.ClanServerParam)
        {
        }

        public ClanParam(CDataClanUserParam userParam, CDataClanServerParam serverParam)
        {
            Name = userParam.Name;
            ShortName = userParam.ShortName;
            EmblemMarkType = userParam.EmblemMarkType;
            EmblemBaseType = userParam.EmblemBaseType;
            EmblemBaseMainColor = userParam.EmblemBaseMainColor;
            EmblemBaseSubColor = userParam.EmblemBaseSubColor;
            Motto = userParam.Motto;
            ActiveDays = userParam.ActiveDays;
            ActiveTime = userParam.ActiveTime;
            Characteristic = userParam.Characteristic;
            IsPublish = userParam.IsPublish;
            Comment = userParam.Comment;
            BoardMessage = userParam.BoardMessage;
            Created = userParam.Created;
            ID = serverParam.ID;
            Lv = serverParam.Lv;
            MemberNum = serverParam.MemberNum;
            MasterInfo = serverParam.MasterInfo;
            IsSystemRestriction = serverParam.IsSystemRestriction;
            IsClanBaseRelease = serverParam.IsClanBaseRelease;
            CanClanBaseRelease = serverParam.CanClanBaseRelease;
            TotalClanPoint = serverParam.TotalClanPoint;
            MoneyClanPoint = serverParam.MoneyClanPoint;
            NextClanPoint = serverParam.NextClanPoint;
        }

        public uint ID { get; set; }
        public ushort Lv { get; set; }
        public ushort MemberNum { get; set; }
        public CDataClanMemberInfo MasterInfo { get; set; }
        public bool IsSystemRestriction { get; set; }
        public bool IsClanBaseRelease { get; set; }
        public bool CanClanBaseRelease { get; set; }
        public uint TotalClanPoint { get; set; }
        public uint MoneyClanPoint { get; set; }
        public uint NextClanPoint { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public byte EmblemMarkType { get; set; }
        public byte EmblemBaseType { get; set; }
        public byte EmblemBaseMainColor { get; set; }
        public byte EmblemBaseSubColor { get; set; }
        public uint Motto { get; set; }
        public uint ActiveDays { get; set; }
        public uint ActiveTime { get; set; }
        public uint Characteristic { get; set; }
        public bool IsPublish { get; set; }
        public string Comment { get; set; }
        public string BoardMessage { get; set; }
        public DateTimeOffset Created { get; set; }

        public ClanName ClanName
        {
            get
            {
                return new ClanName()
                {
                    Name = Name,
                    ShortName = ShortName,
                };
            }
        }

        public CDataClanUserParam ToCDataClanUserParam()
        {
            return new CDataClanUserParam()
            {
                Name = Name,
                ShortName = ShortName,
                EmblemMarkType = EmblemMarkType,
                EmblemBaseType = EmblemBaseType,
                EmblemBaseMainColor = EmblemBaseMainColor,
                EmblemBaseSubColor = EmblemBaseSubColor,
                Motto = Motto,
                ActiveDays = ActiveDays,
                ActiveTime = ActiveTime,
                Characteristic = Characteristic,
                IsPublish = IsPublish,
                Comment = Comment,
                BoardMessage = BoardMessage,
                Created = Created
            };
        }

        public CDataClanServerParam ToCDataClanServerParam()
        {
            return new CDataClanServerParam
            {
                ID = ID,
                Lv = Lv,
                MemberNum = MemberNum,
                MasterInfo = MasterInfo,
                IsSystemRestriction = IsSystemRestriction,
                IsClanBaseRelease = IsClanBaseRelease,
                CanClanBaseRelease = CanClanBaseRelease,
                TotalClanPoint = TotalClanPoint,
                MoneyClanPoint = MoneyClanPoint,
                NextClanPoint = NextClanPoint
            };
        }

        public CDataClanParam ToCDataClanParam()
        {
            return new CDataClanParam
            {
                ClanUserParam = ToCDataClanUserParam(),
                ClanServerParam = ToCDataClanServerParam(),
            };
        }

        public void Update(CDataClanUserParam userParam)
        {
            Name = userParam.Name;
            ShortName = userParam.ShortName;
            EmblemMarkType = userParam.EmblemMarkType;
            EmblemBaseType = userParam.EmblemBaseType;
            EmblemBaseMainColor = userParam.EmblemBaseMainColor;
            EmblemBaseSubColor = userParam.EmblemBaseSubColor;
            Motto = userParam.Motto;
            ActiveDays = userParam.ActiveDays;
            ActiveTime = userParam.ActiveTime;
            Characteristic = userParam.Characteristic;
            IsPublish = userParam.IsPublish;
            Comment = userParam.Comment;
            BoardMessage = userParam.BoardMessage;
            Created = userParam.Created;
        }
    }
}
