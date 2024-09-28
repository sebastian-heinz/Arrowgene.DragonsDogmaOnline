public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "setlevel";
    public override string HelpText               => "usage: `/setlevel [jobid] [level] [pawnname?]` - Set job level.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        if (command.Length == 0)
        {
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        JobId? targetJob = null;

        // Try by id
        if (Byte.TryParse(command[0], out byte parsedJobId))
        {
            targetJob = (JobId)parsedJobId;
        }
        else
        {
            string commandJobName = command[0].ToUpper();
            foreach (string jobName in Enum.GetNames(typeof(JobId)))
            {
                if (jobName.ToUpper() == commandJobName)
                {
                    targetJob = (JobId)Enum.Parse(typeof(JobId), jobName);
                    break;
                }
            }
        }

        if (targetJob == null)
        {
            responses.Add(ChatResponse.CommandError(client, "invalid job, try one of the following (job id or name):"));
            responses.Add(ChatResponse.CommandError(client, "1-Fighter"));
            responses.Add(ChatResponse.CommandError(client, "2-Seeker"));
            responses.Add(ChatResponse.CommandError(client, "3-Hunter"));
            responses.Add(ChatResponse.CommandError(client, "4-Priest"));
            responses.Add(ChatResponse.CommandError(client, "5-ShieldSage"));
            responses.Add(ChatResponse.CommandError(client, "6-Sorcerer"));
            responses.Add(ChatResponse.CommandError(client, "7-Warrior"));
            responses.Add(ChatResponse.CommandError(client, "8-ElementArcher"));
            responses.Add(ChatResponse.CommandError(client, "9-Alchemist"));
            responses.Add(ChatResponse.CommandError(client, "10-SpiritLancer"));
            responses.Add(ChatResponse.CommandError(client, "11-HighScepter"));
            return;
        }

        uint targetLevel = 1;
        if (command.Length >= 2)
        {
            if (UInt32.TryParse(command[1], out uint parsedLevel))
            {
                targetLevel = parsedLevel;
            }
            else
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid level \"{command[1]}\". It must be a number."));
                return;
            }
        }
        targetLevel = Math.Clamp(targetLevel, 1, ExpManager.LV_CAP);

        CharacterCommon targetCharacter = client.Character;

        if (command.Length >= 3)
        {
            var tuple = client.Character.Pawns
                .Select((pawn, index) => new { pawn = pawn, pawnNumber = (byte)(index + 1) })
                .Where(tuple => tuple.pawn.Name == command[2])
                .FirstOrDefault();

            if (tuple == null)
            {
                responses.Add(ChatResponse.CommandError(client, "No pawn was found by that name."));
                return;
            }

            targetCharacter = tuple.pawn;
        }

        CDataCharacterJobData? targetJobData = targetCharacter.CharacterJobDataList.Where(x => x.Job == targetJob).SingleOrDefault();

        if (targetJobData is null) 
        {
            responses.Add(ChatResponse.CommandError(client, "Missing job data. Switch to that job at least once to generate it."));
            return;
        }

        int startingLevel = (int)targetJobData.Lv;
        int levelDifference = targetLevel > targetJobData.Lv ? (int)(targetLevel - targetJobData.Lv) : 0;

        targetJobData.Lv = targetLevel;
        targetJobData.Exp = ExpManager.TotalExpToLevelUpTo(targetLevel, client.GameMode);

        //Handle job points
        uint cumulativeJobPoints = 0;
        if (levelDifference > 0)
        {
            cumulativeJobPoints = (uint)ExpManager.LEVEL_UP_JOB_POINTS_EARNED.Skip(startingLevel+1).Take(levelDifference).Sum(x => x);
        }

        targetJobData.JobPoint += cumulativeJobPoints;

        CDataCharacterJobData newStats = ExpManager.CalculateBaseStats((JobId)targetJob, targetLevel);
        targetJobData.Atk = newStats.Atk;
        targetJobData.MAtk = newStats.MAtk;
        targetJobData.Def = newStats.Def;
        targetJobData.MDef = newStats.MDef;

        string targetName = $"{client.Character.FirstName} {client.Character.LastName}";
        if (targetCharacter is Pawn) targetName = ((Pawn)targetCharacter).Name;

        if (client.Party.Contains(targetCharacter) && targetCharacter.ActiveCharacterJobData.Job == targetJob)
        {
            if (targetCharacter is Character)
            {
                S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
                lvlNtc.Job = (JobId)targetJob;
                lvlNtc.Level = targetLevel;
                lvlNtc.AddJobPoint = 0;
                lvlNtc.TotalJobPoint = targetJobData.JobPoint;
                GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, targetCharacter);
                client.Send(lvlNtc);
            }
            else if (targetCharacter is Pawn)
            {
                S2CJobPawnJobLevelUpNtc lvlNtc = new S2CJobPawnJobLevelUpNtc();
                lvlNtc.PawnId = ((Pawn)targetCharacter).PawnId;
                lvlNtc.Job = (JobId)targetJob;
                lvlNtc.Level = targetLevel;
                lvlNtc.AddJobPoint = 0;
                lvlNtc.TotalJobPoint = targetJobData.JobPoint;
                GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, targetCharacter);
                client.Send(lvlNtc);

                targetName = ((Pawn)targetCharacter).Name;
            }
        }

        server.Database.UpdateCharacterJobData(targetCharacter.CommonId, targetJobData);

        responses.Add(ChatResponse.ServerMessage(client, $"Setting {targetName}'s {targetJob.ToString()} to level {targetLevel}."));
    }
}

return new ChatCommand();