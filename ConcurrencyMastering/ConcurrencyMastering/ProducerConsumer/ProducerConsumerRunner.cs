using System.Threading.Channels;

namespace ConcurrencyMastering.ProducerConsumer;

public static class ProducerConsumerRunner
{
    public static async Task Run()
    {
        var producerBufferSize = 3;
        var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(2) {FullMode = BoundedChannelFullMode.DropWrite}, i => Console.WriteLine($"Dropped {i}"));
        var producers = new List<Task>();
        var consumers = new List<Task>();

        for (var i = 0; i < producerBufferSize; i++) // Start 3 producer threads
        {
            var i1 = i;
            producers.Add(Task.Run(async () => await channel.Writer.WriteAsync(i1)));
        }

        consumers.Add(
            Task.Run(
                async () =>
                {
                    await foreach (var i in channel.Reader.ReadAllAsync())
                    {
                        Console.WriteLine($"Consumed {i}");
                    }
                }));

        await Task.WhenAll(producers);
        channel.Writer.Complete();
        await Task.WhenAll(consumers);
    }
}
