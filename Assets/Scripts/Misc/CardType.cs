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

    public virtual bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return false;
    }
    public bool SumOfLevels(List<(int level, TokenType type)> tokensSubmitted, FindNumber toFind, int compare)
    {
        int totalLevels = 0;
        foreach (var (level, type) in tokensSubmitted)
            totalLevels += level;

        return MyExtensions.Comparison(toFind, totalLevels, compare); 
    }
    public bool WithLevel(List<(int level, TokenType type)> tokensSubmitted, FindNumber toFind, int specificLevel, int compare)
    {
        int withLevel = 0;
        foreach (var (level, type) in tokensSubmitted)
        {
            if (level == specificLevel)
                withLevel++;
        }

        return MyExtensions.Comparison(toFind, withLevel, compare); 
    }

#endregion

}
