namespace ConcurrencyMastering.DiningPhilosopher;

public static class Runner
{
    private static object _tableLock = new();
    public static void Run()
    {
        var philosophersCount = 5;
        var forks = new Fork[philosophersCount];
        for (var i = 0; i < philosophersCount; i++)
        {
            forks[i] = new Fork(i.ToString());
        }
        
        for (var i = 0; i < philosophersCount; i++)
        {
            var index = i;
            _ = Task.Run(
                () =>
                {
                    try
                    {
                        var philosopher = new DiningPhilosopher(
                            index.ToString(),
                            forks[index],
                            forks[(index + 1) % philosophersCount]);

                        while (true)
                        {
                            philosopher.Think();
                            // lock (_tableLock)
                            // {
                                philosopher.Eat();   
                            // }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                });
        }
    }
}

public record Fork(string Name);
