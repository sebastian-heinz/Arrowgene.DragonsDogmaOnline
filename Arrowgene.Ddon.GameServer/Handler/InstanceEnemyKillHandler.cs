using System;
using System.Linq;
using Arrowgene.Ddon.GameServer.Enemy;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Party;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyKillHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyKillReq>
    {
        private static readonly double EXP_MULTIPLIER = 0.02;
        private static readonly double EXP_BOSS_MUTLIPLIER = 0.2;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyKillHandler));

        private readonly EnemyManager _enemyManager;
        private readonly ExpManager _expManager;

        public InstanceEnemyKillHandler(DdonGameServer server) : base(server)
        {
            _enemyManager = server.EnemyManager;
            _expManager = server.ExpManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyKillReq> packet)
        {
            client.Send(new S2CInstanceEnemyKillRes());

            // TODO: Exp and Lvl for the pawns in the party

            foreach(GameClient partyClient in client.Party.Clients)
            {
                EnemySpawn enemyKilled = this._enemyManager.GetAssets(packet.Structure.LayoutId, 0)[(int) packet.Structure.SetId];
                uint gainedExp = this.calculateExp(enemyKilled);
                uint extraBonusExp = 0; // TODO: Figure out what this is for
                this._expManager.AddExp(partyClient, gainedExp, extraBonusExp);

                // Drop Gold and HOBO
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

                // Drop Gold
                uint gold = this.calculateGold(enemyKilled);

                CDataWalletPoint goldWallet = partyClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).Single();
                goldWallet.Value += gold;

                CDataUpdateWalletPoint goldUpdateWalletPoint = new CDataUpdateWalletPoint();
                goldUpdateWalletPoint.Type = WalletType.Gold;
                goldUpdateWalletPoint.AddPoint = (int) gold;
                goldUpdateWalletPoint.Value = goldWallet.Value;
                updateCharacterItemNtc.UpdateWalletList.Add(goldUpdateWalletPoint);

                // PERSIST CHANGES IN DB
                Server.Database.UpdateWalletPoint(partyClient.Character.Id, goldWallet);

                if(enemyKilled.Enemy.IsBloodEnemy)
                {
                    // Drop BO
                    uint bo = this.calculateBO(enemyKilled);

                    CDataWalletPoint boWallet = partyClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.BloodOrbs).Single();
                    boWallet.Value += bo;

                    CDataUpdateWalletPoint boUpdateWalletPoint = new CDataUpdateWalletPoint();
                    boUpdateWalletPoint.Type = WalletType.BloodOrbs;
                    boUpdateWalletPoint.AddPoint = (int) bo;
                    boUpdateWalletPoint.Value = boWallet.Value;
                    updateCharacterItemNtc.UpdateWalletList.Add(boUpdateWalletPoint);

                    // PERSIST CHANGES IN DB
                    Server.Database.UpdateWalletPoint(partyClient.Character.Id, boWallet);
                }

                if(enemyKilled.Enemy.IsHighOrbEnemy)
                {
                    // Drop HO
                    uint ho = this.calculateHO(enemyKilled);

                    CDataWalletPoint hoWallet = partyClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.HighOrbs).Single();
                    hoWallet.Value += ho;

                    CDataUpdateWalletPoint hoUpdateWalletPoint = new CDataUpdateWalletPoint();
                    hoUpdateWalletPoint.Type = WalletType.HighOrbs;
                    hoUpdateWalletPoint.AddPoint = (int) ho;
                    hoUpdateWalletPoint.Value = hoWallet.Value;
                    updateCharacterItemNtc.UpdateWalletList.Add(hoUpdateWalletPoint);

                    // PERSIST CHANGES IN DB
                    Server.Database.UpdateWalletPoint(partyClient.Character.Id, hoWallet);
                }

                if(updateCharacterItemNtc.UpdateItemList.Count != 0 || updateCharacterItemNtc.UpdateWalletList.Count != 0)
                {
                    partyClient.Send(updateCharacterItemNtc);
                }
            }
        }

        private uint calculateExp(EnemySpawn enemyKilled)
        {
            // TODO: PRoper implementation
            int i = Math.Clamp(enemyKilled.Enemy.Lv+1, 0, ExpManager.EXP_UNTIL_NEXT_LV.Length-1);
            return (uint) (ExpManager.EXP_UNTIL_NEXT_LV[i] * this.calculateEnemyTypeMultiplier(enemyKilled));
        }

        private double calculateEnemyTypeMultiplier(EnemySpawn enemyKilled)
        {
            // As it is right now, you have to kill 5 bosses of your level or 50 regular enemies to level up
            if(enemyKilled.Enemy.IsAreaBoss || enemyKilled.Enemy.IsBossBGM || enemyKilled.Enemy.IsBossGauge)
            {
                return EXP_BOSS_MUTLIPLIER;
            }
            else
            {
                return EXP_MULTIPLIER;
            }
        }

        private uint calculateGold(EnemySpawn enemyKilled)
        {
            return 0; // TODO:
        }

        private uint calculateBO(EnemySpawn enemyKilled)
        {
            return enemyKilled.Enemy.Lv;
        }
        private uint calculateHO(EnemySpawn enemyKilled)
        {
            return enemyKilled.Enemy.Lv;
        }
    }
}