using System.Collections.Generic;
using UnityEngine;
using System;

public class Tourism : CardType
{
    public Tourism(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 2, 3))
            player.AddLoseToken(4, (1, TokenType.HouseIcon), logged);
    }
}
