using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity; //TODO: remove this
using Arrowgene.Ddon.GameServer.Dump; // and this, after replacing the Dump.

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipEnhancedGetPacks : GameRequestPacketHandler<C2SEquipEnhancedGetPacksReq, S2CEquipEnhancedGetPacksRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipEnhancedGetPacks));
        
        public EquipEnhancedGetPacks(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipEnhancedGetPacksRes Handle(GameClient client, C2SEquipEnhancedGetPacksReq request)
        {
            S2CEquipEnhancedGetPacksRes res = EntitySerializer.Get<S2CEquipEnhancedGetPacksRes>().Read(InGameDump.data_Dump_111);

            // List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6> TestItemList = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>();

            // CDataS2CEquipEnhancedGetPacksResUnk0Unk6 TestItem = new CDataS2CEquipEnhancedGetPacksResUnk0Unk6()
            // {
            //     Unk0 = 4743,
            //     Unk1 = 3
            // };

            // TestItemList.Add(TestItem);

            // CDataS2CEquipEnhancedGetPacksResUnk0 Example = new CDataS2CEquipEnhancedGetPacksResUnk0()
            // {
            //     Unk0 = 3, //ushort, maybe Addstatus ID???
            //     Unk1 = request.Unk0, // request has a byte, so maybe its category.
            //     Unk2 = "test", // string, UHHHHHHHHHHHHHHHHHHHHHHHHHHH, the request might be malformed lol, this is probably UID??
            //     Unk3 = 3, //ushort again, no fucking idea lmao
            //     Unk4 = 3, //ushort AGAIN?? OH LORDY
            //     Unk5 = new List<CDataWalletPoint>(), //  Rift or Smithy tickets
            //     Unk6 = TestItemList, // List<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>, presumably this is what contains the item ID???
            //     Unk7 = new List<CDataWalletPoint>(), //  Rift or Smithy tickets
            // };

            // List<CDataS2CEquipEnhancedGetPacksResUnk0> ExampleList = new List<CDataS2CEquipEnhancedGetPacksResUnk0>();
            // ExampleList.Add(Example);

            // S2CEquipEnhancedGetPacksRes res = new S2CEquipEnhancedGetPacksRes()
            // {
            //     Unk0 = ExampleList
            // };
            return res;
        }
    }
}