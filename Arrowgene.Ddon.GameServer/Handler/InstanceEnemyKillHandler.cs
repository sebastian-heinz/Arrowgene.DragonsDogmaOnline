using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyKillHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyKillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyKillHandler));


        public InstanceEnemyKillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyKillReq> packet)
        {
            client.Send(new S2CInstanceEnemyKillRes());

            // ------
            // EXP UP

            // TODO: Calculate somehow
            uint gainedExp = 10000;
            uint extraBonusExp = 0; // TODO: Figure out what this is for

            client.Character.ActiveCharacterJobData.Exp += gainedExp;
            client.Character.ActiveCharacterJobData.Exp += extraBonusExp;

            S2CJobCharacterJobExpUpNtc expNtc = new S2CJobCharacterJobExpUpNtc();
            expNtc.JobId = client.Character.ActiveCharacterJobData.Job;
            expNtc.AddExp = gainedExp;
            expNtc.ExtraBonusExp = extraBonusExp;
            expNtc.TotalExp = client.Character.ActiveCharacterJobData.Exp;
            expNtc.Type = 0; // TODO: Figure out
            client.Send(expNtc);


            // --------
            // LEVEL UP

            uint addJobPoint = 0; // TODO:
            client.Character.ActiveCharacterJobData.Lv++; // TODO: Cap to max lvl
            client.Character.ActiveCharacterJobData.JobPoint += addJobPoint;
            // TODO: Update other values in ActiveCharacterJobData

            // Inform client of lvl up
            S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
            lvlNtc.Job = client.Character.Job;
            lvlNtc.Level = client.Character.ActiveCharacterJobData.Lv;
            lvlNtc.AddJobPoint = addJobPoint;
            lvlNtc.TotalJobPoint = client.Character.ActiveCharacterJobData.JobPoint;
            GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, client.Character);
            client.Send(lvlNtc);

            // Inform other party members
            S2CJobCharacterJobLevelUpMemberNtc lvlMemberNtc = new S2CJobCharacterJobLevelUpMemberNtc();
            lvlMemberNtc.CharacterId = client.Character.Id;
            lvlMemberNtc.Job = client.Character.Job;
            lvlMemberNtc.Level = client.Character.ActiveCharacterJobData.Lv;
            GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, client.Character);
            client.Party.SendToAllExcept(lvlMemberNtc, client);

            // Inform all other players in the server
            S2CJobCharacterJobLevelUpOtherNtc lvlOtherNtc = new S2CJobCharacterJobLevelUpOtherNtc();
            lvlOtherNtc.CharacterId = client.Character.Id;
            lvlOtherNtc.Job = client.Character.Job;
            lvlOtherNtc.Level = client.Character.ActiveCharacterJobData.Lv;
            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if(otherClient.Party != client.Party)
                {
                    otherClient.Send(lvlOtherNtc);
                }
            }

            // PERSIST CHANGES IN DB
            Database.UpdateCharacterJobData(client.Character.Id, client.Character.ActiveCharacterJobData);


            // TODO: Exp and Lvl for the client's pawns in the party
        }
    }
}