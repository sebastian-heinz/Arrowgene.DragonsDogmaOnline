using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IBitterblackEarringMixin
    {
        public abstract ushort RollBitterBlackMazeEarringPercent(JobId jobId);
    }
}
