using UnityEngine;
using System.Collections.Generic;

public class Magnate : CardType
{
    public Magnate(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return NumTokensSold(soldTokens, FindNumber.Exact, 3) && SumOfLevels(soldTokens, FindNumber.Exact, 12);
    }
}
