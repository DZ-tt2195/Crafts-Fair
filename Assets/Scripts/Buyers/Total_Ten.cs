using UnityEngine;
using System.Collections.Generic;

public class Total_Ten : CardType
{
    public Total_Ten(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 10);
    }
}
