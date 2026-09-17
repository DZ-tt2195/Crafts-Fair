using UnityEngine;
using System.Collections.Generic;

public class Historian : CardType
{
    public Historian(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.BookIcon, 4);
    }
}
