using System.Linq;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterDecideCharacterIdHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterDecideCharacterIdHandler));


        public CharacterDecideCharacterIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Character.WalletPointList = new List<CDataWalletPoint>()
            {
                // TODO: Figure out what other currencies there are.
                // Pcap currencies:
                new CDataWalletPoint() {
                    Type = 1,
                    Value = 9863202 // (G)
                },
                new CDataWalletPoint() {
                    Type = 2,
                    Value = 2096652 // (RP)
                },
                new CDataWalletPoint() {
                    Type = 3,
                    Value = 100 // BO
                },
                new CDataWalletPoint() {
                    Type = 4,
                    Value = 5648
                },
                new CDataWalletPoint() {
                    Type = 6,
                    Value = 99999
                },
                new CDataWalletPoint() {
                    Type = 9,
                    Value = 200 // (HO?)
                },
                new CDataWalletPoint() {
                    Type = 10,
                    Value = 0
                },
                new CDataWalletPoint() {
                    Type = 11,
                    Value = 8
                },
                new CDataWalletPoint() {
                    Type = 12,
                    Value = 219
                },
                new CDataWalletPoint() {
                    Type = 13,
                    Value = 2
                },
                new CDataWalletPoint() {
                    Type = 14,
                    Value = 2
                },
                new CDataWalletPoint() {
                    Type = 15,
                    Value = 115
                },
                new CDataWalletPoint() {
                    Type = 16,
                    Value = 105
                }
            };

            S2CCharacterDecideCharacterIdRes pcap = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(GameDump.data_Dump_13);
            S2CCharacterDecideCharacterIdRes res = new S2CCharacterDecideCharacterIdRes();
            res.CharacterId = client.Character.Id;
            res.CharacterInfo = new CDataCharacterInfo(client.Character);
            res.Unk0 = pcap.Unk0; // Commenting this makes tons of tutorials pop up
            
            client.Send(res);
            
            // Unlocks menu options such as inventory, warping, etc.
            S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNtc>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
