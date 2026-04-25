using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    /// <summary>
    /// Sent when changing skill pallets and some automatic cases, such as changing vocations and logging in.
    /// </summary>
    public class RpcCtrlCsChange : RpcPacketBase
    {
        private static readonly Logger Logger = LogProvider.Logger<Logger>(typeof(RpcCtrlCsChange));

        /// <summary>
        /// These seem to always be the same?
        /// Doesn't obviously depend on HP, vocation, equipment, position, or stage.
        /// </summary>
        public byte[] Unk { get; set; } = new byte[32];
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

            obj.Unk = ReadBytes(buffer, 32);
            obj.CustomSkillGroup = ReadByte(buffer);

            return obj;
        }
    }
}
