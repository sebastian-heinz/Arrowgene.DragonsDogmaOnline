using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler : GameRequestPacketHandler<C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq, S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler));

        public JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes Handle(GameClient client, C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq request)
        {
            S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes response = new S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes();

            // Stop menu from crashing game by returning an empty list
            // Unfortunately, client sends an error when more than 1 CDataJobOrbDevoteElement is added to the list.
            // Probably due to unknown packet format
#if false
            response.ElementList.Add(new CDataJobOrbDevoteElement()
            {
                ElementId = 1,
                Unk0 = 2,
                JobId = request.Structure.JobId,
                RequireOrb = 10,
                OrbRewardType = 1,
                ParamId = 0,
                ParamValue = 0, // Shows up next to orb reward type
                PosX = 1,
                IsReleased = false
                RequiredElementIDList = new List<CDataCommonU32>() { new CDataCommonU32(0) },
                RequiredQuestList = new List<CDataCommonU32>() { new CDataCommonU32(0) }
            });

            response.ElementList.Add(new CDataJobOrbDevoteElement()
            {
                ElementId = 2,
                Unk0 = 0,
                JobId = request.Structure.JobId,
                RequireOrb = 10,
                OrbRewardType = 2,
                ParamId = 0,
                ParamValue = 0, // Shows up next to orb reward type
                PosX = 2,
                IsReleased = false,
                RequiredElementIDList = new List<CDataCommonU32>() { new CDataCommonU32(1) },
                RequiredQuestList = new List<CDataCommonU32>() { new CDataCommonU32(0) }
            });
#endif
            return response;
        }
    }
}
