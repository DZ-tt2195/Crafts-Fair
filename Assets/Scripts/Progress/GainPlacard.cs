using UnityEngine;

public class GainPlacard : CardType
{
    public GainPlacard(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        player.DrawPlacardRPC(1);
    }
}
