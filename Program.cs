using System.Collections.Concurrent;
using System.Numerics;

namespace HWdop2102CasinoTable;

class Program
{
    private static Semaphore semaphore = new Semaphore(5, 5); // Максимум 5 игроков одновременно
    private static ConcurrentQueue<Player> playersQueue = new ConcurrentQueue<Player>();
    private static List<Player> finishedPlayers = new List<Player>();
    private static Random random = new Random();
    private static int totalPlayers = random.Next(20, 101); // Случайное количество игроков от 20 до 100


    static void Main()
    {
        for (int i = 1; i <= totalPlayers; i++)
        {
            playersQueue.Enqueue(new Player(i, random.Next(100, 1000))); // Игроки с начальной суммой денег
        }

        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < 5; i++) 
        {
            Thread thread = new Thread(PlayGame);
            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        
        using (StreamWriter file = new StreamWriter("casino_report.txt"))
        {
            foreach (var player in finishedPlayers)
            {
                file.WriteLine($"Игрок{player.Id} [{player.StartMoney}] [{player.Money}]");
            }
        }

        Console.WriteLine("Игра окончена. Результаты записаны в файл 'casino_report.txt'.");
    }

    static void PlayGame()
    {
        while (playersQueue.TryDequeue(out Player player))
        {
            semaphore.WaitOne(); 
            try
            {
                player.BetAndPlay();
                Console.WriteLine($"Игрок{player.Id} сделал ставку. Начальная сумма: {player.StartMoney}, текущая сумма: {player.Money}");
                if (player.Money <= 0)
                {
                    Console.WriteLine($"Игрок{player.Id} покидает стол.");
                }
                else
                {
                    playersQueue.Enqueue(player); 
                }
            }
            finally
            {
                semaphore.Release(); 
                if (player.Money <= 0)
                {
                    finishedPlayers.Add(player);
                }
            }
        }
    }
}