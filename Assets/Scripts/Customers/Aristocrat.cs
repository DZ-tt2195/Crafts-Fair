using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Aristocrat : CardType
{
    public Aristocrat(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 4, 2);
    }
}
