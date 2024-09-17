using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanCreateHandler : GameRequestPacketHandler<C2SClanClanCreateReq, S2CClanClanCreateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanCreateHandler));

        public ClanClanCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanCreateRes Handle(GameClient client, C2SClanClanCreateReq request)
        {
            var res = new S2CClanClanCreateRes();

            var memberInfo = new CDataClanMemberInfo()
            {
                Rank = ClanMemberRank.Master,
                Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastLoginTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LeaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Permission = uint.MaxValue
            };

            GameStructure.CDataCharacterListElement(memberInfo.CharacterListElement, client.Character);

            Logger.Info($"Motto: {request.CreateParam.Motto}" +
                $"\nActiveDays: {request.CreateParam.ActiveDays}" +
                $"\nActiveTime: {request.CreateParam.ActiveTime}" +
                $"\nCharacteristic: {request.CreateParam.Characteristic}");

            var serverParam = new CDataClanServerParam()
            {
                ID = 1,
                Lv = 1,
                MemberNum = 1,
                MasterInfo = memberInfo,
                IsSystemRestriction = false,
                IsClanBaseRelease = false,
                CanClanBaseRelease = false,
                TotalClanPoint = 0,
                MoneyClanPoint = 50,
                NextClanPoint = 100,
            };

            res.ClanParam.ClanUserParam = request.CreateParam;
            res.ClanParam.ClanUserParam.Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            res.ClanParam.ClanServerParam = serverParam;
            res.MemberList.Add(memberInfo);

            return res;
        }
    }
}
