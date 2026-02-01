using UnityEngine;
using System.Collections.Generic;

public class Bare_Minimum : CardType
{
    public Bare_Minimum(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return tokensSubmitted.Count == 2;
    }
}
