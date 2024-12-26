using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class INpcExtendedFacility
    {
        /// <summary>
        /// NPC ID associated with the extended options.
        /// </summary>
        public NpcId NpcId { get; protected set; }

        /// <summary>
        /// Gets extended menu options for the NPC.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="client"></param>
        /// <param name="result">The result object for the extended NPC options</param>
        public abstract void GetExtendedOptions(DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result);
    }
}
