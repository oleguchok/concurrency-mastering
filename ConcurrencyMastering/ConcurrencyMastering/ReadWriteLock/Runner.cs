using System.Diagnostics;

namespace ConcurrencyMastering.ReadWriteLock;

public static class Runner
{
    public static void Run()
    {
        var readWriteLock = new ReadWriteLock();
        var num = 0;

        Task task = null;

        for (var i = 0; i < 2; i++)
        {
            task = Task.Run(
                () =>
                {
                    while (true)
                    {
                        readWriteLock.AcquireReadLock();
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId:00} - Reading: {num}");

                        readWriteLock.ReleaseReadLock();
                        Thread.Sleep(100);
                    }
                });
        }

        for (var i = 0; i < 0; i++)
        {
            Task.Run(
                () =>
                {
                    while (true)
                    {
                        readWriteLock.AcquireWriteLock();
                        num++;
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId:00} - WRITING: {num}");

                        readWriteLock.ReleaseWriteLock();
                        Thread.Sleep(100);
                    }
                });
        }

        task.Wait();
    }
}
