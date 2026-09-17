using UnityEngine;
using System;
using System.Collections.Generic;

public class Bishop : CardType
{
    public Bishop(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.ArtIcon, TokenType.BookIcon, TokenType.HouseIcon);
    }
}
