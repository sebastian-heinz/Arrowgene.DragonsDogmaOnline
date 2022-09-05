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
            // TODO: Move to DB
            client.Character.CharacterInfo.UnkCharData1 = new List<UnknownCharacterData1>()
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
                new UnknownCharacterData1()
                {
                    u0 = 2, // RP
                    u1 = 42069
                }
            };
            
            S2CCharacterDecideCharacterIdRes res = EntitySerializer.Get<S2CCharacterDecideCharacterIdRes>().Read(GameDump.data_Dump_13);
            res.CharacterId = client.Character.Id;
            res.CharacterInfo = client.Character.CharacterInfo;
            client.Send(res);
            
            // Unlocks menu options such as inventory, warping, etc.
            S2CCharacterContentsReleaseElementNotice contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNotice>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
