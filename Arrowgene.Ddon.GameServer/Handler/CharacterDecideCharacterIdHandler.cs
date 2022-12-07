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
            S2CCharacterDecideCharacterIdRes pcap = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(GameDump.data_Dump_13);
            S2CCharacterDecideCharacterIdRes res = new S2CCharacterDecideCharacterIdRes();
            res.CharacterId = client.Character.Id;
            res.CharacterInfo = new CDataCharacterInfo(client.Character);
            res.CharacterInfo.CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>()
            {
                // Check nItem::E_STORAGE_TYPE in the PS4 debug symbols for IDs
                new CDataCharacterItemSlotInfo()
                {
                    StorageType = 0x1, // Item Bag (Consumable)
                    SlotMax = 20
                },
                new CDataCharacterItemSlotInfo()
                {
                    StorageType = 0x2, // Item Bag (Material)
                    SlotMax = 20
                },
                new CDataCharacterItemSlotInfo()
                {
                    StorageType = 0x3, // Item Bag (Equipment)
                    SlotMax = 20
                }
            };
            res.CharacterInfo.WalletPointList = new List<CDataWalletPoint>()
            {
                // TODO: Figure out what other currencies there are.
                // Pcap currencies:
                //  1   9863202 (G?)
                //  2   2096652 (RP?)
                //  3   50000
                //  4   5648
                //  6   99999
                //  9   5000
                //  10  0
                //  11  8
                //  12  219
                //  13  2
                //  14  2
                //  15  115
                //  16  105
                new CDataWalletPoint()
                {
                    Type = 2, // RP
                    Value = 42069
                }
            };
            res.Unk0 = pcap.Unk0; // Commenting this makes tons of tutorials pop up
            
            client.Send(res);
            
            // Unlocks menu options such as inventory, warping, etc.
            S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNtc>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
