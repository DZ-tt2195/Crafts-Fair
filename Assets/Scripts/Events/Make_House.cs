using System.Collections.Generic;
using UnityEngine;
using System;

public class Make_House : CardType
{
    public Make_House(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 2, 3))
            player.AddLoseToken(4, (1, TokenType.HouseIcon), logged);
    }
}
