using UnityEngine;
using System.Collections.Generic;

public class Strength_In_Numbers : CardType
{
    public Strength_In_Numbers(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int, TokenType)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        return (placardsSubmitted.Count + 1) == 4;
    }
}
