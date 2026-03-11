using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DispelLockSettingsHandler : GameRequestPacketHandler<C2SDispelLockSettingReq, S2CDispelLockSettingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelLockSettingsHandler));

        public DispelLockSettingsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CDispelLockSettingRes Handle(GameClient client, C2SDispelLockSettingReq request)
        {
            S2CItemUpdateCharacterItemNtc ntc = new();
            int price = 0;
            if (request.IsNormalSeal)
            {
                for (int i = 1; i <= request.SettingUpdates.Where(x => x.LockStatus).Count(); i++)
                {
                    price += (int)(Server.GameSettings.GameServerSettings.DispelSealCostRate * (client.Character.DispelSeals.Count + i));
                }
            }
            else
            {
                price += (int)Server.GameSettings.GameServerSettings.DispelSealResetRate;
                for (int i = 1; i <= request.SettingUpdates.Count; i++)
                {
                    price -= (int)(Server.GameSettings.GameServerSettings.DispelSealCostRate * i);
                }
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                if (price > 0)
                {
                    ntc = Server.WalletManager.RemoveFromWalletNtc2(client.Character, Shared.Model.WalletType.RedDragonMark, (uint)price, connection);
                }
                else
                {
                    ntc = Server.WalletManager.AddToWalletNtc(client, client.Character, Shared.Model.WalletType.RedDragonMark, (uint)(-price), connectionIn:connection);
                }    

                foreach (var update in request.SettingUpdates)
                {
                    if (update.LockStatus)
                    {
                        client.Character.DispelSeals.Add(update.SealIndex);
                        Server.Database.InsertDispelSeal(client.Character.CharacterId, update.SealIndex, connection);
                    }
                    else
                    {
                        client.Character.DispelSeals.Remove(update.SealIndex);
                        Server.Database.DeleteDispelSeal(client.Character.CharacterId, update.SealIndex, connection);
                    }
                }
            });

            client.Send(ntc);

            return new()
            {
                Unk0 = [.. BitterblackMazeRewards.SealUIData.Select(x => new CDataDispelLockSettingUpdate()
                {
                    SealIndex = x.SealIndex,
                    LockStatus = client.Character.DispelSeals.Contains(x.SealIndex)
                })]
            };
        }
    }
}
