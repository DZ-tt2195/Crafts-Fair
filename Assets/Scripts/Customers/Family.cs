using UnityEngine;
using System.Collections.Generic;

public class Family : CardType
{
    public Family(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 3, 4);
    }
}
