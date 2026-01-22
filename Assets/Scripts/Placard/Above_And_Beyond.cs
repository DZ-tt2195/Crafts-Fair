using UnityEngine;
using System.Collections.Generic;

public class Above_And_Beyond : CardType
{
    public Above_And_Beyond(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted)
    {
        return tokensSubmitted.Count >= 4;
    }
}
