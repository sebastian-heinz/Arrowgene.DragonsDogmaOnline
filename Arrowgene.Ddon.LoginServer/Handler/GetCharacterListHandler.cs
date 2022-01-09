using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetCharacterListHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetCharacterListHandler));


        public GetCharacterListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big); // error
            buffer.WriteInt32(0, Endianness.Big); // result

            CDataCharacterListInfo character = new CDataCharacterListInfo();
            character.Element.FirstName = "Dragons";
            character.Element.LastName = "Dogma";

            List<CDataCharacterListInfo> characters = new List<CDataCharacterListInfo>();
            characters.Add(character);
            EntitySerializer.Get<CDataCharacterListInfo>().WriteList(buffer, characters);
            Packet response = new Packet(PacketId.L2C_GET_CHARACTER_LIST_RES, buffer.GetAllBytes(), PacketSource.Server);
            //  client.Send(response);

            client.Send(LoginDump.Dump_24);
        }
    }
}
