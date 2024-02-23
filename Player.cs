using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWdop2102CasinoTable;

public class Player
{
    public int Id { get; private set; }
    public int StartMoney { get; private set; }
    public int Money { get; set; }

    public Player(int id, int money)
    {
        Id = id;
        StartMoney = money;
        Money = money;
    }

    // Метод для совершения ставки и игры
    public void BetAndPlay()
    {
        Random random = new Random();
        int bet = random.Next(1, Math.Min(Money, 100)); // Ставка не больше текущих денег и не больше 100
        int betNumber = random.Next(0, 37);
        int winNumber = random.Next(0, 37);

        if (betNumber == winNumber)
        {
            Money += bet; // Выигрыш удваивает ставку
        }
        else
        {
            Money -= bet; // Проигрыш забирает ставку
        }
    }
}
