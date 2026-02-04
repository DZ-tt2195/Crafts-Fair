using UnityEngine;
using System.Collections.Generic;

public class Unnecessary_Surplus : CardType
{
    public Unnecessary_Surplus(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return player.AllTotalTokens() >= 5;
    }
}
