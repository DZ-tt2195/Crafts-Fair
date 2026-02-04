using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class High_Sword : CardType
{
    public High_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        int numHouse = MyExtensions.SumOfArray(player.GetTokenDict()[TokenType.HouseIcon]);
        player.AddLoseToken(1, (numHouse, TokenType.SwordIcon), logged);
    }
}
