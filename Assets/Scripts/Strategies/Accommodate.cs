using System.Collections.Generic;
using UnityEngine;
using System;

public class Accommodate : CardType
{
    public Accommodate(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 2, 3))
            player.AddLoseToken(4, (1, TokenType.HouseIcon), logged);
    }
}
