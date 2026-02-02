using UnityEngine;
using System.Collections.Generic;

public class Chose_Art : CardType
{
    public Chose_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.ArtIcon);
    }
}
