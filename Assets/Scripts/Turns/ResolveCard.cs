using UnityEngine;
using Photon.Pun;

public class ResolveCard : Turn
{
    public override void MasterStart()
    {
        Card card = TurnManager.inst.TopCard();
        Log.inst.MasterText(true, OnlineTranslate.Online_Next_Card(card.name));
        card.thisCard.MasterStart();
    }
    public override void ForPlayer(Player player)
    {
        Card card = TurnManager.inst.TopCard();
        card.thisCard.ForPlayer(player);
    }

    public override void MasterEnd()
    {
        Card card = TurnManager.inst.TopCard();
        card.thisCard.MasterEnd();
    }
}
