using UnityEngine;
using System;
using System.Collections.Generic;

public class Sentry : CardType
{
    public Sentry(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.TechIcon, TokenType.HouseIcon, TokenType.SwordIcon);
    }
}
