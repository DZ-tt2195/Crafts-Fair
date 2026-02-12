using UnityEngine;
using System.Collections.Generic;
using System;

public class Miser : CardType
{
    public Miser(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        
        int totalTokens = 0;
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
            totalTokens += MyExtensions.SumOfArray(soldTokens[token]);
        return totalTokens == 2;
    }
}
