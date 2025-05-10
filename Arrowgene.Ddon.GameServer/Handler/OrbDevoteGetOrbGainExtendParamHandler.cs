using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetOrbGainExtendParamHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetOrbGainExtendParamHandler));

        public OrbDevoteGetOrbGainExtendParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_50);
            S2COrbDevoteGetOrbGainExtendParamRes Result = new S2COrbDevoteGetOrbGainExtendParamRes();
            Result.ExtendParam = new CDataOrbGainExtendParam()
            {
                HpMax = (ushort) client.Character.StatusInfo.GainHP,
                StaminaMax = (ushort)client.Character.StatusInfo.GainStamina,
                Attack = (ushort)client.Character.StatusInfo.GainAttack,
                Defence = (ushort)client.Character.StatusInfo.GainDefense,
                MagicAttack = (ushort)client.Character.StatusInfo.GainMagicAttack,
                MagicDefence = (ushort)client.Character.StatusInfo.GainMagicDefense,
                AbilityCost = client.Character.ExtendedParams.AbilityCost,
                JewelrySlot = client.Character.ExtendedParams.JewelrySlot,
                UseItemSlot = client.Character.ExtendedParams.UseItemSlot,
                MaterialItemSlot = client.Character.ExtendedParams.MaterialItemSlot,
                EquipItemSlot = client.Character.ExtendedParams.EquipItemSlot,
                MainPawnSlot = client.Character.ExtendedParams.MainPawnSlot,
                SupportPawnSlot = client.Character.ExtendedParams.SupportPawnSlot
            };
            client.Send(Result);
        }
    }
}
