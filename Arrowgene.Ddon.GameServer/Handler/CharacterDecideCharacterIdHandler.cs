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
            res.Unk0 = pcap.Unk0; // Removing this makes tons of tutorials pop up
            
            client.Send(res);
            
            // Unlocks menu options such as inventory, warping, etc.
            S2CCharacterContentsReleaseElementNtc contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNtc>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
