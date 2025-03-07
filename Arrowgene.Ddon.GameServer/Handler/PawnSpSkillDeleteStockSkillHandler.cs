using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnSpSkillDeleteStockSkillHandler : GameRequestPacketHandler<C2SPawnSpSkillDeleteStockSkillReq, S2CPawnSpSkillDeleteStockSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillDeleteStockSkillHandler));
        
        public PawnSpSkillDeleteStockSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnSpSkillDeleteStockSkillRes Handle(GameClient client, C2SPawnSpSkillDeleteStockSkillReq request)
        {
            // TODO: Implement
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_NOT_IMPLEMENTED);
        }
    }
}
