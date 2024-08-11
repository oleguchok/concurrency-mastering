namespace ConcurrencyMastering.BarberShopProblem;

public static class Runner
{
    public static void Run()
    {
        var barberShop = new BarberShop(3);
        barberShop.Open();
        var random = new Random();

        for (var i = 0; i < 20; i++)
        {
            var i1 = i;
            Task.Run(
                () =>
                {
                    Thread.CurrentThread.Name = $"Thread {i1}";
                    var value = random.Next(2000);
                    Thread.Sleep(value);
                    barberShop.ServeClient();
                });
        }

        Thread.Sleep(1000);
        barberShop.Close();
    }
}
