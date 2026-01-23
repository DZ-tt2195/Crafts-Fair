using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class ResolveCard : Turn
{
    public override void MasterStart()
    {
        int currentShuffle = TurnManager.inst.GetInt(ConstantStrings.Shuffle);
        List<Card> currentDeck = TurnManager.inst.GetCardList(ConstantStrings.ProgressDeck);
        List<Card> currentDiscard = TurnManager.inst.GetCardList(ConstantStrings.ProgressDiscard);
        int totalCards = currentDeck.Count+currentDiscard.Count;
        Card currentCard = TurnManager.inst.TopCard();

        Log.inst.MasterText(true, OnlineTranslate.Online_Next_Card(currentShuffle.ToString(), (1+totalCards-currentDeck.Count).ToString(), totalCards.ToString(), currentCard.ToString()));
        MakeDecision.inst.ChangeDisplayedCards(new int[1] {currentCard.photonView.ViewID});
        currentCard.thisCard.MasterStart();
    }
    public override void ForPlayer(Player player)
    {
        Card currentCard = TurnManager.inst.TopCard();
        currentCard.thisCard.ForPlayer(player);
    }

    public override void MasterEnd()
    {
        Card currentCard = TurnManager.inst.TopCard();
        currentCard.thisCard.MasterEnd();
    }
}
