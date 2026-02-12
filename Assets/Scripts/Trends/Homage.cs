using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Homage : CardType
{
    public Homage(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        int numArt = player.GetTokenDict()[TokenType.ArtIcon][1];
        player.AddLoseToken(1, (1, TokenType.ToolIcon), logged);
        player.UpDowngradeToken(1, (1, TokenType.ToolIcon), numArt, logged);
    }
}
