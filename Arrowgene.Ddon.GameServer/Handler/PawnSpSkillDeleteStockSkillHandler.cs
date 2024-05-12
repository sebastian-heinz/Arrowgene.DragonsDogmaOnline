using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnSpSkillDeleteStockSkillHandler : GameStructurePacketHandler<C2SPawnSpSkillDeleteStockSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillDeleteStockSkillHandler));
        
        public PawnSpSkillDeleteStockSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnSpSkillDeleteStockSkillReq> packet)
        {
            // TODO: Implement
            client.Send(new S2CPawnSpSkillDeleteStockSkillRes()
            {
                Error = (uint) ErrorCode.ERROR_CODE_FAIL
            });
        }
    }
}