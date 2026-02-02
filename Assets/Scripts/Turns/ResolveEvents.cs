using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ResolveEvents : Turn
{
    (List<TokenType>, List<Card>) EventsNoCounter()
    {
        List<TokenType> typesToResolve = new();
        List<Card> eventsToResolve = new();
        foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
        {
            string tokencounter = ConstantStrings.TokenCounter(type);
            if (TurnManager.inst.GetInt(tokencounter) <= 0)
            {
                typesToResolve.Add(type);
                eventsToResolve.Add(CreateGame.inst.GetEvent(type).card);
            }
        }
        return (typesToResolve, eventsToResolve);        
    }

    public override void MasterStart()
    {
        List<Card> eventsToResolve = EventsNoCounter().Item2;
        Log.inst.MasterText(true, AutoTranslate.Blank());
        Log.inst.MasterText(true, OnlineTranslate.Online_Events_To_Resolve(eventsToResolve.Count.ToString()));
    }

    public override void ForPlayer(Player player)
    {
        List<Card> eventsToResolve = EventsNoCounter().Item2;
        player.DrawBuyerRPC(2*eventsToResolve.Count);
        Log.inst.NewDecisionContainer(() => ChooseTwist(player, eventsToResolve));
    }

    void ChooseTwist(Player player, List<Card> eventsToResolve)
    {
        MakeDecision.inst.ChooseCardOnScreen(eventsToResolve, AutoTranslate.Ask_Resolve(), DoTwist);

        void DoTwist(Card card)
        {
            Log.inst.AddMyText(false, OnlineTranslate.Online_Resolve_Card(player.name, card.name));
            card.thisCard.EventEffect(player, 1);

            List<Card> newList = eventsToResolve;
            newList.Remove(card);
            if (newList.Count > 0)
                Log.inst.NewDecisionContainer(() => ChooseTwist(player, newList));
        }
    }

    public override void MasterEnd()
    {
        List<TokenType> tokenEventsToAdd = EventsNoCounter().Item1;
        foreach (TokenType type in tokenEventsToAdd)
        {
            string tokencounter = ConstantStrings.TokenCounter(type);
            PhotonCompatible.InstantChangeRoomProp(tokencounter, 2*CreateGame.inst.GetPlayers().Count);
        }
    }
}
