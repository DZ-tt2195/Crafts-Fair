using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Manors : CardType
{
    public Manors(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (1, TokenType.HouseIcon), logged);
        int upgrade = 0;
        int[] array = player.GetTokenDict()[TokenType.BookIcon];
        for (int i = 0; i<array.Length; i++)
        {
            if (array[i] >= 1)
                upgrade++;
        }
        player.UpDowngradeToken(1, (1, TokenType.ToolIcon), upgrade, logged);
    }
}
