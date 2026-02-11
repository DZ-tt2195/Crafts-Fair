using UnityEngine;
using System;
using System.Collections.Generic;

public class Knight : CardType
{
    public Knight(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return soldTokens[TokenType.SwordIcon][6] >= 1;
    }
}
