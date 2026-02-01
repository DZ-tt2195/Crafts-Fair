using UnityEngine;
using System.Collections.Generic;

public class Sequential_Art : CardType
{
    public Sequential_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SequentialLevels(tokensSubmitted, TokenType.ArtIcon, 4);
    }
}
