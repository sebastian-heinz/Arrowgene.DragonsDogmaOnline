using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterSearchHandler : GameRequestPacketHandler<C2SCharacterCharacterSearchReq, S2CCharacterCharacterSearchRes>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterSearchHandler));

        public CharacterCharacterSearchHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCharacterSearchRes Handle(GameClient client, C2SCharacterCharacterSearchReq request)
        {
            S2CCharacterCharacterSearchRes res = new S2CCharacterCharacterSearchRes();
            foreach (Character character in Server.ClientLookup.GetAllCharacter())
            {
                if (!character.FirstName.Contains(
                        request.SearchParam.FirstName, StringComparison.InvariantCultureIgnoreCase)
                    &&
                    !character.LastName.Contains(
                        request.SearchParam.LastName, StringComparison.InvariantCultureIgnoreCase)
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
                                $"FirstName:{request.SearchParam.FirstName} " +
                                $"LastName:{request.SearchParam.LastName}");

            return res;
        }
    }
}
