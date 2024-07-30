namespace ConcurrencyMastering.ReadWriteLock;

public class ReadWriteLock
{
    private readonly object _lock = new();
    private bool _isWriteAcquired;
    private int _readersCount;

    public void AcquireReadLock()
    {
        lock (_lock)
        {
            while (_isWriteAcquired)
            {
                Monitor.Wait(_lock);
            }

            _readersCount++;
        }
    }

    public void ReleaseReadLock()
    {
        lock (_lock)
        {
            _readersCount--;
            if (_readersCount == 0) // batch processing of recent readers
            {
                Monitor.PulseAll(_lock);    
            }
        }
    }

    public void AcquireWriteLock()
    {
        lock (_lock)
        {
            while (_readersCount > 0 || _isWriteAcquired)
            {
                Monitor.Wait(_lock);
            }

            _isWriteAcquired = true;
        }
    }

    public void ReleaseWriteLock()
    {
        lock (_lock)
        {
            _isWriteAcquired = false;
            Monitor.PulseAll(_lock);
        }
    }
}
