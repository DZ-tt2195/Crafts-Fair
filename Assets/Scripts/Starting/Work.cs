using System.Collections.Generic;
using UnityEngine;

public class Work : CardType
{
    public Work(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        Log.inst.NewDecisionContainer(() => TokenStuff(player), 0);
        Log.inst.NewDecisionContainer(() => Submit(player, new(), new()), 0);
    }

    void TokenStuff(Player player)
    {
        
    }

    void Submit(Player player, List<(int, TokenType)> submittedTokens, List<Card> submittedPlacards)
    {
        
    }
}
