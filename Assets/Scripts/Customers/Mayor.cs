using UnityEngine;
using System;
using System.Collections.Generic;

public class Mayor : CardType
{
    public Mayor(Card card, CardData dataFile) : base(card, dataFile)
    {
    }
    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return soldTokens[TokenType.HouseIcon][6] >= 1;
    }
}
