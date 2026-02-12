using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class DisplayTwists : Turn
{
    public override void MasterStart()
    {
        CreateGame.inst.CreateEvents();
    }

    public override void ForPlayer(Player player)
    {
        player.DrawCustomerRPC(4);
        CreateGame.inst.AddPlayerRPC(player);
    }
}
