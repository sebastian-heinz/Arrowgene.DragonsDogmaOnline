using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLoginHandler : StructurePacketHandler<GameClient, C2SConnectionLoginReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionLoginHandler));


        public ConnectionLoginHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SConnectionLoginReq> packet)
        {
            Logger.Debug(client, $"Received SessionKey:{packet.Structure.SessionKey} for platform:{packet.Structure.PlatformType}");

            S2CConnectionLoginRes res = new S2CConnectionLoginRes();
            GameToken token = Database.SelectToken(packet.Structure.SessionKey);
            if (token == null)
            {
                Logger.Error(client, $"SessionKey:{packet.Structure.SessionKey} not found");
                res.Error = 1;
                client.Send(res);
                return;
            }

            if (!Database.DeleteTokenByAccountId(token.AccountId))
            {
                Logger.Error(client, $"Failed to delete session key from DB:{packet.Structure.SessionKey}");
            }


            Account account = Database.SelectAccountById(token.AccountId);
            if (account == null)
            {
                Logger.Error(client, $"AccountId:{token.AccountId} not found");
                res.Error = 1;
                client.Send(res);
                return;
            }

            Character character = Database.SelectCharacter(token.CharacterId);
            if (character == null)
            {
                Logger.Error(client, $"CharacterId:{token.CharacterId} not found");
                res.Error = 1;
                client.Send(res);
                return;
            }

            // TODO: Move to DB
            character.CustomSkills = new List<CDataSetAcquirementParam>() {
                // Main Palette
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs1MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs1MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs2MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs2MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs3MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs3MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs4MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs4MpLv
                },
                // Sub Palette
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 1,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs1MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs1MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 2,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs2MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs2MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 3,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs3MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs3MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 4,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs4MpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs4MpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab1Jb,
                    Type = 0,
                    SlotNo = 1,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab1Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab1Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab2Jb,
                    Type = 0,
                    SlotNo = 2,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab2Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab2Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab3Jb,
                    Type = 0,
                    SlotNo = 3,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab3Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab3Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab4Jb,
                    Type = 0,
                    SlotNo = 4,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab4Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab4Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab5Jb,
                    Type = 0,
                    SlotNo = 5,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab5Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab5Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab6Jb,
                    Type = 0,
                    SlotNo = 6,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab6Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab6Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab7Jb,
                    Type = 0,
                    SlotNo = 7,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab7Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab7Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab8Jb,
                    Type = 0,
                    SlotNo = 8,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab8Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab8Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab9Jb,
                    Type = 0,
                    SlotNo = 9,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab9Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab9Lv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Ab10Jb,
                    Type = 0,
                    SlotNo = 10,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Ab10Id,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Ab10Lv
                }
            };

            client.Account = account;
            client.Character = character;
            client.UpdateIdentity();
            Logger.Info(client, "Logged Into GameServer");

            // update login token for client
            client.Account.LoginToken = GameToken.GenerateLoginToken();
            client.Account.LoginTokenCreated = DateTime.Now;
            if (!Database.UpdateAccount(client.Account))
            {
                Logger.Error(client, "Failed to update OneTimeToken");
                res.Error = 1;
                client.Send(res);
                return;
            }

            Logger.Debug(client, $"Updated OneTimeToken:{client.Account.LoginToken}");

            res.OneTimeToken = client.Account.LoginToken;
            client.Send(res);
        }
    }
}
