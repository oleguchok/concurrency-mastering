namespace ConcurrencyMastering.UberRideProblem;

public static class Runner
{
    public static void Run()
    {
        var uberRide = new UberRide();
        var tasks = new List<Task>();

        for (var i = 0; i < 10; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                Thread.CurrentThread.Name = $"Democrat {index}";
                uberRide.SeatDemocrat();
            }));
        }

        for (var i = 10; i < 20; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                Thread.CurrentThread.Name = $"Republican {index}";
                uberRide.SeatRepublican();
            }));
        }

        // Shuffle the tasks
        var rng = new Random();
        tasks = tasks.OrderBy(_ => rng.Next()).ToList();

        // Start and wait for all tasks
        Task.WaitAll(tasks.ToArray());
    }
}
