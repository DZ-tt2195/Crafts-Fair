using UnityEngine;
using System.Collections.Generic;

public class Above_And_Beyond : CardType
{
    public Above_And_Beyond(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int, TokenType)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        return tokensSubmitted.Count == 4;
    }
}
