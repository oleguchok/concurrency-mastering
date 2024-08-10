namespace ConcurrencyMastering.AsyncToSyncProblem;

public class AsyncExecutor
{
    public virtual void Execute(Action callback)
    {
        Task.Run(
            () =>
            {
                Console.WriteLine("Executing async...");
                Thread.Sleep(2000);
                callback();
            });
    }
}
