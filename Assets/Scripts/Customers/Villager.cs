using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Villager : CardType
{
    public Villager(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 1, 4);
    }
}
