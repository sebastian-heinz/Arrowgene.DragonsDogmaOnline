using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Party;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyKillHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyKillReq>
    {

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyKillHandler));

        private readonly DdonGameServer _gameServer;

        public InstanceEnemyKillHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyKillReq> packet)
        {
            Enemy enemyKilled = client.Party.InstanceEnemyManager.GetAssets(StageId.FromStageLayoutId(packet.Structure.LayoutId), 0)[(int) packet.Structure.SetId];
            List<InstancedGatheringItem> instancedGatheringItems = client.InstanceDropItemManager.GetAssets(packet.Structure.LayoutId, packet.Structure.SetId);
            if(instancedGatheringItems.Count > 0) {
                client.Party.SendToAll(new S2CInstancePopDropItemNtc()
                {
                    LayoutId = packet.Structure.LayoutId,
                    SetId = packet.Structure.SetId,
                    MdlType = enemyKilled.DropsTable.MdlType,
                    PosX = packet.Structure.DropPosX,
                    PosY = packet.Structure.DropPosY,
                    PosZ = packet.Structure.DropPosZ
                });
            }

            // TODO: EnemyId and KillNum
            client.Send(new S2CInstanceEnemyKillRes() { 
                EnemyId = enemyKilled.Id,
                KillNum = 1
            });

            foreach(PartyMember member in client.Party.Members)
            {
                uint bo = enemyKilled.BloodOrbs;
                uint ho = enemyKilled.HighOrbs;
                uint gainedExp = enemyKilled.Experience;
                uint extraBonusExp = 0; // TODO: Figure out what this is for (gp bonus?)

                GameClient memberClient;
                CharacterCommon memberCharacter;
                if(member is PlayerPartyMember)
                {
                    memberClient = ((PlayerPartyMember) member).Client;
                    memberCharacter = memberClient.Character;
                    S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

                    if(enemyKilled.BloodOrbs > 0)
                    {
                        // Drop BO
                        CDataWalletPoint boWallet = memberClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.BloodOrbs).Single();
                        boWallet.Value += bo;

                        CDataUpdateWalletPoint boUpdateWalletPoint = new CDataUpdateWalletPoint();
                        boUpdateWalletPoint.Type = WalletType.BloodOrbs;
                        boUpdateWalletPoint.AddPoint = (int) bo;
                        boUpdateWalletPoint.Value = boWallet.Value;
                        updateCharacterItemNtc.UpdateWalletList.Add(boUpdateWalletPoint);

                        // PERSIST CHANGES IN DB
                        Server.Database.UpdateWalletPoint(memberClient.Character.CharacterId, boWallet);
                    }

                    if(enemyKilled.HighOrbs > 0)
                    {
                        // Drop HO
                        CDataWalletPoint hoWallet = memberClient.Character.WalletPointList.Where(wp => wp.Type == WalletType.HighOrbs).Single();
                        hoWallet.Value += ho;

                        CDataUpdateWalletPoint hoUpdateWalletPoint = new CDataUpdateWalletPoint();
                        hoUpdateWalletPoint.Type = WalletType.HighOrbs;
                        hoUpdateWalletPoint.AddPoint = (int) ho;
                        hoUpdateWalletPoint.Value = hoWallet.Value;
                        updateCharacterItemNtc.UpdateWalletList.Add(hoUpdateWalletPoint);

                        // PERSIST CHANGES IN DB
                        Server.Database.UpdateWalletPoint(memberClient.Character.CharacterId, hoWallet);
                    }

                    if(updateCharacterItemNtc.UpdateItemList.Count != 0 || updateCharacterItemNtc.UpdateWalletList.Count != 0)
                    {
                        memberClient.Send(updateCharacterItemNtc);
                    }
                }
                else if(member is PawnPartyMember)
                {
                    Pawn pawn = ((PawnPartyMember) member).Pawn;
                    memberClient = _gameServer.ClientLookup.GetClientByCharacterId(pawn.CharacterId);
                    memberCharacter = pawn;
                }
                else
                {
                    throw new Exception("Unknown member type");
                }

                _gameServer.ExpManager.AddExp(memberClient, memberCharacter, gainedExp, extraBonusExp);
            }
        }
    }
}
