using UnityEngine;

public class Pageants : CardType
{
    public Pageants(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        if (player.GetCoins() >= 5)
            player.AddLoseToken(4, (1, TokenType.ArtIcon), logged);
    }
}
