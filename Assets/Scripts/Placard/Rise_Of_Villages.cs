using UnityEngine;
using System.Collections.Generic;

public class Rise_Of_Villages : CardType
{
    public Rise_Of_Villages(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        int ranked2 = 0;
        foreach (var token in tokensSubmitted)
        {
            if (token.value == 2)
                ranked2++;
        }
        return ranked2 >= 2;
    }
}
