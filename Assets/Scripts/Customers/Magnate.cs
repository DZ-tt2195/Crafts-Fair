using UnityEngine;
using System.Collections.Generic;

public class Magnate : CardType
{
    public Magnate(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 12);
    }
}
