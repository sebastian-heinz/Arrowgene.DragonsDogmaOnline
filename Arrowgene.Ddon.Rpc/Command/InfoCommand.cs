using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Rpc.Command
{
    public class InfoCommand : IRpcCommand
    {
        public class Info
        {
            public uint StageId { get; set; }
            public byte LayerNo { get; set; }
            public uint GroupId { get; set; }
            public uint StageNo { get; set; }
            public double X { get; set; }
            public float Y { get; set; }
            public double Z { get; set; }
            public uint CharacterId { get; set; }
            public int AccountId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public Info()
            {
                FirstName = "";
                LastName = "";
            }
        }

        public string Name => "SpawnCommand";

        public InfoCommand()
        {
            Infos = new List<Info>();
        }

        public List<Info> Infos { get; set; }

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            Infos.Clear();
            foreach (GameClient client in gameServer.Clients)
            {
                Info info = new Info();
                info.X = client.X;
                info.Y = client.Y;
                info.Z = client.Z;
                info.StageNo = client.StageNo;
                info.StageId = client.Stage.Id;
                info.GroupId = client.Stage.GroupId;
                info.LayerNo = client.Stage.LayerNo;
                Account account = client.Account;
                if (account != null)
                {
                    info.AccountId = account.Id;
                }

                Character character = client.Character;
                if (character != null)
                {
                    info.CharacterId = character.Id;
                    info.FirstName = character.FirstName;
                    info.LastName = character.LastName;
                }

                Infos.Add(info);
            }

            return new RpcCommandResult(this, true);
        }
    }
}
