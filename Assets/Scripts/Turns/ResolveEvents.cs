using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ResolveEvents : Turn
{
    (List<TokenType>, List<Card>) TrendsNoCounter()
    {
        List<TokenType> typesToResolve = new();
        List<Card> trendsToResolve = new();
        foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
        {
            string tokencounter = ConstantStrings.TokenCounter(type);
            if (TurnManager.inst.GetInt(tokencounter) <= 0)
            {
                typesToResolve.Add(type);
                trendsToResolve.Add(CreateGame.inst.GetEvent(type).card);
            }
        }
        return (typesToResolve, trendsToResolve);        
    }

    public override void MasterStart()
    {
        List<Card> trendsToResolve = TrendsNoCounter().Item2;
        Log.inst.MasterText(true, AutoTranslate.Blank());
        Log.inst.MasterText(true, OnlineTranslate.Online_Strategies_To_Resolve(trendsToResolve.Count.ToString()));
    }

    public override void ForPlayer(Player player)
    {
        List<Card> trendsToResolve = TrendsNoCounter().Item2;
        player.DrawCustomerRPC(2*trendsToResolve.Count);
        Log.inst.NewDecisionContainer(() => ChooseTwist(player, trendsToResolve));
    }

    void ChooseTwist(Player player, List<Card> trendsToResolve)
    {
        MakeDecision.inst.ChooseCardOnScreen(trendsToResolve, AutoTranslate.Ask_Resolve(), DoTwist);

        void DoTwist(Card card)
        {
            Log.inst.AddMyText(false, OnlineTranslate.Online_Resolve_Card(player.name, card.name));
            Log.inst.NewDecisionContainer(() => card.thisCard.TrendEffect(player, 1));

            List<Card> newList = trendsToResolve;
            newList.Remove(card);
            if (newList.Count > 0)
                Log.inst.NewDecisionContainer(() => ChooseTwist(player, newList));
        }
    }

    public override void MasterEnd()
    {
        List<TokenType> tokenEventsToAdd = TrendsNoCounter().Item1;
        foreach (TokenType type in tokenEventsToAdd)
        {
            string tokencounter = ConstantStrings.TokenCounter(type);
            PhotonCompatible.InstantChangeRoomProp(tokencounter, 2*CreateGame.inst.GetPlayers().Count);
        }
    }
}
