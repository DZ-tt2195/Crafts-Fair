using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System;

public enum TwistTrigger {StartTurn, WhenSubmit}
public class CardType
{
    public CardData dataFile { get; private set; }

    public CardType(CardData dataFile)
    {
        this.dataFile = dataFile;
    }

#region Twist

    public virtual void TwistEffect(Player player, int logged)
    {
    }

#endregion

#region  Placard

    public virtual bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted)
    {
        return false;
    }

#endregion

}
