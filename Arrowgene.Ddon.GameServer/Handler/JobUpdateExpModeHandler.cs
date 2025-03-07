using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobUpdateExpModeHandler : GameRequestPacketHandler<C2SJobUpdateExpModeReq, S2CJobUpdateExpModeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobUpdateExpModeHandler));

        public JobUpdateExpModeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobUpdateExpModeRes Handle(GameClient client, C2SJobUpdateExpModeReq request)
        {
            //Handle case where the character is somehow missing a PlayPoint structure.
            var missingList = request.UpdateExpModeList.Where(x => !client.Character.PlayPointList.Any(y => y.Job == x.Job)).ToList();
            foreach (var missing in missingList)
            {
                client.Character.PlayPointList.Add(new CDataJobPlayPoint()
                {
                    Job = missing.Job,
                    PlayPoint = new CDataPlayPointData()
                    {
                        PlayPoint = 0,
                        ExpMode = ExpMode.Experience,
                    }
                });
            }

            var res = new S2CJobUpdateExpModeRes()
            {
                PlayPointList = client.Character.PlayPointList.Where(x => request.UpdateExpModeList.Any(y => y.Job == x.Job)).ToList()
            };
            res.PlayPointList.ForEach(x => x.PlayPoint.ExpMode = 3 - x.PlayPoint.ExpMode); //Flip 1 <-> 2

            foreach (CDataJobPlayPoint playpoint in res.PlayPointList)
            {
                Database.ReplaceCharacterPlayPointData(client.Character.CharacterId, playpoint);
            }

            return res;
        }
    }
}
