using UnityEngine;
using System.Collections.Generic;

public class Piper : CardType
{
    public Piper(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 1, 4);
    }
}
