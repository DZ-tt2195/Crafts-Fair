using UnityEngine;
using System.Collections.Generic;

public class Sculptor : CardType
{
    public Sculptor(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.ArtIcon, 4);
    }
}
