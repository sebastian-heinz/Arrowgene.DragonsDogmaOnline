using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    /// <summary>
    /// Sent when changing skill pallets and some automatic cases, such as changing vocations and logging in.
    /// </summary>
    public class RpcCtrlCsChange : RpcPacketBase
    {
        private static readonly Logger Logger = LogProvider.Logger<Logger>(typeof(RpcCtrlCsChange));

        /// <summary>
        /// 0 for the main pallet, 1 for the alt pallet.
        /// </summary>
        public byte CustomSkillGroup { get; set; }

        public RpcCtrlCsChange()
        {
        }

        public override void Handle(Character character, RpcPacketHeader packetHeader, IBuffer buffer)
        {
            // Support only the player for now
            if (packetHeader.SearchId == 0) // SearchId == CharacterId?
            {
                RpcCtrlCsChange obj = ReadPacketData(buffer);
                character.CustomSkillGroup = obj.CustomSkillGroup;
            }
        }

        private RpcCtrlCsChange ReadPacketData(IBuffer buffer)
        {
            RpcCtrlCsChange obj = new RpcCtrlCsChange();

            // TODO: Figure out what the rest of the bytes are for.
            // It's usually a 33 byte packet.
            var bytes = buffer.GetAllBytes();
            obj.CustomSkillGroup = bytes.Last();

            return obj;
        }
    }
}
