using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Focus : CardType
{
    public Focus(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (6, TokenType.ArtIcon), logged);
        int numDowngrade = Mathf.FloorToInt(player.AllTotalTokens()/3);
        player.UpDowngradeToken(1, (6, TokenType.ArtIcon), -1*numDowngrade, logged);
    }
}
