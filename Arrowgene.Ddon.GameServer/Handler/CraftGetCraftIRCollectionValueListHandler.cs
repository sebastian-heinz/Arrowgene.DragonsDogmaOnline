using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftIRCollectionValueListHandler : GameRequestPacketHandler<C2SCraftGetCraftIRCollectionValueListReq, S2CCraftGetCraftIRCollectionValueListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftIRCollectionValueListHandler));

        private static readonly List<CDataCraftSkillRate> SkillRateList = new List<CDataCraftSkillRate>
        {
            new CDataCraftSkillRate()
            {
                PawnType = 1,
                SkillType = 1,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 1,
                SkillType = 2,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 1,
                SkillType = 3,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 1,
                SkillType = 4,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 1,
                SkillType = 5,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 2,
                SkillType = 1,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 2,
                SkillType = 2,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 2,
                SkillType = 3,
                Rate = 100
            },
            new CDataCraftSkillRate()
            {
                PawnType = 2,
                SkillType = 4,
                Rate = 33
            },
            new CDataCraftSkillRate()
            {
                PawnType = 2,
                SkillType = 5,
                Rate = 33
            }
        };

        public CraftGetCraftIRCollectionValueListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftGetCraftIRCollectionValueListRes Handle(GameClient client, C2SCraftGetCraftIRCollectionValueListReq request)
        {
            S2CCraftGetCraftIRCollectionValueListRes res = new S2CCraftGetCraftIRCollectionValueListRes();

            res.SkillRateList = SkillRateList;

            return res;
        }
    }
}
