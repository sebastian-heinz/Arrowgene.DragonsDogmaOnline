using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterSearchHandler : GameStructurePacketHandler<C2SCharacterCharacterSearchReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterSearchHandler));

        public CharacterCharacterSearchHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterSearchReq> packet)
        {
            S2CCharacterCharacterSearchRes res = new S2CCharacterCharacterSearchRes();
            foreach (Character character in Server.ClientLookup.GetAllCharacter())
            {
                if (!character.FirstName.Contains(
                        packet.Structure.SearchParam.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    &&
                    !character.LastName.Contains(
                        packet.Structure.SearchParam.LastName, StringComparison.InvariantCultureIgnoreCase)
                   )
                {
                    continue;
                }

                if (character == client.Character)
                {
                    continue;
                }

                CDataCharacterListElement characterListElement = new CDataCharacterListElement();
                GameStructure.CDataCharacterListElement(characterListElement, character);
                res.CharacterList.Add(characterListElement);
            }

            Logger.Info(client, $"Found: {res.CharacterList.Count} Characters matching for query " +
                                $"FirstName:{packet.Structure.SearchParam.FirstName} " +
                                $"LastName:{packet.Structure.SearchParam.LastName}");

            client.Send(res);
        }
    }
}
