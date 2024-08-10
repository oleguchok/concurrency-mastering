namespace ConcurrencyMastering.AsyncToSyncProblem;

public class SyncExecutor : AsyncExecutor
{
    private readonly AutoResetEvent _resetEvent = new(false);
    
    public override void Execute(Action callback)
    {
        var newCallback = new Action(() =>
        {
            callback();
            _resetEvent.Set();
        });
        
        base.Execute(newCallback);
        _resetEvent.WaitOne();
        _resetEvent.Reset(); // we need to reset if it's manual reset event
    }
}
