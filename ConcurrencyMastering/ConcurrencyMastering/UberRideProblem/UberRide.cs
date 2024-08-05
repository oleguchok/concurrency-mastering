using System.Collections.Concurrent;

namespace ConcurrencyMastering.UberRideProblem;

public class UberRide
{
    private static readonly ConcurrentBag<string> s_cabin = new();
    private readonly Barrier _barrier = new(4, _ => Console.WriteLine($"Ride started: {string.Join(',', s_cabin)}"));
    private readonly SemaphoreSlim _democratsWaiting = new(0);
    private readonly object _lock = new();
    private readonly SemaphoreSlim _republicansWaiting = new(0);
    private int _democratsCount;
    private int _republicansCount;

    public void SeatDemocrat()
    {
        var rideLeader = false;
        Monitor.Enter(_lock);
        _democratsCount++;
        switch (_democratsCount)
        {
            case >= 4:
                _democratsWaiting.Release(3);
                _democratsCount -= 4;
                rideLeader = true;
                break;
            case >= 2 when _republicansCount >= 2:
                _democratsWaiting.Release(1);
                _republicansWaiting.Release(2);
                _democratsCount -= 2;
                _republicansCount -= 2;
                rideLeader = true;
                break;
            default:
                Monitor.Exit(_lock);
                _democratsWaiting.Wait();
                break;
        }

        Console.WriteLine($"{Thread.CurrentThread.Name} seated");
        s_cabin.Add(Thread.CurrentThread.Name!);

        _barrier.SignalAndWait();
        
        
        if (rideLeader)
        {
            s_cabin.Clear();
            Monitor.Exit(_lock);
        }
    }

    public void SeatRepublican()
    {
        var rideLeader = false;
        Monitor.Enter(_lock);
        _republicansCount++;
        switch (_republicansCount)
        {
            case >= 4:
                _republicansWaiting.Release(3);
                _republicansCount -= 4;
                rideLeader = true;
                break;
            case >= 2 when _democratsCount >= 2:
                _democratsWaiting.Release(2);
                _republicansWaiting.Release(1);
                _democratsCount -= 2;
                _republicansCount -= 2;
                rideLeader = true;
                break;
            default:
                Monitor.Exit(_lock);
                _republicansWaiting.Wait();
                break;
        }

        Console.WriteLine($"{Thread.CurrentThread.Name} seated");
        s_cabin.Add(Thread.CurrentThread.Name!);

        _barrier.SignalAndWait();
        
        
        if (rideLeader)
        {
            s_cabin.Clear();
            Monitor.Exit(_lock);
        }
    }
}
