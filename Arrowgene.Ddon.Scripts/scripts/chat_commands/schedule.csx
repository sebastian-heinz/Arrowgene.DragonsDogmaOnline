#load "libs.csx"

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "schedule";
    public override string HelpText => "usage: `/schedule` - Print details about the current scheduled tasks";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var lines = new List<string>();
        lines.Add("# Task Cadence, Task Type, Next Update");
        foreach (var task in server.ScheduleManager.GetTasks())
        {
            if (!task.IsEnabled(server))
            {
                continue;
            }

            TimeSpan ts = TimeSpan.FromSeconds(server.ScheduleManager.TimeToNextTaskUpdate(task.Type));
            var formattedTime = string.Format("{0:D1}d:{1:D2}h:{2:D2}m:{3:D2}s", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            var line = $"{task.TaskTypeName()}: {task.Type} ({(uint)task.Type})\n    {formattedTime}";
            lines.Add(line);
        }

        foreach (var line in lines)
        {
            // Console.WriteLine(line);
            responses.Add(ChatResponse.ServerChat(client, line));
        }
    }
}

return new ChatCommand();
