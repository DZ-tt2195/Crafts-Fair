using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class DisplayTwists : Turn
{
    public override void MasterStart()
    {
        CreateGame.inst.CreateTwists();
    }

    public override void ForPlayer(Player player)
    {
        player.DrawPlacardRPC(4);
        CreateGame.inst.AddPlayerRPC(player);
    }
}
