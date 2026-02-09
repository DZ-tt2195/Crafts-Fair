using UnityEngine;
using System;
using System.Collections.Generic;

public class Bulk_Buyer : CardType
{
    public Bulk_Buyer(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        int totalTokens = 0;
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
            totalTokens += MyExtensions.SumOfArray(soldTokens[token]);
        return totalTokens >= 6;
    }
}
