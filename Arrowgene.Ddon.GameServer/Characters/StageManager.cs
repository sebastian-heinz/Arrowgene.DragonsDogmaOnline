using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class StageManager
    {
        private readonly static List<CDataStageInfo> StageList = EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19).StageList;

        public static StageNo ConvertIdToStageNo(StageId stageId)
        {
            foreach (CDataStageInfo stageInfo in StageList)
            {
                if (stageInfo.Id == stageId.Id)
                    return (StageNo) stageInfo.StageNo;
            }

            return 0; // TODO: Maybe throw an exception?
        }
    }
}
