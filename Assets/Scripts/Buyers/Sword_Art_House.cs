using UnityEngine;
using System;
using System.Collections.Generic;

public class Sword_Art_House : CardType
{
    public Sword_Art_House(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.SwordIcon, TokenType.ArtIcon, TokenType.HouseIcon);
    }
}
