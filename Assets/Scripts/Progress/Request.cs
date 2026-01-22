using UnityEngine;

public class Request : CardType
{
    public Request(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        player.DrawPlacardRPC(1);
    }
}
