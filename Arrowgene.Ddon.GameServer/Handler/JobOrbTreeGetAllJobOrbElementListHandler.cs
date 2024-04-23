using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler : GameStructurePacketHandler<C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler));

        public JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq> Request)
        {
            S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes Response = new S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes();

            // Stop menu from crashing game by returning an empty list
            // Unfortunately, client sends an error when more than 1 CDataJobOrbDevoteElement is added to the list.
            // Probably due to unknown packet format
#if false
            Response.ElementList.Add(new CDataJobOrbDevoteElement()
            {
                ElementId = 1,
                Unk0 = 2,
                JobId = Request.Structure.JobId,
                RequireOrb = 10,
                OrbRewardType = 1,
                ParamId = 0,
                ParamValue = 0, // Shows up next to orb reward type
                PosX = 1,
                PosY = 2,
                IsReleased = false
                RequiredElementIDList = new List<CDataCommonU32>() { new CDataCommonU32(0) },
                RequiredQuestList = new List<CDataCommonU32>() { new CDataCommonU32(0) }
            });

            Response.ElementList.Add(new CDataJobOrbDevoteElement()
            {
                ElementId = 2,
                Unk0 = 0,
                JobId = Request.Structure.JobId,
                RequireOrb = 10,
                OrbRewardType = 2,
                ParamId = 0,
                ParamValue = 0, // Shows up next to orb reward type
                PosX = 2,
                PosY = 2,
                IsReleased = false,
                RequiredElementIDList = new List<CDataCommonU32>() { new CDataCommonU32(0) },
                RequiredQuestList = new List<CDataCommonU32>() { new CDataCommonU32(0) }
            });
#endif
            client.Send(Response);
        }
    }
}
