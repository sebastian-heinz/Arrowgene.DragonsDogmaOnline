using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetCharacterListHandler : LoginRequestPacketHandler<C2LGetCharacterListReq, L2CGetCharacterListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetCharacterListHandler));

        public GetCharacterListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CGetCharacterListRes Handle(LoginClient client, C2LGetCharacterListReq request)
        {
            L2CGetCharacterListRes res = new();

            List<Character> characters = Database.SelectCharactersByAccountId(client.Account.Id, GameMode.Normal);
            Logger.Info(client, $"Found: {characters.Count} Characters");
            foreach (Character c in characters)
            {
                c.Equipment = c.Storage.GetCharacterEquipment();

                CDataCharacterListInfo cResponse = new CDataCharacterListInfo();
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = (uint)c.CharacterId;
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = c.FirstName;
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = c.LastName;
                cResponse.CharacterListElement.CurrentJobBaseInfo.Job = c.Job;
                cResponse.CharacterListElement.CurrentJobBaseInfo.Level = (byte) c.ActiveCharacterJobData.Lv;

                ulong now = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                List<CDataGPCourseValid> ValidCourses = new List<CDataGPCourseValid>();
                foreach (var ValidCourse in Server.AssetRepository.GPCourseInfoAsset.ValidCourses)
                {
                    if (now > ValidCourse.Value.EndTime)
                    {
                        continue;
                    }

                    CDataGPCourseValid cDataGPCourseValid = new CDataGPCourseValid()
                    {
                        Id = c.CharacterId,
                        CourseId = ValidCourse.Value.Id,
                        NameA = Server.AssetRepository.GPCourseInfoAsset.Courses[ValidCourse.Value.Id].Name, // Course Name
                        NameB = Server.AssetRepository.GPCourseInfoAsset.Courses[ValidCourse.Value.Id].IconPath, // Link to a icon
                    };

                    if ((now >= ValidCourse.Value.StartTime) && (now <= ValidCourse.Value.EndTime))
                    {
                        cDataGPCourseValid.StartTime = ValidCourse.Value.StartTime;
                        cDataGPCourseValid.EndTime = ValidCourse.Value.EndTime;
                    }
                    else
                    {
                        cDataGPCourseValid.StartTime = ValidCourse.Value.StartTime;
                    }

                    ValidCourses.Add(cDataGPCourseValid);
                }

                cResponse.GpCourseValidList = ValidCourses;
                cResponse.NextFlowType = 1;
                cResponse.IsClanMemberNotice = 1; // REMOVE
                // maybe?
                //cResponse.CharacterListElement.CurrentJobBaseInfo.Job = c.CharacterInfo.MatchingProfile.CurrentJob;
                //cResponse.CharacterListElement.CurrentJobBaseInfo.Level = (byte) c.CharacterInfo.MatchingProfile.CurrentJobLevel;
                //cResponse.CharacterListElement.EntryJobBaseInfo.Job = c.CharacterInfo.MatchingProfile.EntryJob;
                //cResponse.CharacterListElement.EntryJobBaseInfo.Level = (byte) c.CharacterInfo.MatchingProfile.EntryJobLevel;
                cResponse.EditInfo = c.EditInfo;
                cResponse.MatchingProfile = c.MatchingProfile;
                cResponse.EquipItemInfo = c.Equipment.AsCDataEquipItemInfo(EquipType.Performance)
                    .Union(c.Equipment.AsCDataEquipItemInfo(EquipType.Visual))
                    .ToList();

                cResponse.ClanName = c.ClanName.Name;
                cResponse.ClanNameShort = c.ClanName.ShortName;
                res.CharacterList.Add(cResponse);
            }

            return res;
        }
    }
}
