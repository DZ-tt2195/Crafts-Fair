using UnityEngine;
using System.Collections.Generic;

public class Exact_One : CardType
{
    public Exact_One(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return WithLevel(tokensSubmitted, FindNumber.Exact, 1, 2);
    }
}
