using System.Collections.Generic;
using UnityEngine;
using System;

public class Tourists : CardType
{
    public Tourists(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        if (WithLevel(player.GetTokenDict(), FindNumber.Minimum, 2, 3))
            player.CreateLoseToken(4, (1, TokenType.HouseIcon), logged);
    }
}
