using UnityEngine;

public class Quantity : CardType
{
    public Quantity(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (1, TokenType.ArtIcon), logged);
        player.AddLoseToken(1, (1, TokenType.HouseIcon), logged);
        player.AddLoseToken(1, (1, TokenType.SwordIcon), logged);
        player.AddLoseToken(1, (1, TokenType.TechIcon), logged);
    }
}
