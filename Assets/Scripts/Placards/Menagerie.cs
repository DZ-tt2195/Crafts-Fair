using System;
using System.Collections.Generic;
using UnityEngine;

public class Menagerie : CardType
{
    public Menagerie(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        HashSet<TokenType> required = new() {TokenType.ArtIcon, TokenType.HouseIcon, TokenType.SwordIcon, TokenType.TechIcon};
        return TypesOrNot(tokensSubmitted, required, new());
    }
}
