using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetCharacterListHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetCharacterListHandler));


        public GetCharacterListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big); // error
            buffer.WriteInt32(0, Endianness.Big); // result

            List<CDataCharacterListInfo> characterListResponse = new List<CDataCharacterListInfo>();
            List<Character> characters = Database.SelectCharactersByAccountId(client.Account.Id);
            Logger.Info(client, $"Found: {characters.Count} Characters");
            foreach (Character c in characters)
            {
                CDataCharacterListInfo cResponse = new CDataCharacterListInfo();
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = (uint)c.Id;
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = c.FirstName;
                cResponse.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = c.LastName;
                cResponse.CharacterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
                cResponse.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
                cResponse.EditInfo = c.Visual;
                cResponse.EquipItemInfo = new List<CDataEquipItemInfo>() { // Probably wrong since im only sending visual equipment
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VPrimaryWeapon,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 1
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VSecondaryWeapon,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 2
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VHead,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 3
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VBody,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 4
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VClothing,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 5
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VArm,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 6
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VLeg,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 7
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VLegwear,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 8
                    },
                    new CDataEquipItemInfo {
                        U0 = Server.AssetRepository.ArisenAsset[0].VOverwear,
                        EquipType = 0,
                        EquipSlot = 2,
                        ItemId = 9
                    }
                };
                characterListResponse.Add(cResponse);
            }
            
            EntitySerializer.Get<CDataCharacterListInfo>().WriteList(buffer, characterListResponse);
            Packet response = new Packet(PacketId.L2C_GET_CHARACTER_LIST_RES, buffer.GetAllBytes());
            client.Send(response);
        }
    }
}
