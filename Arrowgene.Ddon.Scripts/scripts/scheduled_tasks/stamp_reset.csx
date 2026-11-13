public class StampResetTask : DailyTask
{
    public StampResetTask(uint hour, uint minute)
        : base(TaskType.LoginStamps, hour, minute) { }

    public override void RunTask(DdonGameServer server)
    {
        foreach (var character in server.ClientLookup.GetAllCharacter())
        {
            server.StampManager.RefreshStamp(character);
        }

        server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.StampReset, null);

        server.Database.ResetCharacterStamps();
    }
}

return new StampResetTask(5, 0);
