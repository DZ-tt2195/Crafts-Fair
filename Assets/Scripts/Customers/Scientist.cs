using UnityEngine;
using System;
using System.Collections.Generic;

public class Scientist : CardType
{
    public Scientist(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.BookIcon, TokenType.HouseIcon, TokenType.ToolIcon);
    }
}
