using UnityEngine;
using System.Collections.Generic;

public class Author : CardType
{
    public Author(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.ToolIcon, TokenType.ArtIcon, TokenType.BookIcon);
    }
}
