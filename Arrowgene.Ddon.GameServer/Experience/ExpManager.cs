using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.GameServer.Experience
{
    public class ExpManager
    {
        private static readonly byte LV_CAP = 120;

        // E.g. EXP_UNTIL_NEXT_LV[3] = 800, meaning as Lv 3 you need 800 exp to level to Lv 4
        private static readonly uint[] EXP_UNTIL_NEXT_LV = new uint[] {
            /********/   0,
            /* Lv 1 */   300,
            /* Lv 2 */   500,
            /* Lv 3 */   800,
            /* Lv 4 */   1200,
            /* Lv 5 */   1700,
            /* Lv 6 */   2300,
            /* Lv 7 */   3000,
            /* Lv 8 */   3800,
            /* Lv 9 */   4700,
            /* Lv 10 */  5700,
            /* Lv 11 */  6800,
            /* Lv 12 */  8000,
            /* Lv 13 */  9300,
            /* Lv 14 */  10700,
            /* Lv 15 */  12200,
            /* Lv 16 */  13800,
            /* Lv 17 */  15500,
            /* Lv 18 */  17300,
            /* Lv 19 */  19200,
            /* Lv 20 */  21200,
            /* Lv 21 */  23300,
            /* Lv 22 */  25500,
            /* Lv 23 */  27800,
            /* Lv 24 */  30200,
            /* Lv 25 */  32700,
            /* Lv 26 */  35300,
            /* Lv 27 */  38000,
            /* Lv 28 */  40800,
            /* Lv 29 */  43700,
            /* Lv 30 */  46700,
            /* Lv 31 */  49800,
            /* Lv 32 */  53000,
            /* Lv 33 */  56300,
            /* Lv 34 */  59700,
            /* Lv 35 */  63200,
            /* Lv 36 */  66800,
            /* Lv 37 */  70500,
            /* Lv 38 */  74300,
            /* Lv 39 */  78200,
            /* Lv 40 */  152500,
            /* Lv 41 */  187100,
            /* Lv 42 */  210000,
            /* Lv 43 */  235300,
            /* Lv 44 */  263200,
            /* Lv 45 */  267700,
            /* Lv 46 */  272300,
            /* Lv 47 */  277000,
            /* Lv 48 */  281800,
            /* Lv 49 */  286700,
            /* Lv 50 */  291700,
            /* Lv 51 */  296800,
            /* Lv 52 */  302000,
            /* Lv 53 */  307300,
            /* Lv 54 */  312700,
            /* Lv 55 */  318200,
            /* Lv 56 */  323800,
            /* Lv 57 */  329500,
            /* Lv 58 */  335300,
            /* Lv 59 */  341200,
            /* Lv 60 */  756600,
            /* Lv 61 */  762700,
            /* Lv 62 */  768900,
            /* Lv 63 */  775200,
            /* Lv 64 */  781600,
            /* Lv 65 */  788100,
            /* Lv 66 */  985000,
            /* Lv 67 */  1085000,
            /* Lv 68 */  1185000,
            /* Lv 69 */  1335000,
            /* Lv 70 */  1535000, // (PP Unlocked)
            /* Lv 71 */  1735000,
            /* Lv 72 */  1935000,
            /* Lv 73 */  2185000,
            /* Lv 74 */  2435000,
            /* Lv 75 */  2735000,
            /* Lv 76 */  3035000,
            /* Lv 77 */  3335000,
            /* Lv 78 */  3685000,
            /* Lv 79 */  4035000,
            /* Lv 80 */  4200000, 
            /* Lv 81 */  4200000,
            /* Lv 82 */  4200000,
            /* Lv 83 */  4200000,
            /* Lv 84 */  4200000,
            /* Lv 85 */  4200000,
            /* Lv 86 */  4200000,
            /* Lv 87 */  4200000,
            /* Lv 88 */  4200000,
            /* Lv 89 */  4200000,
            /* Lv 90 */  4200000,
            /* Lv 91 */  4200000,
            /* Lv 92 */  4200000,
            /* Lv 93 */  4200000,
            /* Lv 94 */  4200000,
            /* Lv 95 */  4200000,
            /* Lv 96 */  4200000,
            /* Lv 97 */  4200000,
            /* Lv 98 */  4200000,
            /* Lv 99 */  4200000,
            /* Lv 100 */ 4461000,
            /* Lv 101 */ 5000000,
            /* Lv 102 */ 5000000,
            /* Lv 103 */ 5000000,
            /* Lv 104 */ 5000000,
            /* Lv 105 */ 5000000,
            /* Lv 106 */ 5000000,
            /* Lv 107 */ 5000000,
            /* Lv 108 */ 5000000,
            /* Lv 109 */ 5000000,
            /* Lv 110 */ 5000000,
            /* Lv 111 */ 5000000,
            /* Lv 112 */ 5000000,
            /* Lv 113 */ 5000000,
            /* Lv 114 */ 5000000,
            /* Lv 115 */ 5000000,
            /* Lv 116 */ 5000000,
            /* Lv 117 */ 5000000,
            /* Lv 118 */ 5000000,
            /* Lv 119 */ 5000000,
        };

        public ExpManager(IDatabase database, GameClientLookup gameClientLookup) 
        {
            this._database = database;
            this._gameClientLookup = gameClientLookup;
        }
        
        protected readonly IDatabase _database;
        protected readonly GameClientLookup _gameClientLookup;
        

        public void AddExp(GameClient client, uint gainedExp, uint extraBonusExp)
        {
            CDataCharacterJobData activeCharacterJobData = client.Character.ActiveCharacterJobData;

            // ------
            // EXP UP

            activeCharacterJobData.Exp += gainedExp;
            activeCharacterJobData.Exp += extraBonusExp;

            S2CJobCharacterJobExpUpNtc expNtc = new S2CJobCharacterJobExpUpNtc();
            expNtc.JobId = activeCharacterJobData.Job;
            expNtc.AddExp = gainedExp;
            expNtc.ExtraBonusExp = extraBonusExp;
            expNtc.TotalExp = activeCharacterJobData.Exp;
            expNtc.Type = 0; // TODO: Figure out
            client.Send(expNtc);


            // --------
            // LEVEL UP
            uint nextLevel = activeCharacterJobData.Lv+1;
            if(activeCharacterJobData.Lv < LV_CAP && activeCharacterJobData.Exp >= this.TotalExpToLevelUpTo(nextLevel)) {
                activeCharacterJobData.Lv = nextLevel;

                uint addJobPoint = 0; // TODO: Figure out
                activeCharacterJobData.JobPoint += addJobPoint;
                // TODO: Update other values in ActiveCharacterJobData

                // Inform client of lvl up
                S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
                lvlNtc.Job = client.Character.Job;
                lvlNtc.Level = activeCharacterJobData.Lv;
                lvlNtc.AddJobPoint = addJobPoint;
                lvlNtc.TotalJobPoint = activeCharacterJobData.JobPoint;
                GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, client.Character);
                client.Send(lvlNtc);

                // Inform other party members
                S2CJobCharacterJobLevelUpMemberNtc lvlMemberNtc = new S2CJobCharacterJobLevelUpMemberNtc();
                lvlMemberNtc.CharacterId = client.Character.Id;
                lvlMemberNtc.Job = client.Character.Job;
                lvlMemberNtc.Level = activeCharacterJobData.Lv;
                GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, client.Character);
                client.Party.SendToAllExcept(lvlMemberNtc, client);

                // Inform all other players in the server
                S2CJobCharacterJobLevelUpOtherNtc lvlOtherNtc = new S2CJobCharacterJobLevelUpOtherNtc();
                lvlOtherNtc.CharacterId = client.Character.Id;
                lvlOtherNtc.Job = client.Character.Job;
                lvlOtherNtc.Level = activeCharacterJobData.Lv;
                foreach (GameClient otherClient in this._gameClientLookup.GetAll())
                {
                    if(otherClient.Party != client.Party)
                    {
                        otherClient.Send(lvlOtherNtc);
                    }
                }
            }

            // PERSIST CHANGES IN DB
            this._database.UpdateCharacterJobData(client.Character.Id, activeCharacterJobData);
        }

        private uint TotalExpToLevelUpTo(uint level) {
            uint totalExp = 0;
            for (int i = 1; i < level; i++)
            {
                totalExp += EXP_UNTIL_NEXT_LV[i];
            }
            return totalExp;
        }
    }
}