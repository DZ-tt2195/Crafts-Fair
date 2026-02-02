using UnityEngine;
using System.Collections.Generic;

public class Total_Four : CardType
{
    public Total_Four(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SumOfLevels(soldTokens, FindNumber.Exact, 4);
    }
}
