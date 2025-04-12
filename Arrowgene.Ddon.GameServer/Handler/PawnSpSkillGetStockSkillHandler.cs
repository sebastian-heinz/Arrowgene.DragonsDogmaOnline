using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnSpSkillGetStockSkillHandler : GameRequestPacketHandler<C2SPawnSpSkillGetStockSkillReq, S2CPawnSpSkillGetStockSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillGetStockSkillHandler));
        
        // All Pawn Special Skills, to their max level
        private static readonly List<CDataSpSkill> AllSpSkills = Enumerable.Range(1, 9).Select(id => new CDataSpSkill() { SpSkillId = (byte) id, SpSkillLv = 3 }).ToList();

        public PawnSpSkillGetStockSkillHandler(DdonGameServer server) : base(server)
        {
        }
        
        public override S2CPawnSpSkillGetStockSkillRes Handle(GameClient client, C2SPawnSpSkillGetStockSkillReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            S2CPawnSpSkillGetStockSkillRes res = new S2CPawnSpSkillGetStockSkillRes
            {
                SpSkillList = AllSpSkills,
                StockSlots = 10 // Value taken from the tutorial picture
            };
            return res;
        }
    }
}
