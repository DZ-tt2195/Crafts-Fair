using UnityEngine;
using System.Collections.Generic;

public class Driver : CardType
{
    public Driver(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 8);
    }
}
