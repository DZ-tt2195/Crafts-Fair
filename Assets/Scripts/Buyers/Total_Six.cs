using UnityEngine;
using System.Collections.Generic;

public class Total_Six : CardType
{
    public Total_Six(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 6);
    }
}
