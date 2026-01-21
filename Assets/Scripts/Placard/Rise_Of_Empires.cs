using UnityEngine;
using System.Collections.Generic;

public class Rise_Of_Empires : CardType
{
    public Rise_Of_Empires(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        int ranked6 = 0;
        foreach (var token in tokensSubmitted)
        {
            if (token.value == 6)
                ranked6++;
        }
        return ranked6 >= 2;
    }
}
