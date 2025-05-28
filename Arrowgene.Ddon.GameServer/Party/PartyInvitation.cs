using System;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Party;

public class PartyInvitation
{
    public GameClient Host { get; set; }
    public GameClient Invitee { get; set; }
    public PartyGroup Party { get; set; }
    public DateTime Date { get; set; }
    private Timer _timer;
    public bool IsTimerDisposed =>
     _timer == null || !_timer.Change(Timeout.Infinite, Timeout.Infinite);

    public void StartTimer(Action<PartyInvitation> onTimeout, int timeoutSec)
    {
        _timer = new Timer(state =>
        {
            onTimeout(this);
            _timer.Dispose();
        }, null, timeoutSec * 1000, Timeout.Infinite);
    }

    public void CancelTimer()
    {
        try
        {
            if (!IsTimerDisposed)
            {
                _timer.Dispose();
            }
        }
        catch { }
    }
}
