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
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs1SpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs1SpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 2,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs2SpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs2SpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 3,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs3SpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs3SpLv
                },
                new CDataSetAcquirementParam() {
                    Job = Server.AssetRepository.ArisenAsset[0].Job,
                    Type = 0,
                    SlotNo = (1<<4) | 4,
                    AcquirementNo = Server.AssetRepository.ArisenAsset[0].Cs4SpId,
                    AcquirementLv = Server.AssetRepository.ArisenAsset[0].Cs4SpLv
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
