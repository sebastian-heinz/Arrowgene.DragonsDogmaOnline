using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DispelGetLockSettingsHandler : GameRequestPacketHandler<C2SDispelGetLockSettingReq, S2CDispelGetLockSettingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelGetLockSettingsHandler));

        public DispelGetLockSettingsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CDispelGetLockSettingRes Handle(GameClient client, C2SDispelGetLockSettingReq request)
        {
            uint earringPage = 1;
            uint braceletPage = 1;

            S2CDispelGetLockSettingRes res = new();
            res.MaxSeals = Server.GameSettings.GameServerSettings.DispelSealMax;
            res.PageData = [.. BitterblackMazeRewards.AppraisalData
                .GroupBy(x => x.SealPage)
                .OrderBy(x => x.Key)
                .Select(x => new CDataDispelLockPageData()
                {
                    PageName = x.First().BaseItem == ItemId.BitterblackEarring ?  $"Earring {earringPage++}" :  $"Bracelet {braceletPage++}",
                    SealIndexList = [.. x.DistinctBy(x => x.SealIndex).Select(x => new CDataCommonU32(x.SealIndex)).OrderBy(x => x.Value)]
                }
                )];

            res.ResetCostData.Add(new()
            {
                Unk0 = 1, // The client complains and doesn't send the C2SLockSettingsReq if this is 0.
                WalletType = WalletType.RedDragonMark,
                Cost = Server.GameSettings.GameServerSettings.DispelSealResetRate,
            });

            res.AddSealCostData.Add(new()
            {
                Unk0 = 1, // The client complains and doesn't send the C2SLockSettingsReq if this is 0.
                WalletType = WalletType.RedDragonMark,
                Cost = Server.GameSettings.GameServerSettings.DispelSealCostRate,
            });

            res.SealData = [.. BitterblackMazeRewards.SealUIData
                .Select(x => new CDataDispelLockSealData()
                {
                    SealIndex = x.SealIndex,
                    DisplayText = x.SealName,
                    LockStatus = client.Character.DispelSeals.Contains(x.SealIndex)
                })];

            return res;
        }
    }
}
