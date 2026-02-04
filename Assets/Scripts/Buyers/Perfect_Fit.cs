using System.Collections.Generic;
using UnityEngine;

public class Perfect_Fit : CardType
{
    public Perfect_Fit(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return player.AllTotalTokens() == 0;
    }
}
