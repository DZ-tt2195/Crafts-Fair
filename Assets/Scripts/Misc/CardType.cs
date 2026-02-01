using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System;

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
    protected bool SumOfLevels(List<(int level, TokenType type)> tokensSubmitted, FindNumber toFind, int compare)
    {
        int totalLevels = 0;
        foreach (var (level, type) in tokensSubmitted)
            totalLevels += level;

        return MyExtensions.Comparison(toFind, totalLevels, compare); 
    }
    protected bool WithLevel(List<(int level, TokenType type)> tokensSubmitted, FindNumber toFind, int specificLevel, int compare)
    {
        int withLevel = 0;
        foreach (var (level, type) in tokensSubmitted)
        {
            if (level == specificLevel)
                withLevel++;
        }

        return MyExtensions.Comparison(toFind, withLevel, compare); 
    }
    protected bool TypesInOrder(List<(int level, TokenType type)> tokensSubmitted, TokenType type1, TokenType type2, TokenType type3)
    {
        int minFirst = int.MaxValue;
        int maxLast = int.MinValue;
        HashSet<int> middleLevels = new();

        foreach (var (level, type) in tokensSubmitted)
        {
            if (type == type1)
                minFirst = Mathf.Min(minFirst, level);
            else if (type == type2)
                middleLevels.Add(level);
            else if (type == type3)
                maxLast = Mathf.Max(maxLast, level);
        }

        if (minFirst >= maxLast) return false;

        for (int level = minFirst + 1; level < maxLast; level++)
        {
            if (middleLevels.Contains(level))
                return true;
        }
        return false;    
    }
    protected bool SequentialLevels(List<(int level, TokenType type)> tokensSubmitted, TokenType toFind, int number)
    {
        HashSet<int> uniqueLevels = new();

        foreach (var (level, type) in tokensSubmitted)
            if (type == toFind) uniqueLevels.Add(level);

        if (uniqueLevels.Count < number) return false;

        foreach (int currentLevel in uniqueLevels)
        {
            int consecutive = 1;
            while (uniqueLevels.Contains(consecutive+currentLevel))
            {
                consecutive++;
                if (consecutive == number) 
                    return true;
            }
        }
        return false;        
    }
    protected bool TypesOrNot(List<(int level, TokenType type)> tokensSubmitted, HashSet<TokenType> required, HashSet<TokenType> banned)
    {
        HashSet<TokenType> checkedOff = new();
        foreach (var (level, type) in tokensSubmitted)
        {
            if (banned.Contains(type))
                return false;
            else if (required.Contains(type))
                checkedOff.Add(type);
        }
        return checkedOff.Count == required.Count;
    }
#endregion

}
