using UnityEngine;
using System.Collections.Generic;

public class Inventor : CardType
{
    public Inventor(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.ToolIcon, 4);
    }
}
