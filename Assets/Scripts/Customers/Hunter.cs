using UnityEngine;
using System.Collections.Generic;

public class Hunter : CardType
{
    public Hunter(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 10);
    }
}
