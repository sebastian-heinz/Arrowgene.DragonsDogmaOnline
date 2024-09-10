using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetCharacterListHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetCharacterListHandler));
        private AssetRepository _AssetRepo;

        public GetCharacterListHandler(DdonLoginServer server) : base(server)
        {
            _AssetRepo = server.AssetRepository;
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big); // error
            buffer.WriteInt32(0, Endianness.Big); // result

            List<CDataCharacterListInfo> characterListResponse = new List<CDataCharacterListInfo>();
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
                foreach (var ValidCourse in _AssetRepo.GPCourseInfoAsset.ValidCourses)
                {
                    if (now > ValidCourse.Value.EndTime)
                    {
                        continue;
                    }

                    CDataGPCourseValid cDataGPCourseValid = new CDataGPCourseValid()
                    {
                        ID = c.CharacterId,
                        CourseID = ValidCourse.Value.Id,
                        Name = _AssetRepo.GPCourseInfoAsset.Courses[ValidCourse.Value.Id].Name, // Course Name
                        ImageAddr = _AssetRepo.GPCourseInfoAsset.Courses[ValidCourse.Value.Id].IconPath, // Link to a icon
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

                characterListResponse.Add(cResponse);
            }

            EntitySerializer.Get<CDataCharacterListInfo>().WriteList(buffer, characterListResponse);
            Packet response = new Packet(PacketId.L2C_GET_CHARACTER_LIST_RES, buffer.GetAllBytes());
            client.Send(response);
        }
    }
}
