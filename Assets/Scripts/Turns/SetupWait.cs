using UnityEngine;

public class SetupWait : Turn
{
    public override void ForPlayer(Player player)
    {
        player.endPause = false;
    }
}
