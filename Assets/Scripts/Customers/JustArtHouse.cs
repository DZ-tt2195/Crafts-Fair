using UnityEngine;
using System.Collections.Generic;

public class JustArtHouse : CardType
{    
    public JustArtHouse(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesOrNot(soldTokens, 2, 
            new() {TokenType.ArtIcon, TokenType.HouseIcon}, 
            new() {TokenType.ToolIcon, TokenType.BookIcon});
    }
}
