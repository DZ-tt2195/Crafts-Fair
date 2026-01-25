using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class DisplayTwists : Turn
{
    public override void MasterStart()
    {
        CreateGame.inst.CreateStartingDeck();
    }

    public override void ForPlayer(Player player)
    {
        player.DrawPlacardRPC(2);
    }
}
