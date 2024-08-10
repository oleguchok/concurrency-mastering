namespace ConcurrencyMastering.AsyncToSyncProblem;

public static class Runner
{
    public static void Run()
    {
        var syncExecutor = new SyncExecutor();
        syncExecutor.Execute(() =>
        {
            Console.WriteLine("Hello from async executor!");
        });
        syncExecutor.Execute(() =>
        {
            Console.WriteLine("Hello from async executor!");
        });
    }
}
