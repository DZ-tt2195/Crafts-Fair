using UnityEngine;
using System.Collections.Generic;

public class Above_And_Beyond : CardType
{
    public Above_And_Beyond(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return tokensSubmitted.Count >= 5;
    }
}
