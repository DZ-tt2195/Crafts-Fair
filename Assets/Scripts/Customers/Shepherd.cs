using UnityEngine;
using System;
using System.Collections.Generic;

public class Shepherd : CardType
{
    public Shepherd(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return HigherTypeVs(soldTokens, 4, TokenType.HouseIcon);
    }
}
