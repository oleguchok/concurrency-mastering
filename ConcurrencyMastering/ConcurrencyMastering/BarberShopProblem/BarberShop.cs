namespace ConcurrencyMastering.BarberShopProblem;

public class BarberShop(int freeChairs)
{
    private readonly AutoResetEvent _barberEvent = new(false);
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly object _lock = new();
    private readonly Random _random = new();
    private readonly Queue<Client> _waitingClients = new();

    public void Open() =>
        _ = Task.Run(
            () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (_waitingClients.Count ==
                        0) // we don't need to lock here, because barber will wake up if the client is added
                    {
                        BarberIsSleeping();
                    }
                    else
                    {
                        Console.WriteLine("Barber is working...");
                        Client client;
                        lock
                            (_lock) // lock here to give new client a chance to get into the queue before when queue is full but barber is about to take a client
                        {
                            _waitingClients.TryDequeue(
                                out client); // we don't need to check if it's empty, because barber is working only if there are clients and there is no other barber
                        }

                        Console.WriteLine($"Barber does haircut for {client.Name}");
                        Thread.Sleep(_random.Next(1000));
                        client.Event.Set();
                    }
                }

                Console.WriteLine("Sorry, we are closed! Come tomorrow!");
                foreach (Client client in _waitingClients)
                {
                    client.Event.Set(); // fake serving clients to not making them wait in closed barber shop forever
                }

                _waitingClients.Clear();
            },
            _cancellationTokenSource.Token);

    private void BarberIsSleeping()
    {
        Console.WriteLine("Barber is sleeping...");
        _barberEvent.WaitOne();
    }

    public bool ServeClient()
    {
        Monitor.Enter(_lock);
        if (_cancellationTokenSource.IsCancellationRequested)
        {
            Monitor.Exit(_lock);
            Console.WriteLine($"{Thread.CurrentThread.Name}, we are closed.");
            return false;
        }


        if (_waitingClients.Count >= freeChairs)
        {
            Console.WriteLine($"Sorry {Thread.CurrentThread.Name}, we are full! Come tomorrow!");
            Monitor.Exit(_lock);
            return false;
        }

        using var mre = new ManualResetEventSlim(false);
        Client client = new(Thread.CurrentThread.Name!, mre);
        _waitingClients.Enqueue(client);
        _barberEvent.Set();

        Monitor.Exit(_lock);

        mre.Wait();
        Console.WriteLine($"Client {client.Name} is served");
        return true;
    }

    public void Close() => _cancellationTokenSource.Cancel();

    private record Client(string Name, ManualResetEventSlim Event);
}
