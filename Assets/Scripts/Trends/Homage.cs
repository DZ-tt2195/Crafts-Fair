using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Homage : CardType
{
    public Homage(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        int numArt = MyExtensions.SumOfArray(player.GetTokenDict()[TokenType.ArtIcon]);
        player.AddLoseToken(1, (numArt, TokenType.SwordIcon), logged);
    }
}
