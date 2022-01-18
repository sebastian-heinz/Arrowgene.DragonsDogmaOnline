using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
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

        public override void Handle(LoginClient client, Packet packet)
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
                cResponse.Element.CharacterID = (uint)c.Id;
                cResponse.Element.FirstName = c.FirstName;
                cResponse.Element.LastName = c.LastName;
                cResponse.EditInfo = c.Visual;
                characterListResponse.Add(cResponse);
            }
            
            EntitySerializer.Get<CDataCharacterListInfo>().WriteList(buffer, characterListResponse);
            Packet response = new Packet(PacketId.L2C_GET_CHARACTER_LIST_RES, buffer.GetAllBytes());
            client.Send(response);

            //  client.Send(LoginDump.Dump_24);
        }
    }
}
