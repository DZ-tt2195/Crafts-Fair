using UnityEngine;
using System.Collections.Generic;

public class Rise_Of_Kingdoms : CardType
{
    public Rise_Of_Kingdoms(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        int ranked4 = 0;
        foreach (var token in tokensSubmitted)
        {
            if (token.value == 4)
                ranked4++;
        }
        return ranked4 >= 2;
    }
}
