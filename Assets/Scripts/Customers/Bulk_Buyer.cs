using UnityEngine;
using System;
using System.Collections.Generic;

public class Bulk_Buyer : CardType
{
    public Bulk_Buyer(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return NumTokensSold(soldTokens, FindNumber.Minimum, 8);
    }
}
