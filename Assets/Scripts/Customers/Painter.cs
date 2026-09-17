using UnityEngine;
using System.Collections.Generic;

public class Painter : CardType
{
    public Painter(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.ArtIcon, 4);
    }
}
