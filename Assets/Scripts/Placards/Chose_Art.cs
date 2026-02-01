using UnityEngine;
using System.Collections.Generic;

public class Chose_Art : CardType
{
    public Chose_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.ArtIcon);
    }
}
