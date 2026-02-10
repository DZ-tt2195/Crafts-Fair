using UnityEngine;
using System.Collections.Generic;

public class Blacksmith : CardType
{
    public Blacksmith(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return player.AllTotalTokens() >= 5;
    }
}
