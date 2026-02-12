using UnityEngine;
using System.Collections.Generic;

public class Backer : CardType
{
    public Backer(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.TechIcon);
    }
}
