using UnityEngine;
using System.Collections.Generic;

public class Total_Eight : CardType
{
    public Total_Eight(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 8);
    }
}
