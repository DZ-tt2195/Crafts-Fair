using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ResolveTwists : Turn
{
    public override void MasterStart()
    {
    }

    public override void ForPlayer(Player player)
    {
        List<Card> twistsToResolve = new();
        foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
        {
            string tokencounter = ConstantStrings.TokenCounter(type);
            if (TurnManager.inst.GetInt(tokencounter) <= 0)
                twistsToResolve.Add(CreateGame.inst.GetTwist(type).card);
        }
        Log.inst.NewDecisionContainer(() => ChooseTwist(player, twistsToResolve));
    }

    void ChooseTwist(Player player, List<Card> twistsToResolve)
    {
        MakeDecision.inst.ChooseCardOnScreen(twistsToResolve, AutoTranslate.Resolve_Twist(), DoTwist);

        void DoTwist(Card card)
        {
            Log.inst.AddMyText(false, OnlineTranslate.Online_Resolve_Card(player.name, card.name));
            card.thisCard.TwistEffect(player, 1);

            List<Card> newList = twistsToResolve;
            newList.Remove(card);
            if (newList.Count > 0)
                Log.inst.NewDecisionContainer(() => ChooseTwist(player, newList));
        }
    }

    public override void MasterEnd()
    {
        foreach (TokenType type in Enum.GetValues(typeof(TokenType)))
        {
            string tokencounter = ConstantStrings.TokenCounter(type);
            if (TurnManager.inst.GetInt(tokencounter) <= 0)
                PhotonCompatible.InstantChangeRoomProp(tokencounter, 2*CreateGame.inst.GetPlayers().Count);
        }
    }
}
