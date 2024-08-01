namespace ConcurrencyMastering.DiningPhilosopher;

public class DiningPhilosopher
{
    private readonly Fork _leftFork;
    private readonly string _name;
    private readonly Random _random = new();
    private readonly Fork _rightFork;

    public DiningPhilosopher(string name, Fork leftFork, Fork rightFork)
    {
        _leftFork = leftFork;
        _rightFork = rightFork;
        _name = name;
    }

    public void Think()
    {
        var timeout = _random.Next(5000);
        Console.WriteLine($"Philosopher {_name} is thinking for {timeout}.");
        // Thread.Sleep(timeout);
    }

    public void Eat()
    {
        if (Monitor.TryEnter(_leftFork))
        {
            Console.WriteLine($"Philosopher {_name} took left fork {_leftFork.Name}.");
            if (Monitor.TryEnter(_rightFork))
            {
                Console.WriteLine($"Philosopher {_name} is eating.");
                Thread.Sleep(_random.Next(3000));
                Monitor.Exit(_rightFork);
            }

            Monitor.Exit(_leftFork);
        }
    }
}
